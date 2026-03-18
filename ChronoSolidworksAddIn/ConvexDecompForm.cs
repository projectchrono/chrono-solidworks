using System;
using System.Windows.Forms;

namespace ChronoEngineAddin
{
    public partial class ConvexDecompForm : Form
    {
        public uint maxHullCount { get; set; }
        public uint maxHullMerge { get; set; }
        public uint maxHullVertices { get; set; }
        public float maxConcavity { get; set; }
        public float smallClusterThreshold { get; set; }
        public float vertexFuseTolerance { get; set; }

        public String outputFolder { get; set; }

        public ConvexDecompForm()
        {
            InitializeComponent();

            // Set Desktop as fallback output folder for saving convex decomposition result
            outputFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
            lab_outFolder.Text = $"Output folder: {outputFolder}";
        }

        public void SetMeshInfo(int numfaces, int numvertexes)
        {
            label_meshinfo.Text = $"Mesh info: {numfaces} faces, {numvertexes} vertices\n";
        }

        public bool IsAddHullsChecked()
        {
            return cb_addHulls.Checked;
        }

        public bool IsSaveChullsVertsChecked()
        {
            return cb_saveChullsVerts.Checked;
        }

        public bool IsSaveChullsObjChecked()
        {
            return cb_saveChullsObj.Checked;
        }

        private void bt_decompose_Click(object sender, EventArgs e)
        {
            maxHullCount = uint.Parse(tb_maxHullCount.Text);
            maxHullMerge = uint.Parse(tb_maxHullMerge.Text);
            maxHullVertices = uint.Parse(tb_maxHullVertices.Text);
            maxConcavity = float.Parse(tb_maxConcavity.Text);
            smallClusterThreshold = float.Parse(tb_smallClusterThreshold.Text);
            vertexFuseTolerance = float.Parse(tb_vertexFuseTolerance.Text);
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
    }
}
