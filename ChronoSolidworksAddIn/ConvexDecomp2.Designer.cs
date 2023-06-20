namespace ChronoEngineAddin
{
    partial class ConvexDecomp2
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
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.numeric_decimate = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numeric_alpha = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numeric_concavity = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numeric_depht = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numeric_possampling = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numeric_anglesampling = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.numeric_anglerefine = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.numeric_posrefine = new System.Windows.Forms.NumericUpDown();
            this.label_meshinfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_decimate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_alpha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_concavity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_depht)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_possampling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_anglesampling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_anglerefine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_posrefine)).BeginInit();
            this.SuspendLayout();
            // 
            // button_ok
            // 
            this.button_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_ok.Location = new System.Drawing.Point(12, 314);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 0;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.Location = new System.Drawing.Point(93, 314);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            // 
            // numeric_decimate
            // 
            this.numeric_decimate.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numeric_decimate.Location = new System.Drawing.Point(214, 29);
            this.numeric_decimate.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numeric_decimate.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numeric_decimate.Name = "numeric_decimate";
            this.numeric_decimate.Size = new System.Drawing.Size(120, 20);
            this.numeric_decimate.TabIndex = 2;
            this.numeric_decimate.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Limit n. of vertexes in original mesh ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Alpha parameter";
            // 
            // numeric_alpha
            // 
            this.numeric_alpha.DecimalPlaces = 3;
            this.numeric_alpha.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numeric_alpha.Location = new System.Drawing.Point(214, 75);
            this.numeric_alpha.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_alpha.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numeric_alpha.Name = "numeric_alpha";
            this.numeric_alpha.Size = new System.Drawing.Size(120, 20);
            this.numeric_alpha.TabIndex = 4;
            this.numeric_alpha.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Concavity parameter";
            // 
            // numeric_concavity
            // 
            this.numeric_concavity.DecimalPlaces = 3;
            this.numeric_concavity.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numeric_concavity.Location = new System.Drawing.Point(214, 98);
            this.numeric_concavity.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_concavity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numeric_concavity.Name = "numeric_concavity";
            this.numeric_concavity.Size = new System.Drawing.Size(120, 20);
            this.numeric_concavity.TabIndex = 6;
            this.numeric_concavity.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Depht of decomposition";
            // 
            // numeric_depht
            // 
            this.numeric_depht.Location = new System.Drawing.Point(214, 133);
            this.numeric_depht.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_depht.Name = "numeric_depht";
            this.numeric_depht.Size = new System.Drawing.Size(120, 20);
            this.numeric_depht.TabIndex = 8;
            this.numeric_depht.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 161);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Position sampling";
            // 
            // numeric_possampling
            // 
            this.numeric_possampling.Location = new System.Drawing.Point(214, 159);
            this.numeric_possampling.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_possampling.Name = "numeric_possampling";
            this.numeric_possampling.Size = new System.Drawing.Size(120, 20);
            this.numeric_possampling.TabIndex = 10;
            this.numeric_possampling.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 187);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Angle sampling";
            // 
            // numeric_anglesampling
            // 
            this.numeric_anglesampling.Location = new System.Drawing.Point(214, 185);
            this.numeric_anglesampling.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_anglesampling.Name = "numeric_anglesampling";
            this.numeric_anglesampling.Size = new System.Drawing.Size(120, 20);
            this.numeric_anglesampling.TabIndex = 12;
            this.numeric_anglesampling.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 239);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Angle refine";
            // 
            // numeric_anglerefine
            // 
            this.numeric_anglerefine.Location = new System.Drawing.Point(214, 237);
            this.numeric_anglerefine.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_anglerefine.Name = "numeric_anglerefine";
            this.numeric_anglerefine.Size = new System.Drawing.Size(120, 20);
            this.numeric_anglerefine.TabIndex = 16;
            this.numeric_anglerefine.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 213);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Position refine";
            // 
            // numeric_posrefine
            // 
            this.numeric_posrefine.Location = new System.Drawing.Point(214, 211);
            this.numeric_posrefine.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_posrefine.Name = "numeric_posrefine";
            this.numeric_posrefine.Size = new System.Drawing.Size(120, 20);
            this.numeric_posrefine.TabIndex = 14;
            this.numeric_posrefine.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label_meshinfo
            // 
            this.label_meshinfo.AutoSize = true;
            this.label_meshinfo.Location = new System.Drawing.Point(12, 9);
            this.label_meshinfo.Name = "label_meshinfo";
            this.label_meshinfo.Size = new System.Drawing.Size(53, 13);
            this.label_meshinfo.TabIndex = 18;
            this.label_meshinfo.Text = "Mesh info";
            // 
            // ConvexDecomp2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 349);
            this.Controls.Add(this.label_meshinfo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numeric_anglerefine);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.numeric_posrefine);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numeric_anglesampling);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numeric_possampling);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numeric_depht);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numeric_concavity);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numeric_alpha);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numeric_decimate);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Name = "ConvexDecomp2";
            this.Text = "Convex Decomposition";
            ((System.ComponentModel.ISupportInitialize)(this.numeric_decimate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_alpha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_concavity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_depht)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_possampling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_anglesampling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_anglerefine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_posrefine)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.NumericUpDown numeric_decimate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numeric_alpha;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numeric_concavity;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numeric_depht;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numeric_possampling;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numeric_anglesampling;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numeric_anglerefine;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numeric_posrefine;
        private System.Windows.Forms.Label label_meshinfo;
    }
}