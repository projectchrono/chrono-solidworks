using ChronoEngine_SwAddin;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media.Media3D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

/// Derived class for exporting a Solidworks assembly to a Chrono::Engine  model.

namespace ChronoEngineAddin
{
    internal class ChModelExporterPython : ChModelExporter
    {
        private string m_asciiText = "";
        private int num_link = 0;
        //private int nbody = -1;

        private Dictionary<string, string> m_exportNamesMap; // map solidworks names vs chrono script names, ie. map[slwd_name] = chrono_name;


        public ChModelExporterPython(ChronoEngine_SwAddin.SWIntegration swIntegration, string save_dir_shapes, string save_filename)
            : base(swIntegration, save_dir_shapes, save_filename) {

            m_exportNamesMap = new Dictionary<string, string>();

        }


        // ============================================================================================================
        // Override base class methods
        // ============================================================================================================
        public override void Export()
        {
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
                m_swIntegration.m_taskpaneHost.GetProgressBar().Start(0, 5, "Exporting to Python");

            num_comp = 0;

            // Write preamble
            m_asciiText = "";
            m_asciiText += "# PyChrono model automatically generated using Chrono::SolidWorks add-in\n";
            m_asciiText += "# Assembly: " + swModel.GetPathName() + "\n\n\n";

            m_asciiText += "import pychrono as chrono \n";
            m_asciiText += "import builtins \n\n";

            m_asciiText += "# Some global settings \n";
            m_asciiText += "sphereswept_r = " + m_swIntegration.m_taskpaneHost.GetNumericSphereSwept().Value.ToString(bz) + "\n";
            m_asciiText += "chrono.ChCollisionModel.SetDefaultSuggestedEnvelope(" + ((double)m_swIntegration.m_taskpaneHost.GetNumericEnvelope().Value * ChScale.L).ToString(bz) + ")\n";
            m_asciiText += "chrono.ChCollisionModel.SetDefaultSuggestedMargin(" + ((double)m_swIntegration.m_taskpaneHost.GetNumericMargin().Value * ChScale.L).ToString(bz) + ")\n";
            m_asciiText += "chrono.ChCollisionSystemBullet.SetContactBreakingThreshold(" + ((double)m_swIntegration.m_taskpaneHost.GetNumericContactBreaking().Value * ChScale.L).ToString(bz) + ")\n\n";

            m_asciiText += "shapes_dir = '" + System.IO.Path.GetFileNameWithoutExtension(m_saveFilename) + "_shapes" + "/' \n\n";

            m_asciiText += "if hasattr(builtins, 'exported_system_relpath'): \n";
            m_asciiText += "    shapes_dir = builtins.exported_system_relpath + shapes_dir \n\n";

            m_asciiText += "exported_items = [] \n\n";

            m_asciiText += "body_0 = chrono.ChBodyAuxRef()\n";
            m_asciiText += "body_0.SetName('ground')\n";
            m_asciiText += "body_0.SetBodyFixed(True)\n";
            m_asciiText += "exported_items.append(body_0)\n\n";


            // Set assembly to Resolved state
            int resolved = ((AssemblyDoc)swModel).ResolveAllLightWeightComponents(true);
            if (resolved != 0)
                MessageBox.Show("Attempt to Resolve assembly failed");


            if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
            {
                // Write down all parts
                TraverseComponentForBodies(swRootComp, 1, -1);

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
                swFeat = (Feature)swModel.FirstFeature();
                TraverseFeaturesForMarkers(swFeat, 1, 0, rootTransform);
            }

            if (m_swIntegration.m_taskpaneHost.GetProgressBar() != null)
                m_swIntegration.m_taskpaneHost.GetProgressBar().End();

            // Write on file
            System.IO.File.WriteAllText(m_saveFilename, m_asciiText);

            System.Windows.Forms.MessageBox.Show("Export to Python completed.");
        }

        public override bool ConvertMate(in Feature swMateFeature, in MathTransform roottrasf, in Component2 assemblyofmates)
        {

            LinkParams link_params;
            GetLinkParameters(swMateFeature, out link_params, roottrasf, assemblyofmates);


            // TODO: redundant part of code
            Mate2 swMate = (Mate2)swMateFeature.GetSpecificFeature2();
            // Fetch the python names using hash map (python names added when scanning parts)
            ModelDocExtension swModelDocExt = default(ModelDocExtension);
            ModelDoc2 swModel = (ModelDoc2)m_swIntegration.m_swApplication.ActiveDoc;
            swModelDocExt = swModel.Extension;

            // Add some comment in Python, to list the referenced SW items
            m_asciiText += "\n# Mate constraint: " + swMateFeature.Name + " [" + swMateFeature.GetTypeName2() + "]" + " type:" + swMate.Type + " align:" + swMate.Alignment + " flip:" + swMate.Flipped + "\n";
            for (int e = 0; e < swMate.GetMateEntityCount(); e++)
            {
                MateEntity2 swEntityN = swMate.MateEntity(e);
                Component2 swCompN = swEntityN.ReferenceComponent;
                String ce_nameN = (string)m_savedParts[swModelDocExt.GetPersistReference3(swCompN)];
                if (swEntityN.ReferenceType2 == 4)
                    ce_nameN = "body_0"; // reference assembly
                m_asciiText += "#   Entity " + e + ": C::E name: " + ce_nameN + " , SW name: " + swCompN.Name2 + " ,  SW ref.type:" + swEntityN.Reference.GetType() + " (" + swEntityN.ReferenceType2 + ")\n";
            }
            //m_asciiText += "\n";

            //// 
            //// WRITE PYHTON CODE CORRESPONDING TO CONSTRAINTS
            ////
            CultureInfo bz = new CultureInfo("en-BZ");


            if (link_params.ref1 == null)
                link_params.ref1 = "body_0";

            if (link_params.ref2 == null)
                link_params.ref2 = "body_0";

            if (link_params.do_ChLinkMateXdistance)
            {
                num_link++;
                String linkname = "link_" + num_link;
                m_asciiText += String.Format(bz, "{0} = chrono.ChLinkMateXdistance()\n", linkname);

                m_asciiText += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cA.X * ChScale.L,
                          link_params.cA.Y * ChScale.L,
                          link_params.cA.Z * ChScale.L);
                m_asciiText += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cB.X * ChScale.L,
                          link_params.cB.Y * ChScale.L,
                          link_params.cB.Z * ChScale.L);
                if (!link_params.entity_0_as_VERTEX)
                    m_asciiText += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                             link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                if (!link_params.entity_1_as_VERTEX)
                    m_asciiText += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                             link_params.dB.X, link_params.dB.Y, link_params.dB.Z);

                // Initialize link, by setting the two csys, in absolute space,
                if (!link_params.swapAB_1)
                    m_asciiText += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dB)\n", linkname, link_params.ref1, link_params.ref2);
                else
                    m_asciiText += String.Format(bz, "{0}.Initialize({1},{2},False,cB,cA,dA)\n", linkname, link_params.ref2, link_params.ref1);

                //if (link_params.do_distance_val!=0)
                m_asciiText += String.Format(bz, "{0}.SetDistance({1})\n", linkname,
                    link_params.do_distance_val * ChScale.L * -1);

                m_asciiText += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);
                // Insert to a list of exported items
                m_asciiText += String.Format(bz, "exported_items.append({0})\n\n", linkname);
            }

            if (link_params.do_ChLinkMateParallel)
            {
                if (Math.Abs(Vector3D.DotProduct(link_params.dA, link_params.dB)) > 0.98)
                {
                    num_link++;
                    String linkname = "link_" + num_link;
                    m_asciiText += String.Format(bz, "{0} = chrono.ChLinkMateParallel()\n", linkname);

                    m_asciiText += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.cA.X * ChScale.L,
                              link_params.cA.Y * ChScale.L,
                              link_params.cA.Z * ChScale.L);
                    m_asciiText += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                    m_asciiText += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.cB.X * ChScale.L,
                              link_params.cB.Y * ChScale.L,
                              link_params.cB.Z * ChScale.L);
                    m_asciiText += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.dB.X, link_params.dB.Y, link_params.dB.Z);

                    if (link_params.do_parallel_flip)
                        m_asciiText += String.Format(bz, "{0}.SetFlipped(True)\n", linkname);

                    // Initialize link, by setting the two csys, in absolute space,
                    if (!link_params.swapAB_1)
                        m_asciiText += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dA,dB)\n", linkname, link_params.ref1, link_params.ref2);
                    else
                        m_asciiText += String.Format(bz, "{0}.Initialize({1},{2},False,cB,cA,dB,dA)\n", linkname, link_params.ref2, link_params.ref1);

                    m_asciiText += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);
                    // Insert to a list of exported items
                    m_asciiText += String.Format(bz, "exported_items.append({0})\n\n", linkname);
                }
                else
                {
                    m_asciiText += "\n# ChLinkMateParallel skipped because directions not parallel! \n";
                }
            }

            if (link_params.do_ChLinkMateOrthogonal)
            {
                if (Math.Abs(Vector3D.DotProduct(link_params.dA, link_params.dB)) < 0.02)
                {
                    num_link++;
                    String linkname = "link_" + num_link;
                    m_asciiText += String.Format(bz, "{0} = chrono.ChLinkMateOrthogonal()\n", linkname);

                    m_asciiText += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.cA.X * ChScale.L,
                              link_params.cA.Y * ChScale.L,
                              link_params.cA.Z * ChScale.L);
                    m_asciiText += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                    m_asciiText += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.cB.X * ChScale.L,
                              link_params.cB.Y * ChScale.L,
                              link_params.cB.Z * ChScale.L);
                    m_asciiText += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.dB.X, link_params.dB.Y, link_params.dB.Z);

                    // Initialize link, by setting the two csys, in absolute space,
                    if (!link_params.swapAB_1)
                        m_asciiText += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dA,dB)\n", linkname, link_params.ref1, link_params.ref2);
                    else
                        m_asciiText += String.Format(bz, "{0}.Initialize({1},{2},False,cB,cA,dB,dA)\n", linkname, link_params.ref2, link_params.ref1);

                    m_asciiText += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);
                    // Insert to a list of exported items
                    m_asciiText += String.Format(bz, "exported_items.append({0})\n\n", linkname);
                }
                else
                {
                    m_asciiText += "\n# ChLinkMateOrthogonal skipped because directions not orthogonal! \n";
                }
            }

            if (link_params.do_ChLinkMateSpherical)
            {
                num_link++;
                String linkname = "link_" + num_link;
                m_asciiText += String.Format(bz, "{0} = chrono.ChLinkMateSpherical()\n", linkname);

                m_asciiText += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cA.X * ChScale.L,
                          link_params.cA.Y * ChScale.L,
                          link_params.cA.Z * ChScale.L);
                m_asciiText += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cB.X * ChScale.L,
                          link_params.cB.Y * ChScale.L,
                          link_params.cB.Z * ChScale.L);

                // Initialize link, by setting the two csys, in absolute space,
                if (!link_params.swapAB_1)
                    m_asciiText += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB)\n", linkname, link_params.ref1, link_params.ref2);
                else
                    m_asciiText += String.Format(bz, "{0}.Initialize({1},{2},False,cB,cA)\n", linkname, link_params.ref2, link_params.ref1);

                m_asciiText += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);
                // Insert to a list of exported items
                m_asciiText += String.Format(bz, "exported_items.append({0})\n\n", linkname);
            }

            if (link_params.do_ChLinkMatePointLine)
            {
                num_link++;
                String linkname = "link_" + num_link;
                m_asciiText += String.Format(bz, "{0} = chrono.ChLinkMateGeneric()\n", linkname);
                m_asciiText += String.Format(bz, "{0}.SetConstrainedCoords(False, True, True, False, False, False)\n", linkname);

                m_asciiText += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cA.X * ChScale.L,
                          link_params.cA.Y * ChScale.L,
                          link_params.cA.Z * ChScale.L);
                m_asciiText += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cB.X * ChScale.L,
                          link_params.cB.Y * ChScale.L,
                          link_params.cB.Z * ChScale.L);
                if (!link_params.entity_0_as_VERTEX)
                    m_asciiText += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n", link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                else
                    m_asciiText += String.Format(bz, "dA = chrono.VNULL\n");
                if (!link_params.entity_1_as_VERTEX)
                    m_asciiText += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n", link_params.dB.X, link_params.dB.Y, link_params.dB.Z);
                else
                    m_asciiText += String.Format(bz, "dB = chrono.VNULL\n");

                // Initialize link, by setting the two csys, in absolute space,
                if (!link_params.swapAB_1)
                    m_asciiText += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dA,dB)\n", linkname, link_params.ref1, link_params.ref2);
                else
                    m_asciiText += String.Format(bz, "{0}.Initialize({1},{2},False,cB,cA,dB,dA)\n", linkname, link_params.ref2, link_params.ref1);

                m_asciiText += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);
                // Insert to a list of exported items
                m_asciiText += String.Format(bz, "exported_items.append({0})\n\n", linkname);
            }



            // Now, do some other special mate type that did not fall in combinations
            // of link_params.do_ChLinkMatePointLine, link_params.do_ChLinkMateSpherical, etc etc

            if (swMateFeature.GetTypeName2() == "MateHinge")
            {
                // auto flip direction if anti aligned (seems that this is assumed automatically in MateHinge in SW)
                if (Vector3D.DotProduct(link_params.dA, link_params.dB) < 0)
                    link_params.dB.Negate();

                // Hinge constraint must be splitted in two C::E constraints: a coaxial and a point-vs-plane
                num_link++;
                String linkname = "link_" + num_link;
                m_asciiText += String.Format(bz, "{0} = chrono.ChLinkMateCoaxial()\n", linkname);

                m_asciiText += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cA.X * ChScale.L,
                          link_params.cA.Y * ChScale.L,
                          link_params.cA.Z * ChScale.L);
                m_asciiText += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                m_asciiText += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cB.X * ChScale.L,
                          link_params.cB.Y * ChScale.L,
                          link_params.cB.Z * ChScale.L);
                m_asciiText += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.dB.X, link_params.dB.Y, link_params.dB.Z);

                m_asciiText += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);


                // Initialize link, by setting the two csys, in absolute space,
                m_asciiText += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dA,dB)\n", linkname, link_params.ref1, link_params.ref2);

                // Insert to a list of exported items
                m_asciiText += String.Format(bz, "exported_items.append({0})\n", linkname);




                num_link++;
                linkname = "link_" + num_link;
                m_asciiText += String.Format(bz, "{0} = chrono.ChLinkMateXdistance()\n", linkname);

                m_asciiText += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cC.X * ChScale.L,
                          link_params.cC.Y * ChScale.L,
                          link_params.cC.Z * ChScale.L);
                m_asciiText += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.dC.X, link_params.dC.Y, link_params.dC.Z);
                m_asciiText += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cD.X * ChScale.L,
                          link_params.cD.Y * ChScale.L,
                          link_params.cD.Z * ChScale.L);
                m_asciiText += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.dD.X, link_params.dD.Y, link_params.dD.Z);

                m_asciiText += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);


                // Initialize link, by setting the two csys, in absolute space,
                if (link_params.entity_2_as_VERTEX)
                    m_asciiText += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dA)\n", linkname, link_params.ref3, link_params.ref4);
                else
                    m_asciiText += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dB)\n", linkname, link_params.ref3, link_params.ref4);

                // Insert to a list of exported items
                m_asciiText += String.Format(bz, "exported_items.append({0})\n", linkname);
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
                    string bodyname = "body_" + nbody;
                    string shapename = "body_" + nbody + "_" + nvisshape;
                    string obj_filename = m_saveDirShapes + "\\" + shapename + ".obj";

                    ModelDoc2 swCompModel = (ModelDoc2)swComp.GetModelDoc();
                    if (!m_savedShapes.ContainsKey(swCompModel.GetPathName()))
                    {
                        try
                        {
                            FileStream ostream = new FileStream(obj_filename, FileMode.Create, FileAccess.ReadWrite);
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
                            System.Windows.Forms.MessageBox.Show("Cannot write to file: " + obj_filename + "\n for component: " + swComp.Name2 + " for path name: " + swCompModel.GetPathName());
                        }
                    }
                    else
                    {
                        // reuse the already-saved shape name
                        shapename = (string)m_savedShapes[swCompModel.GetPathName()];
                    }

                    m_asciiText += String.Format(bz, "\n# Visualization shape \n", bodyname);
                    m_asciiText += String.Format(bz, "{0}_shape = chrono.ChVisualShapeModelFile() \n", shapename);
                    m_asciiText += String.Format(bz, "{0}_shape.SetFilename(shapes_dir +'{0}.obj') \n", shapename);

                    object foo = null;
                    double[] vMatProperties = (double[])swComp.GetMaterialPropertyValues2((int)swInConfigurationOpts_e.swThisConfiguration, foo);

                    if (vMatProperties != null)
                        if (vMatProperties[0] != -1)
                        {
                            m_asciiText += String.Format(bz, "{0}_shape.SetColor(chrono.ChColor({1},{2},{3})) \n", shapename, vMatProperties[0], vMatProperties[1], vMatProperties[2]);
                            m_asciiText += String.Format(bz, "{0}_shape.SetOpacity({1}) \n", shapename, 1.0 - vMatProperties[7]);
                        }

                    MathTransform absframe_chbody = chbody_comp.GetTotalTransform(true);
                    MathTransform absframe_shape = swComp.GetTotalTransform(true);
                    MathTransform absframe_chbody_inv = absframe_chbody.IInverse();
                    MathTransform relframe_shape = absframe_shape.IMultiply(absframe_chbody_inv);  // row-ordered transf. -> reverse mult.order!
                    double[] amatr = (double[])relframe_shape.ArrayData;
                    double[] quat = GetQuaternionFromMatrix(ref relframe_shape);

                    m_asciiText += String.Format(bz, "{0}.AddVisualShape({1}_shape, chrono.ChFrameD(", bodyname, shapename);
                    m_asciiText += String.Format(bz, "chrono.ChVectorD({0},{1},{2}), ", amatr[9] * ChScale.L, amatr[10] * ChScale.L, amatr[11] * ChScale.L);
                    m_asciiText += String.Format(bz, "chrono.ChQuaternionD({0},{1},{2},{3})", quat[0], quat[1], quat[2], quat[3]);
                    m_asciiText += String.Format(bz, "))\n");
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

                    if (build_collision_model)
                    {


                        if (!found_collisionshapes)
                        {
                            found_collisionshapes = true;

                            // fetch SW attribute with Chrono parameters
                            SolidWorks.Interop.sldworks.Attribute myattr = (SolidWorks.Interop.sldworks.Attribute)swCompBase.FindAttribute(m_swIntegration.defattr_collisionParams, 0);

                            m_asciiText += "\n# Collision Model\n";
                            m_asciiText += String.Format(bz, "\n{0}.AddCollisionModel(chrono.ChCollisionModel())\n", bodyname);

                            m_asciiText += "\n# Collision material \n";

                            m_asciiText += String.Format(bz, "{0} = chrono.ChMaterialSurfaceNSC()\n", matname);



                            if (myattr != null)
                            {

                                m_asciiText += "\n# Collision parameters \n";
                                double param_friction = ((Parameter)myattr.GetParameter("friction")).GetDoubleValue();
                                double param_restitution = ((Parameter)myattr.GetParameter("restitution")).GetDoubleValue();
                                double param_rolling_friction = ((Parameter)myattr.GetParameter("rolling_friction")).GetDoubleValue();
                                double param_spinning_friction = ((Parameter)myattr.GetParameter("spinning_friction")).GetDoubleValue();
                                double param_collision_envelope = ((Parameter)myattr.GetParameter("collision_envelope")).GetDoubleValue();
                                double param_collision_margin = ((Parameter)myattr.GetParameter("collision_margin")).GetDoubleValue();
                                int param_collision_family = (int)((Parameter)myattr.GetParameter("collision_family")).GetDoubleValue();

                                m_asciiText += String.Format(bz, "{0}.SetFriction({1:g})\n", matname, param_friction);
                                if (param_restitution != 0)
                                    m_asciiText += String.Format(bz, "{0}.SetRestitution({1:g})\n", matname, param_restitution);
                                if (param_rolling_friction != 0)
                                    m_asciiText += String.Format(bz, "{0}.SetRollingFriction({1:g})\n", matname, param_rolling_friction);
                                if (param_spinning_friction != 0)
                                    m_asciiText += String.Format(bz, "{0}.SetSpinningFriction({1:g})\n", matname, param_spinning_friction);
                                //if (param_collision_envelope != 0.03)
                                m_asciiText += String.Format(bz, "{0}.GetCollisionModel().SetEnvelope({1:g})\n", bodyname, param_collision_envelope * ChScale.L);
                                //if (param_collision_margin != 0.01)
                                m_asciiText += String.Format(bz, "{0}.GetCollisionModel().SetSafeMargin({1:g})\n", bodyname, param_collision_margin * ChScale.L);
                                if (param_collision_family != 0)
                                    m_asciiText += String.Format(bz, "{0}.GetCollisionModel().SetFamily({1})\n", bodyname, param_collision_family);
                            }

                            //// clear model only at 1st subcomponent where coll shapes are found in features:
                            //m_asciiText += "\n# Collision shapes \n";
                            //m_asciiText += String.Format(bz, "{0}.GetCollisionModel().Clear()\n", bodyname);
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
                                    m_asciiText += String.Format(bz, "mr = chrono.ChMatrix33D()\n");
                                    m_asciiText += String.Format(bz, "mr[0,0]=1; mr[1,0]=0; mr[2,0]=0 \n");
                                    m_asciiText += String.Format(bz, "mr[0,1]=0; mr[1,1]=1; mr[2,1]=0 \n");
                                    m_asciiText += String.Format(bz, "mr[0,2]=0; mr[1,2]=0; mr[2,2]=1 \n");
                                    m_asciiText += String.Format(bz, "collshape = chrono.ChCollisionShapeSphere({0},{1});\n",
                                        matname,
                                        rad * ChScale.L);
                                    m_asciiText += String.Format(bz, "{0}.GetCollisionModel().AddShape(collshape,chrono.ChFrameD(chrono.ChVectorD({1},{2},{3}), mr));\n",
                                        bodyname,
                                        center.X * ChScale.L,
                                        center.Y * ChScale.L,
                                        center.Z * ChScale.L);
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
                                    m_asciiText += String.Format(bz, "mr = chrono.ChMatrix33D()\n");
                                    m_asciiText += String.Format(bz, "mr[0,0]={0}; mr[1,0]={1}; mr[2,0]={2} \n", Dx.X, Dx.Y, Dx.Z);
                                    m_asciiText += String.Format(bz, "mr[0,1]={0}; mr[1,1]={1}; mr[2,1]={2} \n", Dy.X, Dy.Y, Dy.Z);
                                    m_asciiText += String.Format(bz, "mr[0,2]={0}; mr[1,2]={1}; mr[2,2]={2} \n", Dz.X, Dz.Y, Dz.Z);
                                    m_asciiText += String.Format(bz, "collshape = chrono.ChCollisionShapeBox({0},{1},{2},{3})\n",
                                        matname,
                                        eX.Length * ChScale.L,
                                        eY.Length * ChScale.L,
                                        eZ.Length * ChScale.L);
                                    m_asciiText += String.Format(bz, "{0}.GetCollisionModel().AddShape(collshape,chrono.ChFrameD(chrono.ChVectorD({1},{2},{3}), mr))\n",
                                        bodyname,
                                        vO.X * ChScale.L,
                                        vO.Y * ChScale.L,
                                        vO.Z * ChScale.L);

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
                                    m_asciiText += String.Format(bz, "p1 = chrono.ChVectorD({0}, {1}, {2})\n", p1.X * ChScale.L, p1.Y * ChScale.L, p1.Z * ChScale.L);
                                    m_asciiText += String.Format(bz, "p2 = chrono.ChVectorD({0}, {1}, {2})\n", p2.X * ChScale.L, p2.Y * ChScale.L, p2.Z * ChScale.L);
                                    m_asciiText += String.Format(bz, "{0}.GetCollisionModel().AddCylinder({1}, {2}, p1, p2)\n", bodyname, matname, rad * ChScale.L);
                                    rbody_converted = true;
                                }

                                if (ConvertToCollisionShapes.SWbodyToConvexHull(swBody, 30) && !rbody_converted)
                                {
                                    Point3D[] vertexes = new Point3D[1]; // will be resized by SWbodyToConvexHull
                                    ConvertToCollisionShapes.SWbodyToConvexHull(swBody, ref vertexes, 30);
                                    if (vertexes.Length > 0)
                                    {
                                        m_asciiText += String.Format(bz, "pt_vect = chrono.vector_ChVectorD()\n");
                                        for (int iv = 0; iv < vertexes.Length; iv++)
                                        {
                                            Point3D vert_l = vertexes[iv];
                                            Point3D vert = PointTransform(vert_l, ref collshape_subcomp_transform);
                                            m_asciiText += String.Format(bz, "pt_vect.push_back(chrono.ChVectorD({0},{1},{2}))\n",
                                                vert.X * ChScale.L,
                                                vert.Y * ChScale.L,
                                                vert.Z * ChScale.L);
                                        }
                                        m_asciiText += String.Format(bz, "collshape = chrono.ChCollisionShapeConvexHull({0},pt_vect)\n",
                                            matname);
                                        m_asciiText += String.Format(bz, "{0}.GetCollisionModel().AddShape(collshape)\n", bodyname);
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
                            string obj_filename = m_saveDirShapes + "\\" + shapename + ".obj";

                            ModelDoc2 swCompModel = (ModelDoc2)swComp.GetModelDoc();
                            if (!m_savedCollisionMeshes.ContainsKey(swCompModel.GetPathName()))
                            {
                                try
                                {
                                    FileStream ostream = new FileStream(obj_filename, FileMode.Create, FileAccess.ReadWrite);
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
                                    System.Windows.Forms.MessageBox.Show("Cannot write to file: " + obj_filename + "\n for component: " + swComp.Name2 + " for path name: " + swCompModel.GetPathName());
                                }
                            }
                            else
                            {
                                // reuse the already-saved shape name
                                shapename = (String)m_savedCollisionMeshes[swCompModel.GetPathName()];
                            }

                            double[] amatr = (double[])collshape_subcomp_transform.ArrayData;
                            //double[] quat = GetQuaternionFromMatrix(ref collshape_subcomp_transform);

                            m_asciiText += String.Format(bz, "\n# Triangle mesh collision shape \n", bodyname);
                            m_asciiText += String.Format(bz, "{0}_mesh = chrono.ChTriangleMeshConnected.CreateFromWavefrontFile(shapes_dir + '{1}.obj', False, True) \n", shapename, shapename);
                            m_asciiText += String.Format(bz, "mr = chrono.ChMatrix33D()\n");
                            m_asciiText += String.Format(bz, "mr[0,0]={0}; mr[1,0]={1}; mr[2,0]={2} \n", amatr[0] * ChScale.L, amatr[1] * ChScale.L, amatr[2] * ChScale.L);
                            m_asciiText += String.Format(bz, "mr[0,1]={0}; mr[1,1]={1}; mr[2,1]={2} \n", amatr[3] * ChScale.L, amatr[4] * ChScale.L, amatr[5] * ChScale.L);
                            m_asciiText += String.Format(bz, "mr[0,2]={0}; mr[1,2]={1}; mr[2,2]={2} \n", amatr[6] * ChScale.L, amatr[7] * ChScale.L, amatr[8] * ChScale.L);
                            m_asciiText += String.Format(bz, "{0}_mesh.Transform(chrono.ChVectorD({1}, {2}, {3}), mr) \n", shapename, amatr[9] * ChScale.L, amatr[10] * ChScale.L, amatr[11] * ChScale.L);                            
                            m_asciiText += String.Format(bz, "collshape = chrono.ChCollisionShapeTriangleMesh({0},{1}_mesh,False,False,sphereswept_r)\n",
                                matname,
                                shapename);
                            m_asciiText += String.Format(bz, "{0}.GetCollisionModel().AddShape(collshape)\n",
                                bodyname);
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
                            SolidWorks.Interop.sldworks.Attribute myattr = (SolidWorks.Interop.sldworks.Attribute)swComp.FindAttribute(m_swIntegration.defattr_collisionParams, 0);

                            MathTransform chbodytransform = swComp.GetTotalTransform(true);
                            double[] amatr;
                            amatr = (double[])chbodytransform.ArrayData;
                            string bodyname = "body_" + num_comp;

                            // Write create body
                            m_asciiText += "# Rigid body part\n";
                            m_asciiText += bodyname + " = chrono.ChBodyAuxRef()" + "\n";

                            m_exportNamesMap[swComp.Name2] = bodyname;


                            // Write name
                            m_asciiText += bodyname + ".SetName('" + swComp.Name2 + "')" + "\n";

                            // Write position
                            m_asciiText += bodyname + ".SetPos(chrono.ChVectorD("
                                + (amatr[9] * ChScale.L).ToString("g", bz) + ","
                                + (amatr[10] * ChScale.L).ToString("g", bz) + ","
                                + (amatr[11] * ChScale.L).ToString("g", bz) + "))" + "\n";

                            // Write rotation
                            double[] quat = GetQuaternionFromMatrix(ref chbodytransform);
                            m_asciiText += String.Format(bz, "{0}.SetRot(chrono.ChQuaternionD({1:g},{2:g},{3:g},{4:g}))\n", bodyname, quat[0], quat[1], quat[2], quat[3]);

                            // Compute mass

                            int nvalid_bodies = 0;
                            TraverseComponent_for_countingmassbodies(swComp, ref nvalid_bodies);

                            int addedb = 0;
                            object[] bodies_nocollshapes = new object[nvalid_bodies];
                            TraverseComponent_for_massbodies(swComp, ref bodies_nocollshapes, ref addedb);

                            MassProperty swMass;
                            swMass = (MassProperty)swComp.IGetModelDoc().Extension.CreateMassProperty();
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

                            m_asciiText += String.Format(bz, "{0}.SetMass({1:g})\n",
                                       bodyname,
                                       mass * ChScale.M);

                            // Write inertia tensor 
                            m_asciiText += String.Format(bz, "{0}.SetInertiaXX(chrono.ChVectorD({1:g},{2:g},{3:g}))\n",
                                       bodyname,
                                       Ixx * ChScale.M * ChScale.L * ChScale.L,
                                       Iyy * ChScale.M * ChScale.L * ChScale.L,
                                       Izz * ChScale.M * ChScale.L * ChScale.L);
                            // Note: C::E assumes that's up to you to put a 'minus' sign in values of Ixy, Iyz, Izx
                            m_asciiText += String.Format(bz, "{0}.SetInertiaXY(chrono.ChVectorD({1:g},{2:g},{3:g}))\n",
                                       bodyname,
                                       -Ixy * ChScale.M * ChScale.L * ChScale.L,
                                       -Izx * ChScale.M * ChScale.L * ChScale.L,
                                       -Iyz * ChScale.M * ChScale.L * ChScale.L);

                            // Write the position of the COG respect to the REF
                            m_asciiText += String.Format(bz, "{0}.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD({1:g},{2:g},{3:g}),chrono.ChQuaternionD(1,0,0,0)))\n",
                                        bodyname,
                                        cogXb * ChScale.L,
                                        cogYb * ChScale.L,
                                        cogZb * ChScale.L);

                            // Write 'fixed' state
                            if (swComp.IsFixed())
                                m_asciiText += String.Format(bz, "{0}.SetBodyFixed(True)\n", bodyname);


                            // Write shapes (saving also Wavefront files .obj)
                            if ((bool)m_swIntegration.m_taskpaneHost.GetCheckboxSurfaces().Checked)
                            {
                                int nvisshape = 0;

                                if (swComp.Visible == (int)swComponentVisibilityState_e.swComponentVisible)
                                    TraverseComponentForVisualShapes(swComp, nLevel, nbody, ref nvisshape, swComp);
                            }

                            // Write markers (SW coordsystems) contained in this component or subcomponents
                            // if any.
                            TraverseComponentForMarkers(swComp, nLevel, nbody);

                            // Write collision shapes (customized SW solid bodies) contained in this component or subcomponents
                            // if any.
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
                                    m_asciiText += String.Format(bz, "{0}.SetCollide(True)\n", bodyname);
                                }
                            }

                            // Insert to a list of exported items
                            m_asciiText += String.Format(bz, "\n" + "exported_items.append({0})\n", bodyname);

                            // End writing body in 
                            m_asciiText += "\n\n\n";


                        } // end if ChBody equivalent (tree leaf or non-flexible assembly)
                    }
                }
            }

            // Things to do also for sub-components of 'non flexible' assemblies: 
            //

            // store in hashtable, will be useful later when adding constraints
            if ((nLevel > 1) && (nbody != -1))
                try
                {
                    string bodyname = "body_" + this.num_comp;

                    ModelDocExtension swModelDocExt = default(ModelDocExtension);
                    ModelDoc2 swModel = (ModelDoc2)this.m_swIntegration.m_swApplication.ActiveDoc;
                    //if (swModel != null)
                    swModelDocExt = swModel.Extension;
                    m_savedParts.Add(swModelDocExt.GetPersistReference3(swComp), bodyname);
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Cannot add part to hashtable?");
                }


            // Traverse all children, proceeding to subassemblies and parts, if any
            // 

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
                // m_asciiText += "# feature: " + swFeat.Name + " [" + swFeat.GetTypeName2() + "]" + "\n";

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
                    m_asciiText += "\n# Auxiliary marker (coordinate system feature)\n";
                    m_asciiText += String.Format(bz, "{0} =chrono.ChMarker()\n", markername);
                    m_asciiText += String.Format(bz, "{0}.SetName('{1}')" + "\n", markername, swFeat.Name);
                    m_asciiText += String.Format(bz, "{0}.AddMarker({1})\n", bodyname, markername);
                    m_asciiText += String.Format(bz, "{0}.Impose_Abs_Coord(chrono.ChCoordsysD(chrono.ChVectorD({1},{2},{3}),chrono.ChQuaternionD({4},{5},{6},{7})))\n",
                               markername,
                               amatr[9] * ChScale.L,
                               amatr[10] * ChScale.L,
                               amatr[11] * ChScale.L,
                               quat[0], quat[1], quat[2], quat[3]);

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
                        string motlawInputs = ((Parameter)motorAttribute.GetParameter("motor_motlaw_inputs")).GetStringValue();

                        ModelDoc2 swModel = (ModelDoc2)m_swIntegration.m_swApplication.ActiveDoc;
                        byte[] selMarkerRef = (byte[])EditChMotor.GetIDFromString(swModel, motorMarker);
                        byte[] selBody1Ref = (byte[])EditChMotor.GetIDFromString(swModel, motorBody1);

                        Feature selectedMarker = (Feature)EditChMotor.GetObjectFromID(swModel, selMarkerRef); // actually, already selected through current traverse
                        SolidWorks.Interop.sldworks.Component2 selectedBody1 = (Component2)EditChMotor.GetObjectFromID(swModel, selBody1Ref);

                        string slaveBodyName = m_exportNamesMap[selectedBody1.Name];

                        // check if master body is ground
                        string masterBodyName;
                        if (motorBody2 == "ground")
                        {
                            masterBodyName = "ground";
                        }
                        else
                        {
                            byte[] selBody2Ref = (byte[])EditChMotor.GetIDFromString(swModel, motorBody2);
                            SolidWorks.Interop.sldworks.Component2 selectedBody2 = (Component2)EditChMotor.GetObjectFromID(swModel, selBody2Ref);
                            masterBodyName = m_exportNamesMap[selectedBody2.Name];
                        }

                        string chMotorClassName = "ChLinkMotor" + motorType;
                        string chMotorConstraintName = "";
                        string chFunctionClassName = "ChFunction_" + motorMotionlaw;
                        string motorQuaternion = "";

                        if (motorType == "LinearPosition" || motorType == "LinearSpeed" || motorType == "LinearForce")
                        {
                            motorQuaternion = "chrono.Q_ROTATE_X_TO_Z";
                            chMotorConstraintName = "GuideConstraint";
                        }
                        else
                        {
                            motorQuaternion = "chrono.QUNIT";
                            chMotorConstraintName = "SpindleConstraint";
                        }

                        String motorInstanceName = "motor_" + nbody + "_" + nmarker;
                        m_asciiText += "\n# Motor from Solidworks marker\n";
                        m_asciiText += String.Format(bz, motorInstanceName + " = chrono." + chMotorClassName + "()\n");
                        m_asciiText += String.Format(bz, motorInstanceName + ".SetName(\"" + motorName + "\")\n");
                        m_asciiText += motorInstanceName + ".Initialize(" + slaveBodyName + ", " + masterBodyName
                                    + ",chrono.ChFrameD(" + markername + ".GetAbsFrame().GetPos()," + markername + ".GetAbsFrame().GetRot()*" + motorQuaternion + "))\n";
                        m_asciiText += "exported_items.append(" + motorInstanceName  + ")\n\n";

                        if (motorConstraints == "False")
                        {
                            m_asciiText += motorInstanceName + ".Set" + chMotorConstraintName + "(False, False, False, False, False)\n";
                        }

                        String motfunInstanceName = "motfun_" + nbody + "_" + nmarker;
                        m_asciiText += $"{motfunInstanceName} = chrono.{chFunctionClassName}({motlawInputs})\n"; // define motion law with inputs, if given
                        m_asciiText += motorInstanceName + ".SetMotorFunction(" + motfunInstanceName + ")\n";
                    }

                    // Expor ChSDA from attributes embedded in marker, if any
                    if ((SolidWorks.Interop.sldworks.Attribute)((Entity)swFeat).FindAttribute(m_swIntegration.defattr_chsda, 0) != null)
                    {
                        // TO BE DONE
                    }

                }

                swFeat = (Feature)swFeat.GetNextFeature();
            }
        }

    }
}
