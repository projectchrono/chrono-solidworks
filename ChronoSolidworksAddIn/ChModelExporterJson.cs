using ChronoEngine_SwAddin;
using Newtonsoft.Json.Linq;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Windows.Media.Media3D;
using System.Globalization;
using System.IO;

namespace ChronoEngineAddin
{
    internal class ChModelExporterJson : ChModelExporter
    {
        private JArray m_ChSystemLinklist;
        private JArray m_ChSystemBodylist;
        private JObject m_ChBodyAuxRefNode;
        private JObject m_ChSystemNode;
        private int m_object_ID_used; // identifies last used value of _object_ID, used to uniquely identify any entity in the JSON file


        private struct JObjectCreator
        {
            public static JObject CreateChVector(double[] vect)
            {
                if (vect.Length != 3)
                    throw new ArgumentException("CreateChVector requires a input argument of size exactly 3.", nameof(vect));

                return new JObject(
                    new JProperty("x", vect[0]),
                    new JProperty("y", vect[1]),
                    new JProperty("z", vect[2])
                    );
            }

            public static JObject CreateChVector(double x, double y, double z)
            {
                return new JObject(
                    new JProperty("x", x),
                    new JProperty("y", y),
                    new JProperty("z", z)
                    );
            }

            public static JObject CreateChQuaternion(double[] quat)
            {
                if (quat.Length != 3)
                    throw new ArgumentException("CreateChQuaternion requires a input argument of size exactly 4.", nameof(quat));

                return new JObject(
                    new JProperty("e0", quat[0]),
                    new JProperty("e1", quat[1]),
                    new JProperty("e2", quat[2]),
                    new JProperty("e3", quat[3])
                    );
            }

            public static JObject CreateChQuaternion(double e0, double e1, double e2, double e3)
            {
                return new JObject(
                    new JProperty("e0", e0),
                    new JProperty("e1", e1),
                    new JProperty("e2", e2),
                    new JProperty("e3", e3)
                    );
            }
        };


        public ChModelExporterJson(ChronoEngine_SwAddin.SWIntegration swIntegration, string save_dir_shapes, string save_filename)
            : base(swIntegration, save_dir_shapes, save_filename) 
        {
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
            this.m_savedCollisionMeshes.Clear();

            swModel = (ModelDoc2)m_swIntegration.m_swApplication.ActiveDoc;
            if (swModel == null) return;
            swConfMgr = (ConfigurationManager)swModel.ConfigurationManager;
            swConf = (Configuration)swConfMgr.ActiveConfiguration;
            swRootComp = (Component2)swConf.GetRootComponent3(true);

            m_swIntegration.m_swApplication.GetUserProgressBar(out m_swIntegration.m_taskpaneHost.GetProgressBar());
            if (m_swIntegration.m_taskpaneHost.GetProgressBar() != null)
                m_swIntegration.m_taskpaneHost.GetProgressBar().Start(0, 5, "Exporting to JSON");

            num_comp = 0;
            m_object_ID_used = 0;

            m_ChSystemNode = new JObject();


            //asciitext += "# Some global settings: \n" +
            //             "sphereswept_r = " + this.numeric_sphereswept.Value.ToString(bz) + "\n" +
            //             "chrono.ChCollisionModel.SetDefaultSuggestedEnvelope(" + ((double)this.numeric_envelope.Value * ChScale.L).ToString(bz) + ")\n" +
            //             "chrono.ChCollisionModel.SetDefaultSuggestedMargin(" + ((double)this.numeric_margin.Value * ChScale.L).ToString(bz) + ")\n" +
            //             "chrono.ChCollisionSystemBullet.SetContactBreakingThreshold(" + ((double)this.numeric_contactbreaking.Value * ChScale.L).ToString(bz) + ")\n\n";


            if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
            {
                // Write down all parts
                m_ChSystemBodylist = new JArray();

                // Add world-fixed object
                var ChBodyGroundNode = new JObject
                (
                    new JProperty("_type", "ChBodyAuxRef"),
                    new JProperty("_object_ID", ++m_object_ID_used),
                    new JProperty("m_name", "ground"),
                    new JProperty("_c_SetBodyFixed", true)
                );

                m_ChSystemBodylist.Add(ChBodyGroundNode);
                
                TraverseComponentForBodies(swRootComp, 1, -1);
                m_ChSystemNode.Add("bodies", m_ChSystemBodylist);


                // Write down all constraints

                MathTransform roottrasf = swRootComp.GetTotalTransform(true);
                if (roottrasf == null)
                {
                    IMathUtility swMath = (IMathUtility)m_swIntegration.m_swApplication.GetMathUtility();
                    double[] nulltr = new double[] { 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 };
                    roottrasf = (MathTransform)swMath.CreateTransform(nulltr);
                }

                m_ChSystemLinklist = new JArray();

                Feature swFeat = (Feature)swModel.FirstFeature();
                TraverseFeaturesForLinks(swFeat, 1, ref roottrasf, ref swRootComp);
                m_ChSystemNode.Add("links", m_ChSystemLinklist);

                TraverseComponentForLinks(swRootComp, 1, ref roottrasf);


                //// Write down all markers in assembly (that are not in sub parts, so they belong to 'ground' object)
                swFeat = (Feature)swModel.FirstFeature();
                m_ChBodyAuxRefNode = ChBodyGroundNode;
                TraverseFeaturesForMarkers(swFeat, 1, 0, roottrasf);

                //System.Windows.Forms.MessageBox.Show("Export to JSON completed.\nBody count: " + (m_ChSystemBodylist.Count - 1) + "\nLink count: " + m_ChSystemLinklist.Count);
            }

            if (m_swIntegration.m_taskpaneHost.GetProgressBar() != null)
                m_swIntegration.m_taskpaneHost.GetProgressBar().End();


            // Write on file
            var ChSystemWrapper = new JObject(new JProperty("system", m_ChSystemNode));
            File.WriteAllText(m_saveFilename, ChSystemWrapper.ToString(Newtonsoft.Json.Formatting.Indented));

            System.Windows.Forms.MessageBox.Show("Export to JSON completed.\nBody count: " + (m_ChSystemBodylist.Count - 1) + "\nLink count: " + m_ChSystemLinklist.Count);
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


            if (link_params.ref1 == null)
                link_params.ref1 = 1.ToString();

            if (link_params.ref2 == null)
                link_params.ref2 = 1.ToString();

            if (link_params.do_ChLinkMateXdistance)
            {
                var link_node = new JObject
                (
                    new JProperty("_type", "ChLinkMateXdistance"),
                    new JProperty("_object_ID", ++m_object_ID_used),
                    new JProperty("_c_Initialize_Body1",
                        new JObject(
                            new JProperty("_type", "ChBodyAuxRef"),
                            new JProperty("_reference_ID", Convert.ToUInt32(!link_params.swapAB_1 ? link_params.ref1 : link_params.ref2)))
                    ),
                    new JProperty("_c_Initialize_Body2",
                        new JObject(
                            new JProperty("_type", "ChBodyAuxRef"),
                            new JProperty("_reference_ID", Convert.ToUInt32(!link_params.swapAB_1 ? link_params.ref2 : link_params.ref1)))
                    ),
                    new JProperty("_c_Initialize_pos_are_relative", false),
                    new JProperty(!link_params.swapAB_1 ? "_c_Initialize_pt1" : "_c_Initialize_pt2", JObjectCreator.CreateChVector(
                        link_params.cA.X * ChScale.L,
                        link_params.cA.Y * ChScale.L,
                        link_params.cA.Z * ChScale.L)
                    ),
                    new JProperty(!link_params.swapAB_1 ? "_c_Initialize_pt2" : "_c_Initialize_pt1", JObjectCreator.CreateChVector(
                        link_params.cB.X * ChScale.L,
                        link_params.cB.Y * ChScale.L,
                        link_params.cB.Z * ChScale.L)
                    ),
                    new JProperty("_c_Initialize_norm2", !link_params.swapAB_1 ? JObjectCreator.CreateChVector(link_params.dB.X, link_params.dB.Y, link_params.dB.Z) : JObjectCreator.CreateChVector(link_params.dA.X, link_params.dA.Y, link_params.dA.Z)),
                    new JProperty("distance", link_params.do_distance_val * ChScale.L * -1),
                    new JProperty("m_name", swMateFeature.Name)
                );

                m_ChSystemLinklist.Add(link_node);
            }

            if (link_params.do_ChLinkMateParallel)
            {
                if (Math.Abs(Vector3D.DotProduct(link_params.dA, link_params.dB)) > 0.98)
                {

                    var link_node = new JObject
                    (
                        new JProperty("_type", "ChLinkMateParallel"),
                        new JProperty("_object_ID", ++m_object_ID_used),
                        new JProperty("_c_Initialize_Body1",
                            new JObject(
                                new JProperty("_type", "ChBodyAuxRef"),
                                new JProperty("_reference_ID", Convert.ToUInt32(!link_params.swapAB_1 ? link_params.ref1 : link_params.ref2)))
                        ),
                        new JProperty("_c_Initialize_Body2",
                            new JObject(
                                new JProperty("_type", "ChBodyAuxRef"),
                                new JProperty("_reference_ID", Convert.ToUInt32(!link_params.swapAB_1 ? link_params.ref2 : link_params.ref1)))
                        ),
                        new JProperty("_c_Initialize_pos_are_relative", false),
                        new JProperty(!link_params.swapAB_1 ? "_c_Initialize_pt1" : "_c_Initialize_pt2", JObjectCreator.CreateChVector(
                            link_params.cA.X * ChScale.L,
                            link_params.cA.Y * ChScale.L,
                            link_params.cA.Z * ChScale.L)
                        ),
                        new JProperty(!link_params.swapAB_1 ? "_c_Initialize_pt2" : "_c_Initialize_pt1", JObjectCreator.CreateChVector(
                            link_params.cB.X * ChScale.L,
                            link_params.cB.Y * ChScale.L,
                            link_params.cB.Z * ChScale.L)
                        ),
                        new JProperty(!link_params.swapAB_1 ? "_c_Initialize_norm1" : "_c_Initialize_norm2", JObjectCreator.CreateChVector(link_params.dA.X, link_params.dA.Y, link_params.dA.Z)),
                        new JProperty(!link_params.swapAB_1 ? "_c_Initialize_norm2" : "_c_Initialize_norm1", JObjectCreator.CreateChVector(link_params.dB.X, link_params.dB.Y, link_params.dB.Z)),
                        new JProperty("m_name", swMateFeature.Name)
                    );

                    if (link_params.do_parallel_flip)
                        link_node.Add("_c_SetFlipped", true);

                    m_ChSystemLinklist.Add(link_node);

                }
                else
                {
                    // ChLinkMateParallel skipped because directions not parallel
                }
            }

            if (link_params.do_ChLinkMateOrthogonal)
            {
                if (Math.Abs(Vector3D.DotProduct(link_params.dA, link_params.dB)) < 0.02)
                {
                    var link_node = new JObject
                    (
                        new JProperty("_type", "ChLinkMateOrthogonal"),
                        new JProperty("_object_ID", ++m_object_ID_used),
                        new JProperty("_c_Initialize_Body1",
                            new JObject(
                                new JProperty("_type", "ChBodyAuxRef"),
                                new JProperty("_reference_ID", Convert.ToUInt32(!link_params.swapAB_1 ? link_params.ref1 : link_params.ref2)))
                        ),
                        new JProperty("_c_Initialize_Body2",
                            new JObject(
                                new JProperty("_type", "ChBodyAuxRef"),
                                new JProperty("_reference_ID", Convert.ToUInt32(!link_params.swapAB_1 ? link_params.ref2 : link_params.ref1)))
                        ),
                        new JProperty("_c_Initialize_pos_are_relative", false),
                        new JProperty(!link_params.swapAB_1 ? "_c_Initialize_pt1" : "_c_Initialize_pt2", JObjectCreator.CreateChVector(
                            link_params.cA.X * ChScale.L,
                            link_params.cA.Y * ChScale.L,
                            link_params.cA.Z * ChScale.L)
                        ),
                        new JProperty(!link_params.swapAB_1 ? "_c_Initialize_pt2" : "_c_Initialize_pt1", JObjectCreator.CreateChVector(
                            link_params.cB.X * ChScale.L,
                            link_params.cB.Y * ChScale.L,
                            link_params.cB.Z * ChScale.L)
                        ),
                        new JProperty(!link_params.swapAB_1 ? "_c_Initialize_norm1" : "_c_Initialize_norm2", JObjectCreator.CreateChVector(link_params.dA.X, link_params.dA.Y, link_params.dA.Z)),
                        new JProperty(!link_params.swapAB_1 ? "_c_Initialize_norm2" : "_c_Initialize_norm1", JObjectCreator.CreateChVector(link_params.dB.X, link_params.dB.Y, link_params.dB.Z)),
                        new JProperty("m_name", swMateFeature.Name)
                    );

                    m_ChSystemLinklist.Add(link_node);

                }
                else
                {
                    // ChLinkMateOrthogonal skipped because directions not orthogonal
                }
            }

            if (link_params.do_ChLinkMateSpherical)
            {
                var link_node = new JObject
                (
                    new JProperty("_type", "ChLinkMateSpherical"),
                    new JProperty("_object_ID", ++m_object_ID_used),
                    new JProperty("_c_Initialize_Body1",
                        new JObject(
                            new JProperty("_type", "ChBodyAuxRef"),
                            new JProperty("_reference_ID", Convert.ToUInt32(!link_params.swapAB_1 ? link_params.ref1 : link_params.ref2)))
                    ),
                    new JProperty("_c_Initialize_Body2",
                        new JObject(
                            new JProperty("_type", "ChBodyAuxRef"),
                            new JProperty("_reference_ID", Convert.ToUInt32(!link_params.swapAB_1 ? link_params.ref2 : link_params.ref1)))
                    ),
                    new JProperty("_c_Initialize_pos_are_relative", false),
                    new JProperty(!link_params.swapAB_1 ? "_c_Initialize_pt1" : "_c_Initialize_pt2", JObjectCreator.CreateChVector(
                        link_params.cA.X * ChScale.L,
                        link_params.cA.Y * ChScale.L,
                        link_params.cA.Z * ChScale.L)
                    ),
                    new JProperty(!link_params.swapAB_1 ? "_c_Initialize_pt2" : "_c_Initialize_pt1", JObjectCreator.CreateChVector(
                        link_params.cB.X * ChScale.L,
                        link_params.cB.Y * ChScale.L,
                        link_params.cB.Z * ChScale.L)
                    ),
                    new JProperty("m_name", swMateFeature.Name)
                );

                m_ChSystemLinklist.Add(link_node);

            }

            if (link_params.do_ChLinkMatePointLine)
            {
                Vector3D dA_temp;
                if (!link_params.entity_0_as_VERTEX)
                    dA_temp = link_params.dA;
                else
                    dA_temp = new Vector3D(0.0, 0.0, 0.0);

                Vector3D dB_temp;
                if (!link_params.entity_1_as_VERTEX)
                    dB_temp = link_params.dB;
                else
                    dB_temp = new Vector3D(0.0, 0.0, 0.0);


                var link_node = new JObject
                (
                    new JProperty("_type", "ChLinkMateGeneric"),
                    new JProperty("_object_ID", ++m_object_ID_used),
                    new JProperty("_c_SetConstrainedCoords", new JArray(false, true, true, false, false, false)),
                    new JProperty("_c_Initialize_Body1",
                        new JObject(
                            new JProperty("_type", "ChBodyAuxRef"),
                            new JProperty("_reference_ID", Convert.ToUInt32(!link_params.swapAB_1 ? link_params.ref1 : link_params.ref2)))
                    ),
                    new JProperty("_c_Initialize_Body2",
                        new JObject(
                            new JProperty("_type", "ChBodyAuxRef"),
                            new JProperty("_reference_ID", Convert.ToUInt32(!link_params.swapAB_1 ? link_params.ref2 : link_params.ref1)))
                    ),
                    new JProperty("_c_Initialize_pos_are_relative", false),
                    new JProperty(!link_params.swapAB_1 ? "_c_Initialize_pt1" : "_c_Initialize_pt2", JObjectCreator.CreateChVector(
                        link_params.cA.X * ChScale.L,
                        link_params.cA.Y * ChScale.L,
                        link_params.cA.Z * ChScale.L)
                    ),
                    new JProperty(!link_params.swapAB_1 ? "_c_Initialize_pt2" : "_c_Initialize_pt1", JObjectCreator.CreateChVector(
                        link_params.cB.X * ChScale.L,
                        link_params.cB.Y * ChScale.L,
                        link_params.cB.Z * ChScale.L)
                    ),
                    new JProperty(!link_params.swapAB_1 ? "_c_Initialize_norm1" : "_c_Initialize_norm2", JObjectCreator.CreateChVector(dA_temp.X, dA_temp.Y, dA_temp.Z)),
                    new JProperty(!link_params.swapAB_1 ? "_c_Initialize_norm2" : "_c_Initialize_norm1", JObjectCreator.CreateChVector(dB_temp.X, dB_temp.Y, dB_temp.Z)),
                    new JProperty("m_name", swMateFeature.Name)
                );

                m_ChSystemLinklist.Add(link_node);

            }

            // Now, do some other special mate type that did not fall in combinations
            // of link_params.do_ChLinkMatePointLine, link_params.do_ChLinkMateSpherical, etc etc
            if (swMateFeature.GetTypeName2() == "MateHinge")
            {
                // auto flip direction if anti aligned (seems that this is assumed automatically in MateHinge in SW)
                if (Vector3D.DotProduct(link_params.dA, link_params.dB) < 0)
                    link_params.dB.Negate();

                // Hinge constraint must be splitted in two C::E constraints: a coaxial and a point-vs-plane
                var link_node1 = new JObject
                (
                    new JProperty("_type", "ChLinkMateCoaxial"),
                    new JProperty("_object_ID", ++m_object_ID_used),
                    new JProperty("_c_Initialize_Body1",
                        new JObject(
                            new JProperty("_type", "ChBodyAuxRef"),
                            new JProperty("_reference_ID", Convert.ToUInt32(!link_params.swapAB_1 ? link_params.ref1 : link_params.ref2)))
                    ),
                    new JProperty("_c_Initialize_Body2",
                        new JObject(
                            new JProperty("_type", "ChBodyAuxRef"),
                            new JProperty("_reference_ID", Convert.ToUInt32(!link_params.swapAB_1 ? link_params.ref2 : link_params.ref1)))
                    ),
                    new JProperty("_c_Initialize_pos_are_relative", false),
                    new JProperty(!link_params.swapAB_1 ? "_c_Initialize_pt1" : "_c_Initialize_pt2", JObjectCreator.CreateChVector(
                        link_params.cA.X * ChScale.L,
                        link_params.cA.Y * ChScale.L,
                        link_params.cA.Z * ChScale.L)
                    ),
                    new JProperty(!link_params.swapAB_1 ? "_c_Initialize_pt2" : "_c_Initialize_pt1", JObjectCreator.CreateChVector(
                        link_params.cB.X * ChScale.L,
                        link_params.cB.Y * ChScale.L,
                        link_params.cB.Z * ChScale.L)
                    ),
                    new JProperty(!link_params.swapAB_1 ? "_c_Initialize_norm1" : "_c_Initialize_norm2", JObjectCreator.CreateChVector(link_params.dA.X, link_params.dA.Y, link_params.dA.Z)),
                    new JProperty(!link_params.swapAB_1 ? "_c_Initialize_norm2" : "_c_Initialize_norm1", JObjectCreator.CreateChVector(link_params.dB.X, link_params.dB.Y, link_params.dB.Z)),
                    new JProperty("m_name", swMateFeature.Name + "_1")
                );

                m_ChSystemLinklist.Add(link_node1);


                ////////////
                ///
                // TODO: DARIOM provide function CreateLink so that these two links can be interpreted as ChLinkMateCoaxial + ChLinkMateXdistance
                // and the LinkParams struct can avoid duplicated entries 
                var link_node2 = new JObject
                (
                    new JProperty("_type", "ChLinkMateXdistance"),
                    new JProperty("_object_ID", ++m_object_ID_used),
                    new JProperty("_c_Initialize_Body1",
                        new JObject(
                            new JProperty("_type", "ChBodyAuxRef"),
                            new JProperty("_reference_ID", Convert.ToUInt32(!link_params.swapAB_1 ? link_params.ref3 : link_params.ref4)))
                    ),
                    new JProperty("_c_Initialize_Body2",
                        new JObject(
                            new JProperty("_type", "ChBodyAuxRef"),
                            new JProperty("_reference_ID", Convert.ToUInt32(!link_params.swapAB_1 ? link_params.ref4 : link_params.ref3)))
                    ),
                    new JProperty("_c_Initialize_pos_are_relative", false),
                    new JProperty(!link_params.swapAB_1 ? "_c_Initialize_pt1" : "_c_Initialize_pt2", JObjectCreator.CreateChVector(
                        link_params.cC.X * ChScale.L,
                        link_params.cC.Y * ChScale.L,
                        link_params.cC.Z * ChScale.L)
                    ),
                    new JProperty(!link_params.swapAB_1 ? "_c_Initialize_pt2" : "_c_Initialize_pt1", JObjectCreator.CreateChVector(
                        link_params.cD.X * ChScale.L,
                        link_params.cD.Y * ChScale.L,
                        link_params.cD.Z * ChScale.L)
                    ),
                    new JProperty("_c_Initialize_norm2", link_params.entity_2_as_VERTEX ? JObjectCreator.CreateChVector(link_params.dC.X, link_params.dC.Y, link_params.dC.Z) : JObjectCreator.CreateChVector(link_params.dD.X, link_params.dD.Y, link_params.dD.Z)),
                    new JProperty("m_name", swMateFeature.Name + "_2")
                );

                m_ChSystemLinklist.Add(link_node2);

            }


            return true;

        }

        public override void TraverseComponentForVisualShapes(Component2 swComp, long nLevel, int nbody, ref int nvisshape, Component2 chbody_comp)
        {
            CultureInfo bz = new CultureInfo("en-BZ");
            object[] bodies;
            object bodyInfo;
            bodies = (object[])swComp.GetBodies3((int)swBodyType_e.swAllBodies, out bodyInfo);

            if (bodies != null && bodies.Length > 0)
            {
                // Export the component shape to a .OBJ file representing its SW body(s)
                nvisshape += 1;
                string shapename = m_ChBodyAuxRefNode.GetValue("m_name") + "_" + nvisshape;
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




                object foo = null;
                double[] vMatProperties = (double[])swComp.GetMaterialPropertyValues2((int)swInConfigurationOpts_e.swThisConfiguration, foo);


                MathTransform absframe_chbody = chbody_comp.GetTotalTransform(true);
                MathTransform absframe_shape = swComp.GetTotalTransform(true);
                MathTransform absframe_chbody_inv = absframe_chbody.IInverse();
                MathTransform relframe_shape = absframe_shape.IMultiply(absframe_chbody_inv);  // row-ordered transf. -> reverse mult.order!
                double[] amatr = (double[])relframe_shape.ArrayData;
                double[] quat = GetQuaternionFromMatrix(ref relframe_shape);

                // TODO: DARIOM should I check if it is using shape of other objects?
                var _c_AddVisualShape_ChVisualShape = new JObject
                (
                    new JProperty("_type", "ChVisualShapeModelFile"),
                    new JProperty("_object_ID", ++m_object_ID_used),
                    new JProperty("filename", obj_filename)
                );
                if (vMatProperties != null && vMatProperties[0] != -1)
                    _c_AddVisualShape_ChVisualShape.Add("_c_SetColor", new JObject(
                        new JProperty("R", vMatProperties[0]),
                        new JProperty("G", vMatProperties[1]),
                        new JProperty("B", vMatProperties[2])
                        ));

                var _c_AddVisualShape_ChFrame = new JObject // TODO: _object_ID
                (
                    new JProperty("_type", "ChFrame"), // TODO: in the export it seems that the type is not exported
                    new JProperty("_c_SetPos", JObjectCreator.CreateChVector(amatr[9] * ChScale.L, amatr[10] * ChScale.L, amatr[11] * ChScale.L)),
                    new JProperty("_c_SetRot", JObjectCreator.CreateChQuaternion(quat[0], quat[1], quat[2], quat[3]))
                );

                m_ChBodyAuxRefNode.Add("_c_AddVisualShape_ChVisualShapes", new JArray(_c_AddVisualShape_ChVisualShape));
                m_ChBodyAuxRefNode.Add("_c_AddVisualShape_ChFrames", new JArray(_c_AddVisualShape_ChFrame));
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

                        JObject collision_model = new JObject();
                        JObject material_collision = new JObject();

                        if (!found_collisionshapes)
                        {
                            found_collisionshapes = true;

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

                                collision_model = new JObject
                                (
                                    new JProperty("_type", "ChCollisionModel"),
                                    new JProperty("_object_ID", ++m_object_ID_used),
                                    new JProperty("model_envelope", param_collision_envelope * ChScale.L),
                                    new JProperty("model_safe_margin", param_collision_margin * ChScale.L),
                                    new JProperty("param_collision_family", param_collision_family)
                                );

                                material_collision = new JObject
                                (
                                    new JProperty("_type", "ChMaterialSurfaceNSC"),
                                    new JProperty("_object_ID", ++m_object_ID_used),
                                    new JProperty("static_friction", param_friction),
                                    new JProperty("restitution", param_restitution),
                                    new JProperty("rolling_friction", param_rolling_friction),
                                    new JProperty("spinning_friction", param_spinning_friction)
                                );

                            }
                            else {
                                // TODO: provide default and shared material?
                            }

                        }

                        bool has_coll_mesh = false;

                        var shape_instances = new JArray();

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

                                    var gsphere = new JObject
                                    (
                                        new JProperty("_type", "ChSphere"),
                                        new JProperty("_object_ID", ++m_object_ID_used),
                                        new JProperty("rad", rad * ChScale.L)
                                    );

                                    var collshape = new JObject
                                    (
                                        new JProperty("_type", "ChCollisionShapeSphere"),
                                        new JProperty("_object_ID", ++m_object_ID_used),
                                        new JProperty("m_material", material_collision),
                                        new JProperty("gsphere", gsphere)
                                    );


                                    JObject shapeframe = getChFrameObject(
                                        new double[] { center.X * ChScale.L, center.Y * ChScale.L, center.Z * ChScale.L },
                                        new double[] { 1, 0, 0, 0 });


                                    var shapeInstance = new JObject(
                                        new JProperty("1", collshape),
                                        new JProperty("2", shapeframe)
                                    );

                                    shape_instances.Add(shapeInstance);

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

                                    double[] rotmat = new double[] { Dx.X, Dx.Y, Dx.Z, Dy.X, Dy.Y, Dy.Z, Dz.X, Dz.Y, Dz.Z };
                                    double[] quat = GetQuaternionFromMatrix(ref rotmat);

                                    var gbox = new JObject
                                    (
                                        new JProperty("_type", "ChBox"),
                                        new JProperty("_object_ID", ++m_object_ID_used),
                                        new JProperty("lengths", JObjectCreator.CreateChVector(eX.Length * ChScale.L, eY.Length * ChScale.L, eZ.Length * ChScale.L))
                                    );

                                    var collshape = new JObject
                                    (
                                        new JProperty("_type", "ChCollisionShapeBox"),
                                        new JProperty("_object_ID", ++m_object_ID_used),
                                        new JProperty("m_material", material_collision),
                                        new JProperty("gbox", gbox)
                                    );


                                    JObject shapeframe = getChFrameObject(
                                        new double[] { vO.X * ChScale.L, vO.Y * ChScale.L, vO.Z * ChScale.L },
                                        quat);


                                    var shapeInstance = new JObject(
                                        new JProperty("1", collshape),
                                        new JProperty("2", shapeframe)
                                    );

                                    shape_instances.Add(shapeInstance);
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
                                    double height = (p2 - p1).Length;

                                    // TODO: check correctness
                                    Vector3D Dz = new Vector3D(p1.X, p1.Y, p1.Z);
                                    Vector3D Dy = new Vector3D();
                                    Vector3D Dx = new Vector3D();
                                    Vector3D Dtest = new Vector3D();
                                    Dz.Normalize();
                                    Dtest = Vector3D.CrossProduct(Dz, new Vector3D(1, 0, 0)).Length > 1e-5 ? new Vector3D(1, 0, 0) : new Vector3D(0, 1, 0);
                                    Dy = Vector3D.CrossProduct(Dz, Dtest);
                                    Dy.Normalize();
                                    Dx = Vector3D.CrossProduct(Dy, Dz);
                                    Dx.Normalize();
                                    double[] rotmat = new double[] { Dx.X, Dx.Y, Dx.Z, Dy.X, Dy.Y, Dy.Z, Dz.X, Dz.Y, Dz.Z };
                                    double[] quat = GetQuaternionFromMatrix(ref rotmat);


                                    var gcylinder = new JObject
                                    (
                                        new JProperty("_type", "ChCylinder"),
                                        new JProperty("_object_ID", ++m_object_ID_used),
                                        new JProperty("r", rad * ChScale.L),
                                        new JProperty("h", height * ChScale.L)
                                    );

                                    var collshape = new JObject
                                    (
                                        new JProperty("_type", "ChCollisionShapeCylinder"),
                                        new JProperty("_object_ID", ++m_object_ID_used),
                                        new JProperty("m_material", material_collision),
                                        new JProperty("gcylinder", gcylinder)
                                    );


                                    JObject shapeframe = getChFrameObject(
                                        new double[] { p1.X * ChScale.L, p1.Y * ChScale.L, p1.Z * ChScale.L },
                                        quat);


                                    var shapeInstance = new JObject(
                                        new JProperty("1", collshape),
                                        new JProperty("2", shapeframe)
                                    );
                                    shape_instances.Add(shapeInstance);

                                    rbody_converted = true;
                                }

                                if (ConvertToCollisionShapes.SWbodyToConvexHull(swBody, 30) && !rbody_converted)
                                {
                                    Point3D[] vertexes = new Point3D[1]; // will be resized by SWbodyToConvexHull
                                    var points = new JArray();
                                    ConvertToCollisionShapes.SWbodyToConvexHull(swBody, ref vertexes, 30);
                                    if (vertexes.Length > 0)
                                    {
                                        for (int iv = 0; iv < vertexes.Length; iv++)
                                        {
                                            Point3D vert_l = vertexes[iv];
                                            Point3D vert = PointTransform(vert_l, ref collshape_subcomp_transform);
                                            points.Add(JObjectCreator.CreateChVector(vert.X * ChScale.L, vert.Y * ChScale.L, vert.Z * ChScale.L));
                                        }

                                        var collshape = new JObject
                                        (
                                            new JProperty("_type", "ChCollisionShapeConvexHull"),
                                            new JProperty("_object_ID", ++m_object_ID_used),
                                            new JProperty("m_material", material_collision),
                                            new JProperty("points", points)
                                        );


                                        JObject shapeframe = getChFrameObject(
                                            new double[] { 0, 0, 0 },
                                            new double[] { 1, 0, 0, 0 });


                                        var shapeInstance = new JObject(
                                            new JProperty("1", collshape),
                                            new JProperty("2", shapeframe)
                                        );

                                        shape_instances.Add(shapeInstance);
                                        rbody_converted = true;

                                    }


                                }


                            } // end dealing with a collision shape

                        } // end solid bodies traversal for converting to coll.shapes



                        if (has_coll_mesh)
                        {
                            // fallback if no primitive collision shape found: use concave trimesh collision model (although inefficient)
                            ncollshape += 1;
                            Body2 swBody = (Body2)bodies[0];
                            string shapename = swBody.Name;
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
                                    System.Windows.Forms.MessageBox.Show("Cannot write to file: " + obj_filename + ";\n for component: " + swComp.Name2 + " for path name: " + swCompModel.GetPathName());
                                }
                            }
                            else
                            {
                                // reuse the already-saved shape name
                                shapename = (String)m_savedCollisionMeshes[swCompModel.GetPathName()];
                            }

                            double[] amatr = (double[])collshape_subcomp_transform.ArrayData;
                            //double[] quat = GetQuaternionFromMatrix(ref collshape_subcomp_transform);

                            var trimesh = new JObject
                            (
                                new JProperty("_type", "ChTriangleMeshConnected"),
                                new JProperty("_object_ID", ++m_object_ID_used),
                                new JProperty("m_filename", shapename + ".obj"),
                                new JProperty("load_normals", false), // TODO: check why different from default
                                new JProperty("load_uv", true),
                                new JProperty("_c_Transform_ChVector", new JObject(
                                    new JProperty("x", amatr[9] * ChScale.L),
                                    new JProperty("y", amatr[10] * ChScale.L),
                                    new JProperty("z", amatr[11] * ChScale.L)
                                    )
                                ),
                                new JProperty("_c_Transform_ChMatrix33", // TODO: check if order is correct; ChMatrix33 is defined as Eigen::Matrix<Real, 3, 3, Eigen::RowMajor>
                                    new JArray(amatr[0] * ChScale.L, amatr[3] * ChScale.L, amatr[6] * ChScale.L,
                                               amatr[1] * ChScale.L, amatr[4] * ChScale.L, amatr[7] * ChScale.L,
                                               amatr[2] * ChScale.L, amatr[5] * ChScale.L, amatr[8] * ChScale.L
                                    )
                                )
                            );

                            var collshape = new JObject
                            (
                                new JProperty("_type", "ChCollisionShapeTriangleMesh"),
                                new JProperty("_object_ID", ++m_object_ID_used),
                                new JProperty("m_material", material_collision),
                                new JProperty("trimesh", trimesh),
                                new JProperty("is_static", false),
                                new JProperty("is_convex", false),
                                new JProperty("radius", m_swIntegration.m_taskpaneHost.GetNumericSphereSwept().Value.ToString(bz))
                            );


                            JObject shapeframe = getChFrameObject(
                                new double[] { 0, 0, 0 },
                                new double[] { 1, 0, 0, 0 });


                            var shapeInstance = new JObject(
                                new JProperty("1", collshape),
                                new JProperty("2", shapeframe)
                            );
                            shape_instances.Add(shapeInstance);


                            //rbody_converted = true;
                        }


                        m_ChBodyAuxRefNode.Add("collision_model", collision_model);
                    } // end if build_collision_model
                }

            } // end collision shapes export

        }

        public override void TraverseComponentForBodies(Component2 swComp, long nLevel, int nbody)
        {

            CultureInfo bz = new CultureInfo("en-BZ");
            object[] childComponentsArray = (object[])swComp.GetChildren();
            //bool found_chbody_equivalent = false;

            if (nLevel > 1 && !swComp.IsSuppressed() && ((swComp.Solving == (int)swComponentSolvingOption_e.swComponentRigidSolving) || (childComponentsArray.Length == 0)))
            {
                // OK! this is a 'leaf' of the tree of ChBody equivalents (a SDW subassembly or part)

                //found_chbody_equivalent = true;

                this.num_comp++;

                if (m_swIntegration.m_taskpaneHost.GetProgressBar() != null)
                {
                    m_swIntegration.m_taskpaneHost.GetProgressBar().UpdateTitle("Exporting " + swComp.Name2 + " ...");
                    m_swIntegration.m_taskpaneHost.GetProgressBar().UpdateProgress(this.num_comp % 5);
                }

                // fetch SW attribute with Chrono parameters
                SolidWorks.Interop.sldworks.Attribute myattr = (SolidWorks.Interop.sldworks.Attribute)swComp.FindAttribute(m_swIntegration.defattr_chbody, 0);

                MathTransform chbodytransform = swComp.GetTotalTransform(true);
                double[] amatr = (double[])chbodytransform.ArrayData;
                double[] quat = GetQuaternionFromMatrix(ref chbodytransform);

                string sanitized_name = swComp.Name2.Replace("/", "_").Replace("\\", "_");

                m_ChBodyAuxRefNode = new JObject
                {
                    new JProperty("_type", "ChBodyAuxRef"),
                    new JProperty("_object_ID", ++m_object_ID_used),
                    new JProperty("m_name", sanitized_name),
                    new JProperty("_c_SetPos", JObjectCreator.CreateChVector(amatr[9] * ChScale.L, amatr[10] * ChScale.L, amatr[11] * ChScale.L)),
                    new JProperty("_c_SetRot", JObjectCreator.CreateChQuaternion(quat[0], quat[1], quat[2], quat[3]))
                };

                // Compute mass

                int nvalid_bodies = 0;
                TraverseComponent_for_countingmassbodies(swComp, ref nvalid_bodies);

                int addedb = 0; // number of added bodies
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

                m_ChBodyAuxRefNode.Add("_c_SetMass", mass * ChScale.M);
                m_ChBodyAuxRefNode.Add("_c_SetInertiaXX", JObjectCreator.CreateChVector(
                               Ixx * ChScale.M * ChScale.L * ChScale.L,
                               Iyy * ChScale.M * ChScale.L * ChScale.L,
                               Izz * ChScale.M * ChScale.L * ChScale.L));
                //// Note: C::E assumes that's up to you to put a 'minus' sign in values of Ixy, Iyz, Izx
                m_ChBodyAuxRefNode.Add("_c_SetInertiaXY", JObjectCreator.CreateChVector(
                               -Ixy * ChScale.M * ChScale.L * ChScale.L,
                               -Izx * ChScale.M * ChScale.L * ChScale.L,
                               -Iyz * ChScale.M * ChScale.L * ChScale.L));

                m_ChBodyAuxRefNode.Add("_c_SetFrame_COG_to_REF__ChFrame__ChVector", JObjectCreator.CreateChVector(cogXb * ChScale.L, cogYb * ChScale.L, cogZb * ChScale.L));
                m_ChBodyAuxRefNode.Add("_c_SetFrame_COG_to_REF__ChFrame__ChQuaternion", JObjectCreator.CreateChQuaternion(1, 0, 0, 0));

                // Write 'fixed' state
                m_ChBodyAuxRefNode.Add("_c_SetBodyFixed", swComp.IsFixed() ? true : false);



                // Write shapes (saving also Wavefront files .obj)
                if (m_swIntegration.m_taskpaneHost.GetCheckboxSurfaces().Checked)
                {
                    int nvisshape = 0;

                    if (swComp.Visible == (int)swComponentVisibilityState_e.swComponentVisible)
                        TraverseComponentForVisualShapes(swComp, nLevel, nbody, ref nvisshape, swComp);
                }

                // Write markers (SW coordsystems) contained in this component or subcomponents
                // if any.
                TraverseComponentForMarkers(swComp, nLevel, nbody);

                // TODO: Chrono serialization is not capable of handling collisions yet

                //// Write collision shapes (customized SW solid bodies) contained in this component or subcomponents if any.
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
                        m_ChBodyAuxRefNode.Add("_c_SetCollide", true);
                    }
                }

                m_ChSystemBodylist.Add(new JObject(m_ChBodyAuxRefNode));

                // store in hashtable, will be useful later when adding constraints
                // TODO: before, it was after this 'if' with the condition 'if ((nLevel > 1) && (m_object_ID_used != 0))'

                try
                {
                    ModelDocExtension swModelDocExt = default(ModelDocExtension);
                    ModelDoc2 swModel = (ModelDoc2)m_swIntegration.m_swApplication.ActiveDoc;
                    //if (swModel != null)
                    swModelDocExt = swModel.Extension;
                    m_savedParts.Add(swModelDocExt.GetPersistReference3(swComp), m_ChBodyAuxRefNode.GetValue("_object_ID"));
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Cannot add part to hashtable?");
                }


            } // end if ChBody equivalent (tree leaf or non-flexible assembly)
            else
            {
            }


            // Things to do also for sub-components of 'non flexible' assemblies: 
            //

            // Traverse all children, proceeding to subassemblies and parts, if any
            for (long i = 0; i < childComponentsArray.Length; i++)
            {
                Component2 swChildComp = (Component2)childComponentsArray[i];

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

            var marklist = new JArray();

            while ((swFeat != null))
            {
                // asciitext += "# feature: " + swFeat.Name + " [" + swFeat.GetTypeName2() + "]" + "\n";


                // TODO: DARIOM are markers attached to the proper body and in the proper way?
                if (swFeat.GetTypeName2() == "CoordSys")
                {
                    nmarker++;
                    CoordinateSystemFeatureData swCoordSys = (CoordinateSystemFeatureData)swFeat.GetDefinition();
                    MathTransform tr = swCoordSys.Transform;

                    MathTransform tr_part = swCompTotalTrasf;
                    MathTransform tr_abs = tr.IMultiply(tr_part);  // row-ordered transf. -> reverse mult.order!

                    double[] quat = GetQuaternionFromMatrix(ref tr_abs);
                    double[] amatr = (double[])tr_abs.ArrayData;
                    double[] vect = { amatr[9] * ChScale.L, amatr[10] * ChScale.L, amatr[11] * ChScale.L };

                    var marklist_node = new JObject {
                        new JProperty("_type", "ChMarker"),
                        new JProperty("_object_ID", ++m_object_ID_used),
                        new JProperty("m_name", swFeat.Name),
                        // while the ChBody::ArchiveIn would already take care of setting it through AddMarker,
                        // this call will happen after the following Impose_Abs_Coord, thus causing crash
                        new JProperty("Body", new JObject(
                            new JProperty("_type", "ChBodyAuxRef"),
                            new JProperty("_reference_ID", m_ChBodyAuxRefNode.GetValue("_object_ID"))
                            )
                        ),
                        new JProperty("_c_Impose_Abs_Coord__ChCoordsys__ChVector", JObjectCreator.CreateChVector(amatr[9] * ChScale.L, amatr[10] * ChScale.L, amatr[11] * ChScale.L)),
                        new JProperty("_c_Impose_Abs_Coord__ChCoordsys__ChQuaternion", JObjectCreator.CreateChQuaternion(quat[0], quat[1], quat[2], quat[3]))
                    };

                    marklist.Add(marklist_node);
                }

                swFeat = (Feature)swFeat.GetNextFeature();
            }

            m_ChBodyAuxRefNode.Add("markers", marklist);

        }


        private static JObject getChFrameObject(double[] pos, double[] rot) {

            var shapeframe = new JObject
            (
                new JProperty("coord",
                    new JObject(
                        new JProperty("pos",
                            new JObject(
                                new JProperty("x", pos[0]),
                                new JProperty("y", pos[1]),
                                new JProperty("z", pos[2])

                            )
                        ),
                        new JProperty("rot",
                            new JObject(
                                new JProperty("e0", rot[0]),
                                new JProperty("e1", rot[1]),
                                new JProperty("e2", rot[2]),
                                new JProperty("e3", rot[3])

                            )
                        )
                    )
                )
            );

            return shapeframe;

        }

    }
}
