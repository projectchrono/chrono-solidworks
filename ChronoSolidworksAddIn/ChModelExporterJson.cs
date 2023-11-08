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
        private JArray ChSystemLinklistArray;
        private JArray ChSystemBodylistArray;
        private JObject ChBodyAuxRefNode;
        private int _object_ID_used; // identifies last used value of _object_ID, used to uniquely identify any entity in the JSON file

        public ChModelExporterJson(ChronoEngine_SwAddin.SWIntegration swIntegration)
            : base(swIntegration) 
        {
            ChSystemLinklistArray = new JArray();
        }

        public struct JObjectCreator
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

        public void Export(ref JObject ChSystemNode)
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
            _object_ID_used = 0;


            //asciitext += "# Some global settings: \n" +
            //             "sphereswept_r = " + this.numeric_sphereswept.Value.ToString(bz) + "\n" +
            //             "chrono.ChCollisionModel.SetDefaultSuggestedEnvelope(" + ((double)this.numeric_envelope.Value * ChScale.L).ToString(bz) + ")\n" +
            //             "chrono.ChCollisionModel.SetDefaultSuggestedMargin(" + ((double)this.numeric_margin.Value * ChScale.L).ToString(bz) + ")\n" +
            //             "chrono.ChCollisionSystemBullet.SetContactBreakingThreshold(" + ((double)this.numeric_contactbreaking.Value * ChScale.L).ToString(bz) + ")\n\n";


            if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
            {
                // Write down all parts
                var ChSystemBodylistArray = new JArray();

                // Add world-fixed object
                var ChBodyGroundNode = new JObject
                (
                    new JProperty("_type", "ChBodyAuxRef"),
                    new JProperty("_object_ID", ++_object_ID_used),
                    new JProperty("m_name", "ground"),
                    new JProperty("_c_SetBodyFixed", true)
                );

                ChSystemBodylistArray.Add(ChBodyGroundNode);

                TraverseComponentForBodies(swRootComp, 1);

                ChSystemNode.Add("bodies", ChSystemBodylistArray);


                // Write down all constraints

                MathTransform roottrasf = swRootComp.GetTotalTransform(true);
                if (roottrasf == null)
                {
                    IMathUtility swMath = (IMathUtility)m_swIntegration.m_swApplication.GetMathUtility();
                    double[] nulltr = new double[] { 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 };
                    roottrasf = (MathTransform)swMath.CreateTransform(nulltr);
                }

                var ChSystemLinklistArray = new JArray();

                Feature swFeat = (Feature)swModel.FirstFeature();
                TraverseFeaturesForLinks(swFeat, 1, ref roottrasf, ref swRootComp);
                ChSystemNode.Add("links", ChSystemLinklistArray);

                TraverseComponentForLinks(swRootComp, 1, ref roottrasf);


                //// Write down all markers in assembly (that are not in sub parts, so they belong to 'ground' object)
                swFeat = (Feature)swModel.FirstFeature();
                TraverseFeaturesForMarkers(swFeat, 1, roottrasf);

                System.Windows.Forms.MessageBox.Show("Export to JSON completed.\nBody count: " + (ChSystemBodylistArray.Count - 1).ToString() + "\nLink count: " + ChSystemLinklistArray.Count.ToString());


            }


            if (m_swIntegration.m_taskpaneHost.GetProgressBar() != null)
                m_swIntegration.m_taskpaneHost.GetProgressBar().End();
        }



        // ============================================================================================================
        // Override base class methods
        // ============================================================================================================

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
                    new JProperty("_object_ID", ++_object_ID_used),
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

                ChSystemLinklistArray.Add(link_node);
            }

            if (link_params.do_ChLinkMateParallel)
            {
                if (Math.Abs(Vector3D.DotProduct(link_params.dA, link_params.dB)) > 0.98)
                {

                    var link_node = new JObject
                    (
                        new JProperty("_type", "ChLinkMateParallel"),
                        new JProperty("_object_ID", ++_object_ID_used),
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

                    ChSystemLinklistArray.Add(link_node);

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
                        new JProperty("_object_ID", ++_object_ID_used),
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

                    ChSystemLinklistArray.Add(link_node);

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
                    new JProperty("_object_ID", ++_object_ID_used),
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

                ChSystemLinklistArray.Add(link_node);

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
                    new JProperty("_object_ID", ++_object_ID_used),
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

                ChSystemLinklistArray.Add(link_node);

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
                    new JProperty("_object_ID", ++_object_ID_used),
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

                ChSystemLinklistArray.Add(link_node1);


                ////////////
                ///
                // TODO: DARIOM provide function CreateLink so that these two links can be interpreted as ChLinkMateCoaxial + ChLinkMateXdistance
                // and the LinkParams struct can avoid duplicated entries 
                var link_node2 = new JObject
                (
                    new JProperty("_type", "ChLinkMateXdistance"),
                    new JProperty("_object_ID", ++_object_ID_used),
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

                ChSystemLinklistArray.Add(link_node2);

            }


            return true;

        }

        public override void TraverseComponentForVisualShapes(Component2 swComp, long nLevel, ref int nvisshape, Component2 chbody_comp)
        {
            CultureInfo bz = new CultureInfo("en-BZ");
            object[] bodies;
            object bodyInfo;
            bodies = (object[])swComp.GetBodies3((int)swBodyType_e.swAllBodies, out bodyInfo);

            if (bodies != null && bodies.Length > 0)
            {
                // Export the component shape to a .OBJ file representing its SW body(s)
                nvisshape += 1;
                string shapename = ChBodyAuxRefNode.GetValue("m_name") + "_" + nvisshape;
                string obj_filename = this.save_dir_shapes + "\\" + shapename + ".obj";

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

                // TODO: DARIOM should I check if is using shape of other objects?
                var _c_AddVisualShape_ChVisualShape = new JObject
                (
                    new JProperty("_type", "ChModelFileShape"),
                    new JProperty("_object_ID", ++_object_ID_used),
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

                ChBodyAuxRefNode.Add("_c_AddVisualShape_ChVisualShapes", new JArray(_c_AddVisualShape_ChVisualShape));
                ChBodyAuxRefNode.Add("_c_AddVisualShape_ChFrames", new JArray(_c_AddVisualShape_ChFrame));
            }



            // Recursive scan of subcomponents

            Component2 swChildComp;
            object[] vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                if (swChildComp.Visible == (int)swComponentVisibilityState_e.swComponentVisible)
                    TraverseComponentForVisualShapes(swChildComp, nLevel + 1, ref nvisshape, chbody_comp);
            }
        }

        public override void TraverseFeaturesForCollisionShapes(Component2 swComp, long nLevel, ref MathTransform chbodytransform, ref bool found_collisionshapes, Component2 swCompBase, ref int ncollshape)
        {
            //
            // TODO
            //
        }

        public override void TraverseComponentForBodies(Component2 swComp, long nLevel)
        {
            CultureInfo bz = new CultureInfo("en-BZ");
            object[] vmyChildComp = (object[])swComp.GetChildren();
            //bool found_chbody_equivalent = false;

            if (nLevel > 1 & _object_ID_used == 0 & !swComp.IsSuppressed() && (swComp.Solving == (int)swComponentSolvingOption_e.swComponentRigidSolving) || (vmyChildComp.Length == 0))
            {
                // OK! this is a 'leaf' of the tree of ChBody equivalents (a SDW subassebly or part)

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
                double[] amatr;
                amatr = (double[])chbodytransform.ArrayData;

                double[] quat = GetQuaternionFromMatrix(ref chbodytransform);

                ChBodyAuxRefNode = new JObject
                {
                    new JProperty("_type", "ChBodyAuxRef"),
                    new JProperty("_object_ID", ++_object_ID_used),
                    new JProperty("m_name", swComp.Name2),
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

                ChBodyAuxRefNode.Add("_c_SetMass", mass * ChScale.M);
                ChBodyAuxRefNode.Add("_c_SetInertiaXX", JObjectCreator.CreateChVector(
                               Ixx * ChScale.M * ChScale.L * ChScale.L,
                               Iyy * ChScale.M * ChScale.L * ChScale.L,
                               Izz * ChScale.M * ChScale.L * ChScale.L));
                //// Note: C::E assumes that's up to you to put a 'minus' sign in values of Ixy, Iyz, Izx
                ChBodyAuxRefNode.Add("_c_SetInertiaXY", JObjectCreator.CreateChVector(
                               -Ixy * ChScale.M * ChScale.L * ChScale.L,
                               -Izx * ChScale.M * ChScale.L * ChScale.L,
                               -Iyz * ChScale.M * ChScale.L * ChScale.L));

                ChBodyAuxRefNode.Add("_c_SetFrame_COG_to_REF__ChFrame__ChVector", JObjectCreator.CreateChVector(cogXb * ChScale.L, cogYb * ChScale.L, cogZb * ChScale.L));
                ChBodyAuxRefNode.Add("_c_SetFrame_COG_to_REF__ChFrame__ChQuaternion", JObjectCreator.CreateChQuaternion(1, 0, 0, 0));

                // Write 'fixed' state
                ChBodyAuxRefNode.Add("_c_SetBodyFixed", swComp.IsFixed() ? true : false);



                // Write shapes (saving also Wavefront files .obj)
                if (m_swIntegration.m_taskpaneHost.GetCheckboxSurfaces().Checked)
                {
                    int nvisshape = 0;

                    if (swComp.Visible == (int)swComponentVisibilityState_e.swComponentVisible)
                        TraverseComponentForVisualShapes(swComp, nLevel, ref nvisshape, swComp);
                }

                // Write markers (SW coordsystems) contained in this component or subcomponents
                // if any.
                TraverseComponentForMarkers(swComp, nLevel);

                // TODO: Chrono serialization is not capable of handling collisions yet

                //// Write collision shapes (customized SW solid bodies) contained in this component or subcomponents
                //// if any.
                //bool param_collide = true;
                //if (myattr != null)
                //    param_collide = Convert.ToBoolean(((Parameter)myattr.GetParameter("collision_on")).GetDoubleValue());

                //if (param_collide)
                //{
                //    bool found_collisionshapes = false;
                //    int ncollshapes = 0;

                //    PythonTraverseComponent_for_collshapes(swComp, nLevel, ref asciitext, nbody, ref chbodytransform, ref found_collisionshapes, swComp, ref ncollshapes);
                //    if (found_collisionshapes)
                //    {
                //        asciitext += String.Format(bz, "{0}.GetCollisionModel().BuildModel()\n", bodyname);
                //        asciitext += String.Format(bz, "{0}.SetCollide(True)\n", bodyname);
                //    }
                //}

                ChSystemBodylistArray.Add(ChBodyAuxRefNode);

                // store in hashtable, will be useful later when adding constraints
                // TODO: before, it was after this 'if' with the condition 'if ((nLevel > 1) && (_object_ID_used != 0))'

                try
                {
                    ModelDocExtension swModelDocExt = default(ModelDocExtension);
                    ModelDoc2 swModel = (ModelDoc2)m_swIntegration.m_swApplication.ActiveDoc;
                    //if (swModel != null)
                    swModelDocExt = swModel.Extension;
                    m_savedParts.Add(swModelDocExt.GetPersistReference3(swComp), ChBodyAuxRefNode.GetValue("_object_ID"));
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Cannot add part to hashtable?");
                }

            } // end if ChBody equivalent (tree leaf or non-flexible assembly)


            // Things to do also for sub-components of 'non flexible' assemblies: 
            //




            // Traverse all children, proceeding to subassemblies and parts, if any
            // 

            object[] vChildComp;
            Component2 swChildComp;

            vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                TraverseComponentForBodies(swChildComp, nLevel + 1);
            }


        }

        public override void TraverseComponentForMarkers(Component2 swComp, long nLevel)
        {
            // Look if component contains markers
            Feature swFeat = (Feature)swComp.FirstFeature();
            MathTransform swCompTotalTrasf = swComp.GetTotalTransform(true);
            TraverseFeaturesForMarkers(swFeat, nLevel, swCompTotalTrasf);

            // Recursive scan of subcomponents

            Component2 swChildComp;
            object[] vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                TraverseComponentForMarkers(swChildComp, nLevel + 1);
            }
        }

        public override void TraverseFeaturesForMarkers(Feature swFeat, long nLevel, MathTransform swCompTotalTrasf)
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
                        new JProperty("_object_ID", ++_object_ID_used),
                        new JProperty("m_name", swFeat.Name),
                        // while the ChBody::ArchiveIn would already take care of setting it through AddMarker,
                        // this call will happen after the following Impose_Abs_Coord, thus causing crash
                        new JProperty("Body", new JObject(
                            new JProperty("_type", "ChBodyAuxRef"),
                            new JProperty("_reference_ID", ChBodyAuxRefNode.GetValue("_object_ID"))
                            )
                        ),
                        new JProperty("_c_Impose_Abs_Coord__ChCoordsys__ChVector", JObjectCreator.CreateChVector(amatr[9] * ChScale.L, amatr[10] * ChScale.L, amatr[11] * ChScale.L)),
                        new JProperty("_c_Impose_Abs_Coord__ChCoordsys__ChQuaternion", JObjectCreator.CreateChQuaternion(quat[0], quat[1], quat[2], quat[3]))
                    };

                    marklist.Add(marklist_node);
                }

                swFeat = (Feature)swFeat.GetNextFeature();
            }

            ChBodyAuxRefNode.Add("markers", marklist);

        }
   
    }
}
