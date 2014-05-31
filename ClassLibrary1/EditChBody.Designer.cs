namespace ChronoEngineAddin
{
    partial class EditChBody
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
            this.button_Cancel = new System.Windows.Forms.Button();
            this.label_friction = new System.Windows.Forms.Label();
            this.numeric_friction = new System.Windows.Forms.NumericUpDown();
            this.label_rolling_friction = new System.Windows.Forms.Label();
            this.numeric_rolling_friction = new System.Windows.Forms.NumericUpDown();
            this.label_spinning_friction = new System.Windows.Forms.Label();
            this.numeric_spinning_friction = new System.Windows.Forms.NumericUpDown();
            this.label_restitution = new System.Windows.Forms.Label();
            this.numeric_restitution = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numeric_collision_margin = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numeric_collision_envelope = new System.Windows.Forms.NumericUpDown();
            this.comboBox_collision_family = new System.Windows.Forms.ComboBox();
            this.label_collision_family = new System.Windows.Forms.Label();
            this.checkBox_collide = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_friction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_rolling_friction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_spinning_friction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_restitution)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_collision_margin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_collision_envelope)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_ok
            // 
            this.button_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_ok.Location = new System.Drawing.Point(6, 309);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 1;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(87, 309);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 2;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // label_friction
            // 
            this.label_friction.AutoSize = true;
            this.label_friction.Location = new System.Drawing.Point(23, 35);
            this.label_friction.Name = "label_friction";
            this.label_friction.Size = new System.Drawing.Size(93, 13);
            this.label_friction.TabIndex = 7;
            this.label_friction.Text = "Friction coefficient";
            // 
            // numeric_friction
            // 
            this.numeric_friction.DecimalPlaces = 3;
            this.numeric_friction.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numeric_friction.Location = new System.Drawing.Point(201, 33);
            this.numeric_friction.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numeric_friction.Name = "numeric_friction";
            this.numeric_friction.Size = new System.Drawing.Size(120, 20);
            this.numeric_friction.TabIndex = 6;
            this.numeric_friction.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            // 
            // label_rolling_friction
            // 
            this.label_rolling_friction.AutoSize = true;
            this.label_rolling_friction.Location = new System.Drawing.Point(23, 61);
            this.label_rolling_friction.Name = "label_rolling_friction";
            this.label_rolling_friction.Size = new System.Drawing.Size(125, 13);
            this.label_rolling_friction.TabIndex = 9;
            this.label_rolling_friction.Text = "Rolling friction coefficient";
            // 
            // numeric_rolling_friction
            // 
            this.numeric_rolling_friction.DecimalPlaces = 3;
            this.numeric_rolling_friction.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numeric_rolling_friction.Location = new System.Drawing.Point(201, 59);
            this.numeric_rolling_friction.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numeric_rolling_friction.Name = "numeric_rolling_friction";
            this.numeric_rolling_friction.Size = new System.Drawing.Size(120, 20);
            this.numeric_rolling_friction.TabIndex = 8;
            // 
            // label_spinning_friction
            // 
            this.label_spinning_friction.AutoSize = true;
            this.label_spinning_friction.Location = new System.Drawing.Point(23, 83);
            this.label_spinning_friction.Name = "label_spinning_friction";
            this.label_spinning_friction.Size = new System.Drawing.Size(134, 13);
            this.label_spinning_friction.TabIndex = 11;
            this.label_spinning_friction.Text = "Spinning friction coefficient";
            // 
            // numeric_spinning_friction
            // 
            this.numeric_spinning_friction.DecimalPlaces = 3;
            this.numeric_spinning_friction.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numeric_spinning_friction.Location = new System.Drawing.Point(201, 81);
            this.numeric_spinning_friction.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numeric_spinning_friction.Name = "numeric_spinning_friction";
            this.numeric_spinning_friction.Size = new System.Drawing.Size(120, 20);
            this.numeric_spinning_friction.TabIndex = 10;
            // 
            // label_restitution
            // 
            this.label_restitution.AutoSize = true;
            this.label_restitution.Location = new System.Drawing.Point(23, 112);
            this.label_restitution.Name = "label_restitution";
            this.label_restitution.Size = new System.Drawing.Size(109, 13);
            this.label_restitution.TabIndex = 13;
            this.label_restitution.Text = "Restitution coefficient";
            // 
            // numeric_restitution
            // 
            this.numeric_restitution.DecimalPlaces = 3;
            this.numeric_restitution.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numeric_restitution.Location = new System.Drawing.Point(201, 106);
            this.numeric_restitution.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_restitution.Name = "numeric_restitution";
            this.numeric_restitution.Size = new System.Drawing.Size(120, 20);
            this.numeric_restitution.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Inward collision margin [m]";
            // 
            // numeric_collision_margin
            // 
            this.numeric_collision_margin.DecimalPlaces = 4;
            this.numeric_collision_margin.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numeric_collision_margin.Location = new System.Drawing.Point(201, 151);
            this.numeric_collision_margin.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_collision_margin.Name = "numeric_collision_margin";
            this.numeric_collision_margin.Size = new System.Drawing.Size(120, 20);
            this.numeric_collision_margin.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 180);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Outward collision envelope [m]";
            // 
            // numeric_collision_envelope
            // 
            this.numeric_collision_envelope.DecimalPlaces = 4;
            this.numeric_collision_envelope.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numeric_collision_envelope.Location = new System.Drawing.Point(201, 173);
            this.numeric_collision_envelope.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_collision_envelope.Name = "numeric_collision_envelope";
            this.numeric_collision_envelope.Size = new System.Drawing.Size(120, 20);
            this.numeric_collision_envelope.TabIndex = 16;
            // 
            // comboBox_collision_family
            // 
            this.comboBox_collision_family.FormattingEnabled = true;
            this.comboBox_collision_family.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15"});
            this.comboBox_collision_family.Location = new System.Drawing.Point(201, 207);
            this.comboBox_collision_family.Name = "comboBox_collision_family";
            this.comboBox_collision_family.Size = new System.Drawing.Size(121, 21);
            this.comboBox_collision_family.TabIndex = 18;
            // 
            // label_collision_family
            // 
            this.label_collision_family.AutoSize = true;
            this.label_collision_family.Location = new System.Drawing.Point(23, 210);
            this.label_collision_family.Name = "label_collision_family";
            this.label_collision_family.Size = new System.Drawing.Size(74, 13);
            this.label_collision_family.TabIndex = 19;
            this.label_collision_family.Text = "Collision family";
            // 
            // checkBox_collide
            // 
            this.checkBox_collide.AutoSize = true;
            this.checkBox_collide.Checked = true;
            this.checkBox_collide.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_collide.Location = new System.Drawing.Point(6, 0);
            this.checkBox_collide.Name = "checkBox_collide";
            this.checkBox_collide.Size = new System.Drawing.Size(193, 17);
            this.checkBox_collide.TabIndex = 20;
            this.checkBox_collide.Text = "Collide in Chrono::Engine simulation";
            this.checkBox_collide.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_collide);
            this.groupBox1.Location = new System.Drawing.Point(6, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(320, 221);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "          ";
            // 
            // EditChBody
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 344);
            this.Controls.Add(this.label_collision_family);
            this.Controls.Add(this.comboBox_collision_family);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numeric_collision_envelope);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numeric_collision_margin);
            this.Controls.Add(this.label_restitution);
            this.Controls.Add(this.numeric_restitution);
            this.Controls.Add(this.label_spinning_friction);
            this.Controls.Add(this.numeric_spinning_friction);
            this.Controls.Add(this.label_rolling_friction);
            this.Controls.Add(this.numeric_rolling_friction);
            this.Controls.Add(this.label_friction);
            this.Controls.Add(this.numeric_friction);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.groupBox1);
            this.Name = "EditChBody";
            this.Text = "Chrono rigid body properties";
            ((System.ComponentModel.ISupportInitialize)(this.numeric_friction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_rolling_friction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_spinning_friction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_restitution)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_collision_margin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_collision_envelope)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Label label_friction;
        private System.Windows.Forms.NumericUpDown numeric_friction;
        private System.Windows.Forms.Label label_rolling_friction;
        private System.Windows.Forms.NumericUpDown numeric_rolling_friction;
        private System.Windows.Forms.Label label_spinning_friction;
        private System.Windows.Forms.NumericUpDown numeric_spinning_friction;
        private System.Windows.Forms.Label label_restitution;
        private System.Windows.Forms.NumericUpDown numeric_restitution;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numeric_collision_margin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numeric_collision_envelope;
        private System.Windows.Forms.ComboBox comboBox_collision_family;
        private System.Windows.Forms.Label label_collision_family;
        private System.Windows.Forms.CheckBox checkBox_collide;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}