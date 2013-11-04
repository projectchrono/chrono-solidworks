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
    public partial class ConvexDecomp2 : Form
    {
        public ConvexDecomp2()
        {
            InitializeComponent();
        }

        public void SetMeshInfo(int numfaces, int numvertexes)
        {
            this.label_meshinfo.Text = "Original mesh: " + numfaces + " faces and " + numvertexes + " vertexes.";
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            m_alpha = (double)this.numeric_alpha.Value;
            m_concavity = (double)this.numeric_concavity.Value;
            m_depht = (int)this.numeric_depht.Value;
            m_posrefine = (int)this.numeric_posrefine.Value;
            m_anglerefine = (int)this.numeric_anglerefine.Value;
            m_positionsampling = (int)this.numeric_possampling.Value;
            m_anglesampling = (int)this.numeric_anglesampling.Value;
            m_decimate = (int)this.numeric_decimate.Value;
        }

        public double m_alpha;
        public double m_concavity;
        public int m_depht;
        public int m_posrefine;
        public int m_anglerefine;
        public int m_positionsampling;
        public int m_anglesampling;
        public int m_decimate;
    }
}
