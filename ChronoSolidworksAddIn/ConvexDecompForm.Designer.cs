namespace ChronoEngineAddin
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.numeric_compacity = new System.Windows.Forms.NumericUpDown();
            this.numeric_volumeweight = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numeric_connectdistance = new System.Windows.Forms.NumericUpDown();
            this.numeric_minclusters = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numeric_maxnvertexespercluster = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numeric_concavity = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numeric_smallclusterthreshold = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.numeric_maxvertexpermesh = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBox_addextradistpoints = new System.Windows.Forms.CheckBox();
            this.checkBox_addextrafacepoints = new System.Windows.Forms.CheckBox();
            this.label_meshinfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_compacity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_volumeweight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_connectdistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_minclusters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_maxnvertexespercluster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_concavity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_smallclusterthreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_maxvertexpermesh)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(13, 335);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(123, 335);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // numeric_compacity
            // 
            this.numeric_compacity.DecimalPlaces = 4;
            this.numeric_compacity.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numeric_compacity.Location = new System.Drawing.Point(152, 146);
            this.numeric_compacity.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numeric_compacity.Name = "numeric_compacity";
            this.numeric_compacity.Size = new System.Drawing.Size(120, 20);
            this.numeric_compacity.TabIndex = 2;
            this.numeric_compacity.Value = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            // 
            // numeric_volumeweight
            // 
            this.numeric_volumeweight.DecimalPlaces = 4;
            this.numeric_volumeweight.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numeric_volumeweight.Location = new System.Drawing.Point(152, 172);
            this.numeric_volumeweight.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numeric_volumeweight.Name = "numeric_volumeweight";
            this.numeric_volumeweight.Size = new System.Drawing.Size(120, 20);
            this.numeric_volumeweight.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Compacity";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 174);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Volume weight";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 201);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Connect distance";
            // 
            // numeric_connectdistance
            // 
            this.numeric_connectdistance.BackColor = System.Drawing.SystemColors.Info;
            this.numeric_connectdistance.DecimalPlaces = 4;
            this.numeric_connectdistance.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numeric_connectdistance.Location = new System.Drawing.Point(152, 199);
            this.numeric_connectdistance.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numeric_connectdistance.Name = "numeric_connectdistance";
            this.numeric_connectdistance.Size = new System.Drawing.Size(120, 20);
            this.numeric_connectdistance.TabIndex = 7;
            this.numeric_connectdistance.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // numeric_minclusters
            // 
            this.numeric_minclusters.Location = new System.Drawing.Point(152, 43);
            this.numeric_minclusters.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numeric_minclusters.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_minclusters.Name = "numeric_minclusters";
            this.numeric_minclusters.Size = new System.Drawing.Size(120, 20);
            this.numeric_minclusters.TabIndex = 9;
            this.numeric_minclusters.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Minimum n. of clusters";
            // 
            // numeric_maxnvertexespercluster
            // 
            this.numeric_maxnvertexespercluster.Location = new System.Drawing.Point(152, 69);
            this.numeric_maxnvertexespercluster.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numeric_maxnvertexespercluster.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numeric_maxnvertexespercluster.Name = "numeric_maxnvertexespercluster";
            this.numeric_maxnvertexespercluster.Size = new System.Drawing.Size(120, 20);
            this.numeric_maxnvertexespercluster.TabIndex = 11;
            this.numeric_maxnvertexespercluster.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(135, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Max n. of vertex per cluster";
            // 
            // numeric_concavity
            // 
            this.numeric_concavity.BackColor = System.Drawing.SystemColors.Info;
            this.numeric_concavity.DecimalPlaces = 2;
            this.numeric_concavity.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numeric_concavity.Location = new System.Drawing.Point(152, 95);
            this.numeric_concavity.Name = "numeric_concavity";
            this.numeric_concavity.Size = new System.Drawing.Size(120, 20);
            this.numeric_concavity.TabIndex = 13;
            this.numeric_concavity.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 97);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Concavity";
            // 
            // numeric_smallclusterthreshold
            // 
            this.numeric_smallclusterthreshold.DecimalPlaces = 4;
            this.numeric_smallclusterthreshold.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numeric_smallclusterthreshold.Location = new System.Drawing.Point(152, 121);
            this.numeric_smallclusterthreshold.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_smallclusterthreshold.Name = "numeric_smallclusterthreshold";
            this.numeric_smallclusterthreshold.Size = new System.Drawing.Size(120, 20);
            this.numeric_smallclusterthreshold.TabIndex = 15;
            this.numeric_smallclusterthreshold.Value = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Small cluster threshold";
            // 
            // numeric_maxvertexpermesh
            // 
            this.numeric_maxvertexpermesh.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numeric_maxvertexpermesh.Location = new System.Drawing.Point(152, 17);
            this.numeric_maxvertexpermesh.Maximum = new decimal(new int[] {
            9000000,
            0,
            0,
            0});
            this.numeric_maxvertexpermesh.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numeric_maxvertexpermesh.Name = "numeric_maxvertexpermesh";
            this.numeric_maxvertexpermesh.Size = new System.Drawing.Size(120, 20);
            this.numeric_maxvertexpermesh.TabIndex = 17;
            this.numeric_maxvertexpermesh.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(129, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Max n. of vertex per mesh";
            // 
            // checkBox_addextradistpoints
            // 
            this.checkBox_addextradistpoints.AutoSize = true;
            this.checkBox_addextradistpoints.BackColor = System.Drawing.SystemColors.Control;
            this.checkBox_addextradistpoints.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.checkBox_addextradistpoints.Location = new System.Drawing.Point(15, 236);
            this.checkBox_addextradistpoints.Name = "checkBox_addextradistpoints";
            this.checkBox_addextradistpoints.Size = new System.Drawing.Size(145, 17);
            this.checkBox_addextradistpoints.TabIndex = 18;
            this.checkBox_addextradistpoints.Text = "Add extra distance points";
            this.checkBox_addextradistpoints.UseVisualStyleBackColor = false;
            // 
            // checkBox_addextrafacepoints
            // 
            this.checkBox_addextrafacepoints.AutoSize = true;
            this.checkBox_addextrafacepoints.Location = new System.Drawing.Point(15, 259);
            this.checkBox_addextrafacepoints.Name = "checkBox_addextrafacepoints";
            this.checkBox_addextrafacepoints.Size = new System.Drawing.Size(126, 17);
            this.checkBox_addextrafacepoints.TabIndex = 19;
            this.checkBox_addextrafacepoints.Text = "Add extra face points";
            this.checkBox_addextrafacepoints.UseVisualStyleBackColor = true;
            // 
            // label_meshinfo
            // 
            this.label_meshinfo.AutoSize = true;
            this.label_meshinfo.Location = new System.Drawing.Point(12, 290);
            this.label_meshinfo.Name = "label_meshinfo";
            this.label_meshinfo.Size = new System.Drawing.Size(53, 13);
            this.label_meshinfo.TabIndex = 20;
            this.label_meshinfo.Text = "Mesh info";
            // 
            // ConvexDecompForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 370);
            this.Controls.Add(this.label_meshinfo);
            this.Controls.Add(this.checkBox_addextrafacepoints);
            this.Controls.Add(this.checkBox_addextradistpoints);
            this.Controls.Add(this.numeric_maxvertexpermesh);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.numeric_smallclusterthreshold);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numeric_concavity);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numeric_maxnvertexespercluster);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numeric_minclusters);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numeric_connectdistance);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numeric_volumeweight);
            this.Controls.Add(this.numeric_compacity);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "ConvexDecompForm";
            this.Text = "Convex decomposition settings";
            ((System.ComponentModel.ISupportInitialize)(this.numeric_compacity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_volumeweight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_connectdistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_minclusters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_maxnvertexespercluster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_concavity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_smallclusterthreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_maxvertexpermesh)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NumericUpDown numeric_compacity;
        private System.Windows.Forms.NumericUpDown numeric_volumeweight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numeric_connectdistance;
        private System.Windows.Forms.NumericUpDown numeric_minclusters;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numeric_maxnvertexespercluster;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numeric_concavity;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numeric_smallclusterthreshold;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numeric_maxvertexpermesh;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkBox_addextradistpoints;
        private System.Windows.Forms.CheckBox checkBox_addextrafacepoints;
        private System.Windows.Forms.Label label_meshinfo;
    }
}