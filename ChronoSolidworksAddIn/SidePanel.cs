using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Windows.Media.Media3D;
using System.IO;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swcommands;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using SolidWorksTools;
using ChronoEngineAddin;

using Microsoft.Win32;
//using ConvertToCollisionShapes;



// for JSON export
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows;


// TODO: DARIOM replace PythonXXX and JsonXXX functions with overloaded variants with samename if possible

namespace ChronoEngine_SwAddin
{

    public struct JObjectCreator {
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


    [ComVisible(true)]
    [ProgId(SWTASKPANE_PROGID)]
    public partial class SWTaskpaneHost : UserControl
    {
        public const string SWTASKPANE_PROGID = "ChronoEngine.Taskpane";
        public ISldWorks mSWApplication;
        public SWIntegration mSWintegration;
        internal System.Windows.Forms.SaveFileDialog SaveFileDialog1;
        internal int num_comp;
        internal int _object_ID_used; // identifies last used value of _object_ID, used to uniquely identify any entity in the JSON file
        internal string save_dir_shapes = "";
        internal string save_filename = "";
        internal UserProgressBar swProgress;
        internal Hashtable saved_parts;
        internal Hashtable saved_shapes;
        internal Hashtable saved_collisionmeshes;


        class myBytearrayHashComparer : IEqualityComparer
        {
            public new bool Equals(object x, object y)
            {
                byte[] a1 = (byte[])x;
                byte[] a2 = (byte[])y;
                if (a1.Length != a2.Length)
                    return false;

                for (int i = 0; i < a1.Length; i++)
                    if (a1[i] != a2[i])
                        return false;

                return true;
            }

            public int GetHashCode(object x)
            {
                byte[] obj = (byte[])x;
                if (obj == null || obj.Length == 0)
                    return 0;
                var hashCode = 0;
                for (var i = 0; i < obj.Length; i++)
                    // Rotate by 3 bits and XOR the new value.
                    hashCode = (hashCode << 3) | (hashCode >> (29)) ^ obj[i];
                return hashCode;
            }

        }


        public SWTaskpaneHost()
        {
            InitializeComponent();
            this.SaveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.saved_parts = new Hashtable(new myBytearrayHashComparer());
            this.saved_shapes = new Hashtable();
            this.saved_collisionmeshes = new Hashtable();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {

        }

        private void Export_Click(object sender, EventArgs e)
        {
            ChScale.L = (double)this.numeric_scale_L.Value;
            ChScale.M = (double)this.numeric_scale_M.Value;
            ChScale.T = (double)this.numeric_scale_T.Value;

            ModelDoc2 swModel;
            swModel = (ModelDoc2)this.mSWApplication.ActiveDoc;
            if (swModel == null)
            {
                System.Windows.Forms.MessageBox.Show("Please open an assembly!");
                return;
            }
            if (swModel.GetType() != (int)swDocumentTypes_e.swDocASSEMBLY)
            {
                System.Windows.Forms.MessageBox.Show("Please use this command when an assembly is open.");
                return;
            }

            if ((sender as Button).Name.ToString() == "button_ExportToPython")
            {
                this.SaveFileDialog1.Filter = "PyChrono Python script (*.py)|*.py";
                this.SaveFileDialog1.DefaultExt = "py";
            }
            else if ((sender as Button).Name.ToString() == "button_ExportToJson")
            {
                this.SaveFileDialog1.Filter = "Chrono JSON File (*.json)|*.json";
                this.SaveFileDialog1.DefaultExt = "json";
            }
            //this.SaveFileDialog1.FileName = "mechanism";
            this.SaveFileDialog1.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            DialogResult result = SaveFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {

                this.save_filename = SaveFileDialog1.FileName;

                if (this.checkBox_surfaces.Checked)
                {
                    string save_directory = System.IO.Path.GetDirectoryName(SaveFileDialog1.FileName);
                    this.save_dir_shapes = save_directory + "\\" + System.IO.Path.GetFileNameWithoutExtension(this.save_filename) + "_shapes";
                    //System.Windows.Forms.MessageBox.Show("DIRECTORY FOR SHAPES: " + save_dir_shapes);
                    DirectoryInfo mi = System.IO.Directory.CreateDirectory(this.save_dir_shapes);
                    if (mi.Exists == false)
                        System.Windows.Forms.MessageBox.Show("ERROR. Can't create directory for .obj surfaces: " + this.save_dir_shapes);

                    // ***TEST*** Dump also hierarchy for test
                    string asciidump = "";
                    this.ExportToDump(ref asciidump);
                    FileStream dumpstream = new FileStream(this.save_dir_shapes + "\\" + "dump_log.txt", FileMode.Create, FileAccess.ReadWrite);
                    StreamWriter dumpwriter = new StreamWriter(dumpstream);
                    dumpwriter.Write(asciidump);
                    dumpwriter.Flush();
                    dumpstream.Close();
                }

                // Do the conversion into a Python text block!!!
                // This will scan the low level hierarchy of the assembly
                // and its mating constraints and create the proper Chrono::Engine
                // links.


                if ((sender as Button).Name.ToString() == "button_ExportToPython")
                {
                    string asciitext = "";

                    this.ExportToPython(ref asciitext);

                    byte[] byteArray = Encoding.ASCII.GetBytes(asciitext);
                    MemoryStream stream = new MemoryStream(byteArray);

                    Stream fileStream;
                    fileStream = SaveFileDialog1.OpenFile();
                    stream.Position = 0;
                    stream.WriteTo(fileStream);
                    fileStream.Close();
                }
                else if ((sender as Button).Name.ToString() == "button_ExportToJson")
                {
                    var ChSystemNode = new JObject();
                    this.ExportToJson(ref ChSystemNode);

                    var ChSystemWrapper = new JObject(new JProperty("system", ChSystemNode));

                    File.WriteAllText(SaveFileDialog1.FileName, ChSystemWrapper.ToString(Formatting.Indented));

                }




                if (this.checkBox_savetest.Checked && sender.ToString() == "button_ExportToPython") // TODO: Json cannot handle collisions yet
                {
                    string save_directory = System.IO.Path.GetDirectoryName(SaveFileDialog1.FileName);
                    try
                    {
                        string InstallPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\ChronoSolidworks", "InstallPath", "CHRONO_SOLIDWORKS_PATH_NOT_FOUND");
                        //System.Windows.Forms.MessageBox.Show("GetInstallPath: " + InstallPath);
                        //System.Windows.Forms.MessageBox.Show("GetCurrentWorkingDirectory: " + this.mSWApplication.GetCurrentWorkingDirectory());
                        //System.Windows.Forms.MessageBox.Show("GetExecutablePath: " + this.mSWApplication.GetExecutablePath());
                        //System.Windows.Forms.MessageBox.Show("GetDataFolder: " + this.mSWApplication.GetDataFolder(true));

                        if (!System.IO.File.Exists(save_directory + "\\run_test.py"))
                            System.IO.File.Copy(InstallPath + "\\run_test.py", save_directory + "\\run_test.py");
                        if (!System.IO.File.Exists(save_directory + "\\_template_POV.pov"))
                            System.IO.File.Copy(InstallPath + "\\_template_POV.pov", save_directory + "\\_template_POV.pov");
                    }
                    catch (Exception exc)
                    {
                        System.Windows.Forms.MessageBox.Show("Cannot write the test Python program. \n Make sure that the template chrono_solidworks\\run_test.py is in your SolidWorks directory.\n\n" + exc.Message);
                    }
                }
            }

            // System.Windows.Forms.MessageBox.Show("Ok, saved");

        }


        //
        // Traverse for Python dumping
        //

        public void ExportToPython(ref string asciitext)
        {
            CultureInfo bz = new CultureInfo("en-BZ");

            ModelDoc2 swModel;
            ConfigurationManager swConfMgr;
            Configuration swConf;
            Component2 swRootComp;

            this.saved_parts.Clear();
            this.saved_shapes.Clear();
            this.saved_collisionmeshes.Clear();

            swModel = (ModelDoc2)this.mSWApplication.ActiveDoc;
            if (swModel == null) return;
            swConfMgr = (ConfigurationManager)swModel.ConfigurationManager;
            swConf = (Configuration)swConfMgr.ActiveConfiguration;
            swRootComp = (Component2)swConf.GetRootComponent3(true);

            this.mSWApplication.GetUserProgressBar(out this.swProgress);
            if (this.swProgress != null)
                this.swProgress.Start(0, 5, "Exporting to Python");

            num_comp = 0;

            asciitext = "# PyChrono script generated from SolidWorks using Chrono::SolidWorks add-in \n" +
                        "# Assembly: " + swModel.GetPathName() + "\n\n\n";

            asciitext += "import pychrono as chrono \n";
            asciitext += "import builtins \n\n";

            asciitext += "# Some global settings: \n" +
                         "sphereswept_r = " + this.numeric_sphereswept.Value.ToString(bz) + "\n" +
                         "chrono.ChCollisionModel.SetDefaultSuggestedEnvelope(" + ((double)this.numeric_envelope.Value * ChScale.L).ToString(bz) + ")\n" +
                         "chrono.ChCollisionModel.SetDefaultSuggestedMargin(" + ((double)this.numeric_margin.Value * ChScale.L).ToString(bz) + ")\n" +
                         "chrono.ChCollisionSystemBullet.SetContactBreakingThreshold(" + ((double)this.numeric_contactbreaking.Value * ChScale.L).ToString(bz) + ")\n\n";

            asciitext += "shapes_dir = '" + System.IO.Path.GetFileNameWithoutExtension(this.save_filename) + "_shapes" + "/' \n\n";

            asciitext += "if hasattr(builtins, 'exported_system_relpath'): \n";
            asciitext += "    shapes_dir = builtins.exported_system_relpath + shapes_dir \n\n";

            asciitext += "exported_items = [] \n\n";

            asciitext += "body_0 = chrono.ChBodyAuxRef()\n" +
                         "body_0.SetName('ground')\n" +
                         "body_0.SetBodyFixed(True)\n" +
                         "exported_items.append(body_0)\n\n";



            if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
            {
                // Write down all parts

                PythonTraverseComponent_for_ChBody(swRootComp, 1, ref asciitext, -1);


                // Write down all constraints

                MathTransform roottrasf = swRootComp.GetTotalTransform(true);
                if (roottrasf == null)
                {
                    IMathUtility swMath = (IMathUtility)this.mSWApplication.GetMathUtility();
                    double[] nulltr = new double[] { 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 };
                    roottrasf = (MathTransform)swMath.CreateTransform(nulltr);
                }

                Feature swFeat = (Feature)swModel.FirstFeature();
                PythonTraverseFeatures_for_links(swFeat, 1, ref asciitext, ref roottrasf, ref swRootComp);

                PythonTraverseComponent_for_links(swRootComp, 1, ref asciitext, ref roottrasf);


                // Write down all markers in assembly (that are not in sub parts, so they belong to 'ground' object)

                swFeat = (Feature)swModel.FirstFeature();
                PythonTraverseFeatures_for_markers(swFeat, 1, ref asciitext, 0, roottrasf);

            }

            System.Windows.Forms.MessageBox.Show("Export to Python completed.");

            if (this.swProgress != null)
                this.swProgress.End();
        }


        public void ExportToJson(ref JObject ChSystemNode)
        {
            CultureInfo bz = new CultureInfo("en-BZ");

            ModelDoc2 swModel;
            ConfigurationManager swConfMgr;
            Configuration swConf;
            Component2 swRootComp;

            this.saved_parts.Clear();
            this.saved_shapes.Clear();
            this.saved_collisionmeshes.Clear();

            swModel = (ModelDoc2)this.mSWApplication.ActiveDoc;
            if (swModel == null) return;
            swConfMgr = (ConfigurationManager)swModel.ConfigurationManager;
            swConf = (Configuration)swConfMgr.ActiveConfiguration;
            swRootComp = (Component2)swConf.GetRootComponent3(true);

            this.mSWApplication.GetUserProgressBar(out this.swProgress);
            if (this.swProgress != null)
                this.swProgress.Start(0, 5, "Exporting to JSON");

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

                JsonTraverseComponent_for_ChBody(swRootComp, 1, ref ChSystemBodylistArray);

                ChSystemNode.Add("bodies", ChSystemBodylistArray);


                // Write down all constraints

                MathTransform roottrasf = swRootComp.GetTotalTransform(true);
                if (roottrasf == null)
                {
                    IMathUtility swMath = (IMathUtility)this.mSWApplication.GetMathUtility();
                    double[] nulltr = new double[] { 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 };
                    roottrasf = (MathTransform)swMath.CreateTransform(nulltr);
                }

                var ChSystemLinklistArray = new JArray();

                Feature swFeat = (Feature)swModel.FirstFeature();
                JsonTraverseFeatures_for_links(swFeat, 1, ref ChSystemLinklistArray, ref roottrasf, ref swRootComp);
                ChSystemNode.Add("links", ChSystemLinklistArray);

                JsonTraverseComponent_for_links(swRootComp, 1, ref ChSystemLinklistArray, ref roottrasf);


                //// Write down all markers in assembly (that are not in sub parts, so they belong to 'ground' object)
                swFeat = (Feature)swModel.FirstFeature();
                JsonTraverseFeatures_for_markers(swFeat, 1, ref ChBodyGroundNode, roottrasf);

                System.Windows.Forms.MessageBox.Show("Export to JSON completed.\nBody count: " + (ChSystemBodylistArray.Count - 1).ToString() + "\nLink count: " + ChSystemLinklistArray.Count.ToString());


            }


            if (this.swProgress != null)
                this.swProgress.End();
        }


        //
        // LINK EXPORTING FUNCTIONS
        // 

        public void PythonTraverseComponent_for_links(Component2 swComp, long nLevel, ref string asciitext, ref MathTransform roottrasf)
        {
            // Scan assembly features and save mating info

            if (nLevel > 1)
            {
                Feature swFeat = (Feature)swComp.FirstFeature();
                PythonTraverseFeatures_for_links(swFeat, nLevel, ref asciitext, ref roottrasf, ref swComp);
            }

            // Recursive scan of subassemblies

            object[] vChildComp;
            Component2 swChildComp;

            vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                if (swChildComp.Solving == (int)swComponentSolvingOption_e.swComponentFlexibleSolving)
                    PythonTraverseComponent_for_links(swChildComp, nLevel + 1, ref asciitext, ref roottrasf);
            }
        }

        public void PythonTraverseFeatures_for_links(Feature swFeat, long nLevel, ref string asciitext, ref MathTransform roottrasf, ref Component2 assemblyofmates)
        {
            Feature swSubFeat;

            int num_link = 0;

            while ((swFeat != null))
            {
                // Export mates as constraints

                if ((swFeat.GetTypeName2() == "MateGroup") &&
                    (this.checkBox_constraints.Checked))
                {
                    swSubFeat = (Feature)swFeat.GetFirstSubFeature();

                    while ((swSubFeat != null))
                    {
                        ConvertMates.ConvertMateToPython(swSubFeat, ref asciitext, mSWApplication, saved_parts, ref num_link, roottrasf, assemblyofmates);

                        swSubFeat = (Feature)swSubFeat.GetNextSubFeature();

                    } // end while loop on subfeatures mates

                } // end if mate group

                swFeat = (Feature)swFeat.GetNextFeature();

            } // end while loop on features

        }

        public void JsonTraverseComponent_for_links(Component2 swComp, long nLevel, ref JArray ChSystemLinklistArray, ref MathTransform roottrasf)
        {
            // Scan assembly features and save mating info

            if (nLevel > 1)
            {
                Feature swFeat = (Feature)swComp.FirstFeature();
                JsonTraverseFeatures_for_links(swFeat, nLevel, ref ChSystemLinklistArray, ref roottrasf, ref swComp);
            }

            // Recursive scan of subassemblies

            object[] vChildComp;
            Component2 swChildComp;

            vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                if (swChildComp.Solving == (int)swComponentSolvingOption_e.swComponentFlexibleSolving)
                    JsonTraverseComponent_for_links(swChildComp, nLevel + 1, ref ChSystemLinklistArray, ref roottrasf);
            }
        }

        public void JsonTraverseFeatures_for_links(Feature swFeat, long nLevel, ref JArray ChSystemLinklistArray, ref MathTransform roottrasf, ref Component2 assemblyofmates)
        {
            Feature swSubFeat;

            while ((swFeat != null))
            {
                // Export mates as constraints

                if ((swFeat.GetTypeName2() == "MateGroup") &&
                    (this.checkBox_constraints.Checked))
                {
                    swSubFeat = (Feature)swFeat.GetFirstSubFeature();

                    while ((swSubFeat != null))
                    {
                        ConvertMates.ConvertMateToJson(swSubFeat, ref ChSystemLinklistArray, mSWApplication, saved_parts, ref _object_ID_used, roottrasf, assemblyofmates);

                        swSubFeat = (Feature)swSubFeat.GetNextSubFeature();

                    } // end while loop on subfeatures mates

                } // end if mate group

                swFeat = (Feature)swFeat.GetNextFeature();

            } // end while loop on features

        }



        //
        // BODY EXPORTING FUNCTIONS
        // 

        public void PythonTraverseComponent_for_markers(Component2 swComp, long nLevel, ref string asciitext, int nbody)
        {
            // Look if component contains markers
            Feature swFeat = (Feature)swComp.FirstFeature();
            MathTransform swCompTotalTrasf = swComp.GetTotalTransform(true);
            PythonTraverseFeatures_for_markers(swFeat, nLevel, ref asciitext, nbody, swCompTotalTrasf);

            // Recursive scan of subcomponents

            Component2 swChildComp;
            object[] vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                PythonTraverseComponent_for_markers(swChildComp, nLevel + 1, ref asciitext, nbody);
            }
        }

        public void PythonTraverseFeatures_for_markers(Feature swFeat, long nLevel, ref string asciitext, int nbody, MathTransform swCompTotalTrasf)
        {
            CultureInfo bz = new CultureInfo("en-BZ");

            int nmarker = 0;

            String bodyname = "body_" + nbody;

            while ((swFeat != null))
            {
                // asciitext += "# feature: " + swFeat.Name + " [" + swFeat.GetTypeName2() + "]" + "\n";

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
                    asciitext += "\n# Auxiliary marker (coordinate system feature)\n";
                    asciitext += String.Format(bz, "{0} =chrono.ChMarker()\n", markername);
                    asciitext += String.Format(bz, "{0}.SetName('{1}')" + "\n", markername, swFeat.Name);
                    asciitext += String.Format(bz, "{0}.AddMarker({1})\n", bodyname, markername);
                    asciitext += String.Format(bz, "{0}.Impose_Abs_Coord(chrono.ChCoordsysD(chrono.ChVectorD({1},{2},{3}),chrono.ChQuaternionD({4},{5},{6},{7})))\n",
                               markername,
                               amatr[9] * ChScale.L,
                               amatr[10] * ChScale.L,
                               amatr[11] * ChScale.L,
                               quat[0], quat[1], quat[2], quat[3]);
                }

                swFeat = (Feature)swFeat.GetNextFeature();
            }
        }

        public void JsonTraverseComponent_for_markers(Component2 swComp, long nLevel, ref JObject ChBodyAuxRefNode)
        {
            // Look if component contains markers
            Feature swFeat = (Feature)swComp.FirstFeature();
            MathTransform swCompTotalTrasf = swComp.GetTotalTransform(true);
            JsonTraverseFeatures_for_markers(swFeat, nLevel, ref ChBodyAuxRefNode, swCompTotalTrasf);

            // Recursive scan of subcomponents

            Component2 swChildComp;
            object[] vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                JsonTraverseComponent_for_markers(swChildComp, nLevel + 1, ref ChBodyAuxRefNode);
            }
        }

        public void JsonTraverseFeatures_for_markers(Feature swFeat, long nLevel, ref JObject ChBodyAuxRefNode, MathTransform swCompTotalTrasf)
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


        public void PythonTraverseComponent_for_visualizationshapes(Component2 swComp, long nLevel, ref string asciitext, int nbody, ref int nvisshape, Component2 chbody_comp)
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
                    string obj_filename = this.save_dir_shapes + "\\" + shapename + ".obj";

                    ModelDoc2 swCompModel = (ModelDoc2)swComp.GetModelDoc();
                    if (!this.saved_shapes.ContainsKey(swCompModel.GetPathName()))
                    {
                        try
                        {
                            FileStream ostream = new FileStream(obj_filename, FileMode.Create, FileAccess.ReadWrite);
                            StreamWriter writer = new StreamWriter(ostream); //, new UnicodeEncoding());
                            string asciiobj = "";
                            if (this.swProgress != null)
                                this.swProgress.UpdateTitle("Exporting " + swComp.Name2 + " (tesselate) ...");
                            // Write the OBJ converted visualization shapes:
                            TesselateToObj.Convert(swComp, ref asciiobj, this.checkBox_saveUV.Checked, ref this.swProgress, true, false);
                            writer.Write(asciiobj);
                            writer.Flush();
                            ostream.Close();

                            this.saved_shapes.Add(swCompModel.GetPathName(), shapename);
                        }
                        catch (Exception)
                        {
                            System.Windows.Forms.MessageBox.Show("Cannot write to file: " + obj_filename + "\n for component: " + swComp.Name2 + " for path name: " + swCompModel.GetPathName());
                        }
                    }
                    else
                    {
                        // reuse the already-saved shape name
                        shapename = (String)saved_shapes[swCompModel.GetPathName()];
                    }

                    asciitext += String.Format(bz, "\n# Visualization shape \n", bodyname);
                    asciitext += String.Format(bz, "{0}_shape = chrono.ChModelFileShape() \n", shapename);
                    asciitext += String.Format(bz, "{0}_shape.SetFilename(shapes_dir +'{0}.obj') \n", shapename);

                    object foo = null;
                    double[] vMatProperties = (double[])swComp.GetMaterialPropertyValues2((int)swInConfigurationOpts_e.swThisConfiguration, foo);

                    if (vMatProperties != null)
                        if (vMatProperties[0] != -1)
                        {
                            asciitext += String.Format(bz, "{0}_shape.SetColor(chrono.ChColor({1},{2},{3})) \n", shapename,
                                vMatProperties[0], vMatProperties[1], vMatProperties[2]);
                            asciitext += String.Format(bz, "{0}_shape.SetFading({1}) \n", shapename, vMatProperties[7]);
                        }

                    MathTransform absframe_chbody = chbody_comp.GetTotalTransform(true);
                    MathTransform absframe_shape = swComp.GetTotalTransform(true);
                    MathTransform absframe_chbody_inv = absframe_chbody.IInverse();
                    MathTransform relframe_shape = absframe_shape.IMultiply(absframe_chbody_inv);  // row-ordered transf. -> reverse mult.order!
                    double[] amatr = (double[])relframe_shape.ArrayData;
                    double[] quat = GetQuaternionFromMatrix(ref relframe_shape);

                    asciitext += String.Format(bz, "{0}.AddVisualShape({1}_shape, chrono.ChFrameD(", bodyname, shapename);
                    asciitext += String.Format(bz, "chrono.ChVectorD({0},{1},{2}), ", amatr[9] * ChScale.L, amatr[10] * ChScale.L, amatr[11] * ChScale.L);
                    asciitext += String.Format(bz, "chrono.ChQuaternionD({0},{1},{2},{3})", quat[0], quat[1], quat[2], quat[3]);
                    asciitext += String.Format(bz, "))\n");
                }



            // Recursive scan of subcomponents

            Component2 swChildComp;
            object[] vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                if (swChildComp.Visible == (int)swComponentVisibilityState_e.swComponentVisible)
                    PythonTraverseComponent_for_visualizationshapes(swChildComp, nLevel + 1, ref asciitext, nbody, ref nvisshape, chbody_comp);
            }
        }

        public void JsonTraverseComponent_for_visualizationshapes(Component2 swComp, long nLevel, ref JObject ChBodyAuxRefNode, ref int nvisshape, Component2 chbody_comp)
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
                if (!this.saved_shapes.ContainsKey(swCompModel.GetPathName()))
                {
                    try
                    {
                        FileStream ostream = new FileStream(obj_filename, FileMode.Create, FileAccess.ReadWrite);
                        StreamWriter writer = new StreamWriter(ostream); //, new UnicodeEncoding());
                        string asciiobj = "";
                        if (this.swProgress != null)
                            this.swProgress.UpdateTitle("Exporting " + swComp.Name2 + " (tesselate) ...");
                        // Write the OBJ converted visualization shapes:
                        TesselateToObj.Convert(swComp, ref asciiobj, this.checkBox_saveUV.Checked, ref this.swProgress, true, false);
                        writer.Write(asciiobj);
                        writer.Flush();
                        ostream.Close();

                        this.saved_shapes.Add(swCompModel.GetPathName(), shapename);
                    }
                    catch (Exception)
                    {
                        System.Windows.Forms.MessageBox.Show("Cannot write to file: " + obj_filename + "\n for component: " + swComp.Name2 + " for path name: " + swCompModel.GetPathName());
                    }
                }
                else
                {
                    // reuse the already-saved shape name
                    shapename = (String)saved_shapes[swCompModel.GetPathName()];
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
                    JsonTraverseComponent_for_visualizationshapes(swChildComp, nLevel + 1, ref ChBodyAuxRefNode, ref nvisshape, chbody_comp);
            }
        }


        public void PythonTraverseComponent_for_collshapes(Component2 swComp, long nLevel, ref string asciitext, int nbody, ref MathTransform chbodytransform, ref bool found_collisionshapes, Component2 swCompBase, ref int ncollshape)
        {
            // Look if component contains collision shapes (customized SW solid bodies):
            PythonTraverseFeatures_for_collshapes(swComp, nLevel, ref asciitext, nbody, ref chbodytransform, ref found_collisionshapes, swCompBase, ref ncollshape);

            // Recursive scan of subcomponents

            Component2 swChildComp;
            object[] vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                PythonTraverseComponent_for_collshapes(swChildComp, nLevel + 1, ref asciitext, nbody, ref chbodytransform, ref found_collisionshapes, swCompBase, ref ncollshape);
            }
        }

        public void PythonTraverseFeatures_for_collshapes(Component2 swComp, long nLevel, ref string asciitext, int nbody, ref MathTransform chbodytransform, ref bool found_collisionshapes, Component2 swCompBase, ref int ncollshape)
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
            if (this.checkBox_collshapes.Checked)
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
                            SolidWorks.Interop.sldworks.Attribute myattr = (SolidWorks.Interop.sldworks.Attribute)swCompBase.FindAttribute(this.mSWintegration.defattr_chbody, 0);


                            asciitext += "\n# Collision material \n";

                            asciitext += String.Format(bz, "{0} = chrono.ChMaterialSurfaceNSC()\n", matname);



                            if (myattr != null)
                            {

                                asciitext += "\n# Collision parameters \n";
                                double param_friction = ((Parameter)myattr.GetParameter("friction")).GetDoubleValue();
                                double param_restitution = ((Parameter)myattr.GetParameter("restitution")).GetDoubleValue();
                                double param_rolling_friction = ((Parameter)myattr.GetParameter("rolling_friction")).GetDoubleValue();
                                double param_spinning_friction = ((Parameter)myattr.GetParameter("spinning_friction")).GetDoubleValue();
                                double param_collision_envelope = ((Parameter)myattr.GetParameter("collision_envelope")).GetDoubleValue();
                                double param_collision_margin = ((Parameter)myattr.GetParameter("collision_margin")).GetDoubleValue();
                                int param_collision_family = (int)((Parameter)myattr.GetParameter("collision_family")).GetDoubleValue();

                                asciitext += String.Format(bz, "{0}.SetFriction({1:g})\n", matname, param_friction);
                                if (param_restitution != 0)
                                    asciitext += String.Format(bz, "{0}.SetRestitution({1:g})\n", matname, param_restitution);
                                if (param_rolling_friction != 0)
                                    asciitext += String.Format(bz, "{0}.SetRollingFriction({1:g})\n", matname, param_rolling_friction);
                                if (param_spinning_friction != 0)
                                    asciitext += String.Format(bz, "{0}.SetSpinningFriction({1:g})\n", matname, param_spinning_friction);
                                //if (param_collision_envelope != 0.03)
                                asciitext += String.Format(bz, "{0}.GetCollisionModel().SetEnvelope({1:g})\n", bodyname, param_collision_envelope * ChScale.L);
                                //if (param_collision_margin != 0.01)
                                asciitext += String.Format(bz, "{0}.GetCollisionModel().SetSafeMargin({1:g})\n", bodyname, param_collision_margin * ChScale.L);
                                if (param_collision_family != 0)
                                    asciitext += String.Format(bz, "{0}.GetCollisionModel().SetFamily({1})\n", bodyname, param_collision_family);
                            }

                            // clear model only at 1st subcomponent where coll shapes are found in features:
                            asciitext += "\n# Collision shapes \n";
                            asciitext += String.Format(bz, "{0}.GetCollisionModel().ClearModel()\n", bodyname);
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
                                    Point3D center = SWTaskpaneHost.PointTransform(center_l, ref collshape_subcomp_transform);
                                    asciitext += String.Format(bz, "{0}.GetCollisionModel().AddSphere({1}, {2}, chrono.ChVectorD({3},{4},{5}))\n",
                                        bodyname, matname,
                                        rad * ChScale.L,
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
                                    Point3D vC = SWTaskpaneHost.PointTransform(vC_l, ref collshape_subcomp_transform);
                                    Vector3D eX = SWTaskpaneHost.DirTransform(eX_l, ref collshape_subcomp_transform);
                                    Vector3D eY = SWTaskpaneHost.DirTransform(eY_l, ref collshape_subcomp_transform);
                                    Vector3D eZ = SWTaskpaneHost.DirTransform(eZ_l, ref collshape_subcomp_transform);
                                    Point3D vO = vC + 0.5 * eX + 0.5 * eY + 0.5 * eZ;
                                    Vector3D Dx = eX; Dx.Normalize();
                                    Vector3D Dy = eY; Dy.Normalize();
                                    Vector3D Dz = Vector3D.CrossProduct(Dx, Dy);
                                    asciitext += String.Format(bz, "mr = chrono.ChMatrix33D()\n");
                                    asciitext += String.Format(bz, "mr[0,0]={0}; mr[1,0]={1}; mr[2,0]={2} \n", Dx.X, Dx.Y, Dx.Z);
                                    asciitext += String.Format(bz, "mr[0,1]={0}; mr[1,1]={1}; mr[2,1]={2} \n", Dy.X, Dy.Y, Dy.Z);
                                    asciitext += String.Format(bz, "mr[0,2]={0}; mr[1,2]={1}; mr[2,2]={2} \n", Dz.X, Dz.Y, Dz.Z);
                                    asciitext += String.Format(bz, "{0}.GetCollisionModel().AddBox({1}, {2},{3},{4},chrono.ChVectorD({5},{6},{7}),mr)\n",
                                        bodyname, matname,
                                        eX.Length * ChScale.L,
                                        eY.Length * ChScale.L,
                                        eZ.Length * ChScale.L,
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
                                    Point3D p1 = SWTaskpaneHost.PointTransform(p1_l, ref collshape_subcomp_transform);
                                    Point3D p2 = SWTaskpaneHost.PointTransform(p2_l, ref collshape_subcomp_transform);
                                    asciitext += String.Format(bz, "p1 = chrono.ChVectorD({0}, {1}, {2})\n", p1.X * ChScale.L, p1.Y * ChScale.L, p1.Z * ChScale.L);
                                    asciitext += String.Format(bz, "p2 = chrono.ChVectorD({0}, {1}, {2})\n", p2.X * ChScale.L, p2.Y * ChScale.L, p2.Z * ChScale.L);
                                    asciitext += String.Format(bz, "{0}.GetCollisionModel().AddCylinder({1}, {2}, p1, p2)\n", bodyname, matname, rad * ChScale.L);
                                    rbody_converted = true;
                                }

                                if (ConvertToCollisionShapes.SWbodyToConvexHull(swBody, 30) && !rbody_converted)
                                {
                                    Point3D[] vertexes = new Point3D[1]; // will be resized by SWbodyToConvexHull
                                    ConvertToCollisionShapes.SWbodyToConvexHull(swBody, ref vertexes, 30);
                                    if (vertexes.Length > 0)
                                    {
                                        asciitext += String.Format(bz, "pt_vect = chrono.vector_ChVectorD()\n");
                                        for (int iv = 0; iv < vertexes.Length; iv++)
                                        {
                                            Point3D vert_l = vertexes[iv];
                                            Point3D vert = SWTaskpaneHost.PointTransform(vert_l, ref collshape_subcomp_transform);
                                            asciitext += String.Format(bz, "pt_vect.push_back(chrono.ChVectorD({0},{1},{2}))\n",
                                                vert.X * ChScale.L,
                                                vert.Y * ChScale.L,
                                                vert.Z * ChScale.L);
                                        }
                                        asciitext += String.Format(bz, "{0}.GetCollisionModel().AddConvexHull({1}, pt_vect)\n", bodyname, matname);
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
                            string obj_filename = this.save_dir_shapes + "\\" + shapename + ".obj";

                            ModelDoc2 swCompModel = (ModelDoc2)swComp.GetModelDoc();
                            if (!this.saved_collisionmeshes.ContainsKey(swCompModel.GetPathName()))
                            {
                                try
                                {
                                    FileStream ostream = new FileStream(obj_filename, FileMode.Create, FileAccess.ReadWrite);
                                    StreamWriter writer = new StreamWriter(ostream); //, new UnicodeEncoding());
                                    string asciiobj = "";
                                    if (this.swProgress != null)
                                        this.swProgress.UpdateTitle("Exporting collision shape" + swComp.Name2 + " (tesselate) ...");
                                    // Write the OBJ converted visualization shapes:
                                    TesselateToObj.Convert(swComp, ref asciiobj, this.checkBox_saveUV.Checked, ref this.swProgress, false, true);
                                    writer.Write(asciiobj);
                                    writer.Flush();
                                    ostream.Close();

                                    this.saved_collisionmeshes.Add(swCompModel.GetPathName(), shapename);
                                }
                                catch (Exception)
                                {
                                    System.Windows.Forms.MessageBox.Show("Cannot write to file: " + obj_filename + "\n for component: " + swComp.Name2 + " for path name: " + swCompModel.GetPathName());
                                }
                            }
                            else
                            {
                                // reuse the already-saved shape name
                                shapename = (String)this.saved_collisionmeshes[swCompModel.GetPathName()];
                            }

                            double[] amatr = (double[])collshape_subcomp_transform.ArrayData;
                            double[] quat = GetQuaternionFromMatrix(ref collshape_subcomp_transform);

                            asciitext += String.Format(bz, "\n# Triangle mesh collision shape \n", bodyname);
                            asciitext += String.Format(bz, "{0}_mesh = chrono.ChTriangleMeshConnected.CreateFromWavefrontFile(shapes_dir + '{1}.obj', False, True) \n", shapename, shapename);
                            asciitext += String.Format(bz, "mr = chrono.ChMatrix33D()\n");
                            asciitext += String.Format(bz, "mr[0,0]={0}; mr[1,0]={1}; mr[2,0]={2} \n", amatr[0] * ChScale.L, amatr[1] * ChScale.L, amatr[2] * ChScale.L);
                            asciitext += String.Format(bz, "mr[0,1]={0}; mr[1,1]={1}; mr[2,1]={2} \n", amatr[3] * ChScale.L, amatr[4] * ChScale.L, amatr[5] * ChScale.L);
                            asciitext += String.Format(bz, "mr[0,2]={0}; mr[1,2]={1}; mr[2,2]={2} \n", amatr[6] * ChScale.L, amatr[7] * ChScale.L, amatr[8] * ChScale.L);
                            asciitext += String.Format(bz, "{0}_mesh.Transform(chrono.ChVectorD({1}, {2}, {3}), mr) \n", shapename, amatr[9] * ChScale.L, amatr[10] * ChScale.L, amatr[11] * ChScale.L);
                            asciitext += String.Format(bz, "{0}.GetCollisionModel().AddTriangleMesh({1}, {2}_mesh, False, False, ", bodyname, matname, shapename);
                            asciitext += String.Format(bz, "chrono.ChVectorD(0,0,0), chrono.ChMatrix33D(chrono.ChQuaternionD(1,0,0,0)), sphereswept_r) \n");
                            //rbody_converted = true;
                        }


                    } // end if build_collision_model
                }

            } // end collision shapes export

        }


        public void PythonTraverseComponent_for_ChBody(Component2 swComp, long nLevel, ref string asciitext, int nbody)
        {
            CultureInfo bz = new CultureInfo("en-BZ");
            object[] vmyChildComp = (object[])swComp.GetChildren();
            //bool found_chbody_equivalent = false;

            if (nLevel > 1)
                if (nbody == -1)
                    if (!swComp.IsSuppressed()) // skip body if marked as 'suppressed'
                    {
                        if ((swComp.Solving == (int)swComponentSolvingOption_e.swComponentRigidSolving) || (vmyChildComp.Length == 0))
                        {
                            // OK! this is a 'leaf' of the tree of ChBody equivalents (a SDW subassebly or part)

                            //found_chbody_equivalent = true;

                            this.num_comp++;

                            nbody = this.num_comp;  // mark the rest of recursion as 'n-th body found'

                            if (this.swProgress != null)
                            {
                                this.swProgress.UpdateTitle("Exporting " + swComp.Name2 + " ...");
                                this.swProgress.UpdateProgress(this.num_comp % 5);
                            }

                            // fetch SW attribute with Chrono parameters
                            SolidWorks.Interop.sldworks.Attribute myattr = (SolidWorks.Interop.sldworks.Attribute)swComp.FindAttribute(this.mSWintegration.defattr_chbody, 0);

                            MathTransform chbodytransform = swComp.GetTotalTransform(true);
                            double[] amatr;
                            amatr = (double[])chbodytransform.ArrayData;
                            string bodyname = "body_" + this.num_comp;

                            // Write create body
                            asciitext += "# Rigid body part\n";
                            asciitext += bodyname + " = chrono.ChBodyAuxRef()" + "\n";

                            // Write name
                            asciitext += bodyname + ".SetName('" + swComp.Name2 + "')" + "\n";

                            // Write position
                            asciitext += bodyname + ".SetPos(chrono.ChVectorD("
                                       + (amatr[9] * ChScale.L).ToString("g", bz) + ","
                                       + (amatr[10] * ChScale.L).ToString("g", bz) + ","
                                       + (amatr[11] * ChScale.L).ToString("g", bz) + "))" + "\n";

                            // Write rotation
                            double[] quat = GetQuaternionFromMatrix(ref chbodytransform);
                            asciitext += String.Format(bz, "{0}.SetRot(chrono.ChQuaternionD({1:g},{2:g},{3:g},{4:g}))\n",
                                       bodyname, quat[0], quat[1], quat[2], quat[3]);

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

                            asciitext += String.Format(bz, "{0}.SetMass({1:g})\n",
                                       bodyname,
                                       mass * ChScale.M);

                            // Write inertia tensor 
                            asciitext += String.Format(bz, "{0}.SetInertiaXX(chrono.ChVectorD({1:g},{2:g},{3:g}))\n",
                                       bodyname,
                                       Ixx * ChScale.M * ChScale.L * ChScale.L,
                                       Iyy * ChScale.M * ChScale.L * ChScale.L,
                                       Izz * ChScale.M * ChScale.L * ChScale.L);
                            // Note: C::E assumes that's up to you to put a 'minus' sign in values of Ixy, Iyz, Izx
                            asciitext += String.Format(bz, "{0}.SetInertiaXY(chrono.ChVectorD({1:g},{2:g},{3:g}))\n",
                                       bodyname,
                                       -Ixy * ChScale.M * ChScale.L * ChScale.L,
                                       -Izx * ChScale.M * ChScale.L * ChScale.L,
                                       -Iyz * ChScale.M * ChScale.L * ChScale.L);

                            // Write the position of the COG respect to the REF
                            asciitext += String.Format(bz, "{0}.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD({1:g},{2:g},{3:g}),chrono.ChQuaternionD(1,0,0,0)))\n",
                                        bodyname,
                                        cogXb * ChScale.L,
                                        cogYb * ChScale.L,
                                        cogZb * ChScale.L);

                            // Write 'fixed' state
                            if (swComp.IsFixed())
                                asciitext += String.Format(bz, "{0}.SetBodyFixed(True)\n", bodyname);


                            // Write shapes (saving also Wavefront files .obj)
                            if (this.checkBox_surfaces.Checked)
                            {
                                int nvisshape = 0;

                                if (swComp.Visible == (int)swComponentVisibilityState_e.swComponentVisible)
                                    PythonTraverseComponent_for_visualizationshapes(swComp, nLevel, ref asciitext, nbody, ref nvisshape, swComp);
                            }

                            // Write markers (SW coordsystems) contained in this component or subcomponents
                            // if any.
                            PythonTraverseComponent_for_markers(swComp, nLevel, ref asciitext, nbody);

                            // Write collision shapes (customized SW solid bodies) contained in this component or subcomponents
                            // if any.
                            bool param_collide = true;
                            if (myattr != null)
                                param_collide = Convert.ToBoolean(((Parameter)myattr.GetParameter("collision_on")).GetDoubleValue());

                            if (param_collide)
                            {
                                bool found_collisionshapes = false;
                                int ncollshapes = 0;

                                PythonTraverseComponent_for_collshapes(swComp, nLevel, ref asciitext, nbody, ref chbodytransform, ref found_collisionshapes, swComp, ref ncollshapes);
                                if (found_collisionshapes)
                                {
                                    asciitext += String.Format(bz, "{0}.GetCollisionModel().BuildModel()\n", bodyname);
                                    asciitext += String.Format(bz, "{0}.SetCollide(True)\n", bodyname);
                                }
                            }

                            // Insert to a list of exported items
                            asciitext += String.Format(bz, "\n" + "exported_items.append({0})\n", bodyname);

                            // End writing body in Python-
                            asciitext += "\n\n\n";


                        } // end if ChBody equivalent (tree leaf or non-flexible assembly)
                    }


            // Things to do also for sub-components of 'non flexible' assemblies: 
            //

            // store in hashtable, will be useful later when adding constraints
            if ((nLevel > 1) && (nbody != -1))
                try
                {
                    string bodyname = "body_" + this.num_comp;

                    ModelDocExtension swModelDocExt = default(ModelDocExtension);
                    ModelDoc2 swModel = (ModelDoc2)this.mSWApplication.ActiveDoc;
                    //if (swModel != null)
                    swModelDocExt = swModel.Extension;
                    this.saved_parts.Add(swModelDocExt.GetPersistReference3(swComp), bodyname);
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Cannot add part to hashtable?");
                }


            // Traverse all children, proceeding to subassemblies and parts, if any
            // 

            object[] vChildComp;
            Component2 swChildComp;

            vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                PythonTraverseComponent_for_ChBody(swChildComp, nLevel + 1, ref asciitext, nbody);
            }


        }


        public void JsonTraverseComponent_for_ChBody(Component2 swComp, long nLevel, ref JArray ChSystemBodylistArray)
        {
            CultureInfo bz = new CultureInfo("en-BZ");
            object[] vmyChildComp = (object[])swComp.GetChildren();
            //bool found_chbody_equivalent = false;

            if (nLevel > 1 & _object_ID_used == 0 & !swComp.IsSuppressed() && (swComp.Solving == (int)swComponentSolvingOption_e.swComponentRigidSolving) || (vmyChildComp.Length == 0))
            {
                // OK! this is a 'leaf' of the tree of ChBody equivalents (a SDW subassebly or part)

                //found_chbody_equivalent = true;

                this.num_comp++;

                if (this.swProgress != null)
                {
                    this.swProgress.UpdateTitle("Exporting " + swComp.Name2 + " ...");
                    this.swProgress.UpdateProgress(this.num_comp % 5);
                }

                // fetch SW attribute with Chrono parameters
                SolidWorks.Interop.sldworks.Attribute myattr = (SolidWorks.Interop.sldworks.Attribute)swComp.FindAttribute(this.mSWintegration.defattr_chbody, 0);

                MathTransform chbodytransform = swComp.GetTotalTransform(true);
                double[] amatr;
                amatr = (double[])chbodytransform.ArrayData;

                double[] quat = GetQuaternionFromMatrix(ref chbodytransform);

                var ChBodyAuxRefNode = new JObject
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
                ChBodyAuxRefNode.Add("_c_SetBodyFixed", swComp.IsFixed() ? true: false);



                // Write shapes (saving also Wavefront files .obj)
                if (this.checkBox_surfaces.Checked)
                {
                    int nvisshape = 0;

                    if (swComp.Visible == (int)swComponentVisibilityState_e.swComponentVisible)
                        JsonTraverseComponent_for_visualizationshapes(swComp, nLevel, ref ChBodyAuxRefNode, ref nvisshape, swComp);
                }

                // Write markers (SW coordsystems) contained in this component or subcomponents
                // if any.
                JsonTraverseComponent_for_markers(swComp, nLevel, ref ChBodyAuxRefNode);

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
                    ModelDoc2 swModel = (ModelDoc2)this.mSWApplication.ActiveDoc;
                    //if (swModel != null)
                    swModelDocExt = swModel.Extension;
                    this.saved_parts.Add(swModelDocExt.GetPersistReference3(swComp), ChBodyAuxRefNode.GetValue("_object_ID"));
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

                JsonTraverseComponent_for_ChBody(swChildComp, nLevel + 1, ref ChSystemBodylistArray);
            }


        }




        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




        //
        // Traverse everything
        //

        public void ExportToDump(ref string asciitext)
        {
            ModelDoc2 swModel;
            ConfigurationManager swConfMgr;
            Configuration swConf;
            Component2 swRootComp;

            swModel = (ModelDoc2)this.mSWApplication.ActiveDoc;
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





        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



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


        private void checkBox_constraints_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button_setcollshape_Click(object sender, EventArgs e)
        {
            ModelDoc2 swModel;
            swModel = (ModelDoc2)this.mSWApplication.ActiveDoc;
            if (swModel == null)
            {
                System.Windows.Forms.MessageBox.Show("Please open a part and select a solid body!");
                return;
            }

            SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;

            if (swSelMgr.GetSelectedObjectCount2(-1) == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select one or more solid bodies!");
                return;
            }

            /* TEST
            // stub for code to try to convert whole body:
            ModelDoc2 selpart;
            Component2 selcomp;
            
            for (int isel = 1; isel <= swSelMgr.GetSelectedObjectCount2(-1); isel++ )
            {
                if ((swSelectType_e)swSelMgr.GetSelectedObjectType3(isel, -1) == swSelectType_e.swSelCOMPONENTS)
                {
                    selcomp = (Component2)swSelMgr.GetSelectedObject6(isel, -1);
                    if (selcomp != null)
                    {
                        selpart = (ModelDoc2)selcomp.GetModelDoc2();
                        String partinfo = "Component2! Part/assembly name:" + selcomp.Name + "\n" + "Config:" + selcomp.ReferencedConfiguration + "\n" + "Path:" + selpart.GetPathName();
                        System.Windows.Forms.MessageBox.Show(partinfo);
                    }
                }
            }
            */


            for (int isel = 1; isel <= swSelMgr.GetSelectedObjectCount2(-1); isel++)
            {
                if ((swSelectType_e)swSelMgr.GetSelectedObjectType3(isel, -1) != swSelectType_e.swSelSOLIDBODIES)
                {
                    System.Windows.Forms.MessageBox.Show("This function can be applied only to solid bodies! Select one or more bodies before using it.");
                    return;
                }

                bool rbody_converted = false;
                Body2 swBody = (Body2)swSelMgr.GetSelectedObject6(isel, -1);

                // ----- Try to see if this is a sphere

                if (ConvertToCollisionShapes.SWbodyToSphere(swBody))
                {
                    string mname = swBody.Name;
                    mname.Replace("COLL.SPHERE-", "");
                    swBody.Name = "COLL.SPHERE-" + mname;
                    rbody_converted = true;
                }

                // ----- Try to see if this is a box

                if (ConvertToCollisionShapes.SWbodyToBox(swBody))
                {
                    string mname = swBody.Name;
                    mname.Replace("COLL.BOX-", "");
                    swBody.Name = "COLL.BOX-" + mname;
                    rbody_converted = true;
                }

                // ----- Try to see if this is a cylinder

                if (ConvertToCollisionShapes.SWbodyToCylinder(swBody))
                {
                    string mname = swBody.Name;
                    mname.Replace("COLL.CYLINDER-", "");
                    swBody.Name = "COLL.CYLINDER-" + mname;
                    rbody_converted = true;
                }

                // ----- Try to see if this is a convex hull (except if already converted bacause it's a box)

                if (ConvertToCollisionShapes.SWbodyToConvexHull(swBody, 30) && !rbody_converted)
                {
                    string mname = swBody.Name;
                    mname.Replace("COLL.CONV.HULL-", "");
                    swBody.Name = "COLL.CONV.HULL-" + mname;
                    rbody_converted = true;
                }


                // Change color of the body if it can be used as collision shape
                if (rbody_converted)
                {
                    //int materr = swBody.SetMaterialProperty("Default", "Materiali personalizzati.sldmat", "CollisionShapeMaterial");
                    int materr = swBody.SetMaterialProperty("Default", "", "Air");
                    if (materr != 1)
                        materr = swBody.SetMaterialProperty("Default", "", "Aria");
                    if (materr != 1)
                        materr = swBody.SetMaterialProperty("Default", "", "Luft");
                    if (materr != 1)
                        System.Windows.Forms.MessageBox.Show("Warning! could not assign 'air' material to the collision shape: " + swBody.Name + "\n This can affect mass computations.");

                    double[] mcolor = new double[9] { 1, 0, 0, 1, 1, 1, 0, 0.7, 0 }; //  R, G, B, Ambient, Diffuse, Specular, Shininess, Transparency, Emission 
                    swBody.MaterialPropertyValues2 = mcolor;

                    // must force rebuild otherwise one cannot see in the design tree that an 'air' material has been added to collision bodies, etc.
                    //swModel.Rebuild((int)swRebuildOptions_e.swForceRebuildAll);
                    swModel.ForceRebuild3(false);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Selected solid body is not of cylinder/box/sphere/convexhull type. Cannot convert to collision shape. Fallback solution: use concave tri-mesh collision (slower, less robust).");
                    return;
                }

            } // end loop on selected items

        }

        private void checkBox_surfaces_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox_surfaces.Checked)
            {
                this.checkBox_separateobj.Enabled = true;
                this.checkBox_saveUV.Enabled = true;
            }
            else
            {
                this.checkBox_separateobj.Enabled = false;
                this.checkBox_saveUV.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button_runtest_Click(object sender, EventArgs e)
        {
            CultureInfo bz = new CultureInfo("en-BZ");

            try
            {
                string InstallPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\ChronoSolidworks", "InstallPath", "CHRONO_SOLIDWORKS_PATH_NOT_FOUND");
                System.Diagnostics.ProcessStartInfo startInfo;
                System.Diagnostics.Process process;
                string save_directory = System.IO.Path.GetDirectoryName(this.save_filename);
                string directory = save_directory;
                string pyArgs = "";
                pyArgs += " -f " + System.IO.Path.GetFileName(this.save_filename); //was .GetFileNameWithoutExtension(this.save_filename);
                pyArgs += " -d " + this.numeric_dt.Value.ToString(bz);
                pyArgs += " -T " + this.numeric_length.Value.ToString(bz);
                pyArgs += " --datapath " + "\"" + InstallPath + "/data/" + "\"";
                if (this.comboBox1.SelectedIndex == 0)
                    pyArgs += " -v irrlicht";
                if (this.comboBox1.SelectedIndex == 1)
                    pyArgs += " -v pov";

                string script = "run_test.py";
                startInfo = new System.Diagnostics.ProcessStartInfo("Python.exe");
                startInfo.WorkingDirectory = directory;
                startInfo.Arguments = script + " " + pyArgs;
                startInfo.UseShellExecute = false;
                // startInfo.CreateNoWindow = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;

                process = new System.Diagnostics.Process();
                process.StartInfo = startInfo;
                process.Start();

                string cmdError = process.StandardError.ReadToEnd();
                string cmdOutput = process.StandardOutput.ReadToEnd();

                //  process.WaitForExit();
                //  process.Close();

                //System.Windows.Forms.MessageBox.Show("Launching Python.exe with parameters: \n" + startInfo.Arguments.ToString());
                //System.Windows.Forms.MessageBox.Show("Output: \n" + cmdOutput);
                if (cmdError.Length >1)
                    System.Windows.Forms.MessageBox.Show("Error: \n" + cmdError);
            }
            catch (Exception myex)
            {
                System.Windows.Forms.MessageBox.Show("Cannot execute the test Python program. \n - Make sure that you already saved with 'save test.py' enabled; \n - Make sure you have Python.exe available in command line PATH. \n\n\nException:\n" + myex.ToString());
            }
        }

        private void checkBox_scale_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox_scale.Checked)
            {
                this.numeric_scale_L.Enabled = true;
                this.numeric_scale_M.Enabled = true;
                this.numeric_scale_T.Enabled = true;
            }
            else
            {
                this.numeric_scale_L.Enabled = false;
                this.numeric_scale_M.Enabled = false;
                this.numeric_scale_T.Enabled = false;
            }
        }

        private void button_convexdecomp_Click(object sender, EventArgs e)
        {
            ModelDoc2 swModel;
            swModel = (ModelDoc2)this.mSWApplication.ActiveDoc;
            if (swModel == null)
            {
                System.Windows.Forms.MessageBox.Show("Please open a part and select a solid body!");
                return;
            }

            SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;

            if (swSelMgr.GetSelectedObjectCount2(-1) == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select one or more solid bodies!");
                return;
            }

            for (int isel = 1; isel <= swSelMgr.GetSelectedObjectCount2(-1); isel++)
            {
                if ((swSelectType_e)swSelMgr.GetSelectedObjectType3(isel, -1) != swSelectType_e.swSelSOLIDBODIES)
                {
                    System.Windows.Forms.MessageBox.Show("This function can be applied only to solid bodies! Select one or more bodies before using it.");
                    return;
                }
                /*
                IComponent2 mcomponent = swSelMgr.GetSelectedObjectsComponent4(isel, -1);
                object bodyInfo;
                object[] bodies;
                bodies = (object[])mcomponent.GetBodies3((int)swBodyType_e.swAllBodies, out bodyInfo);
                if (bodies.Length == 1)
                     System.Windows.Forms.MessageBox.Show("Warning! This is the only body in this part. It will converted to collision body -you will loose visualization and mass computation.");
                */
                //bool rbody_converted = false;
                Body2 swBodyIn = (Body2)swSelMgr.GetSelectedObject6(isel, -1);

                // ----- tesselate
                Face2 swFace = null;
                Tessellation swTessellation = null;

                bool bResult = false;

                // Pass in null so the whole body will be tessellated
                swTessellation = (Tessellation)swBodyIn.GetTessellation(null);

                // Set up the Tessellation object
                swTessellation.NeedFaceFacetMap = true;
                swTessellation.NeedVertexParams = true;
                swTessellation.NeedVertexNormal = true;
                swTessellation.ImprovedQuality = true;

                int group_vstride = 0;

                // How to handle matches across common edges
                swTessellation.MatchType = (int)swTesselationMatchType_e.swTesselationMatchFacetTopology;

                // Do it
                if (swProgress != null)
                    swProgress.UpdateTitle("Tesselation process...");
                bResult = swTessellation.Tessellate();

                // Now get the facet data per face
                int[] aFacetIds;
                int iNumFacetIds;
                int[] aFinIds;
                int[] aVertexIds;
                double[] aVertexCoords1;

                int numv = swTessellation.GetVertexCount();
                int numf = swTessellation.GetFacetCount();
                int iface = 0;

                // Get all vertexes
                vhacd_CLI.Vect3D[] myvertexes = new vhacd_CLI.Vect3D[numv];
                vhacd_CLI.Triangle[] mytriangles = new vhacd_CLI.Triangle[numf];

                for (int iv = 0; iv < numv; iv++)
                {
                    if ((swProgress != null) && (iv % 200 == 0))
                        swProgress.UpdateTitle("Vertexes process: " + iv + "-th vertex ...");
                    aVertexCoords1 = (double[])swTessellation.GetVertexPoint(iv);
                    myvertexes[iv] = new vhacd_CLI.Vect3D(
                        aVertexCoords1[0],
                        aVertexCoords1[1],
                        aVertexCoords1[2]);
                }

                // Loop over faces
                swFace = (Face2)swBodyIn.GetFirstFace();

                while (swFace != null)
                {
                    aFacetIds = (int[])swTessellation.GetFaceFacets(swFace);

                    iNumFacetIds = aFacetIds.Length;

                    for (int iFacetIdIdx = 0; iFacetIdIdx < iNumFacetIds; iFacetIdIdx++)
                    {
                        if ((swProgress != null) && (iFacetIdIdx % 100 == 0))
                            swProgress.UpdateTitle("Faces process: " + iFacetIdIdx + "-th face ...");

                        aFinIds = (int[])swTessellation.GetFacetFins(aFacetIds[iFacetIdIdx]);

                        // There should always be three fins per facet
                        int iFinIdx = 0;
                        aVertexIds = (int[])swTessellation.GetFinVertices(aFinIds[iFinIdx]);
                        int ip1 = aVertexIds[0] + group_vstride;
                        iFinIdx = 1;
                        aVertexIds = (int[])swTessellation.GetFinVertices(aFinIds[iFinIdx]);
                        int ip2 = aVertexIds[0] + group_vstride;
                        iFinIdx = 2;
                        aVertexIds = (int[])swTessellation.GetFinVertices(aFinIds[iFinIdx]);
                        int ip3 = aVertexIds[0] + group_vstride;

                        mytriangles[iface] = new vhacd_CLI.Triangle(ip1, ip2, ip3);

                        iface++;
                    }
                    swFace = (Face2)swFace.GetNextFace();
                }

                group_vstride += swTessellation.GetVertexCount();

                // construct a new customer dialog
                ConvexDecomp2 myCustomerDialog = new ConvexDecomp2();
                myCustomerDialog.SetMeshInfo(numf, numv);
                // show the modal dialog
                if (myCustomerDialog.ShowDialog() == DialogResult.OK)
                {

                    // Perform convexdecomposition
                    vhacd_CLI.vhacd_CLI_wrapper myConvexDecomp = new vhacd_CLI.vhacd_CLI_wrapper();

                    myConvexDecomp.SetMesh(myvertexes, mytriangles);

                    myConvexDecomp.targetNTrianglesDecimatedMesh = myCustomerDialog.m_decimate;
                    myConvexDecomp.alpha = myCustomerDialog.m_alpha;
                    myConvexDecomp.concavity = myCustomerDialog.m_concavity;
                    myConvexDecomp.depth = myCustomerDialog.m_depht;
                    myConvexDecomp.posSampling = myCustomerDialog.m_positionsampling;
                    myConvexDecomp.angleSampling = myCustomerDialog.m_anglesampling;
                    myConvexDecomp.posRefine = myCustomerDialog.m_posrefine;
                    myConvexDecomp.angleRefine = myCustomerDialog.m_anglerefine;

                    int nhulls = myConvexDecomp.Compute(true, false);

                    Body2 myNewBody = null;
                    IPartDoc mpart = null;

                    if (myConvexDecomp.GetNClusters() > 0)
                    {
                        Configuration swConf;
                        Component2 swRootComp;

                        swConf = (Configuration)swModel.GetActiveConfiguration();
                        swRootComp = (Component2)swConf.GetRootComponent3(true);

                        if (swModel.GetType() == (int)swDocumentTypes_e.swDocPART)
                        {
                            mpart = (IPartDoc)swModel;
                        }
                    }

                    for (int nc = 0; nc < myConvexDecomp.GetNClusters(); nc++)
                    {
                        vhacd_CLI.Vect3D[] hull_vertexes = new vhacd_CLI.Vect3D[0];
                        vhacd_CLI.Triangle[] hull_triangles = new vhacd_CLI.Triangle[0];
                        int npoints = myConvexDecomp.GetClusterConvexHull(nc, ref hull_vertexes, ref hull_triangles);

                        if (mpart != null)
                        {
                            //myNewBody = (Body2)mpart.CreateNewBody();
                            myNewBody = (Body2)mpart.ICreateNewBody2();

                            for (int ihf = 0; ihf < hull_triangles.GetLength(0); ihf++)
                            {
                                double[] vPt = new double[9];
                                vPt[0] = hull_vertexes[hull_triangles[ihf].p1].X;
                                vPt[1] = hull_vertexes[hull_triangles[ihf].p1].Y;
                                vPt[2] = hull_vertexes[hull_triangles[ihf].p1].Z;
                                vPt[3] = hull_vertexes[hull_triangles[ihf].p2].X;
                                vPt[4] = hull_vertexes[hull_triangles[ihf].p2].Y;
                                vPt[5] = hull_vertexes[hull_triangles[ihf].p2].Z;
                                vPt[6] = hull_vertexes[hull_triangles[ihf].p3].X;
                                vPt[7] = hull_vertexes[hull_triangles[ihf].p3].Y;
                                vPt[8] = hull_vertexes[hull_triangles[ihf].p3].Z;
                                myNewBody.CreatePlanarTrimSurfaceDLL((Object)vPt, null);
                            }
                            bool created = myNewBody.CreateBodyFromSurfaces();
                            if (created)
                            {
                                ModelDocExtension mextension = swModel.Extension;
                                Feature mlastfeature = mextension.GetLastFeatureAdded();
                                mlastfeature.Select2(true, nc); // does not append?
                                /*  ***TO DO***
                                int materr = myNewBody.SetMaterialProperty("Default", "", "Air");
                                if (materr != 1)
                                    materr = myNewBody.SetMaterialProperty("Default", "", "Aria");
                                if (materr != 1)
                                    materr = myNewBody.SetMaterialProperty("Default", "", "Luft");
                                //if (materr != 1)
                                //    System.Windows.Forms.MessageBox.Show("Warning! could not assign 'air' material to the collision shape: " + myNewBody.Name + "\n This can affect mass computations.");
                                */
                                double[] mcolor = new double[9] { 1, 0, 0, 1, 1, 1, 0, 0.7, 0 }; //  R, G, B, Ambient, Diffuse, Specular, Shininess, Transparency, Emission 
                                myNewBody.MaterialPropertyValues2 = mcolor;  // it does not work
                            }
                        }
                    } // end loop on clusters
                    /* ***TO DO***
                    // all new 'imported body' features must be grouped into a folder to avoid confusion:
                    IFeatureManager swFeatMgr = swModel.FeatureManager;
                    Feature swFeat = swFeatMgr.InsertFeatureTreeFolder2((int)swFeatureTreeFolderType_e.swFeatureTreeFolder_Containing);
                    */
                    // must force rebuild otherwise one cannot see in the design tree that an 'air' material has been added to collision bodies, etc.
                    //swModel.Rebuild((int)swRebuildOptions_e.swForceRebuildAll);
                    swModel.ForceRebuild3(false);

                } // end if user choose OK to decompose

            } // end loop on selected items

        }

        private void button_chrono_property_Click(object sender, EventArgs e)
        {
            ModelDoc2 swModel;
            swModel = (ModelDoc2)this.mSWApplication.ActiveDoc;
            if (swModel == null)
            {
                System.Windows.Forms.MessageBox.Show("Please open an assembly and select a part!");
                return;
            }

            SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;

            if (swSelMgr.GetSelectedObjectCount2(-1) == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select one or more parts!");
                return;
            }
            /*
            //***TEST***
            AttributeDef defattr_test = (AttributeDef)this.mSWApplication.DefineAttribute("mytestt");
            defattr_test.AddParameter("testpar", (int)swParamType_e.swParamTypeDouble, 0.6, 0);
            defattr_test.Register();

            Component2 swPart = (Component2)swSelMgr.GetSelectedObject6(1, -1);
            ModelDoc2 swPartModel = (ModelDoc2)swPart.GetModelDoc2();
            //ModelDoc2 swModel = (ModelDoc2)this.ActiveDoc;
             System.Windows.Forms.MessageBox.Show("attach, v CreateInstance5");
             SolidWorks.Interop.sldworks.Attribute myattr = defattr_test.CreateInstance5(swModel, swPart, "test_data", 0, (int)swInConfigurationOpts_e.swAllConfiguration);
            */

            bool selected_part = false;
            for (int isel = 1; isel <= swSelMgr.GetSelectedObjectCount2(-1); isel++)
                if ((swSelectType_e)swSelMgr.GetSelectedObjectType3(isel, -1) == swSelectType_e.swSelCOMPONENTS)
                {
                    selected_part = true;
                }


            if (!selected_part)
            {
                System.Windows.Forms.MessageBox.Show("Chrono properties can be edited only for parts! Select one or more parts before using it.");
                return;
            }


            // Open modal dialog
            EditChBody myCustomerDialog = new EditChBody();

            // Update dialog properties properties from the selected part(s) (i.e. ChBody in C::E) 
            if (myCustomerDialog.UpdateFromSelection(swSelMgr, ref this.mSWintegration.defattr_chbody))
            {
                // Show the modal dialog
                if (myCustomerDialog.ShowDialog() == DialogResult.OK)
                {
                    // If user pressed OK, apply settings to all selected parts (i.e. ChBody in C::E):
                    myCustomerDialog.StoreToSelection(swSelMgr, ref this.mSWintegration.defattr_chbody);//ref this.mSWintegration.defattr_chconveyor);
                }
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ModelDoc2 swModel;
            swModel = (ModelDoc2)this.mSWApplication.ActiveDoc;
            if (swModel == null)
            {
                System.Windows.Forms.MessageBox.Show("Please open a part and select a solid body!");
                return;
            }

            SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;

            if (swSelMgr.GetSelectedObjectCount2(-1) == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select one or more solid bodies!");
                return;
            }

            string message = "";

            for (int isel = 1; isel <= swSelMgr.GetSelectedObjectCount2(-1); isel++)
            {
                if ((swSelectType_e)swSelMgr.GetSelectedObjectType3(isel, -1) != swSelectType_e.swSelSOLIDBODIES)
                {
                    System.Windows.Forms.MessageBox.Show("This function can be applied only to solid bodies! Select one or more bodies before using it.");
                    return;
                }

                bool rbody_converted = false;
                Body2 swBody = (Body2)swSelMgr.GetSelectedObject6(isel, -1);

                string mname = swBody.Name;
                mname.Replace("COLLMESH-", "");
                swBody.Name = "COLLMESH-" + mname;
                rbody_converted = true;

                swModel.ForceRebuild3(false);


                // ----- Try to see if it was possible to use a faster method.

                if (ConvertToCollisionShapes.SWbodyToSphere(swBody))
                {
                    message += "  " + swBody.Name + " is a sphere primitive, \n";
                }
                if (ConvertToCollisionShapes.SWbodyToBox(swBody))
                {
                    message += "  " + swBody.Name + " is a box primitive, \n";
                }
                if (ConvertToCollisionShapes.SWbodyToCylinder(swBody))
                {
                    message += "  " + swBody.Name + " is a cylinder primitive, \n";
                }
                if (ConvertToCollisionShapes.SWbodyToConvexHull(swBody, 30) && !rbody_converted)
                {
                    message += "  " + swBody.Name + " is a convex hull primitive, \n";
                }

            } // end loop on selected items

            if (message != "")
            {
                System.Windows.Forms.MessageBox.Show("Hint: \n" + message + "so you might better use the conversion into primitive collision shapes, that is faster and more robust thatn the generic triangle mesh collision.");
            }

        }

    }  // end class



}  // end namespace