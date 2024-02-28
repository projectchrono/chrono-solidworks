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

using System.Reflection;

using Microsoft.Win32;

// for JSON export
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.Security.Cryptography;
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
        internal System.Windows.Forms.SaveFileDialog m_saveFileDialog;
        internal System.Windows.Forms.FolderBrowserDialog m_folderBrowserDialog;
        internal string save_dir_shapes = "";
        internal string save_filename = "";
        internal UserProgressBar swProgress;

        public SWTaskpaneHost()
        {
            InitializeComponent();
            this.m_saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.m_folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
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
                this.m_saveFileDialog.Filter = "PyChrono Python script (*.py)|*.py";
                this.m_saveFileDialog.DefaultExt = "py";
            }
            else if ((sender as Button).Name.ToString() == "button_ExportToCpp")
            {
                this.m_saveFileDialog.Filter = "Chrono C++ File (*.cpp)|*.cpp";
                this.m_saveFileDialog.DefaultExt = "cpp";
            }
            else if ((sender as Button).Name.ToString() == "button_ExportToJson")
            {
                this.m_saveFileDialog.Filter = "Chrono JSON File (*.json)|*.json";
                this.m_saveFileDialog.DefaultExt = "json";
            }

            this.m_saveFileDialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            DialogResult result = m_saveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.save_filename = m_saveFileDialog.FileName;

                if (this.checkBox_surfaces.Checked)
                {
                    string save_directory = System.IO.Path.GetDirectoryName(this.save_filename);
                    this.save_dir_shapes = save_directory + "\\" + System.IO.Path.GetFileNameWithoutExtension(this.save_filename) + "_shapes";
                    DirectoryInfo mi = System.IO.Directory.CreateDirectory(this.save_dir_shapes);
                    if (mi.Exists == false)
                        System.Windows.Forms.MessageBox.Show("ERROR. Can't create directory for .obj surfaces: " + this.save_dir_shapes);

                    // ***TEST*** Dump also hierarchy for test
                    ChModelExporterText textExporter = new ChModelExporterText(mSWintegration, save_dir_shapes, save_filename);
                    textExporter.Export();

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
                #if HAS_CHRONO_CSHARP
                    ChModelExporterCSharp jsonExporter = new ChModelExporterCSharp(mSWintegration, save_dir_shapes, save_filename);
                    jsonExporter.SetGravityAcceleration(getGravityAcceleration());
                    jsonExporter.SetSolver(cbSolver.Text);
                    jsonExporter.Export();
                #else
                    MessageBox.Show("Chono SolidWorks AddIn has been built without JSON support. Only a debugging log will be generated.")
                #endif
                }

            }
        }

        private double[] getGravityAcceleration()
        {
            try
            {
                double[] gravity = new double[3];
                gravity[0] = Convert.ToDouble(this.textGravAccX.Text);
                gravity[1] = Convert.ToDouble(this.textGravAccY.Text);
                gravity[2] = Convert.ToDouble(this.textGravAccZ.Text);
                return gravity;
            }
            catch{
                MessageBox.Show("Invalid gravity acceleration values. Using default values (0, -9.81, 0)");
                    return new double[] {0, -9.81, 0};
            }
        }


        private void button_setPrimitiveCollShape_Click(object sender, EventArgs e)
        {
            ModelDoc2 swModel;
            swModel = (ModelDoc2)this.mSWApplication.ActiveDoc;
            if (swModel == null)
            {
                System.Windows.Forms.MessageBox.Show("Please open a part and select a solid body.");
                return;
            }

            SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;

            if (swSelMgr.GetSelectedObjectCount2(-1) == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select one or more solid bodies.");
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
                    System.Windows.Forms.MessageBox.Show("This function can be applied only to solid bodies. Select one or more bodies before using it.");
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
                    EditCollisionParameters myCustomerDialog = new EditCollisionParameters();

                    // Update dialog properties properties from the selected part(s) (i.e. ChBody in C::E) 
                    if (myCustomerDialog.UpdateFromSelection(swSelMgr, ref this.mSWintegration.defattr_collisionParams))
                    {
                        // Show the modal dialog
                        if (myCustomerDialog.ShowDialog() == DialogResult.OK)
                        {
                            // If user pressed OK, apply settings to all selected parts (i.e. ChBody in C::E):
                            myCustomerDialog.StoreToSelection(swSelMgr, ref this.mSWintegration.defattr_collisionParams);//ref this.mSWintegration.defattr_chconveyor);
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

        private void button_settrimeshcoll_Click(object sender, EventArgs e)
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

        private void butt_chronoMotors_Click(object sender, EventArgs e)
        {
            ModelDoc2 swModel;
            swModel = (ModelDoc2)this.mSWApplication.ActiveDoc;
            SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;

            EditChMotor myCustomerDialog = new EditChMotor(ref swSelMgr, ref mSWintegration);
            //myCustomerDialog.ShowDialog(); // show modal
            myCustomerDialog.Show();
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

        private static void createEmptyDirectory(string directoryPath)
        {
            const int maxRetries = 5;
            const int millisecondsDelay = 30;

            bool deleted_folder = false;

            for (int i = 0; i < maxRetries; ++i)
            {
                try
                {
                    if (Directory.Exists(directoryPath))
                    {
                        Directory.Delete(directoryPath, true);
                    }

                    deleted_folder = true;
                }
                catch (IOException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (UnauthorizedAccessException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
            }

            if (deleted_folder)
            {
                Directory.CreateDirectory(directoryPath);
            }
            else
            {
                MessageBox.Show("ERROR: Cannot create directory: " + directoryPath);
            }

        }

        private void but_runSimulation_Click(object sender, EventArgs e)
        {
#if HAS_CHRONO_CSHARP

            // Try to write simulation files in temp path
            string save_directory = Path.GetTempPath();
            bool hasWritePermission = false;

            try
            {
                using (FileStream fs = File.Create(
                    Path.Combine(
                        save_directory,
                        Path.GetRandomFileName()
                    ),
                    1,
                    FileOptions.DeleteOnClose)
                )
                { }
                hasWritePermission = true;
            }
            catch
            {
                hasWritePermission = false;
            }

            if (!hasWritePermission)
            {
                m_folderBrowserDialog.SelectedPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments); // InitialDirectory
                m_folderBrowserDialog.Description = "Select simulation folder";
                DialogResult result = m_folderBrowserDialog.ShowDialog();

                if (result != DialogResult.OK)
                {
                    MessageBox.Show("Simulation cancelled.");
                }
                save_directory = m_folderBrowserDialog.SelectedPath;
            }

            // at this point save_directory should be set to a writable path
            this.save_filename = save_directory + "/simulation_temp.dat";

            if (this.checkBox_surfaces.Checked)
            {
                this.save_dir_shapes = save_directory + "/" + System.IO.Path.GetFileNameWithoutExtension(this.save_filename) + "_shapes";
                createEmptyDirectory(this.save_dir_shapes);
            }

            // initialize the C# model
            var chrono_system_creator = new ChModelExporterCSharp(mSWintegration, save_dir_shapes, this.save_filename);
            chrono_system_creator.SetGravityAcceleration(getGravityAcceleration());
            chrono_system_creator.SetSolver(cbSolver.Text);
            chrono_system_creator.PrepareChronoSystem(true);

            // locate data folder inside add-in installation folder
            string addin_folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            chrono.SetChronoDataPath(addin_folder + "/data/");

            ChSystemNSC chrono_system = chrono_system_creator.GetChronoSystem();

            var vis = new ChVisualSystemIrrlicht();
            vis.SetWindowSize(800, 600);
            vis.SetWindowTitle("Chrono::SolidWorks Simulation");
            vis.Initialize();
            vis.AddLogo();
            vis.AddSkyBox();
            vis.AddTypicalLights();
            vis.AddCamera(new ChVectorD(2, 2, 2));
            vis.AttachSystem(chrono_system);

            chrono_system.SetSolverMaxIterations((int)nud_numIterations.Value);

            var realtime_timer = new ChRealtimeStepTimer();
            double timestep = (double)numeric_dt.Value;

            while (vis.Run())
            {
                vis.BeginScene();
                vis.Render();
                vis.EndScene();

                chrono_system.DoStepDynamics(timestep);
                realtime_timer.Spin(timestep);
            }

#else
            MessageBox.Show("Chrono::SolidWorks Simulation can run only with C# module enabled")
#endif
        }

    }  // end class



}  // end namespace