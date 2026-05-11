namespace ChronoSolidworksAddin
{
    partial class ConvexDecompForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bt_decompose = new System.Windows.Forms.Button();
            this.bt_cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cb_addHulls = new System.Windows.Forms.CheckBox();
            this.label_meshinfo = new System.Windows.Forms.Label();
            this.tb_maxHullCount = new System.Windows.Forms.TextBox();
            this.tb_maxHullMerge = new System.Windows.Forms.TextBox();
            this.tb_maxHullVertices = new System.Windows.Forms.TextBox();
            this.tb_maxConcavity = new System.Windows.Forms.TextBox();
            this.tb_smallClusterThreshold = new System.Windows.Forms.TextBox();
            this.tb_vertexFuseTolerance = new System.Windows.Forms.TextBox();
            this.cb_saveChullsVerts = new System.Windows.Forms.CheckBox();
            this.cb_saveChullsObj = new System.Windows.Forms.CheckBox();
            this.lab_outFolder = new System.Windows.Forms.Label();
            this.bt_selectOutFolder = new System.Windows.Forms.Button();
            this.gb_output = new System.Windows.Forms.GroupBox();
            this.gb_HACDv2 = new System.Windows.Forms.GroupBox();
            this.rb_HACDv2 = new System.Windows.Forms.RadioButton();
            this.rb_VHACD = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.gb_VHACD = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tb_maxRecursionDepth = new System.Windows.Forms.TextBox();
            this.tb_minVolumePercError = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tb_voxelResolution = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tb_maxVertsPerChull = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tb_maxChullCount = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cb_shrinkWrap = new System.Windows.Forms.CheckBox();
            this.gb_output.SuspendLayout();
            this.gb_HACDv2.SuspendLayout();
            this.gb_VHACD.SuspendLayout();
            this.SuspendLayout();
            // 
            // bt_decompose
            // 
            this.bt_decompose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bt_decompose.Location = new System.Drawing.Point(194, 419);
            this.bt_decompose.Name = "bt_decompose";
            this.bt_decompose.Size = new System.Drawing.Size(75, 23);
            this.bt_decompose.TabIndex = 0;
            this.bt_decompose.Text = "Decompose";
            this.bt_decompose.UseVisualStyleBackColor = true;
            this.bt_decompose.Click += new System.EventHandler(this.bt_decompose_Click);
            // 
            // bt_cancel
            // 
            this.bt_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_cancel.Location = new System.Drawing.Point(286, 419);
            this.bt_cancel.Name = "bt_cancel";
            this.bt_cancel.Size = new System.Drawing.Size(75, 23);
            this.bt_cancel.TabIndex = 1;
            this.bt_cancel.Text = "Cancel";
            this.bt_cancel.UseVisualStyleBackColor = true;
            this.bt_cancel.Click += new System.EventHandler(this.bt_cancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 155);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Vertex fuse tolerance";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Max hull merge";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Max hull vertices";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Max concavity";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Small cluster threshold";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Max hull count";
            // 
            // cb_addHulls
            // 
            this.cb_addHulls.AutoSize = true;
            this.cb_addHulls.BackColor = System.Drawing.SystemColors.Control;
            this.cb_addHulls.Checked = true;
            this.cb_addHulls.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_addHulls.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.cb_addHulls.Location = new System.Drawing.Point(12, 268);
            this.cb_addHulls.Name = "cb_addHulls";
            this.cb_addHulls.Size = new System.Drawing.Size(175, 17);
            this.cb_addHulls.TabIndex = 18;
            this.cb_addHulls.Text = "Add convex hulls to SLDW part";
            this.cb_addHulls.UseVisualStyleBackColor = false;
            // 
            // label_meshinfo
            // 
            this.label_meshinfo.AutoSize = true;
            this.label_meshinfo.Location = new System.Drawing.Point(12, 9);
            this.label_meshinfo.Name = "label_meshinfo";
            this.label_meshinfo.Size = new System.Drawing.Size(56, 13);
            this.label_meshinfo.TabIndex = 20;
            this.label_meshinfo.Text = "Mesh info:";
            // 
            // tb_maxHullCount
            // 
            this.tb_maxHullCount.Location = new System.Drawing.Point(203, 22);
            this.tb_maxHullCount.Name = "tb_maxHullCount";
            this.tb_maxHullCount.Size = new System.Drawing.Size(133, 20);
            this.tb_maxHullCount.TabIndex = 21;
            this.tb_maxHullCount.Text = "512";
            // 
            // tb_maxHullMerge
            // 
            this.tb_maxHullMerge.Location = new System.Drawing.Point(203, 48);
            this.tb_maxHullMerge.Name = "tb_maxHullMerge";
            this.tb_maxHullMerge.Size = new System.Drawing.Size(133, 20);
            this.tb_maxHullMerge.TabIndex = 22;
            this.tb_maxHullMerge.Text = "256";
            // 
            // tb_maxHullVertices
            // 
            this.tb_maxHullVertices.Location = new System.Drawing.Point(203, 74);
            this.tb_maxHullVertices.Name = "tb_maxHullVertices";
            this.tb_maxHullVertices.Size = new System.Drawing.Size(133, 20);
            this.tb_maxHullVertices.TabIndex = 23;
            this.tb_maxHullVertices.Text = "64";
            // 
            // tb_maxConcavity
            // 
            this.tb_maxConcavity.Location = new System.Drawing.Point(203, 100);
            this.tb_maxConcavity.Name = "tb_maxConcavity";
            this.tb_maxConcavity.Size = new System.Drawing.Size(133, 20);
            this.tb_maxConcavity.TabIndex = 24;
            this.tb_maxConcavity.Text = "0.2000";
            // 
            // tb_smallClusterThreshold
            // 
            this.tb_smallClusterThreshold.Location = new System.Drawing.Point(203, 126);
            this.tb_smallClusterThreshold.Name = "tb_smallClusterThreshold";
            this.tb_smallClusterThreshold.Size = new System.Drawing.Size(133, 20);
            this.tb_smallClusterThreshold.TabIndex = 25;
            this.tb_smallClusterThreshold.Text = "0.0000";
            // 
            // tb_vertexFuseTolerance
            // 
            this.tb_vertexFuseTolerance.Location = new System.Drawing.Point(203, 152);
            this.tb_vertexFuseTolerance.Name = "tb_vertexFuseTolerance";
            this.tb_vertexFuseTolerance.Size = new System.Drawing.Size(133, 20);
            this.tb_vertexFuseTolerance.TabIndex = 26;
            this.tb_vertexFuseTolerance.Text = "1e-9";
            // 
            // cb_saveChullsVerts
            // 
            this.cb_saveChullsVerts.AutoSize = true;
            this.cb_saveChullsVerts.BackColor = System.Drawing.SystemColors.Control;
            this.cb_saveChullsVerts.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.cb_saveChullsVerts.Location = new System.Drawing.Point(6, 19);
            this.cb_saveChullsVerts.Name = "cb_saveChullsVerts";
            this.cb_saveChullsVerts.Size = new System.Drawing.Size(168, 17);
            this.cb_saveChullsVerts.TabIndex = 27;
            this.cb_saveChullsVerts.Text = "Save convex hulls vertices list";
            this.cb_saveChullsVerts.UseVisualStyleBackColor = false;
            // 
            // cb_saveChullsObj
            // 
            this.cb_saveChullsObj.AutoSize = true;
            this.cb_saveChullsObj.BackColor = System.Drawing.SystemColors.Control;
            this.cb_saveChullsObj.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.cb_saveChullsObj.Location = new System.Drawing.Point(6, 42);
            this.cb_saveChullsObj.Name = "cb_saveChullsObj";
            this.cb_saveChullsObj.Size = new System.Drawing.Size(147, 17);
            this.cb_saveChullsObj.TabIndex = 28;
            this.cb_saveChullsObj.Text = "Save convex hulls as .obj";
            this.cb_saveChullsObj.UseVisualStyleBackColor = false;
            // 
            // lab_outFolder
            // 
            this.lab_outFolder.AutoSize = true;
            this.lab_outFolder.Location = new System.Drawing.Point(6, 75);
            this.lab_outFolder.Name = "lab_outFolder";
            this.lab_outFolder.Size = new System.Drawing.Size(71, 13);
            this.lab_outFolder.TabIndex = 29;
            this.lab_outFolder.Text = "Output folder:";
            // 
            // bt_selectOutFolder
            // 
            this.bt_selectOutFolder.Location = new System.Drawing.Point(223, 15);
            this.bt_selectOutFolder.Name = "bt_selectOutFolder";
            this.bt_selectOutFolder.Size = new System.Drawing.Size(113, 23);
            this.bt_selectOutFolder.TabIndex = 30;
            this.bt_selectOutFolder.Text = "Select output folder";
            this.bt_selectOutFolder.UseVisualStyleBackColor = true;
            this.bt_selectOutFolder.Click += new System.EventHandler(this.bt_selectOutFolder_Click);
            // 
            // gb_output
            // 
            this.gb_output.Controls.Add(this.cb_saveChullsVerts);
            this.gb_output.Controls.Add(this.bt_selectOutFolder);
            this.gb_output.Controls.Add(this.cb_saveChullsObj);
            this.gb_output.Controls.Add(this.lab_outFolder);
            this.gb_output.Location = new System.Drawing.Point(12, 303);
            this.gb_output.Name = "gb_output";
            this.gb_output.Size = new System.Drawing.Size(349, 100);
            this.gb_output.TabIndex = 31;
            this.gb_output.TabStop = false;
            this.gb_output.Text = "Output files";
            // 
            // gb_HACDv2
            // 
            this.gb_HACDv2.Controls.Add(this.gb_VHACD);
            this.gb_HACDv2.Controls.Add(this.label8);
            this.gb_HACDv2.Controls.Add(this.label1);
            this.gb_HACDv2.Controls.Add(this.tb_vertexFuseTolerance);
            this.gb_HACDv2.Controls.Add(this.label4);
            this.gb_HACDv2.Controls.Add(this.tb_smallClusterThreshold);
            this.gb_HACDv2.Controls.Add(this.label5);
            this.gb_HACDv2.Controls.Add(this.tb_maxConcavity);
            this.gb_HACDv2.Controls.Add(this.label6);
            this.gb_HACDv2.Controls.Add(this.tb_maxHullVertices);
            this.gb_HACDv2.Controls.Add(this.label7);
            this.gb_HACDv2.Controls.Add(this.tb_maxHullMerge);
            this.gb_HACDv2.Controls.Add(this.tb_maxHullCount);
            this.gb_HACDv2.Location = new System.Drawing.Point(12, 72);
            this.gb_HACDv2.Name = "gb_HACDv2";
            this.gb_HACDv2.Size = new System.Drawing.Size(349, 180);
            this.gb_HACDv2.TabIndex = 32;
            this.gb_HACDv2.TabStop = false;
            this.gb_HACDv2.Text = "HACDv2 settings";
            // 
            // rb_HACDv2
            // 
            this.rb_HACDv2.AutoSize = true;
            this.rb_HACDv2.Checked = true;
            this.rb_HACDv2.Location = new System.Drawing.Point(116, 37);
            this.rb_HACDv2.Name = "rb_HACDv2";
            this.rb_HACDv2.Size = new System.Drawing.Size(67, 17);
            this.rb_HACDv2.TabIndex = 33;
            this.rb_HACDv2.TabStop = true;
            this.rb_HACDv2.Text = "HACDv2";
            this.rb_HACDv2.UseVisualStyleBackColor = true;
            // 
            // rb_VHACD
            // 
            this.rb_VHACD.AutoSize = true;
            this.rb_VHACD.Location = new System.Drawing.Point(207, 37);
            this.rb_VHACD.Name = "rb_VHACD";
            this.rb_VHACD.Size = new System.Drawing.Size(62, 17);
            this.rb_VHACD.TabIndex = 34;
            this.rb_VHACD.Text = "VHACD";
            this.rb_VHACD.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 35;
            this.label2.Text = "Select algorithm:";
            // 
            // gb_VHACD
            // 
            this.gb_VHACD.Controls.Add(this.cb_shrinkWrap);
            this.gb_VHACD.Controls.Add(this.label10);
            this.gb_VHACD.Controls.Add(this.label3);
            this.gb_VHACD.Controls.Add(this.label9);
            this.gb_VHACD.Controls.Add(this.tb_maxRecursionDepth);
            this.gb_VHACD.Controls.Add(this.tb_minVolumePercError);
            this.gb_VHACD.Controls.Add(this.label11);
            this.gb_VHACD.Controls.Add(this.tb_voxelResolution);
            this.gb_VHACD.Controls.Add(this.label12);
            this.gb_VHACD.Controls.Add(this.tb_maxVertsPerChull);
            this.gb_VHACD.Controls.Add(this.label13);
            this.gb_VHACD.Controls.Add(this.tb_maxChullCount);
            this.gb_VHACD.Location = new System.Drawing.Point(0, 0);
            this.gb_VHACD.Name = "gb_VHACD";
            this.gb_VHACD.Size = new System.Drawing.Size(349, 180);
            this.gb_VHACD.TabIndex = 33;
            this.gb_VHACD.TabStop = false;
            this.gb_VHACD.Text = "VHACD settings";
            this.gb_VHACD.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Max hull count";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 133);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(103, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Max recursion depth";
            // 
            // tb_maxRecursionDepth
            // 
            this.tb_maxRecursionDepth.Location = new System.Drawing.Point(203, 130);
            this.tb_maxRecursionDepth.Name = "tb_maxRecursionDepth";
            this.tb_maxRecursionDepth.Size = new System.Drawing.Size(133, 20);
            this.tb_maxRecursionDepth.TabIndex = 26;
            this.tb_maxRecursionDepth.Text = "10";
            // 
            // tb_minVolumePercError
            // 
            this.tb_minVolumePercError.Location = new System.Drawing.Point(203, 104);
            this.tb_minVolumePercError.Name = "tb_minVolumePercError";
            this.tb_minVolumePercError.Size = new System.Drawing.Size(133, 20);
            this.tb_minVolumePercError.TabIndex = 25;
            this.tb_minVolumePercError.Text = "1.0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 51);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(86, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Max hull vertices";
            // 
            // tb_voxelResolution
            // 
            this.tb_voxelResolution.Location = new System.Drawing.Point(203, 78);
            this.tb_voxelResolution.Name = "tb_voxelResolution";
            this.tb_voxelResolution.Size = new System.Drawing.Size(133, 20);
            this.tb_voxelResolution.TabIndex = 24;
            this.tb_voxelResolution.Text = "1000";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(13, 81);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(81, 13);
            this.label12.TabIndex = 12;
            this.label12.Text = "Voxel resolution";
            // 
            // tb_maxVertsPerChull
            // 
            this.tb_maxVertsPerChull.Location = new System.Drawing.Point(203, 48);
            this.tb_maxVertsPerChull.Name = "tb_maxVertsPerChull";
            this.tb_maxVertsPerChull.Size = new System.Drawing.Size(133, 20);
            this.tb_maxVertsPerChull.TabIndex = 23;
            this.tb_maxVertsPerChull.Text = "64";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(13, 107);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(142, 13);
            this.label13.TabIndex = 14;
            this.label13.Text = "Min volume percentage error";
            // 
            // tb_maxChullCount
            // 
            this.tb_maxChullCount.Location = new System.Drawing.Point(203, 22);
            this.tb_maxChullCount.Name = "tb_maxChullCount";
            this.tb_maxChullCount.Size = new System.Drawing.Size(133, 20);
            this.tb_maxChullCount.TabIndex = 21;
            this.tb_maxChullCount.Text = "256";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(13, 157);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "Shrink wrap";
            // 
            // cb_shrinkWrap
            // 
            this.cb_shrinkWrap.AutoSize = true;
            this.cb_shrinkWrap.Location = new System.Drawing.Point(203, 156);
            this.cb_shrinkWrap.Name = "cb_shrinkWrap";
            this.cb_shrinkWrap.Size = new System.Drawing.Size(64, 17);
            this.cb_shrinkWrap.TabIndex = 28;
            this.cb_shrinkWrap.Text = "enabled";
            this.cb_shrinkWrap.UseVisualStyleBackColor = true;
            // 
            // ConvexDecompForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 454);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rb_VHACD);
            this.Controls.Add(this.rb_HACDv2);
            this.Controls.Add(this.gb_HACDv2);
            this.Controls.Add(this.gb_output);
            this.Controls.Add(this.label_meshinfo);
            this.Controls.Add(this.cb_addHulls);
            this.Controls.Add(this.bt_cancel);
            this.Controls.Add(this.bt_decompose);
            this.Name = "ConvexDecompForm";
            this.Text = "Convex Decomposition";
            this.gb_output.ResumeLayout(false);
            this.gb_output.PerformLayout();
            this.gb_HACDv2.ResumeLayout(false);
            this.gb_HACDv2.PerformLayout();
            this.gb_VHACD.ResumeLayout(false);
            this.gb_VHACD.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_decompose;
        private System.Windows.Forms.Button bt_cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cb_addHulls;
        private System.Windows.Forms.Label label_meshinfo;
        private System.Windows.Forms.TextBox tb_maxHullCount;
        private System.Windows.Forms.TextBox tb_maxHullMerge;
        private System.Windows.Forms.TextBox tb_maxHullVertices;
        private System.Windows.Forms.TextBox tb_maxConcavity;
        private System.Windows.Forms.TextBox tb_smallClusterThreshold;
        private System.Windows.Forms.TextBox tb_vertexFuseTolerance;
        private System.Windows.Forms.CheckBox cb_saveChullsVerts;
        private System.Windows.Forms.CheckBox cb_saveChullsObj;
        private System.Windows.Forms.Label lab_outFolder;
        private System.Windows.Forms.Button bt_selectOutFolder;
        private System.Windows.Forms.GroupBox gb_output;
        private System.Windows.Forms.GroupBox gb_HACDv2;
        private System.Windows.Forms.RadioButton rb_HACDv2;
        private System.Windows.Forms.RadioButton rb_VHACD;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gb_VHACD;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tb_maxRecursionDepth;
        private System.Windows.Forms.TextBox tb_minVolumePercError;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tb_voxelResolution;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tb_maxVertsPerChull;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tb_maxChullCount;
        private System.Windows.Forms.CheckBox cb_shrinkWrap;
        private System.Windows.Forms.Label label10;
    }
}