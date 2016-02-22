using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChronoEngineAddin
{
    public partial class ConvexDecompForm : Form
    {
        public ConvexDecompForm()
        {
            InitializeComponent();
        }

        public void SetMeshInfo(int numfaces, int numvertexes)
        {
            this.label_meshinfo.Text = "Original mesh: " + numfaces + " faces and " + numvertexes + " vertexes."; 
        }

        // press OK
        private void button1_Click(object sender, EventArgs e)
        {
            concavity = (double)this.numeric_concavity.Value;
            smallclusterthreshold = (double)this.numeric_smallclusterthreshold.Value;
            compacity = (double)this.numeric_compacity.Value;
            volumeweight = (double)this.numeric_volumeweight.Value;
            connectdistance = (double)this.numeric_connectdistance.Value;
            maxvertexespermesh = (int) this.numeric_maxvertexpermesh.Value;
            minclusters = (int)this.numeric_minclusters.Value;
            maxvertexespercluster = (int)this.numeric_maxnvertexespercluster.Value;
            addextradistancepoints = this.checkBox_addextradistpoints.Checked;
            addextrafacepoints = this.checkBox_addextrafacepoints.Checked;
        }

        public double concavity;
        public double smallclusterthreshold;
        public double compacity;
        public double volumeweight;
        public double connectdistance;
        public int maxvertexespermesh;
        public int minclusters;
        public int maxvertexespercluster;
        public bool addextradistancepoints;
        public bool addextrafacepoints;
    }
}
