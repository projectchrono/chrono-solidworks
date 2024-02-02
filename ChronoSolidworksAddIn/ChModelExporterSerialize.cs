using ChronoEngine_SwAddin;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using static ChronoGlobals;


#if HAS_CHRONO_CSHARP

namespace ChronoEngineAddin
{
    internal class ChModelExporterSerialize : ChModelExporter
    {
        ChSystemNSC chrono_system = new ChSystemNSC();
        private int num_link = 0;
        private int nbody = -1;
        Hashtable m_bodylist = new Hashtable(new myBytearrayHashComparer());

        string m_saveRelDirShapes;
        bool m_export_with_fullpath;
        double sphereswept_r = 0.0;

        // temporary variables
        ChBodyAuxRef newbody = new ChBodyAuxRef();
        ChBodyAuxRef body_ground = new ChBodyAuxRef();


        public ChModelExporterSerialize(ChronoEngine_SwAddin.SWIntegration swIntegration, string save_dir_shapes, string save_filename)
            : base(swIntegration, save_dir_shapes, save_filename)
        {

            m_saveRelDirShapes = System.IO.Path.GetFileNameWithoutExtension(m_saveFilename) + "_shapes";
        }

        public ChSystemNSC GetChronoSystem()
        {
            return chrono_system;
        }

        public void PrepareChronoSystem(bool export_with_fullpath = false)
        {

            m_export_with_fullpath = export_with_fullpath;
            CultureInfo bz = new CultureInfo("en-BZ");

            ModelDoc2 swModel;
            ConfigurationManager swConfMgr;
            Configuration swConf;
            Component2 swRootComp;

            m_savedParts.Clear();
            m_savedShapes.Clear();
            m_savedCollisionMeshes.Clear();

            swModel = (ModelDoc2)m_swIntegration.m_swApplication.ActiveDoc;
            if (swModel == null)
                return;
            swConfMgr = (ConfigurationManager)swModel.ConfigurationManager;
            swConf = (Configuration)swConfMgr.ActiveConfiguration;
            swRootComp = (Component2)swConf.GetRootComponent3(true);

            m_swIntegration.m_swApplication.GetUserProgressBar(out m_swIntegration.m_taskpaneHost.GetProgressBar());
            if (m_swIntegration.m_taskpaneHost.GetProgressBar() != null)
                m_swIntegration.m_taskpaneHost.GetProgressBar().Start(0, 5, "Serializing to JSON");

            num_comp = 0;

            chrono_system.SetCollisionSystemType(ChCollisionSystem.Type.BULLET);


            // Write preamble
            sphereswept_r = (double)m_swIntegration.m_taskpaneHost.GetNumericSphereSwept().Value;
            ChCollisionModel.SetDefaultSuggestedEnvelope((double)m_swIntegration.m_taskpaneHost.GetNumericEnvelope().Value * ChScale.L);
            ChCollisionModel.SetDefaultSuggestedMargin((double)m_swIntegration.m_taskpaneHost.GetNumericMargin().Value * ChScale.L);
            ChCollisionSystemBullet.SetContactBreakingThreshold((double)m_swIntegration.m_taskpaneHost.GetNumericContactBreaking().Value * ChScale.L);

            body_ground = new ChBodyAuxRef();
            body_ground.SetNameString("ground");
            body_ground.SetBodyFixed(true);
            chrono_system.Add(body_ground);


            // Set assembly to Resolved state
            int resolved = ((AssemblyDoc)swModel).ResolveAllLightWeightComponents(true);
            if (resolved != 0)
                MessageBox.Show("Attempt to Resolve assembly failed");


            if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
            {
                // Write down all parts
                TraverseComponentForBodies(swRootComp, 1, nbody);

                // Write down all constraints
                MathTransform rootTransform = swRootComp.GetTotalTransform(true);
                if (rootTransform == null)
                {
                    IMathUtility swMath = (IMathUtility)this.m_swIntegration.m_swApplication.GetMathUtility();
                    double[] nulltr = new double[] { 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 };
                    rootTransform = (MathTransform)swMath.CreateTransform(nulltr);
                }

                Feature swFeat = (Feature)swModel.FirstFeature();
                TraverseFeaturesForLinks(swFeat, 1, ref rootTransform, ref swRootComp);

                TraverseComponentForLinks(swRootComp, 1, ref rootTransform);

                // Write down all markers in assembly (that are not in sub parts, so they belong to 'ground' object)
                //nbody = 0; // RESET TO body_0 (ground)
                newbody = body_ground;
                swFeat = (Feature)swModel.FirstFeature();
                TraverseFeaturesForMarkers(swFeat, 1, nbody, rootTransform);
            }


            if (m_swIntegration.m_taskpaneHost.GetProgressBar() != null)
                m_swIntegration.m_taskpaneHost.GetProgressBar().End();
        }


        // ============================================================================================================
        // Override base class methods
        // ============================================================================================================
        public override void Export()
        {

            PrepareChronoSystem(false);

            chrono_system.SerializeToJSON(m_saveFilename);

            System.Windows.Forms.MessageBox.Show("Export to JSON completed.");


        }

        public ChBodyFrame GetChBodyFrameFromEntity(MateEntity2 swEntity)
        {
            ChBodyAuxRef body_auxref;

            Component2 swComp = swEntity.ReferenceComponent;
            ModelDoc2 swModel = (ModelDoc2)m_swIntegration.m_swApplication.ActiveDoc;
            ModelDocExtension swModelDocExt = swModel.Extension;


            //if (!m_bodylist.TryGetValue(swModelDocExt.GetPersistReference3(swComp), out body_auxref))
            //{
            //    body_auxref = body_ground;
            //}

            body_auxref = m_bodylist[swModelDocExt.GetPersistReference3(swComp)];

            if (body_auxref == null)
            {
                body_auxref = body_ground;
            }

            // ChBodyAuxRef inherits from ChBody, ChBody inherits from ChBodyFrame but is its second inheritance
            // since C# have limited support for multiple inheritance is simply takes only the first inheritance
            // here we need to operate on the underlying shared_ptrs and make the cast by ourselves
            ChBodyFrame body_bframe = chrono.CastToChBodyFrame(body_auxref);

            return body_bframe;

        }



        public override bool ConvertMate(in Feature swMateFeature, in MathTransform roottrasf, in Component2 assemblyofmates)
        {

            LinkParams link_params;
            GetLinkParameters(swMateFeature, out link_params, roottrasf, assemblyofmates);

            //// 
            //// WRITE C# CODE CORRESPONDING TO CONSTRAINTS
            ////
            CultureInfo bz = new CultureInfo("en-BZ");


            // m_savedParts it's a map of [persistentreference, bodynames] since 'bodynames' is needed by CPP and Python export
            // however here we need to have proper Chrono bodies, retrieving them from m_bodylist that is a map of [persistentreference, ChBodyAuxRef]
            ChBodyFrame body1, body2, body3, body4;

            {

                if (swMateFeature == null)
                    return false;

                object foo = null;
                bool[] suppressedflags = (bool[])swMateFeature.IsSuppressed2((int)swInConfigurationOpts_e.swThisConfiguration, foo);

                if (suppressedflags[0] == true)
                    return false;

                Mate2 swMate = (Mate2)swMateFeature.GetSpecificFeature2();

                if (swMate.GetMateEntityCount() < 2)
                    return false;

                // Get the mated parts
                body1 = GetChBodyFrameFromEntity(swMate.MateEntity(0));
                body2 = GetChBodyFrameFromEntity(swMate.MateEntity(1));




                if (swMateFeature.GetTypeName2() == "MateHinge")
                {
                    body3 = GetChBodyFrameFromEntity(swMate.MateEntity(2));
                    body4 = GetChBodyFrameFromEntity(swMate.MateEntity(3));
                }
                else
                {
                    body3 = null;
                    body4 = null;
                }
            }


            if (link_params.do_ChLinkMateXdistance)
            {
                num_link++;
                String linkname = "link_" + num_link;

                ChLinkMateXdistance newlink = new ChLinkMateXdistance();
                ChVectorD cA = new ChVectorD(link_params.cA.X * ChScale.L, link_params.cA.Y * ChScale.L, link_params.cA.Z * ChScale.L);
                ChVectorD cB = new ChVectorD(link_params.cB.X * ChScale.L, link_params.cB.Y * ChScale.L, link_params.cB.Z * ChScale.L);

                ChVectorD dA = new ChVectorD();
                if (!link_params.entity_0_as_VERTEX)
                    dA = new ChVectorD(link_params.dA.X, link_params.dA.Y, link_params.dA.Z);

                ChVectorD dB = new ChVectorD();
                if (!link_params.entity_1_as_VERTEX)
                    dB = new ChVectorD(link_params.dB.X, link_params.dB.Y, link_params.dB.Z);


                // Initialize link, by setting the two csys, in absolute space
                if (!link_params.swapAB_1)
                    newlink.Initialize(body1, body2, false, cA, cB, dB);
                else
                    newlink.Initialize(body2, body1, false, cB, cA, dA);

                //if (link_params.do_distance_val!=0)
                newlink.SetDistance(link_params.do_distance_val * ChScale.L * -1);

                newlink.SetNameString(swMateFeature.Name);

                chrono_system.Add(newlink);
            }

            if (link_params.do_ChLinkMateParallel)
            {
                if (Math.Abs(Vector3D.DotProduct(link_params.dA, link_params.dB)) > 0.98)
                {
                    num_link++;
                    String linkname = "link_" + num_link;
                    ChLinkMateParallel newlink = new ChLinkMateParallel();

                    ChVectorD cA = new ChVectorD(
                              link_params.cA.X * ChScale.L,
                              link_params.cA.Y * ChScale.L,
                              link_params.cA.Z * ChScale.L);
                    ChVectorD dA = new ChVectorD(
                              link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                    ChVectorD cB = new ChVectorD(
                              link_params.cB.X * ChScale.L,
                              link_params.cB.Y * ChScale.L,
                              link_params.cB.Z * ChScale.L);
                    ChVectorD dB = new ChVectorD(
                              link_params.dB.X, link_params.dB.Y, link_params.dB.Z);

                    if (link_params.do_parallel_flip)
                        newlink.SetFlipped(true);

                    // Initialize link, by setting the two csys, in absolute space,
                    if (!link_params.swapAB_1)
                        newlink.Initialize(body1, body2, false, cA, cB, dA, dB);
                    else
                        newlink.Initialize(body2, body1, false, cB, cA, dB, dA);

                    newlink.SetNameString(swMateFeature.Name);


                    chrono_system.Add(newlink);

                }
                else
                {
                    //m_asciiText += "\n# ChLinkMateParallel skipped because directions not parallel! \n";
                }
            }

            if (link_params.do_ChLinkMateOrthogonal)
            {
                if (Math.Abs(Vector3D.DotProduct(link_params.dA, link_params.dB)) < 0.02)
                {
                    num_link++;
                    String linkname = "link_" + num_link;
                    ChLinkMateOrthogonal newlink = new ChLinkMateOrthogonal();

                    ChVectorD cA = new ChVectorD(
                              link_params.cA.X * ChScale.L,
                              link_params.cA.Y * ChScale.L,
                              link_params.cA.Z * ChScale.L);
                    ChVectorD dA = new ChVectorD(
                              link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                    ChVectorD cB = new ChVectorD(
                              link_params.cB.X * ChScale.L,
                              link_params.cB.Y * ChScale.L,
                              link_params.cB.Z * ChScale.L);
                    ChVectorD dB = new ChVectorD(
                              link_params.dB.X, link_params.dB.Y, link_params.dB.Z);

                    // Initialize link, by setting the two csys, in absolute space,
                    if (!link_params.swapAB_1)
                        newlink.Initialize(body1, body2, false, cA, cB, dA, dB);
                    else
                        newlink.Initialize(body2, body1, false, cB, cA, dB, dA);

                    newlink.SetNameString(swMateFeature.Name);

                    chrono_system.Add(newlink);


                }
                else
                {
                    //m_asciiText += "\n# ChLinkMateOrthogonal skipped because directions not orthogonal! \n";
                }
            }

            if (link_params.do_ChLinkMateSpherical)
            {
                num_link++;
                String linkname = "link_" + num_link;
                ChLinkMateSpherical newlink = new ChLinkMateSpherical();

                ChVectorD cA = new ChVectorD(
                          link_params.cA.X * ChScale.L,
                          link_params.cA.Y * ChScale.L,
                          link_params.cA.Z * ChScale.L);
                ChVectorD cB = new ChVectorD(
                          link_params.cB.X * ChScale.L,
                          link_params.cB.Y * ChScale.L,
                          link_params.cB.Z * ChScale.L);

                // Initialize link, by setting the two csys, in absolute space,
                if (!link_params.swapAB_1)
                    newlink.Initialize(body1, body2, false, cA, cB);
                else
                    newlink.Initialize(body2, body1, false, cB, cA);

                newlink.SetNameString(swMateFeature.Name);

                chrono_system.Add(newlink);

            }

            if (link_params.do_ChLinkMatePointLine)
            {
                num_link++;
                String linkname = "link_" + num_link;
                ChLinkMateGeneric newlink = new ChLinkMateGeneric();
                newlink.SetConstrainedCoords(false, true, true, false, false, false);

                ChVectorD cA = new ChVectorD(
                          link_params.cA.X * ChScale.L,
                          link_params.cA.Y * ChScale.L,
                          link_params.cA.Z * ChScale.L);
                ChVectorD cB = new ChVectorD(
                          link_params.cB.X * ChScale.L,
                          link_params.cB.Y * ChScale.L,
                          link_params.cB.Z * ChScale.L);

                ChVectorD dA;
                if (!link_params.entity_0_as_VERTEX)
                    dA = new ChVectorD(link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                else
                    dA = new ChVectorD(0, 0, 0);

                ChVectorD dB;
                if (!link_params.entity_1_as_VERTEX)
                    dB = new ChVectorD(link_params.dB.X, link_params.dB.Y, link_params.dB.Z);
                else
                    dB = new ChVectorD(0, 0, 0);

                // Initialize link, by setting the two csys, in absolute space,
                if (!link_params.swapAB_1)
                    newlink.Initialize(body1, body2, false, cA, cB, dA, dB);
                else
                    newlink.Initialize(body2, body1, false, cB, cA, dB, dA);

                newlink.SetNameString(swMateFeature.Name);

                chrono_system.Add(newlink);


            }



            // Now, do some other special mate type that did not fall in combinations
            // of link_params.do_ChLinkMatePointLine, link_params.do_ChLinkMateSpherical, etc etc

            if (swMateFeature.GetTypeName2() == "MateHinge")
            {
                // auto flip direction if anti aligned (seems that this is assumed automatically in MateHinge in SW)
                if (Vector3D.DotProduct(link_params.dA, link_params.dB) < 0)
                    link_params.dB.Negate();

                {

                    // Hinge constraint must be splitted in two C::E constraints: a coaxial and a point-vs-plane
                    num_link++;
                    String linkname = "link_" + num_link;
                    ChLinkMateCoaxial newlink = new ChLinkMateCoaxial();

                    ChVectorD cA = new ChVectorD(
                              link_params.cA.X * ChScale.L,
                              link_params.cA.Y * ChScale.L,
                              link_params.cA.Z * ChScale.L);
                    ChVectorD dA = new ChVectorD(
                              link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                    ChVectorD cB = new ChVectorD(
                              link_params.cB.X * ChScale.L,
                              link_params.cB.Y * ChScale.L,
                              link_params.cB.Z * ChScale.L);
                    ChVectorD dB = new ChVectorD(
                              link_params.dB.X, link_params.dB.Y, link_params.dB.Z);

                    newlink.SetNameString(swMateFeature.Name);


                    // Initialize link, by setting the two csys, in absolute space,
                    newlink.Initialize(body1, body2, false, cA, cB, dA, dB);

                    chrono_system.Add(newlink);

                }

                {
                    num_link++;
                    String linkname = "link_" + num_link;
                    ChLinkMateXdistance newlink = new ChLinkMateXdistance();

                    ChVectorD cA = new ChVectorD(
                              link_params.cC.X * ChScale.L,
                              link_params.cC.Y * ChScale.L,
                              link_params.cC.Z * ChScale.L);
                    ChVectorD dA = new ChVectorD(
                              link_params.dC.X, link_params.dC.Y, link_params.dC.Z);
                    ChVectorD cB = new ChVectorD(
                              link_params.cD.X * ChScale.L,
                              link_params.cD.Y * ChScale.L,
                              link_params.cD.Z * ChScale.L);
                    ChVectorD dB = new ChVectorD(
                              link_params.dD.X, link_params.dD.Y, link_params.dD.Z);

                    newlink.SetNameString(swMateFeature.Name);


                    // Initialize link, by setting the two csys, in absolute space,
                    if (link_params.entity_2_as_VERTEX)
                        newlink.Initialize(body3, body4, false, cA, cB, dA);
                    else
                        newlink.Initialize(body3, body4, false, cA, cB, dB);

                    chrono_system.Add(newlink);

                }
            }


            return true;
        }

        public override void TraverseComponentForVisualShapes(Component2 swComp, long nLevel, int nbody, ref int nvisshape, Component2 chbody_comp)
        {
            CultureInfo bz = new CultureInfo("en-BZ");
            object[] bodies;
            object bodyInfo;
            bodies = (object[])swComp.GetBodies3((int)swBodyType_e.swAllBodies, out bodyInfo);

            if (bodies != null)
                if (bodies.Length > 0)
                {
                    // Export the component shape to a .OBJ file representing its SW body(s)
                    nvisshape += 1;
                    string shapename = "body_" + nbody + "_" + nvisshape;
                    string obj_filename_full = m_saveDirShapes.Replace("\\", "/") + "/" + shapename + ".obj";
                    string obj_filename_rel = m_saveRelDirShapes.Replace("\\", "/") + "/" + shapename + ".obj";

                    ChVisualShape visshape;

                    ModelDoc2 swCompModel = (ModelDoc2)swComp.GetModelDoc();
                    if (!m_savedShapes.ContainsKey(swCompModel.GetPathName()))
                    {
                        try
                        {
                            FileStream ostream = new FileStream(obj_filename_full, FileMode.Create, FileAccess.ReadWrite);
                            StreamWriter writer = new StreamWriter(ostream); //, new UnicodeEncoding());
                            string asciiobj = "";
                            if (m_swIntegration.m_taskpaneHost.GetProgressBar() != null)
                                m_swIntegration.m_taskpaneHost.GetProgressBar().UpdateTitle("Exporting " + swComp.Name2 + " (tesselate) ...");
                            // Write the OBJ converted visualization shapes:
                            TesselateToObj.Convert(swComp, ref asciiobj, m_swIntegration.m_taskpaneHost.GetCheckboxSaveUV().Checked, ref m_swIntegration.m_taskpaneHost.GetProgressBar(), true, false);
                            writer.Write(asciiobj);
                            writer.Flush();
                            ostream.Close();

                            m_savedShapes.Add(swCompModel.GetPathName(), shapename);
                        }
                        catch (Exception)
                        {
                            System.Windows.Forms.MessageBox.Show("Cannot write to file: " + obj_filename_full + "\n for component: " + swComp.Name2 + " for path name: " + swCompModel.GetPathName());
                        }
                    }
                    else
                    {
                        // reuse the already-saved shape name
                        shapename = (string)m_savedShapes[swCompModel.GetPathName()];
                        obj_filename_rel = m_saveRelDirShapes.Replace("\\", "/") + "/" + shapename + ".obj";
                    }

                    visshape = new ChVisualShapeModelFile();
                    if (m_export_with_fullpath)
                    {
                        ((ChVisualShapeModelFile)visshape).SetFilename(obj_filename_full);
                    }
                    else
                    {
                        ((ChVisualShapeModelFile)visshape).SetFilename(obj_filename_rel);
                    }


                    object foo = null;
                    double[] vMatProperties = (double[])swComp.GetMaterialPropertyValues2((int)swInConfigurationOpts_e.swThisConfiguration, foo);

                    if (vMatProperties != null)
                        if (vMatProperties[0] != -1)
                        {
                            visshape.SetColor(new ChColor((float)vMatProperties[0], (float)vMatProperties[1], (float)vMatProperties[2]));
                            visshape.SetOpacity((float)(1.0 - vMatProperties[7]));
                        }

                    MathTransform absframe_chbody = chbody_comp.GetTotalTransform(true);
                    MathTransform absframe_shape = swComp.GetTotalTransform(true);
                    MathTransform absframe_chbody_inv = absframe_chbody.IInverse();
                    MathTransform relframe_shape = absframe_shape.IMultiply(absframe_chbody_inv);  // row-ordered transf. -> reverse mult.order!
                    double[] amatr = (double[])relframe_shape.ArrayData;
                    double[] quat = GetQuaternionFromMatrix(ref relframe_shape);

                    newbody.AddVisualShape(visshape,
                        new ChFrameD(new ChVectorD(amatr[9] * ChScale.L, amatr[10] * ChScale.L, amatr[11] * ChScale.L),
                        new ChQuaternionD(quat[0], quat[1], quat[2], quat[3])));
                }



            // Recursive scan of subcomponents

            Component2 swChildComp;
            object[] vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                if (swChildComp.Visible == (int)swComponentVisibilityState_e.swComponentVisible)
                    TraverseComponentForVisualShapes(swChildComp, nLevel + 1, nbody, ref nvisshape, chbody_comp);
            }
        }

        public override void TraverseFeaturesForCollisionShapes(Component2 swComp, long nLevel, int nbody, ref MathTransform chbodytransform, ref bool found_collisionshapes, Component2 swCompBase, ref int ncollshape)
        {
            CultureInfo bz = new CultureInfo("en-BZ");
            Feature swFeat;
            swFeat = (Feature)swComp.FirstFeature();

            String bodyname = "body_" + nbody;
            String matname = "mat_" + nbody;

            MathTransform subcomp_transform = swComp.GetTotalTransform(true);
            MathTransform invchbody_trasform = (MathTransform)chbodytransform.Inverse();
            MathTransform collshape_subcomp_transform = subcomp_transform.IMultiply(invchbody_trasform); // row-ordered transf. -> reverse mult.order!

            // Export collision shapes
            if (m_swIntegration.m_taskpaneHost.GetCheckboxCollisionShapes().Checked)
            {
                object[] bodies;
                object bodyInfo;
                bodies = (object[])swComp.GetBodies3((int)swBodyType_e.swAllBodies, out bodyInfo);

                if (bodies != null)
                {
                    // see if it contains some collision shape
                    bool build_collision_model = false;
                    for (int ib = 0; ib < bodies.Length; ib++)
                    {
                        Body2 swBody = (Body2)bodies[ib];
                        if (swBody.Name.StartsWith("COLL.") || swBody.Name.StartsWith("COLLMESH"))
                            build_collision_model = true;
                    }
                    ChMaterialSurfaceNSC collision_material = new ChMaterialSurfaceNSC();

                    if (build_collision_model)
                    {

                        if (!found_collisionshapes)
                        {
                            found_collisionshapes = true;
                            newbody.AddCollisionModel(new ChCollisionModel());

                            // fetch SW attribute with Chrono parameters
                            SolidWorks.Interop.sldworks.Attribute myattr = (SolidWorks.Interop.sldworks.Attribute)swCompBase.FindAttribute(m_swIntegration.defattr_chbody, 0);

                            if (myattr != null)
                            {

                                double param_friction = ((Parameter)myattr.GetParameter("friction")).GetDoubleValue();
                                double param_restitution = ((Parameter)myattr.GetParameter("restitution")).GetDoubleValue();
                                double param_rolling_friction = ((Parameter)myattr.GetParameter("rolling_friction")).GetDoubleValue();
                                double param_spinning_friction = ((Parameter)myattr.GetParameter("spinning_friction")).GetDoubleValue();
                                double param_collision_envelope = ((Parameter)myattr.GetParameter("collision_envelope")).GetDoubleValue();
                                double param_collision_margin = ((Parameter)myattr.GetParameter("collision_margin")).GetDoubleValue();
                                int param_collision_family = (int)((Parameter)myattr.GetParameter("collision_family")).GetDoubleValue();

                                collision_material.SetFriction((float)param_friction);
                                if (param_restitution != 0)
                                    collision_material.SetRestitution((float)param_restitution);
                                if (param_rolling_friction != 0)
                                    collision_material.SetRollingFriction((float)param_rolling_friction);
                                if (param_spinning_friction != 0)
                                    collision_material.SetSpinningFriction((float)param_spinning_friction);
                                //if (param_collision_envelope != 0.03)
                                newbody.GetCollisionModel().SetEnvelope((float)(param_collision_envelope * ChScale.L));
                                //if (param_collision_margin != 0.01)
                                newbody.GetCollisionModel().SetSafeMargin((float)(param_collision_margin * ChScale.L));
                                if (param_collision_family != 0)
                                    newbody.GetCollisionModel().SetFamily(param_collision_family);
                            }

                            //// clear model only at 1st subcomponent where coll shapes are found in features:
                            //newbody.GetCollisionModel().Clear();
                        }

                        bool has_coll_mesh = false;

                        for (int ib = 0; ib < bodies.Length; ib++)
                        {
                            Body2 swBody = (Body2)bodies[ib];

                            if (swBody.Name.StartsWith("COLLMESH"))
                            {
                                has_coll_mesh = true;
                            }

                            if (swBody.Name.StartsWith("COLL."))
                            {
                                bool rbody_converted = false;
                                if (ConvertToCollisionShapes.SWbodyToSphere(swBody))
                                {
                                    Point3D center_l = new Point3D(); // in local subcomponent
                                    double rad = 0;
                                    ConvertToCollisionShapes.SWbodyToSphere(swBody, ref rad, ref center_l);
                                    Point3D center = PointTransform(center_l, ref collshape_subcomp_transform);
                                    ChMatrix33D mr = new ChMatrix33D();
                                    mr.setitem(0, 0, 1.0); mr.setitem(1, 0, 0.0); mr.setitem(2, 0, 0.0);
                                    mr.setitem(0, 1, 0.0); mr.setitem(1, 1, 1.0); mr.setitem(2, 1, 0.0);
                                    mr.setitem(0, 2, 0.0); mr.setitem(1, 2, 0.0); mr.setitem(2, 2, 1.0);
                                    ChCollisionShapeSphere collshape = new ChCollisionShapeSphere(collision_material, rad * ChScale.L);
                                    newbody.GetCollisionModel().AddShape(
                                        collshape,
                                        new ChFrameD(
                                            new ChVectorD(center.X * ChScale.L,
                                                          center.Y * ChScale.L,
                                                          center.Z * ChScale.L),
                                            mr)
                                        );
                                    rbody_converted = true;
                                }
                                if (ConvertToCollisionShapes.SWbodyToBox(swBody))
                                {
                                    Point3D vC_l = new Point3D();
                                    Vector3D eX_l = new Vector3D(); Vector3D eY_l = new Vector3D(); Vector3D eZ_l = new Vector3D();
                                    ConvertToCollisionShapes.SWbodyToBox(swBody, ref vC_l, ref eX_l, ref eY_l, ref eZ_l);
                                    Point3D vC = PointTransform(vC_l, ref collshape_subcomp_transform);
                                    Vector3D eX = DirTransform(eX_l, ref collshape_subcomp_transform);
                                    Vector3D eY = DirTransform(eY_l, ref collshape_subcomp_transform);
                                    Vector3D eZ = DirTransform(eZ_l, ref collshape_subcomp_transform);
                                    Point3D vO = vC + 0.5 * eX + 0.5 * eY + 0.5 * eZ;
                                    Vector3D Dx = eX; Dx.Normalize();
                                    Vector3D Dy = eY; Dy.Normalize();
                                    Vector3D Dz = Vector3D.CrossProduct(Dx, Dy);
                                    ChMatrix33D mr = new ChMatrix33D();
                                    mr.setitem(0, 0, Dx.X); mr.setitem(1, 0, Dx.Y); mr.setitem(2, 0, Dx.Z);
                                    mr.setitem(0, 1, Dy.X); mr.setitem(1, 1, Dy.Y); mr.setitem(2, 1, Dy.Z);
                                    mr.setitem(0, 2, Dz.X); mr.setitem(1, 2, Dz.Y); mr.setitem(2, 2, Dz.Z);
                                    ChCollisionShapeBox collshape = new ChCollisionShapeBox(collision_material, eX.Length * ChScale.L, eY.Length * ChScale.L, eZ.Length * ChScale.L);
                                    newbody.GetCollisionModel().AddShape(
                                        collshape,
                                        new ChFrameD(
                                            new ChVectorD(vO.X * ChScale.L, vO.Y * ChScale.L, vO.Z * ChScale.L),
                                            mr)
                                        );

                                    rbody_converted = true;
                                }
                                if (ConvertToCollisionShapes.SWbodyToCylinder(swBody))
                                {
                                    Point3D p1_l = new Point3D();
                                    Point3D p2_l = new Point3D();
                                    double rad = 0;
                                    ConvertToCollisionShapes.SWbodyToCylinder(swBody, ref p1_l, ref p2_l, ref rad);
                                    Point3D p1 = PointTransform(p1_l, ref collshape_subcomp_transform);
                                    Point3D p2 = PointTransform(p2_l, ref collshape_subcomp_transform);
                                    ChVectorD ch_p1 = new ChVectorD(p1.X * ChScale.L, p1.Y * ChScale.L, p1.Z * ChScale.L);
                                    ChVectorD ch_p2 = new ChVectorD(p2.X * ChScale.L, p2.Y * ChScale.L, p2.Z * ChScale.L);
                                    newbody.GetCollisionModel().AddCylinder(collision_material, rad * ChScale.L, ch_p1, ch_p2);
                                    rbody_converted = true;
                                }

                                if (ConvertToCollisionShapes.SWbodyToConvexHull(swBody, 30) && !rbody_converted)
                                {
                                    Point3D[] vertexes = new Point3D[1]; // will be resized by SWbodyToConvexHull
                                    ConvertToCollisionShapes.SWbodyToConvexHull(swBody, ref vertexes, 30);
                                    if (vertexes.Length > 0)
                                    {
                                        vector_ChVectorD pt_vect = new vector_ChVectorD();
                                        for (int iv = 0; iv < vertexes.Length; iv++)
                                        {
                                            Point3D vert_l = vertexes[iv];
                                            Point3D vert = PointTransform(vert_l, ref collshape_subcomp_transform);
                                            pt_vect.Add(new ChVectorD(
                                                vert.X * ChScale.L,
                                                vert.Y * ChScale.L,
                                                vert.Z * ChScale.L));
                                        }
                                        ChCollisionShapeConvexHull collshape = new ChCollisionShapeConvexHull(collision_material, pt_vect);
                                        newbody.GetCollisionModel().AddShape(collshape);
                                    }
                                    rbody_converted = true;
                                }


                            } // end dealing with a collision shape

                        } // end solid bodies traversal for converting to coll.shapes



                        if (has_coll_mesh)
                        {
                            // fallback if no primitive collision shape found: use concave trimesh collision model (although inefficient)
                            ncollshape += 1;
                            string shapename = "body_" + nbody + "_" + ncollshape + "_collision";
                            string obj_filename_full = m_saveDirShapes.Replace("\\", "/") + "/" + shapename + ".obj";
                            string obj_filename_rel = m_saveRelDirShapes.Replace("\\", "/") + "/" + shapename + ".obj";

                            ModelDoc2 swCompModel = (ModelDoc2)swComp.GetModelDoc();
                            if (!m_savedCollisionMeshes.ContainsKey(swCompModel.GetPathName()))
                            {
                                try
                                {
                                    FileStream ostream = new FileStream(obj_filename_full, FileMode.Create, FileAccess.ReadWrite);
                                    StreamWriter writer = new StreamWriter(ostream); //, new UnicodeEncoding());
                                    string asciiobj = "";
                                    if (m_swIntegration.m_taskpaneHost.GetProgressBar() != null)
                                        m_swIntegration.m_taskpaneHost.GetProgressBar().UpdateTitle("Exporting collision shape" + swComp.Name2 + " (tesselate) ...");
                                    // Write the OBJ converted visualization shapes:
                                    TesselateToObj.Convert(swComp, ref asciiobj, m_swIntegration.m_taskpaneHost.GetCheckboxSaveUV().Checked, ref m_swIntegration.m_taskpaneHost.GetProgressBar(), false, true);
                                    writer.Write(asciiobj);
                                    writer.Flush();
                                    ostream.Close();

                                    m_savedCollisionMeshes.Add(swCompModel.GetPathName(), shapename);
                                }
                                catch (Exception)
                                {
                                    System.Windows.Forms.MessageBox.Show("Cannot write to file: " + obj_filename_full + "\n for component: " + swComp.Name2 + " for path name: " + swCompModel.GetPathName());
                                }
                            }
                            else
                            {
                                // reuse the already-saved shape name
                                shapename = (String)m_savedCollisionMeshes[swCompModel.GetPathName()];
                                obj_filename_full = m_saveDirShapes.Replace("\\", "/") + "/" + shapename + ".obj";
                                obj_filename_rel = m_saveRelDirShapes.Replace("\\", "/") + "/" + shapename + ".obj";
                            }

                            double[] amatr = (double[])collshape_subcomp_transform.ArrayData;
                            //double[] quat = GetQuaternionFromMatrix(ref collshape_subcomp_transform);

                            {
                                ChTriangleMeshConnected newmesh = new ChTriangleMeshConnected();
                                newmesh.LoadWavefrontMesh(obj_filename_full, false, true);
                                //newmesh._SetFilename(obj_filename_rel); // TWEAK: this function is added just for convenience in the SWIG wrapper, it doesn't exist in pure Chrono
                                ChMatrix33D mr = new ChMatrix33D();
                                mr.setitem(0, 0, amatr[0] * ChScale.L); mr.setitem(1, 0, amatr[1] * ChScale.L); mr.setitem(2, 0, amatr[2] * ChScale.L);
                                mr.setitem(0, 1, amatr[3] * ChScale.L); mr.setitem(1, 1, amatr[4] * ChScale.L); mr.setitem(2, 1, amatr[5] * ChScale.L);
                                mr.setitem(0, 2, amatr[6] * ChScale.L); mr.setitem(1, 2, amatr[7] * ChScale.L); mr.setitem(2, 2, amatr[8] * ChScale.L);
                                newmesh.Transform(new ChVectorD(amatr[9] * ChScale.L, amatr[10] * ChScale.L, amatr[11] * ChScale.L), mr);
                                ChCollisionShapeTriangleMesh collshape = new ChCollisionShapeTriangleMesh(collision_material, newmesh, false, false, sphereswept_r);
                                newbody.GetCollisionModel().AddShape(collshape);
                            }
                            //rbody_converted = true;
                        }


                    } // end if build_collision_model
                }

            } // end collision shapes export

        }

        public override void TraverseComponentForBodies(Component2 swComp, long nLevel, int nbody)
        {
            CultureInfo bz = new CultureInfo("en-BZ");
            object[] vmyChildComp = (object[])swComp.GetChildren();


            if (nLevel > 1)
            {
                if (nbody == -1)
                {
                    if (!swComp.IsSuppressed()) // skip body if marked as 'suppressed'
                    {
                        if ((swComp.Solving == (int)swComponentSolvingOption_e.swComponentRigidSolving) || (vmyChildComp.Length == 0))
                        {
                            // OK! this is a 'leaf' of the tree of ChBody equivalents (a SDW subassebly or part)
                            num_comp++;

                            nbody = num_comp;  // mark the rest of recursion as 'n-th body found'

                            if (m_swIntegration.m_taskpaneHost.GetProgressBar() != null)
                            {
                                m_swIntegration.m_taskpaneHost.GetProgressBar().UpdateTitle("Exporting " + swComp.Name2 + " ...");
                                m_swIntegration.m_taskpaneHost.GetProgressBar().UpdateProgress(num_comp % 5);
                            }

                            // fetch SW attribute with Chrono parameters
                            SolidWorks.Interop.sldworks.Attribute myattr = (SolidWorks.Interop.sldworks.Attribute)swComp.FindAttribute(m_swIntegration.defattr_chbody, 0);

                            MathTransform chbodytransform = swComp.GetTotalTransform(true);
                            double[] amatr;
                            amatr = (double[])chbodytransform.ArrayData;
                            string bodyname = "body_" + num_comp;

                            // RESET body
                            newbody = new ChBodyAuxRef();


                            // Write name
                            newbody.SetName(swComp.Name2);
                            newbody.SetPos(new ChVectorD(amatr[9] * ChScale.L, amatr[10] * ChScale.L, amatr[11] * ChScale.L));
                            double[] quat = GetQuaternionFromMatrix(ref chbodytransform);
                            newbody.SetRot(new ChQuaternionD(quat[0], quat[1], quat[2], quat[3]));

                            int nvalid_bodies = 0;
                            TraverseComponent_for_countingmassbodies(swComp, ref nvalid_bodies);

                            int addedb = 0;
                            object[] bodies_nocollshapes = new object[nvalid_bodies];
                            TraverseComponent_for_massbodies(swComp, ref bodies_nocollshapes, ref addedb);

                            MassProperty swMass = swComp.IGetModelDoc().Extension.CreateMassProperty();
                            bool boolstatus = false;
                            boolstatus = swMass.AddBodies((object[])bodies_nocollshapes);
                            swMass.SetCoordinateSystem(chbodytransform);
                            swMass.UseSystemUnits = true;
                            //note: do not set here the COG-to-REF position because here SW express it in absolute coords
                            // double cogX = ((double[])swMass.CenterOfMass)[0];
                            // double cogY = ((double[])swMass.CenterOfMass)[1];
                            // double cogZ = ((double[])swMass.CenterOfMass)[2];
                            double mass = swMass.Mass;
                            double[] Itensor = (double[])swMass.GetMomentOfInertia((int)swMassPropertyMoment_e.swMassPropertyMomentAboutCenterOfMass);
                            double Ixx = Itensor[0];
                            double Iyy = Itensor[4];
                            double Izz = Itensor[8];
                            double Ixy = Itensor[1];
                            double Izx = Itensor[2];
                            double Iyz = Itensor[5];

                            MassProperty swMassb;
                            swMassb = (MassProperty)swComp.IGetModelDoc().Extension.CreateMassProperty();
                            bool boolstatusb = false;
                            boolstatusb = swMassb.AddBodies(bodies_nocollshapes);
                            swMassb.UseSystemUnits = true;
                            double cogXb = ((double[])swMassb.CenterOfMass)[0];
                            double cogYb = ((double[])swMassb.CenterOfMass)[1];
                            double cogZb = ((double[])swMassb.CenterOfMass)[2];

                            newbody.SetMass(mass * ChScale.M);

                            // Write inertia tensor 
                            newbody.SetInertiaXX(new ChVectorD(Ixx * ChScale.M * ChScale.L * ChScale.L,
                                                               Iyy * ChScale.M * ChScale.L * ChScale.L,
                                                               Izz * ChScale.M * ChScale.L * ChScale.L));
                            // Note: C::E assumes that's up to you to put a 'minus' sign in values of Ixy, Iyz, Izx
                            newbody.SetInertiaXY(new ChVectorD(-Ixy * ChScale.M * ChScale.L * ChScale.L,
                                                               -Izx * ChScale.M * ChScale.L * ChScale.L,
                                                               -Iyz * ChScale.M * ChScale.L * ChScale.L));

                            // Write the position of the COG respect to the REF
                            newbody.SetFrame_COG_to_REF(
                                new ChFrameD(
                                new ChVectorD(cogXb * ChScale.L, cogYb * ChScale.L, cogZb * ChScale.L),
                                new ChQuaternionD(1, 0, 0, 0)));

                            // Write 'fixed' state
                            if (swComp.IsFixed())
                                newbody.SetBodyFixed(true);


                            // Write shapes (saving also Wavefront files .obj)
                            if ((bool)m_swIntegration.m_taskpaneHost.GetCheckboxSurfaces().Checked)
                            {
                                int nvisshape = 0;

                                if (swComp.Visible == (int)swComponentVisibilityState_e.swComponentVisible)
                                    TraverseComponentForVisualShapes(swComp, nLevel, nbody, ref nvisshape, swComp);
                            }

                            // Write markers (SW coordsystems) contained in this component or subcomponents if any.
                            TraverseComponentForMarkers(swComp, nLevel, nbody);

                            // Write collision shapes (customized SW solid bodies) contained in this component or subcomponents if any.
                            bool param_collide = true;
                            if (myattr != null)
                                param_collide = Convert.ToBoolean(((Parameter)myattr.GetParameter("collision_on")).GetDoubleValue());

                            if (param_collide)
                            {
                                bool found_collisionshapes = false;
                                int ncollshapes = 0;

                                TraverseComponentForCollisionShapes(swComp, nLevel, nbody, ref chbodytransform, ref found_collisionshapes, swComp, ref ncollshapes);
                                if (found_collisionshapes)
                                {
                                    newbody.SetCollide(true);
                                }
                            }

                            chrono_system.Add(newbody);

                        } // end if ChBody equivalent (tree leaf or non-flexible assembly)
                    }
                }
            }

            // Things to do also for sub-components of 'non flexible' assemblies: 


            // Store all the Components into the Dictionary (including DATUMPLANES and other components that can be part of joints)
            // And for each of them store the pointer to the corresponding Part that should actually be linked
            if ((nLevel > 1) && (nbody != -1))
            {
                try
                {
                    ModelDoc2 swModel = (ModelDoc2)this.m_swIntegration.m_swApplication.ActiveDoc;
                    //if (swModel != null)
                    ModelDocExtension swModelDocExt = swModel.Extension;
                    m_bodylist.Add(swModelDocExt.GetPersistReference3(swComp), newbody);

                    // TODO: this below is done only to avoid that GetLinkParams think that the body has not been registered
                    string bodyname = "body_" + num_comp;
                    m_savedParts.Add(swModelDocExt.GetPersistReference3(swComp), bodyname);
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Cannot add part to hashtable?");
                }
            }


            // Traverse all children, proceeding to subassemblies and parts, if any


            //nbody = -1; // RESET COUNTER
            object[] vChildComp;
            Component2 swChildComp;
            vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];
                TraverseComponentForBodies(swChildComp, nLevel + 1, nbody);
            }
        }

        public override void TraverseComponentForMarkers(Component2 swComp, long nLevel, int nbody)
        {
            // Look if component contains markers
            Feature swFeat = (Feature)swComp.FirstFeature();
            MathTransform swCompTotalTrasf = swComp.GetTotalTransform(true);
            TraverseFeaturesForMarkers(swFeat, nLevel, nbody, swCompTotalTrasf);

            // Recursive scan of subcomponents

            Component2 swChildComp;
            object[] vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                TraverseComponentForMarkers(swChildComp, nLevel + 1, nbody);
            }
        }

        public override void TraverseFeaturesForMarkers(Feature swFeat, long nLevel, int nbody, MathTransform swCompTotalTrasf)
        {
            CultureInfo bz = new CultureInfo("en-BZ");

            int nmarker = 0;

            string bodyname = "body_" + nbody;

            while ((swFeat != null))
            {
                // m_asciiText += "# feature: " + swFeat.Name + " [" + swFeat.GetTypeName2() + "]" 

                // Export markers, if any (as coordinate systems)
                if (swFeat.GetTypeName2() == "CoordSys")
                {
                    nmarker++;
                    CoordinateSystemFeatureData swCoordSys = (CoordinateSystemFeatureData)swFeat.GetDefinition();
                    MathTransform tr = swCoordSys.Transform;

                    MathTransform tr_part = swCompTotalTrasf;
                    MathTransform tr_abs = tr.IMultiply(tr_part);  // row-ordered transf. -> reverse mult.order!

                    double[] quat = GetQuaternionFromMatrix(ref tr_abs);
                    double[] amatr = (double[])tr_abs.ArrayData;
                    String markername = "marker_" + nbody + "_" + nmarker;
                    ChMarker newmarker = new ChMarker();
                    newmarker.SetNameString(swFeat.Name);
                    newbody.AddMarker(newmarker);
                    newmarker.Impose_Abs_Coord(
                        new ChCoordsysD(
                            new ChVectorD(amatr[9] * ChScale.L, amatr[10] * ChScale.L, amatr[11] * ChScale.L),
                            new ChQuaternionD(quat[0], quat[1], quat[2], quat[3])));


                    // Export ChMotor from attributes embedded in marker, if any
                    if ((SolidWorks.Interop.sldworks.Attribute)((Entity)swFeat).FindAttribute(m_swIntegration.defattr_chmotor, 0) != null)
                    {
                        SolidWorks.Interop.sldworks.Attribute motorAttribute = (SolidWorks.Interop.sldworks.Attribute)((Entity)swFeat).FindAttribute(m_swIntegration.defattr_chmotor, 0);

                        string motorName = ((Parameter)motorAttribute.GetParameter("motor_name")).GetStringValue();
                        string motorType = ((Parameter)motorAttribute.GetParameter("motor_type")).GetStringValue();
                        string motorMotionlaw = ((Parameter)motorAttribute.GetParameter("motor_motionlaw")).GetStringValue();
                        string motorConstraints = ((Parameter)motorAttribute.GetParameter("motor_constraints")).GetStringValue();
                        string motorMarker = ((Parameter)motorAttribute.GetParameter("motor_marker")).GetStringValue();
                        string motorBody1 = ((Parameter)motorAttribute.GetParameter("motor_body1")).GetStringValue();
                        string motorBody2 = ((Parameter)motorAttribute.GetParameter("motor_body2")).GetStringValue();

                        ModelDoc2 swModel = (ModelDoc2)m_swIntegration.m_swApplication.ActiveDoc;
                        byte[] selMarkerRef = (byte[])EditChMotor.GetIDFromString(swModel, motorMarker);
                        byte[] selBody1Ref = (byte[])EditChMotor.GetIDFromString(swModel, motorBody1);
                        byte[] selBody2Ref = (byte[])EditChMotor.GetIDFromString(swModel, motorBody2);

                        Feature selectedMarker = (Feature)EditChMotor.GetObjectFromID(swModel, selMarkerRef); // actually, already selected through current traverse
                        SolidWorks.Interop.sldworks.Component2 selectedBody1 = (Component2)EditChMotor.GetObjectFromID(swModel, selBody1Ref);
                        SolidWorks.Interop.sldworks.Component2 selectedBody2 = (Component2)EditChMotor.GetObjectFromID(swModel, selBody2Ref);

                        ModelDocExtension swModelDocExt = swModel.Extension;

                        ChFunction motfun;
                        switch (motorMotionlaw)
                        {
                            case "Const":
                                motfun = new ChFunction_Const();
                                break;
                            case "ConstAcc":
                                motfun = new ChFunction_ConstAcc();
                                break;
                            case "Cycloidal":
                                motfun = new ChFunction_Cycloidal();
                                break;
                            case "DoubleS":
                                motfun = new ChFunction_DoubleS();
                                break;
                            case "Poly345":
                                motfun = new ChFunction_Poly345();
                                break;
                            case "Setpoint":
                                motfun = new ChFunction_Setpoint();
                                break;
                            case "Sine":
                                motfun = new ChFunction_Sine();
                                break;
                            default:
                                throw new Exception("ChFunction type does not exist");
                        }

                        ChBodyAuxRef motbody1 = m_bodylist[swModelDocExt.GetPersistReference3(selectedBody1)];
                        ChBodyAuxRef motbody2 = m_bodylist[swModelDocExt.GetPersistReference3(selectedBody2)];


                        ChLinkMotor motor;
                        switch (motorType)
                        {
                            case "LinearPosition":
                                motor = new ChLinkMotorLinearPosition();
                                if (motorConstraints == "False")
                                {
                                    chrono.CastToChLinkMotorLinearPosition(motor).SetGuideConstraint(false, false, false, false, false);
                                }
                                break;
                            case "LinearSpeed":
                                motor = new ChLinkMotorLinearSpeed();
                                if (motorConstraints == "False")
                                {
                                    chrono.CastToChLinkMotorLinearSpeed(motor).SetGuideConstraint(false, false, false, false, false);
                                }
                                break;
                            case "LinearForce":
                                motor = new ChLinkMotorLinearForce();
                                if (motorConstraints == "False")
                                {
                                    chrono.CastToChLinkMotorLinearForce(motor).SetGuideConstraint(false, false, false, false, false);
                                }
                                break;
                            case "RotationAngle":
                                motor = new ChLinkMotorRotationAngle();
                                if (motorConstraints == "False")
                                {
                                    chrono.CastToChLinkMotorRotationAngle(motor).SetSpindleConstraint(false, false, false, false, false);
                                }
                                break;
                            case "RotationSpeed":
                                motor = new ChLinkMotorRotationSpeed();
                                if (motorConstraints == "False")
                                {
                                    chrono.CastToChLinkMotorRotationSpeed(motor).SetSpindleConstraint(false, false, false, false, false);
                                }
                                break;
                            case "RotationTorque":
                                motor = new ChLinkMotorRotationTorque();
                                if (motorConstraints == "False")
                                {
                                    chrono.CastToChLinkMotorRotationTorque(motor).SetSpindleConstraint(false, false, false, false, false);
                                }
                                break;
                            default:
                                throw new Exception("ChLinkMotor type does not exist");
                        }

                        // rotate frame based on motorized degree of freedom of the link
                        ChQuaternionD motorQuaternion = new ChQuaternionD();
                        if (motorType == "LinearPosition" || motorType == "LinearSpeed" || motorType == "LinearForce")
                        {
                            motorQuaternion = chrono.Q_ROTATE_X_TO_Z;
                        }
                        else
                        {
                            motorQuaternion = chrono.QUNIT;
                        }

                        motor.SetName(motorName);
                        motor.Initialize(motbody1, motbody2, new ChFrameD(newmarker.GetAbsFrame().GetPos(), chrono.Qcross(newmarker.GetAbsFrame().GetRot(), motorQuaternion)));
                        motor.SetMotorFunction(motfun);
                        chrono_system.Add(motor);

                    }
                }

                swFeat = (Feature)swFeat.GetNextFeature();
            }
        }


    }
}


#endif // HAS_CHRONO_CSHARP