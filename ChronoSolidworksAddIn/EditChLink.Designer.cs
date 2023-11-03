namespace ChronoEngineAddin
{
    partial class EditChLink
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
            this.lab_motorType = new System.Windows.Forms.Label();
            this.lab_motorControl = new System.Windows.Forms.Label();
            this.lab_matesSelected = new System.Windows.Forms.Label();
            this.cb_motorType = new System.Windows.Forms.ComboBox();
            this.cb_motorControl = new System.Windows.Forms.ComboBox();
            this.lst_matesSelected = new System.Windows.Forms.ListView();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.butt_addMate = new System.Windows.Forms.Button();
            this.butt_addMotor = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lab_motorType
            // 
            this.lab_motorType.AutoSize = true;
            this.lab_motorType.Location = new System.Drawing.Point(12, 22);
            this.lab_motorType.Name = "lab_motorType";
            this.lab_motorType.Size = new System.Drawing.Size(60, 13);
            this.lab_motorType.TabIndex = 0;
            this.lab_motorType.Text = "Motor type:";
            // 
            // lab_motorControl
            // 
            this.lab_motorControl.AutoSize = true;
            this.lab_motorControl.Location = new System.Drawing.Point(12, 72);
            this.lab_motorControl.Name = "lab_motorControl";
            this.lab_motorControl.Size = new System.Drawing.Size(72, 13);
            this.lab_motorControl.TabIndex = 1;
            this.lab_motorControl.Text = "Motor control:";
            // 
            // lab_matesSelected
            // 
            this.lab_matesSelected.AutoSize = true;
            this.lab_matesSelected.Location = new System.Drawing.Point(12, 261);
            this.lab_matesSelected.Name = "lab_matesSelected";
            this.lab_matesSelected.Size = new System.Drawing.Size(82, 13);
            this.lab_matesSelected.TabIndex = 2;
            this.lab_matesSelected.Text = "Mates selected:";
            // 
            // cb_motorType
            // 
            this.cb_motorType.FormattingEnabled = true;
            this.cb_motorType.Items.AddRange(new object[] {
            "Rotation",
            "Translation"});
            this.cb_motorType.Location = new System.Drawing.Point(117, 19);
            this.cb_motorType.Name = "cb_motorType";
            this.cb_motorType.Size = new System.Drawing.Size(191, 21);
            this.cb_motorType.TabIndex = 3;
            this.cb_motorType.Text = "Rotation";
            // 
            // cb_motorControl
            // 
            this.cb_motorControl.FormattingEnabled = true;
            this.cb_motorControl.Items.AddRange(new object[] {
            "Position",
            "Velocity",
            "Torque"});
            this.cb_motorControl.Location = new System.Drawing.Point(117, 69);
            this.cb_motorControl.Name = "cb_motorControl";
            this.cb_motorControl.Size = new System.Drawing.Size(191, 21);
            this.cb_motorControl.TabIndex = 4;
            this.cb_motorControl.Text = "Position";
            // 
            // lst_matesSelected
            // 
            this.lst_matesSelected.HideSelection = false;
            this.lst_matesSelected.Location = new System.Drawing.Point(117, 187);
            this.lst_matesSelected.Name = "lst_matesSelected";
            this.lst_matesSelected.Size = new System.Drawing.Size(191, 97);
            this.lst_matesSelected.TabIndex = 5;
            this.lst_matesSelected.UseCompatibleStateImageBehavior = false;
            this.lst_matesSelected.View = System.Windows.Forms.View.List;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "ChFunction_Const",
            "ChFunction_ConstAcc",
            "ChFunction_Cycloidal",
            "ChFunction_DoubleS",
            "ChFunction_Poly345",
            "ChFunction_Setpoint",
            "ChFunction_Sine"});
            this.comboBox1.Location = new System.Drawing.Point(117, 127);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(191, 21);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.Text = "ChFunction_Setpoint";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Motion law:";
            // 
            // butt_addMate
            // 
            this.butt_addMate.Location = new System.Drawing.Point(15, 200);
            this.butt_addMate.Name = "butt_addMate";
            this.butt_addMate.Size = new System.Drawing.Size(75, 23);
            this.butt_addMate.TabIndex = 8;
            this.butt_addMate.Text = "Add mate";
            this.butt_addMate.UseVisualStyleBackColor = true;
            this.butt_addMate.Click += new System.EventHandler(this.butt_addMate_Click);
            // 
            // butt_addMotor
            // 
            this.butt_addMotor.Location = new System.Drawing.Point(117, 318);
            this.butt_addMotor.Name = "butt_addMotor";
            this.butt_addMotor.Size = new System.Drawing.Size(75, 23);
            this.butt_addMotor.TabIndex = 9;
            this.butt_addMotor.Text = "Add motor";
            this.butt_addMotor.UseVisualStyleBackColor = true;
            // 
            // EditChLink
            // 
            this.ClientSize = new System.Drawing.Size(337, 353);
            this.Controls.Add(this.butt_addMotor);
            this.Controls.Add(this.butt_addMate);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lst_matesSelected);
            this.Controls.Add(this.cb_motorControl);
            this.Controls.Add(this.cb_motorType);
            this.Controls.Add(this.lab_matesSelected);
            this.Controls.Add(this.lab_motorControl);
            this.Controls.Add(this.lab_motorType);
            this.Name = "EditChLink";
            this.Text = "Chrono link properties";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.Label lab_motorType;
        private System.Windows.Forms.Label lab_motorControl;
        private System.Windows.Forms.Label lab_matesSelected;
        private System.Windows.Forms.ComboBox cb_motorType;
        private System.Windows.Forms.ComboBox cb_motorControl;
        private System.Windows.Forms.ListView lst_matesSelected;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button butt_addMate;
        private System.Windows.Forms.Button butt_addMotor;
    }
}