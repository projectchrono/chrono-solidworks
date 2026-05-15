using ChronoSolidworks_SwAddin;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ChronoSolidworksAddin
{
    public partial class ConvexDecompForm : Form
    {
        public string outputFolder { get; set; }

        // If true, use of HACDv2 algorithm; VHACD otherwise
        private bool use_HACDv2 { get; set; }

        // HACDv2 convex decomposition settings
        private struct SettingsHACDv2
        {
            public uint maxHullCount { get; set; }
            public uint maxHullMerge { get; set; }
            public uint maxHullVertices { get; set; }
            public float maxConcavity { get; set; }
            public float smallClusterThreshold { get; set; }
            public float vertexFuseTolerance { get; set; }
        }

        // VHACD convex decomposition settings
        private struct SettingsVHACD
        {
            public uint maxChullCount { get; set; }
            public uint maxVertsPerChull { get; set; }
            public uint voxelResolution { get; set; }
            public double minVolumePercError { get; set; }
            public uint maxRecursionDepth { get; set; }
            public bool shrinkWrap { get; set; }
        }

        private SettingsHACDv2 m_settingsHACDv2;
        private SettingsVHACD m_settingsVHACD;

        public ConvexDecompForm()
        {
            InitializeComponent();

            // Set Desktop as fallback output folder for saving convex decomposition result
            outputFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
            lab_outFolder.Text = $"Output folder: {outputFolder}";

            // Enable visibility of decomposition settings
            rb_HACDv2.CheckedChanged += rb_HACDv2_CheckedChanged;

            use_HACDv2 = rb_HACDv2.Checked;
            gb_VHACD.Visible = !rb_HACDv2.Checked; // VHACD on top
        }

        public bool PerformConvexDecomposition(ModelDoc2 swModel, SelectionMgr swSelMgr, UserProgressBar swProgress)
        {
            // Loop through solid bodies
            for (int isel = 1; isel <= swSelMgr.GetSelectedObjectCount2(-1); isel++)
            {
                // Setup -------------------------------------------------------
                if ((swSelectType_e)swSelMgr.GetSelectedObjectType3(isel, -1) != swSelectType_e.swSelSOLIDBODIES)
                {
                    MessageBox.Show("This function can be applied only to Solid Bodies.\nSelect one or more Solid Bodies before using it", "Warning");
                    return false;
                }
                Body2 swBodyIn = (Body2)swSelMgr.GetSelectedObject6(isel, -1);

                // Perform body tessellation -----------------------------------
                Face2 swFace = null;
                Tessellation swTessellation = null;

                // Pass in null so the whole body will be tessellated
                swTessellation = (Tessellation)swBodyIn.GetTessellation(null);

                // Set up the Tessellation object
                swTessellation.NeedFaceFacetMap = true;
                swTessellation.NeedVertexParams = true;
                swTessellation.NeedVertexNormal = true;
                swTessellation.ImprovedQuality = true;

                // How to handle matches across common edges
                swTessellation.MatchType = (int)swTesselationMatchType_e.swTesselationMatchFacetTopology;

                // Execute tessellation
                if (swProgress != null)
                    swProgress.UpdateTitle("Tessellation process...");
                bool bResult = swTessellation.Tessellate();

                // Retrieve facet data -----------------------------------------
                int numVerts = swTessellation.GetVertexCount();
                int numTriangles = swTessellation.GetFacetCount();

                // Get all vertices
                ChVector3d[] tessVertices = new ChVector3d[numVerts];
                ChTriangle[] tessTriangles = new ChTriangle[numTriangles];

                for (int iv = 0; iv < numVerts; iv++)
                {
                    if ((swProgress != null) && (iv % 200 == 0))
                        swProgress.UpdateTitle($"Vertices process: {iv}-th vertex...");

                    double[] aVertexCoords1 = (double[])swTessellation.GetVertexPoint(iv);
                    tessVertices[iv] = new ChVector3d(aVertexCoords1[0], aVertexCoords1[1], aVertexCoords1[2]);
                }

                // Loop over faces
                swFace = (Face2)swBodyIn.GetFirstFace();
                int group_vstride = 0;
                int iface = 0;

                while (swFace != null)
                {
                    int[] aFacetIds = (int[])swTessellation.GetFaceFacets(swFace);
                    int iNumFacetIds = aFacetIds.Length;

                    for (int iFacetIdIdx = 0; iFacetIdIdx < iNumFacetIds; iFacetIdIdx++)
                    {
                        if ((swProgress != null) && (iFacetIdIdx % 100 == 0))
                            swProgress.UpdateTitle($"Faces process: {iFacetIdIdx}-th face...");

                        int[] aFinIds = (int[])swTessellation.GetFacetFins(aFacetIds[iFacetIdIdx]);

                        // There should always be three fins per facet
                        int iFinIdx = 0;
                        int[] aVertexIds = (int[])swTessellation.GetFinVertices(aFinIds[iFinIdx]);

                        int ip1 = aVertexIds[0] + group_vstride;
                        iFinIdx = 1;
                        aVertexIds = (int[])swTessellation.GetFinVertices(aFinIds[iFinIdx]);
                        int ip2 = aVertexIds[0] + group_vstride;
                        iFinIdx = 2;
                        aVertexIds = (int[])swTessellation.GetFinVertices(aFinIds[iFinIdx]);
                        int ip3 = aVertexIds[0] + group_vstride;

                        tessTriangles[iface] = new ChTriangle(tessVertices[ip1], tessVertices[ip2], tessVertices[ip3]);

                        ++iface;
                    }

                    swFace = (Face2)swFace.GetNextFace();
                }

                // Chrono trimesh
                ChTriangleMeshSoup modelMesh = new ChTriangleMeshSoup();
                foreach (ChTriangle tri in tessTriangles)
                {
                    modelMesh.AddTriangle(tri);
                }

                group_vstride += swTessellation.GetVertexCount();

                // Convex decomposition ----------------------------------------
                label_meshinfo.Text = $"Mesh info: {numTriangles} faces, {numVerts} vertices\n";

                // Perform the convex decomposition using the desired algorithm and parameters
                ChConvexDecomposition decomposition = null;
                if (use_HACDv2)
                {
                    decomposition = new ChConvexDecompositionHACDv2();
                    try
                    {
                        decomposition.Reset();
                        decomposition.AddTriangleMesh(modelMesh);

                        ((ChConvexDecompositionHACDv2)decomposition).SetParameters(
                            m_settingsHACDv2.maxHullCount,
                            m_settingsHACDv2.maxHullMerge,
                            m_settingsHACDv2.maxHullVertices,
                            m_settingsHACDv2.maxConcavity,
                            m_settingsHACDv2.smallClusterThreshold,
                            m_settingsHACDv2.vertexFuseTolerance
                        );
                        decomposition.ComputeConvexDecomposition();
                        MessageBox.Show($"HACDv2 convex decomposition successful.\nNumber of decomposed convex hulls: {decomposition.GetHullCount()}", "Info");
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show($"Unable to perform HACDv2 convex decomposition\n{exc.Message}", "Error");
                        return false;
                    }
                }
                else
                {
                    decomposition = new ChConvexDecompositionVHACD();
                    try
                    {
                        decomposition.Reset();
                        decomposition.AddTriangleMesh(modelMesh);

                        ((ChConvexDecompositionVHACD)decomposition).SetParameters(
                            m_settingsVHACD.maxChullCount,
                            m_settingsVHACD.maxVertsPerChull,
                            m_settingsVHACD.voxelResolution,
                            m_settingsVHACD.minVolumePercError,
                            m_settingsVHACD.maxRecursionDepth,
                            m_settingsVHACD.shrinkWrap
                        );
                        decomposition.ComputeConvexDecomposition();
                        MessageBox.Show($"VHACD convex decomposition successful.\nNumber of decomposed convex hulls: {decomposition.GetHullCount()}", "Info");
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show($"Unable to perform VHACD convex decomposition\n{exc.Message}", "Error");
                        return false;
                    }
                }

                // Create Solidworks bodies corresponding to computed convex hulls
                if (cb_addHulls.Checked)
                {
                    uint nhulls = decomposition.GetHullCount();
                    Body2 newSldwBody = null;
                    IPartDoc mpart = null;

                    if (nhulls > 0)
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

                    Random rnd = new Random(); // for random chulls colors
                    for (uint ic = 0; ic < nhulls; ic++)
                    {
                        ChTriangleMeshSoup hullMesh = new ChTriangleMeshSoup();
                        decomposition.GetConvexHullResult(ic, hullMesh);

                        if (mpart != null)
                        {
                            newSldwBody = mpart.ICreateNewBody2();

                            for (uint i = 0; i < hullMesh.GetNumTriangles(); ++i)
                            {
                                double[] vPt = new double[9]; // flattened data of triangle three vertices (9 numbers)
                                vPt[0] = hullMesh.GetTriangle(i).p1.x;
                                vPt[1] = hullMesh.GetTriangle(i).p1.y;
                                vPt[2] = hullMesh.GetTriangle(i).p1.z;
                                vPt[3] = hullMesh.GetTriangle(i).p2.x;
                                vPt[4] = hullMesh.GetTriangle(i).p2.y;
                                vPt[5] = hullMesh.GetTriangle(i).p2.z;
                                vPt[6] = hullMesh.GetTriangle(i).p3.x;
                                vPt[7] = hullMesh.GetTriangle(i).p3.y;
                                vPt[8] = hullMesh.GetTriangle(i).p3.z;
                                newSldwBody.CreatePlanarTrimSurfaceDLL(vPt, null);
                            }

                            bool created = newSldwBody.CreateBodyFromSurfaces();
                            if (created)
                            {
                                ModelDocExtension swExt = swModel.Extension;
                                Feature lastFeature = swExt.GetLastFeatureAdded(); // get feature representing last new body
                                double[] color = new double[9] { rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble(), 0.8, 0.8, 0.8, 16, 0.1, 0 }; // R, G, B, Ambient, Diffuse, Specular, Shininess, Transparency, Emission 
                                lastFeature.SetMaterialPropertyValues2(color, (int)swInConfigurationOpts_e.swThisConfiguration, null);
                            }
                        }
                    } // end loop on computed convex hulls

                    //swModel.GraphicsRedraw2();
                    swModel.ForceRebuild3(false);

                } // end IsAddHullsChecked()

                // Save convex decomposition result to disk, if required
                if (cb_saveChullsObj.Checked)
                {
                    string partName = Path.GetFileNameWithoutExtension(swModel.GetTitle());
                    if (use_HACDv2)
                        ((ChConvexDecompositionHACDv2)decomposition).WriteConvexHullsAsWavefrontObj($"{outputFolder}/{partName}_convex_decomp.obj");
                    else
                        ((ChConvexDecompositionVHACD)decomposition).WriteConvexHullsAsWavefrontObj($"{outputFolder}/{partName}_convex_decomp.obj");
                }

                if (cb_saveChullsVerts.Checked)
                {
                    string partName = Path.GetFileNameWithoutExtension(swModel.GetTitle());
                    if (use_HACDv2)
                        ((ChConvexDecompositionHACDv2)decomposition).WriteConvexHullsAsChullsFile($"{outputFolder}/{partName}_convex_decomp.chulls");
                    else
                        ((ChConvexDecompositionVHACD)decomposition).WriteConvexHullsAsChullsFile($"{outputFolder}/{partName}_convex_decomp.chulls");
                }

            } // end loop on selected items

            return true;
        }

        private void bt_decompose_Click(object sender, EventArgs e)
        {
            if (use_HACDv2)
            {
                m_settingsHACDv2.maxHullCount = uint.Parse(tb_maxHullCount.Text);
                m_settingsHACDv2.maxHullMerge = uint.Parse(tb_maxHullMerge.Text);
                m_settingsHACDv2.maxHullVertices = uint.Parse(tb_maxHullVertices.Text);
                m_settingsHACDv2.maxConcavity = float.Parse(tb_maxConcavity.Text);
                m_settingsHACDv2.smallClusterThreshold = float.Parse(tb_smallClusterThreshold.Text);
                m_settingsHACDv2.vertexFuseTolerance = float.Parse(tb_vertexFuseTolerance.Text);
            }
            else
            {
                m_settingsVHACD.maxChullCount = uint.Parse(tb_maxChullCount.Text);
                m_settingsVHACD.maxVertsPerChull = uint.Parse(tb_maxVertsPerChull.Text);
                m_settingsVHACD.voxelResolution = uint.Parse(tb_voxelResolution.Text);
                m_settingsVHACD.minVolumePercError = double.Parse(tb_minVolumePercError.Text);
                m_settingsVHACD.maxRecursionDepth = uint.Parse(tb_maxRecursionDepth.Text);
                m_settingsVHACD.shrinkWrap = cb_shrinkWrap.Checked;
            }
        }

        private void bt_selectOutFolder_Click(object sender, EventArgs e)
        {
            // Folder select dialog
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select a folder";
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    outputFolder = folderDialog.SelectedPath;
                    lab_outFolder.Text = $"Output folder: {outputFolder}";
                }
            }
        }

        private void bt_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void rb_HACDv2_CheckedChanged(object sender, EventArgs e)
        {
            use_HACDv2 = rb_HACDv2.Checked;
            gb_VHACD.Visible = !rb_HACDv2.Checked; // VHACD on top
        }

    }
}
