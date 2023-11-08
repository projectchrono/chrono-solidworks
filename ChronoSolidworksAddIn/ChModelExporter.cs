using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using ChronoEngine_SwAddin;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

/// Base class for exporting a Solidworks assembly to a Chrono::Engine model.
/// Derived classes must implement concrete methods for export in various languages.

namespace ChronoEngineAddin
{
    abstract class ChModelExporter
    {
        protected ChronoEngine_SwAddin.SWIntegration m_swIntegration;
        protected Hashtable m_savedParts;
        protected Hashtable m_savedShapes;
        protected Hashtable m_savedCollisionMeshes;
        protected string save_dir_shapes = "";
        protected string save_filename = "";
        protected int num_comp;
        //protected int _object_ID_used; // identifies last used value of _object_ID, used to uniquely identify any entity in the JSON file
        protected Dictionary<string, string> m_exportNamesMap; // map solidworks names vs chrono script names, ie. map[slwd_name] = chrono_name;


        public ChModelExporter(ChronoEngine_SwAddin.SWIntegration swIntegration)
        {
            m_swIntegration = swIntegration;
        }



        // ============================================================================================================
        // Abstract methods
        // ============================================================================================================
        #region Abstract methods

        public abstract bool ConvertMate(in Feature swMateFeature, in MathTransform roottrasf, in Component2 assemblyofmates); //ref int num_link

        public abstract void TraverseComponentForVisualShapes(Component2 swComp, long nLevel, ref int nVisShape, Component2 chBodyComp); //, int nBody

        public abstract void TraverseFeaturesForCollisionShapes(Component2 swComp, long nLevel, ref MathTransform chBodyTransform, ref bool foundCollShapes, Component2 swCompBase, ref int nCollShape); //, int nBody

        public abstract void TraverseComponentForBodies(Component2 swComp, long nLevel); //, int nBody

        public abstract void TraverseComponentForMarkers(Component2 swComp, long nLevel); //, int nBody

        public abstract void TraverseFeaturesForMarkers(Feature swFeat, long nLevel, MathTransform swCompTotalTransform); //, int nBody

        #endregion


        // ============================================================================================================
        // Common functions
        // ============================================================================================================
        #region Utility functions

        public void TraverseFeaturesForLinks(Feature swFeat, long nLevel, ref MathTransform rootTransform, ref Component2 assemblyOfMates)
        {
            Feature swSubFeat;
            int num_link = 0;

            while (swFeat != null)
            {
                // Export mates as constraints
                if ((swFeat.GetTypeName2() == "MateGroup") && m_swIntegration.m_taskpaneHost.GetCheckboxConstraints().Checked)
                {
                    swSubFeat = (Feature)swFeat.GetFirstSubFeature();

                    while (swSubFeat != null)
                    {
                        if (!swSubFeat.IsSuppressed())
                        {
                            if (ChronoEngine_SwAddin.ConvertMates.IsMateTypeExportable(swSubFeat.GetTypeName2()))
                            {
                                ConvertMate(swSubFeat, rootTransform, assemblyOfMates);
                            }
                            else if (swSubFeat.GetTypeName2() == "FtrFolder")
                            {
                                TraverseFeaturesForLinks(swSubFeat, nLevel + 1, ref rootTransform, ref assemblyOfMates);
                            }
                        }
                        swSubFeat = (Feature)swSubFeat.GetNextSubFeature();
                    } // end while loop on subfeatures mates

                } // end if mate group

                swFeat = (Feature)swFeat.GetNextFeature();

            } // end while loop on features
        }

        public void TraverseComponentForLinks(Component2 swComp, long nLevel, ref MathTransform rootTransform)
        {
            // Scan assembly features and save mating info
            if (nLevel > 1)
            {
                Feature swFeat = (Feature)swComp.FirstFeature();
                TraverseFeaturesForLinks(swFeat, nLevel, ref rootTransform, ref swComp);
            }

            // Recursive scan of subassemblies

            object[] vChildComp;
            Component2 swChildComp;

            vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                if (swChildComp.Solving == (int)swComponentSolvingOption_e.swComponentFlexibleSolving)
                    TraverseComponentForLinks(swChildComp, nLevel + 1, ref rootTransform);
            }
        }

        public void TraverseComponentForCollisionShapes(Component2 swComp, long nLevel, ref MathTransform chbodytransform, ref bool found_collisionshapes, Component2 swCompBase, ref int ncollshape)
        {
            // Look if component contains collision shapes (customized SW solid bodies):
            TraverseFeaturesForCollisionShapes(swComp, nLevel, ref chbodytransform, ref found_collisionshapes, swCompBase, ref ncollshape);

            // Recursive scan of subcomponents

            Component2 swChildComp;
            object[] vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                TraverseComponentForCollisionShapes(swChildComp, nLevel + 1, ref chbodytransform, ref found_collisionshapes, swCompBase, ref ncollshape);
            }
        }

        public void TraverseComponent_for_countingmassbodies(in Component2 swComp, ref int valid_bodies)
        {
            // Add bodies of this component to the list
            object[] bodies;
            object bodyInfo;
            bodies = (object[])swComp.GetBodies3((int)swBodyType_e.swAllBodies, out bodyInfo);

            if (bodies != null)
            {
                // note: some bodies might be collision shapes and must not enter the mass computation:
                for (int ib = 0; ib < bodies.Length; ib++)
                {
                    Body2 abody = (Body2)bodies[ib];
                    if (!(abody.Name.StartsWith("COLL.")))
                    {
                        valid_bodies += 1;
                    }
                }
            }

            // Recursive scan of subcomponents

            Component2 swChildComp;
            object[] vChildComp = (object[])swComp.GetChildren();
            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];
                TraverseComponent_for_countingmassbodies(swChildComp, ref valid_bodies);
            }
        }

        public void TraverseComponent_for_massbodies(in Component2 swComp, ref object[] obodies, ref int addedb)
        {
            // Add bodies of this component to the list
            object[] bodies;
            object bodyInfo;
            bodies = (object[])swComp.GetBodies3((int)swBodyType_e.swAllBodies, out bodyInfo);

            if (bodies != null)
            {
                // note: some bodies might be collision shapes and must not enter the mass computation:
                for (int ib = 0; ib < bodies.Length; ib++)
                {
                    Body2 abody = (Body2)bodies[ib];
                    if (!(abody.Name.StartsWith("COLL.")))
                    {
                        obodies[addedb] = bodies[ib];
                        addedb += 1;
                    }
                }

            }

            // Recursive scan of subcomponents

            Component2 swChildComp;
            object[] vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                TraverseComponent_for_massbodies(swChildComp, ref obodies, ref addedb);
            }
        }

        public void ExportToDump(ref string asciitext)
        {
            ModelDoc2 swModel;
            ConfigurationManager swConfMgr;
            Configuration swConf;
            Component2 swRootComp;

            swModel = (ModelDoc2)m_swIntegration.m_swApplication.ActiveDoc;
            swConfMgr = (ConfigurationManager)swModel.ConfigurationManager;
            swConf = (Configuration)swConfMgr.ActiveConfiguration;
            swRootComp = (Component2)swConf.GetRootComponent3(true);

            asciitext = "# Dump hierarchy from SolidWorks \n" +
                        "# Assembly: " + swModel.GetPathName() + "\n\n\n";

            // The root component (root assembly) cannot work in DumpTraverseComponent() 
            // cause SW api limit, so call feature traversal using this custom step:
            Feature swFeat = (Feature)swModel.FirstFeature();
            DumpTraverseFeatures(swFeat, 1, ref asciitext);

            // Traverse all sub components
            if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
            {
                DumpTraverseComponent(swRootComp, 1, ref asciitext);
            }
        }

        public void DumpTraverseFeatures(Feature swFeat, long nLevel, ref string asciitext)
        {
            Feature swSubFeat;
            string sPadStr = " ";
            long i = 0;

            for (i = 0; i <= nLevel; i++)
            {
                sPadStr = sPadStr + "  ";
            }

            while ((swFeat != null))
            {
                asciitext += sPadStr + "    -" + swFeat.Name + " [" + swFeat.GetTypeName2() + "]" + "\n";
                swSubFeat = (Feature)swFeat.GetFirstSubFeature();
                if ((swSubFeat != null))
                {
                    DumpTraverseFeatures(swSubFeat, nLevel + 1, ref asciitext);
                }
                if (nLevel == 1)
                {
                    swFeat = (Feature)swFeat.GetNextFeature();
                }
                else
                {
                    swFeat = (Feature)swFeat.GetNextSubFeature();
                }
            }
        }

        public void DumpTraverseComponent(Component2 swComp, long nLevel, ref string asciitext)
        {
            // *** SCAN THE COMPONENT FEATURES

            if (!swComp.IsRoot())
            {
                Feature swFeat;
                swFeat = (Feature)swComp.FirstFeature();
                DumpTraverseFeatures(swFeat, nLevel, ref asciitext);
            }

            // *** RECURSIVE SCAN CHILDREN COMPONENTS

            object[] vChildComp;
            Component2 swChildComp;
            string sPadStr = " ";
            long i = 0;

            for (i = 0; i <= nLevel - 1; i++)
            {
                sPadStr = sPadStr + "  ";
            }

            vChildComp = (object[])swComp.GetChildren();

            for (i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                asciitext += sPadStr + "+" + swChildComp.Name2 + " <" + swChildComp.ReferencedConfiguration + ">" + "\n";

                // DumpTraverseComponentFeatures(swChildComp, nLevel, ref asciitext);

                DumpTraverseComponent(swChildComp, nLevel + 1, ref asciitext);
            }
        }

        protected struct LinkParams
        {
            public bool swapAB_1;
            public bool do_ChLinkMateXdistance;
            public double do_distance_val;
            public bool do_ChLinkMateParallel;
            public bool do_parallel_flip;
            public bool do_ChLinkMateOrthogonal;
            public bool do_ChLinkMateSpherical;
            public bool do_ChLinkMatePointLine;
            public int entity0_ref;
            public int entity1_ref;
            public int entity2_ref;
            public int entity3_ref;
            public bool entity_0_as_FACE;
            public bool entity_0_as_EDGE;
            public bool entity_0_as_VERTEX;
            public bool entity_1_as_FACE;
            public bool entity_1_as_EDGE;
            public bool entity_1_as_VERTEX;
            public bool entity_2_as_VERTEX;
            public bool entity_3_as_VERTEX;
            public Point3D cA;
            public Point3D cB;
            public Vector3D dA;
            public Vector3D dB;
            public Point3D cC;
            public Point3D cD;
            public Vector3D dC;
            public Vector3D dD;
            public string ref1;
            public string ref2;
            public string ref3;
            public string ref4;
        };

        protected bool GetLinkParameters(in Feature swMateFeature, out LinkParams link_params, in MathTransform roottrasf, in Component2 assemblyofmates)
        {
            link_params = new LinkParams
            {
                swapAB_1 = false,
                do_ChLinkMateXdistance = false,
                do_distance_val = 0,
                do_ChLinkMateParallel = false,
                do_parallel_flip = false,
                do_ChLinkMateOrthogonal = false,
                do_ChLinkMateSpherical = false,
                do_ChLinkMatePointLine = false,
                entity0_ref = 0,
                entity1_ref = 0,
                entity2_ref = 0,
                entity3_ref = 0,
                entity_0_as_FACE = false,
                entity_0_as_EDGE = false,
                entity_0_as_VERTEX = false,
                entity_1_as_FACE = false,
                entity_1_as_EDGE = false,
                entity_1_as_VERTEX = false,
                entity_2_as_VERTEX = false,
                entity_3_as_VERTEX = false,
                cA = new Point3D(0, 0, 0),
                cB = new Point3D(0, 0, 0),
                dA = new Vector3D(1, 0, 0),
                dB = new Vector3D(1, 0, 0),
                cC = new Point3D(0, 0, 0),
                cD = new Point3D(0, 0, 0),
                dC = new Vector3D(1, 0, 0),
                dD = new Vector3D(1, 0, 0),
                ref1 = null,
                ref2 = null,
                ref3 = null,
                ref4 = null
            };

            if (swMateFeature == null)
                return false;

            Mate2 swMate = (Mate2)swMateFeature.GetSpecificFeature2();

            if (swMate == null)
                return false;

            object foo = null;
            bool[] suppressedflags;
            suppressedflags = (bool[])swMateFeature.IsSuppressed2((int)swInConfigurationOpts_e.swThisConfiguration, foo);
            if (suppressedflags[0] == true)
                return false;

            if (swMate.GetMateEntityCount() < 2)
                return false;

            // Get the mated parts
            MateEntity2 swEntityA = swMate.MateEntity(0);
            MateEntity2 swEntityB = swMate.MateEntity(1);
            Component2 swCompA = swEntityA.ReferenceComponent;
            Component2 swCompB = swEntityB.ReferenceComponent;
            double[] paramsA = (double[])swEntityA.EntityParams;
            double[] paramsB = (double[])swEntityB.EntityParams;
            // this is needed because parts might reside in subassemblies, and mate params are expressed in parent subassembly
            MathTransform invroottrasf = (MathTransform)roottrasf.Inverse();
            MathTransform trA = roottrasf;
            MathTransform trB = roottrasf;

            if (assemblyofmates != null)
            {
                MathTransform partrasfA = assemblyofmates.GetTotalTransform(true);
                if (partrasfA != null)
                    trA = partrasfA.IMultiply(invroottrasf); // row-ordered transf. -> reverse mult.order!
                MathTransform partrasfB = assemblyofmates.GetTotalTransform(true);
                if (partrasfB != null)
                    trB = partrasfB.IMultiply(invroottrasf); // row-ordered transf. -> reverse mult.order!
            }


            // Fetch the python names using hash map (python names added when scanning parts)
            ModelDocExtension swModelDocExt = default(ModelDocExtension);
            ModelDoc2 swModel = (ModelDoc2)m_swIntegration.m_swApplication.ActiveDoc;
            swModelDocExt = swModel.Extension;
            link_params.ref1 = (string)m_savedParts[swModelDocExt.GetPersistReference3(swCompA)];
            link_params.ref2 = (string)m_savedParts[swModelDocExt.GetPersistReference3(swCompB)];

            // Only constraints between two parts or part & layout can be created
            if (link_params.ref1 == link_params.ref2) // covers both the case of both null or both equal
                return false;



            //
            // For each type of SW mate, see which C::E mate constraint(s) 
            // must be created. Some SW mates correspond to more than one C::E constraints.
            // 



            // to simplify things later...
            // NOTE: swMate.MateEntity(0).Reference.GetType() seems equivalent to  swMate.MateEntity(0).ReferenceType2  
            // but in some cases the latter fails.
            /*
            bool entity_0_as_FACE   = (swMate.MateEntity(0).Reference.GetType() == (int)swSelectType_e.swSelFACES);
            bool entity_0_as_EDGE   = (swMate.MateEntity(0).Reference.GetType() == (int)swSelectType_e.swSelEDGES) ||
                                        (swMate.MateEntity(0).Reference.GetType() == (int)swSelectType_e.swSelSKETCHSEGS) ||
                                        (swMate.MateEntity(0).Reference.GetType() == (int)swSelectType_e.swSelDATUMAXES);
            bool entity_0_as_VERTEX = (swMate.MateEntity(0).Reference.GetType() == (int)swSelectType_e.swSelVERTICES) ||
                                        (swMate.MateEntity(0).Reference.GetType() == (int)swSelectType_e.swSelSKETCHPOINTS) ||
                                        (swMate.MateEntity(0).Reference.GetType() == (int)swSelectType_e.swSelDATUMPOINTS);

            bool entity_1_as_FACE   = (swMate.MateEntity(1).Reference.GetType() == (int)swSelectType_e.swSelFACES);
            bool entity_1_as_EDGE   = (swMate.MateEntity(1).Reference.GetType() == (int)swSelectType_e.swSelEDGES) ||
                                        (swMate.MateEntity(1).Reference.GetType() == (int)swSelectType_e.swSelSKETCHSEGS) ||
                                        (swMate.MateEntity(1).Reference.GetType() == (int)swSelectType_e.swSelDATUMAXES);
            bool entity_1_as_VERTEX = (swMate.MateEntity(1).Reference.GetType() == (int)swSelectType_e.swSelVERTICES) ||
                                        (swMate.MateEntity(1).Reference.GetType() == (int)swSelectType_e.swSelSKETCHPOINTS) ||
                                        (swMate.MateEntity(1).Reference.GetType() == (int)swSelectType_e.swSelDATUMPOINTS);
            */

            // NOTE: swMate.MateEntity(0).Reference.GetType() seems equivalent to  swMate.MateEntity(0).ReferenceType2  
            // but in some cases the latter fails. However, sometimes swMate.MateEntity(0).Reference.GetType() is null ReferenceType2 is ok,
            // so do the following trick:
            link_params.entity0_ref = swMate.MateEntity(0).Reference.GetType();
            if (link_params.entity0_ref == (int)swSelectType_e.swSelNOTHING)
                link_params.entity0_ref = swMate.MateEntity(0).ReferenceType2;
            link_params.entity1_ref = swMate.MateEntity(1).Reference.GetType();
            if (link_params.entity1_ref == (int)swSelectType_e.swSelNOTHING)
                link_params.entity1_ref = swMate.MateEntity(1).ReferenceType2;

            link_params.entity_0_as_FACE = (link_params.entity0_ref == (int)swSelectType_e.swSelFACES) ||
                                                (link_params.entity0_ref == (int)swSelectType_e.swSelDATUMPLANES);
            link_params.entity_0_as_EDGE = (link_params.entity0_ref == (int)swSelectType_e.swSelEDGES) ||
                                                (link_params.entity0_ref == (int)swSelectType_e.swSelSKETCHSEGS) ||
                                                (link_params.entity0_ref == (int)swSelectType_e.swSelDATUMAXES);
            link_params.entity_0_as_VERTEX = (link_params.entity0_ref == (int)swSelectType_e.swSelVERTICES) ||
                                                (link_params.entity0_ref == (int)swSelectType_e.swSelSKETCHPOINTS) ||
                                                (link_params.entity0_ref == (int)swSelectType_e.swSelDATUMPOINTS);

            link_params.entity_1_as_FACE = (link_params.entity1_ref == (int)swSelectType_e.swSelFACES) ||
                                                (link_params.entity1_ref == (int)swSelectType_e.swSelDATUMPLANES);
            link_params.entity_1_as_EDGE = (link_params.entity1_ref == (int)swSelectType_e.swSelEDGES) ||
                                                (link_params.entity1_ref == (int)swSelectType_e.swSelSKETCHSEGS) ||
                                                (link_params.entity1_ref == (int)swSelectType_e.swSelDATUMAXES);
            link_params.entity_1_as_VERTEX = (link_params.entity1_ref == (int)swSelectType_e.swSelVERTICES) ||
                                                (link_params.entity1_ref == (int)swSelectType_e.swSelSKETCHPOINTS) ||
                                                (link_params.entity1_ref == (int)swSelectType_e.swSelDATUMPOINTS);


            Point3D cAloc = new Point3D(paramsA[0], paramsA[1], paramsA[2]);
            link_params.cA = PointTransform(cAloc, ref trA);
            Point3D cBloc = new Point3D(paramsB[0], paramsB[1], paramsB[2]);
            link_params.cB = PointTransform(cBloc, ref trB);

            if (!link_params.entity_0_as_VERTEX)
            {
                Vector3D dAloc = new Vector3D(paramsA[3], paramsA[4], paramsA[5]);
                link_params.dA = DirTransform(dAloc, ref trA);
            }

            if (!link_params.entity_1_as_VERTEX)
            {
                Vector3D dBloc = new Vector3D(paramsB[3], paramsB[4], paramsB[5]);
                link_params.dB = DirTransform(dBloc, ref trB);
            }



            if (swMateFeature.GetTypeName2() == "MateCoincident")
            {
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMateXdistance = true;
                    link_params.do_ChLinkMateParallel = true;
                }
                if ((link_params.entity_0_as_EDGE) &&
                    (link_params.entity_1_as_EDGE))
                {
                    link_params.do_ChLinkMatePointLine = true;
                    link_params.do_ChLinkMateParallel = true;
                }
                if ((link_params.entity_0_as_VERTEX) &&
                    (link_params.entity_1_as_VERTEX))
                {
                    link_params.do_ChLinkMateSpherical = true;
                }
                if ((link_params.entity_0_as_VERTEX) &&
                    (link_params.entity_1_as_EDGE))
                {
                    link_params.do_ChLinkMatePointLine = true;
                }
                if ((link_params.entity_0_as_EDGE) &&
                    (link_params.entity_1_as_VERTEX))
                {
                    link_params.do_ChLinkMatePointLine = true;
                    link_params.swapAB_1 = true;
                }
                if ((link_params.entity_0_as_VERTEX) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMateXdistance = true;
                }
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_VERTEX))
                {
                    link_params.do_ChLinkMateXdistance = true;
                    link_params.swapAB_1 = true;
                }
                if ((link_params.entity_0_as_EDGE) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMateXdistance = true;
                    link_params.do_ChLinkMateOrthogonal = true;
                }
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_EDGE))
                {
                    link_params.do_ChLinkMateXdistance = true;
                    link_params.do_ChLinkMateOrthogonal = true;
                    link_params.swapAB_1 = true;
                }

                if (swMate.Alignment == (int)swMateAlign_e.swMateAlignANTI_ALIGNED)
                    link_params.do_parallel_flip = true;
            }

            if (swMateFeature.GetTypeName2() == "MateConcentric")
            {
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMatePointLine = true;
                    link_params.do_ChLinkMateParallel = true;
                }
                if ((link_params.entity_0_as_EDGE) &&
                    (link_params.entity_1_as_EDGE))
                {
                    link_params.do_ChLinkMatePointLine = true;
                    link_params.do_ChLinkMateParallel = true;
                }
                if ((link_params.entity_0_as_VERTEX) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMatePointLine = true;
                }
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_VERTEX))
                {
                    link_params.do_ChLinkMatePointLine = true;
                    link_params.swapAB_1 = true;
                }
                if ((link_params.entity_0_as_EDGE) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMatePointLine = true;
                    link_params.do_ChLinkMateParallel = true;
                }
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_EDGE))
                {
                    link_params.do_ChLinkMatePointLine = true;
                    link_params.do_ChLinkMateParallel = true;
                    link_params.swapAB_1 = true;
                }

                if (swMate.Alignment == (int)swMateAlign_e.swMateAlignANTI_ALIGNED)
                    link_params.do_parallel_flip = true;
            }

            if (swMateFeature.GetTypeName2() == "MateParallel")
            {
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMateParallel = true;
                }
                if ((link_params.entity_0_as_EDGE) &&
                    (link_params.entity_1_as_EDGE))
                {
                    link_params.do_ChLinkMateParallel = true;
                }
                if ((link_params.entity_0_as_EDGE) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMateOrthogonal = true;
                }
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_EDGE))
                {
                    link_params.do_ChLinkMateOrthogonal = true;
                    link_params.swapAB_1 = true;
                }

                if (swMate.Alignment == (int)swMateAlign_e.swMateAlignANTI_ALIGNED)
                    link_params.do_parallel_flip = true;
            }


            if (swMateFeature.GetTypeName2() == "MatePerpendicular")
            {
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMateOrthogonal = true;
                }
                if ((link_params.entity_0_as_EDGE) &&
                    (link_params.entity_1_as_EDGE))
                {
                    link_params.do_ChLinkMateOrthogonal = true;
                }
                if ((link_params.entity_0_as_EDGE) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMateParallel = true;
                }
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_EDGE))
                {
                    link_params.do_ChLinkMateParallel = true;
                    link_params.swapAB_1 = true;
                }

                if (swMate.Alignment == (int)swMateAlign_e.swMateAlignANTI_ALIGNED)
                    link_params.do_parallel_flip = true;
            }

            if (swMateFeature.GetTypeName2() == "MateDistanceDim")
            {
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMateXdistance = true;
                    link_params.do_ChLinkMateParallel = true;
                }
                if ((link_params.entity_0_as_VERTEX) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMateXdistance = true;
                }
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_VERTEX))
                {
                    link_params.do_ChLinkMateXdistance = true;
                    link_params.swapAB_1 = true;
                }

                //***TO DO*** cases of distance line-vs-line and line-vs-vertex and vert-vert.
                //           Those will require another .cpp ChLinkMate specialized class(es).

                // Get the imposed distance value, in SI units
                string confnames = "";
                link_params.do_distance_val = swMate.DisplayDimension.GetDimension2(0).IGetSystemValue3((int)swInConfigurationOpts_e.swThisConfiguration, 0, ref confnames);

                // Get aligment and flipped properties
                int alignment = swMate.Alignment;
                bool isflipped = swMate.Flipped;

                if (swMate.Alignment == (int)swMateAlign_e.swMateAlignALIGNED)
                {
                    if (isflipped)
                        link_params.do_distance_val = -link_params.do_distance_val;
                }
                else if (alignment == (int)swMateAlign_e.swMateAlignANTI_ALIGNED)
                {
                    link_params.do_parallel_flip = true;
                    if (!isflipped)
                        link_params.do_distance_val = -link_params.do_distance_val;
                }

            }

            if (swMateFeature.GetTypeName2() == "MateHinge")
            {
                // NOTE!!! The 'hinge' mate uses 4 references: fetch the two others remaining
                // and build another C::E link, for point-vs-face mating

                MateEntity2 swEntityC = swMate.MateEntity(2);
                MateEntity2 swEntityD = swMate.MateEntity(3);
                Component2 swCompC = swEntityC.ReferenceComponent;
                Component2 swCompD = swEntityD.ReferenceComponent;
                double[] paramsC = (double[])swEntityC.EntityParams;
                double[] paramsD = (double[])swEntityD.EntityParams;
                link_params.ref3 = (string)m_savedParts[swModelDocExt.GetPersistReference3(swCompC)];
                link_params.ref4 = (string)m_savedParts[swModelDocExt.GetPersistReference3(swCompD)];
                MathTransform trC = roottrasf;
                MathTransform trD = roottrasf;

                if (assemblyofmates != null)
                {
                    MathTransform partrasfC = assemblyofmates.GetTotalTransform(true);
                    if (partrasfC != null)
                        trC = partrasfC.IMultiply(invroottrasf);
                    MathTransform partrasfD = assemblyofmates.GetTotalTransform(true);
                    if (partrasfD != null)
                        trD = partrasfD.IMultiply(invroottrasf);
                }

                // NOTE: swMate.MateEntity(0).Reference.GetType() seems equivalent to  swMate.MateEntity(0).ReferenceType2  
                // but in some cases the latter fails. However, sometimes swMate.MateEntity(0).Reference.GetType() is null ReferenceType2 is ok,
                // so do the following trick:
                link_params.entity2_ref = swMate.MateEntity(2).Reference.GetType();
                if (link_params.entity2_ref == (int)swSelectType_e.swSelNOTHING)
                    link_params.entity2_ref = swMate.MateEntity(2).ReferenceType2;
                link_params.entity3_ref = swMate.MateEntity(3).Reference.GetType();
                if (link_params.entity3_ref == (int)swSelectType_e.swSelNOTHING)
                    link_params.entity3_ref = swMate.MateEntity(3).ReferenceType2;

                link_params.entity_2_as_VERTEX = (link_params.entity2_ref == (int)swSelectType_e.swSelVERTICES) ||
                                            (link_params.entity2_ref == (int)swSelectType_e.swSelSKETCHPOINTS) ||
                                            (link_params.entity2_ref == (int)swSelectType_e.swSelDATUMPOINTS);
                link_params.entity_3_as_VERTEX = (link_params.entity3_ref == (int)swSelectType_e.swSelVERTICES) ||
                                            (link_params.entity3_ref == (int)swSelectType_e.swSelSKETCHPOINTS) ||
                                            (link_params.entity3_ref == (int)swSelectType_e.swSelDATUMPOINTS);



                Point3D cCloc = new Point3D(paramsC[0], paramsC[1], paramsC[2]);
                link_params.cC = PointTransform(cCloc, ref trC);
                Point3D cDloc = new Point3D(paramsD[0], paramsD[1], paramsD[2]);
                link_params.cD = PointTransform(cDloc, ref trD);

                if (!link_params.entity_2_as_VERTEX)
                {
                    Vector3D dCloc = new Vector3D(paramsC[3], paramsC[4], paramsC[5]);
                    link_params.dC = DirTransform(dCloc, ref trC);
                }

                if (!link_params.entity_3_as_VERTEX)
                {
                    Vector3D dDloc = new Vector3D(paramsD[3], paramsD[4], paramsD[5]);
                    link_params.dD = DirTransform(dDloc, ref trD);
                }
            }

            return true;
        }

        protected static bool IsMateTypeExportable(string typeName2)
        {
            System.Collections.Generic.List<string> acceptedMates = new System.Collections.Generic.List<string>();

            // TODO: check which mates are *actually* exportable to Chrono
            acceptedMates.Add("MateCamTangent");
            acceptedMates.Add("MateCoincident");
            acceptedMates.Add("MateConcentric");
            acceptedMates.Add("MateDistanceDim");
            acceptedMates.Add("MateGearDim");
            acceptedMates.Add("MateHinge");
            acceptedMates.Add("MateInPlace");
            acceptedMates.Add("MateLinearCoupler");
            acceptedMates.Add("MateLock");
            acceptedMates.Add("MateParallel");
            acceptedMates.Add("MatePerpendicular");
            acceptedMates.Add("MatePlanarAngleDim");
            acceptedMates.Add("MateProfileCenter");
            acceptedMates.Add("MateRackPinionDim");
            acceptedMates.Add("MateScrew");
            acceptedMates.Add("MateSlot");
            acceptedMates.Add("MateSymmetric");
            acceptedMates.Add("MateTangent");
            acceptedMates.Add("MateUniversalJoint");
            acceptedMates.Add("MateWidth");

            return acceptedMates.Contains(typeName2);
        }


        #endregion


        // ============================================================================================================
        // Math functions
        // ============================================================================================================
        #region Math functions

        public double[] GetQuaternionFromMatrix(ref MathTransform trasf)
        {
            double[] q = new double[4];
            double[] amatr = (double[])trasf.ArrayData;
            double s, tr;
            // for speed reasons: ..
            double m00 = amatr[0];
            double m01 = amatr[3];
            double m02 = amatr[6];
            double m10 = amatr[1];
            double m11 = amatr[4];
            double m12 = amatr[7];
            double m20 = amatr[2];
            double m21 = amatr[5];
            double m22 = amatr[8];

            tr = m00 + m11 + m22;       // diag sum

            if (tr >= 0)
            {
                s = Math.Sqrt(tr + 1.0);
                q[0] = 0.5 * s;
                s = 0.5 / s;
                q[1] = (m21 - m12) * s;
                q[2] = (m02 - m20) * s;
                q[3] = (m10 - m01) * s;
            }
            else
            {
                int i = 0;

                if (m11 > m00)
                {
                    i = 1;
                    if (m22 > m11) i = 2;
                }
                else
                {
                    if (m22 > m00) i = 2;
                }

                switch (i)
                {
                    case 0:
                        s = Math.Sqrt(m00 - m11 - m22 + 1);
                        q[1] = 0.5 * s;
                        s = 0.5 / s;
                        q[2] = (m01 + m10) * s;
                        q[3] = (m20 + m02) * s;
                        q[0] = (m21 - m12) * s;
                        break;
                    case 1:
                        s = Math.Sqrt(m11 - m22 - m00 + 1);
                        q[2] = 0.5 * s;
                        s = 0.5 / s;
                        q[3] = (m12 + m21) * s;
                        q[1] = (m01 + m10) * s;
                        q[0] = (m02 - m20) * s;
                        break;
                    case 2:
                        s = Math.Sqrt(m22 - m00 - m11 + 1);
                        q[3] = 0.5 * s;
                        s = 0.5 / s;
                        q[1] = (m20 + m02) * s;
                        q[2] = (m12 + m21) * s;
                        q[0] = (m10 - m01) * s;
                        break;
                }
            }
            return q;
        }

        public static Matrix3D GetMatrixFromMathTransform(ref MathTransform trasf)
        {
            double[] amatr = (double[])trasf.ArrayData;

            Matrix3D res = new Matrix3D(amatr[0], amatr[1], amatr[2], 0,
                                        amatr[3], amatr[4], amatr[5], 0,
                                        amatr[6], amatr[7], amatr[8], 0,
                                        amatr[9], amatr[10], amatr[11], 1);
            return res;
        }

        public static Point3D PointTransform(Point3D pt, ref MathTransform trasf)
        {
            //Point3D pp = new Point3D(pt.X,pt.Y,pt.Z);
            Matrix3D M = GetMatrixFromMathTransform(ref trasf);
            return M.Transform(pt);
        }

        public static Vector3D DirTransform(Vector3D dir, ref MathTransform trasf)
        {
            Matrix3D M = GetMatrixFromMathTransform(ref trasf);
            M.OffsetX = 0; M.OffsetY = 0; M.OffsetZ = 0;
            return M.Transform(dir);
        }

        #endregion


    }
}
