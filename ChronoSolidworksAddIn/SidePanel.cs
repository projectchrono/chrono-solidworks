using System;
using System.Collections;
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

// for JSON export
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
//using static ChronoEngine_SwAddin.ConvertMates;


namespace ChronoEngine_SwAddin
{
    [ComVisible(true)]
    [ProgId(SWTASKPANE_PROGID)]
    public partial class SWTaskpaneHost : UserControl
    {
        public const string SWTASKPANE_PROGID = "ChronoEngine.Taskpane";
        public ISldWorks mSWApplication;
        public SWIntegration mSWintegration;
        internal System.Windows.Forms.SaveFileDialog SaveFileDialog1;
        internal string save_dir_shapes = "";
        internal string save_filename = "";
        internal UserProgressBar swProgress;

        public SWTaskpaneHost()
        {
            InitializeComponent();
            this.SaveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
        }



        // ============================================================================================================
        // Panel interaction functions
        // ============================================================================================================
        private void ExportClick(object sender, EventArgs e)
        {
            ChScale.L = (double)this.numeric_scale_L.Value;
            ChScale.M = (double)this.numeric_scale_M.Value;
            ChScale.T = (double)this.numeric_scale_T.Value;

            //m_exportNamesMap = new Dictionary<string, string>();

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
            else if ((sender as Button).Name.ToString() == "button_ExportToCpp")
            {
                this.SaveFileDialog1.Filter = "Chrono C++ File (*.cpp)|*.cpp";
                this.SaveFileDialog1.DefaultExt = "cpp";
            }
            else if ((sender as Button).Name.ToString() == "button_ExportToJson")
            {
                this.SaveFileDialog1.Filter = "Chrono JSON File (*.json)|*.json";
                this.SaveFileDialog1.DefaultExt = "json";
            }

            this.SaveFileDialog1.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            DialogResult result = SaveFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.save_filename = SaveFileDialog1.FileName;

                if (this.checkBox_surfaces.Checked)
                {
                    string save_directory = System.IO.Path.GetDirectoryName(SaveFileDialog1.FileName);
                    this.save_dir_shapes = save_directory + "\\" + System.IO.Path.GetFileNameWithoutExtension(this.save_filename) + "_shapes";
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


                // Do the conversion into a Python/C++/Json text block.
                // This will scan the low level hierarchy of the assembly
                // and its mating constraints and create the proper Chrono::Engine links.
                if ((sender as Button).Name.ToString() == "button_ExportToPython")
                {
                    ChModelExporterPython pythonExporter = new ChModelExporterPython(mSWintegration, save_dir_shapes, save_filename);
                    pythonExporter.Export();
                }
                else if ((sender as Button).Name.ToString() == "button_ExportToCpp")
                {
                    ChModelExporterCpp cppExporter = new ChModelExporterCpp(mSWintegration, save_dir_shapes, save_filename);
                    cppExporter.Export();
                }
                else if ((sender as Button).Name.ToString() == "button_ExportToJson")
                {
                    ChModelExporterJson jsonExporter = new ChModelExporterJson(mSWintegration, save_dir_shapes, save_filename);
                    jsonExporter.Export();
                }

                if (this.checkBox_savetest.Checked && sender.ToString() == "button_ExportToPython") // TODO: Json cannot handle collisions yet
                {
                    string save_directory = System.IO.Path.GetDirectoryName(SaveFileDialog1.FileName);
                    try
                    {
                        string InstallPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\ChronoSolidworks", "InstallPath", "CHRONO_SOLIDWORKS_PATH_NOT_FOUND");

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
                if (cmdError.Length > 1)
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
            {
                if ((swSelectType_e)swSelMgr.GetSelectedObjectType3(isel, -1) == swSelectType_e.swSelCOMPONENTS)
                {
                    selected_part = true;

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
            }

            if (!selected_part)
            {
                System.Windows.Forms.MessageBox.Show("Chrono properties can be edited only for parts! Select one or more parts before using it.");
                return;
            }
        }

        private void butt_chronoMotors_Click(object sender, EventArgs e)
        {
            ModelDoc2 swModel;
            swModel = (ModelDoc2)this.mSWApplication.ActiveDoc;
            SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;

            EditChMotor myCustomerDialog = new EditChMotor(ref swSelMgr, ref mSWintegration);
            //myCustomerDialog.ShowDialog(); // show modal
            myCustomerDialog.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        // ============================================================================================================
        // Accessors for private UI items
        // ============================================================================================================
        public ref System.Windows.Forms.CheckBox GetCheckboxConstraints()
        {
            return ref checkBox_constraints;
        }

        public ref SolidWorks.Interop.sldworks.UserProgressBar GetProgressBar()
        {
            return ref swProgress;
        }

        public ref System.Windows.Forms.CheckBox GetCheckboxSaveUV()
        {
            return ref checkBox_saveUV;
        }

        public ref System.Windows.Forms.CheckBox GetCheckboxCollisionShapes()
        {
            return ref checkBox_collshapes;
        }

        public ref System.Windows.Forms.CheckBox GetCheckboxSurfaces()
        {
            return ref checkBox_surfaces;
        }

        public ref System.Windows.Forms.NumericUpDown GetNumericEnvelope()
        {
            return ref numeric_envelope;
        }

        public ref System.Windows.Forms.NumericUpDown GetNumericMargin()
        {
            return ref numeric_margin;
        }

        public ref System.Windows.Forms.NumericUpDown GetNumericContactBreaking()
        {
            return ref numeric_contactbreaking;
        }

        public ref System.Windows.Forms.NumericUpDown GetNumericSphereSwept()
        {
            return ref numeric_sphereswept;
        }




        // ============================================================================================================
        // Hierarchy dump methods
        // ============================================================================================================
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


    }  // end class



}  // end namespace