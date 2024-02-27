namespace ChronoEngineAddin
{
    partial class EditChMotor
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
            this.cb_motorType = new System.Windows.Forms.ComboBox();
            this.cb_motionLaw = new System.Windows.Forms.ComboBox();
            this.lab_motionlaw = new System.Windows.Forms.Label();
            this.butt_addBodyMaster = new System.Windows.Forms.Button();
            this.butt_createMotor = new System.Windows.Forms.Button();
            this.butt_addMarker = new System.Windows.Forms.Button();
            this.gp_motorConfig = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_motlawInputs = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chb_motorConstraint = new System.Windows.Forms.CheckBox();
            this.lab_motorConstraints = new System.Windows.Forms.Label();
            this.txt_motorName = new System.Windows.Forms.TextBox();
            this.lab_motorName = new System.Windows.Forms.Label();
            this.gb_entitySelection = new System.Windows.Forms.GroupBox();
            this.cbMasterGround = new System.Windows.Forms.CheckBox();
            this.butt_bodySlaveSelected = new System.Windows.Forms.Button();
            this.txt_bodyMasterSelected = new System.Windows.Forms.TextBox();
            this.txt_bodySlaveSelected = new System.Windows.Forms.TextBox();
            this.txt_markerSelected = new System.Windows.Forms.TextBox();
            this.gp_motorConfig.SuspendLayout();
            this.gb_entitySelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // lab_motorType
            // 
            this.lab_motorType.AutoSize = true;
            this.lab_motorType.Location = new System.Drawing.Point(8, 60);
            this.lab_motorType.Name = "lab_motorType";
            this.lab_motorType.Size = new System.Drawing.Size(60, 13);
            this.lab_motorType.TabIndex = 0;
            this.lab_motorType.Text = "Motor type:";
            // 
            // cb_motorType
            // 
            this.cb_motorType.FormattingEnabled = true;
            this.cb_motorType.Items.AddRange(new object[] {
            "RotationAngle",
            "RotationSpeed",
            "RotationTorque",
            "LinearPosition",
            "LinearSpeed",
            "LinearForce"});
            this.cb_motorType.Location = new System.Drawing.Point(170, 57);
            this.cb_motorType.Name = "cb_motorType";
            this.cb_motorType.Size = new System.Drawing.Size(180, 21);
            this.cb_motorType.TabIndex = 3;
            this.cb_motorType.Text = "RotationAngle";
            // 
            // cb_motionLaw
            // 
            this.cb_motionLaw.FormattingEnabled = true;
            this.cb_motionLaw.Items.AddRange(new object[] {
            "Const",
            "ConstAcc",
            "Cycloidal",
            "DoubleS",
            "Poly345",
            "Setpoint",
            "Sine"});
            this.cb_motionLaw.Location = new System.Drawing.Point(170, 93);
            this.cb_motionLaw.Name = "cb_motionLaw";
            this.cb_motionLaw.Size = new System.Drawing.Size(180, 21);
            this.cb_motionLaw.TabIndex = 7;
            this.cb_motionLaw.Text = "Setpoint";
            // 
            // lab_motionlaw
            // 
            this.lab_motionlaw.AutoSize = true;
            this.lab_motionlaw.Location = new System.Drawing.Point(8, 96);
            this.lab_motionlaw.Name = "lab_motionlaw";
            this.lab_motionlaw.Size = new System.Drawing.Size(61, 13);
            this.lab_motionlaw.TabIndex = 6;
            this.lab_motionlaw.Text = "Motion law:";
            // 
            // butt_addBodyMaster
            // 
            this.butt_addBodyMaster.Location = new System.Drawing.Point(11, 95);
            this.butt_addBodyMaster.Name = "butt_addBodyMaster";
            this.butt_addBodyMaster.Size = new System.Drawing.Size(140, 30);
            this.butt_addBodyMaster.TabIndex = 8;
            this.butt_addBodyMaster.Text = "Select Master Body";
            this.butt_addBodyMaster.UseVisualStyleBackColor = true;
            this.butt_addBodyMaster.Click += new System.EventHandler(this.butt_addBodyMaster_Click);
            // 
            // butt_createMotor
            // 
            this.butt_createMotor.Location = new System.Drawing.Point(123, 404);
            this.butt_createMotor.Name = "butt_createMotor";
            this.butt_createMotor.Size = new System.Drawing.Size(140, 30);
            this.butt_createMotor.TabIndex = 9;
            this.butt_createMotor.Text = "Create motor";
            this.butt_createMotor.UseVisualStyleBackColor = true;
            this.butt_createMotor.Click += new System.EventHandler(this.butt_createMotor_Click);
            // 
            // butt_addMarker
            // 
            this.butt_addMarker.Location = new System.Drawing.Point(11, 25);
            this.butt_addMarker.Name = "butt_addMarker";
            this.butt_addMarker.Size = new System.Drawing.Size(140, 30);
            this.butt_addMarker.TabIndex = 12;
            this.butt_addMarker.Text = "Select Marker";
            this.butt_addMarker.UseVisualStyleBackColor = true;
            this.butt_addMarker.Click += new System.EventHandler(this.butt_addMarker_Click);
            // 
            // gp_motorConfig
            // 
            this.gp_motorConfig.Controls.Add(this.label2);
            this.gp_motorConfig.Controls.Add(this.txt_motlawInputs);
            this.gp_motorConfig.Controls.Add(this.label1);
            this.gp_motorConfig.Controls.Add(this.chb_motorConstraint);
            this.gp_motorConfig.Controls.Add(this.lab_motorConstraints);
            this.gp_motorConfig.Controls.Add(this.txt_motorName);
            this.gp_motorConfig.Controls.Add(this.lab_motorName);
            this.gp_motorConfig.Controls.Add(this.cb_motionLaw);
            this.gp_motorConfig.Controls.Add(this.lab_motorType);
            this.gp_motorConfig.Controls.Add(this.cb_motorType);
            this.gp_motorConfig.Controls.Add(this.lab_motionlaw);
            this.gp_motorConfig.Location = new System.Drawing.Point(15, 12);
            this.gp_motorConfig.Name = "gp_motorConfig";
            this.gp_motorConfig.Size = new System.Drawing.Size(356, 210);
            this.gp_motorConfig.TabIndex = 13;
            this.gp_motorConfig.TabStop = false;
            this.gp_motorConfig.Text = "Motor Configuration";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 162);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "(Optional, comma separated)";
            // 
            // txt_motlawInputs
            // 
            this.txt_motlawInputs.Location = new System.Drawing.Point(170, 178);
            this.txt_motlawInputs.Name = "txt_motlawInputs";
            this.txt_motlawInputs.Size = new System.Drawing.Size(180, 20);
            this.txt_motlawInputs.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 181);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Motion law inputs:";
            // 
            // chb_motorConstraint
            // 
            this.chb_motorConstraint.AutoSize = true;
            this.chb_motorConstraint.Location = new System.Drawing.Point(170, 128);
            this.chb_motorConstraint.Name = "chb_motorConstraint";
            this.chb_motorConstraint.Size = new System.Drawing.Size(64, 17);
            this.chb_motorConstraint.TabIndex = 18;
            this.chb_motorConstraint.Text = "enabled";
            this.chb_motorConstraint.UseVisualStyleBackColor = true;
            // 
            // lab_motorConstraints
            // 
            this.lab_motorConstraints.AutoSize = true;
            this.lab_motorConstraints.Location = new System.Drawing.Point(8, 129);
            this.lab_motorConstraints.Name = "lab_motorConstraints";
            this.lab_motorConstraints.Size = new System.Drawing.Size(120, 13);
            this.lab_motorConstraints.TabIndex = 17;
            this.lab_motorConstraints.Text = "Motor guide constraints:";
            // 
            // txt_motorName
            // 
            this.txt_motorName.Location = new System.Drawing.Point(170, 22);
            this.txt_motorName.Name = "txt_motorName";
            this.txt_motorName.Size = new System.Drawing.Size(180, 20);
            this.txt_motorName.TabIndex = 16;
            // 
            // lab_motorName
            // 
            this.lab_motorName.AutoSize = true;
            this.lab_motorName.Location = new System.Drawing.Point(8, 25);
            this.lab_motorName.Name = "lab_motorName";
            this.lab_motorName.Size = new System.Drawing.Size(66, 13);
            this.lab_motorName.TabIndex = 8;
            this.lab_motorName.Text = "Motor name:";
            // 
            // gb_entitySelection
            // 
            this.gb_entitySelection.Controls.Add(this.cbMasterGround);
            this.gb_entitySelection.Controls.Add(this.butt_bodySlaveSelected);
            this.gb_entitySelection.Controls.Add(this.txt_bodyMasterSelected);
            this.gb_entitySelection.Controls.Add(this.txt_bodySlaveSelected);
            this.gb_entitySelection.Controls.Add(this.txt_markerSelected);
            this.gb_entitySelection.Controls.Add(this.butt_addMarker);
            this.gb_entitySelection.Controls.Add(this.butt_addBodyMaster);
            this.gb_entitySelection.Location = new System.Drawing.Point(15, 234);
            this.gb_entitySelection.Name = "gb_entitySelection";
            this.gb_entitySelection.Size = new System.Drawing.Size(356, 164);
            this.gb_entitySelection.TabIndex = 14;
            this.gb_entitySelection.TabStop = false;
            this.gb_entitySelection.Text = "Entity Selection";
            // 
            // cbMasterGround
            // 
            this.cbMasterGround.AutoSize = true;
            this.cbMasterGround.Location = new System.Drawing.Point(172, 128);
            this.cbMasterGround.Name = "cbMasterGround";
            this.cbMasterGround.Size = new System.Drawing.Size(154, 17);
            this.cbMasterGround.TabIndex = 17;
            this.cbMasterGround.Text = "Set ground as Master Body";
            this.cbMasterGround.UseVisualStyleBackColor = true;
            this.cbMasterGround.CheckedChanged += new System.EventHandler(this.cbMasterGround_CheckedChanged);
            // 
            // butt_bodySlaveSelected
            // 
            this.butt_bodySlaveSelected.Location = new System.Drawing.Point(11, 58);
            this.butt_bodySlaveSelected.Name = "butt_bodySlaveSelected";
            this.butt_bodySlaveSelected.Size = new System.Drawing.Size(140, 30);
            this.butt_bodySlaveSelected.TabIndex = 15;
            this.butt_bodySlaveSelected.Text = "Select Slave Body";
            this.butt_bodySlaveSelected.UseVisualStyleBackColor = true;
            this.butt_bodySlaveSelected.Click += new System.EventHandler(this.butt_bodySlaveSelected_Click);
            // 
            // txt_bodyMasterSelected
            // 
            this.txt_bodyMasterSelected.Location = new System.Drawing.Point(171, 102);
            this.txt_bodyMasterSelected.Name = "txt_bodyMasterSelected";
            this.txt_bodyMasterSelected.ReadOnly = true;
            this.txt_bodyMasterSelected.Size = new System.Drawing.Size(179, 20);
            this.txt_bodyMasterSelected.TabIndex = 14;
            // 
            // txt_bodySlaveSelected
            // 
            this.txt_bodySlaveSelected.Location = new System.Drawing.Point(171, 64);
            this.txt_bodySlaveSelected.Name = "txt_bodySlaveSelected";
            this.txt_bodySlaveSelected.ReadOnly = true;
            this.txt_bodySlaveSelected.Size = new System.Drawing.Size(179, 20);
            this.txt_bodySlaveSelected.TabIndex = 13;
            // 
            // txt_markerSelected
            // 
            this.txt_markerSelected.Location = new System.Drawing.Point(171, 31);
            this.txt_markerSelected.Name = "txt_markerSelected";
            this.txt_markerSelected.ReadOnly = true;
            this.txt_markerSelected.Size = new System.Drawing.Size(179, 20);
            this.txt_markerSelected.TabIndex = 0;
            // 
            // EditChMotor
            // 
            this.ClientSize = new System.Drawing.Size(387, 446);
            this.Controls.Add(this.butt_createMotor);
            this.Controls.Add(this.gp_motorConfig);
            this.Controls.Add(this.gb_entitySelection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "EditChMotor";
            this.Text = "Chrono Motor Properties";
            this.TopMost = true;
            this.gp_motorConfig.ResumeLayout(false);
            this.gp_motorConfig.PerformLayout();
            this.gb_entitySelection.ResumeLayout(false);
            this.gb_entitySelection.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.Label lab_motorType;
        private System.Windows.Forms.ComboBox cb_motorType;
        private System.Windows.Forms.ComboBox cb_motionLaw;
        private System.Windows.Forms.Label lab_motionlaw;
        private System.Windows.Forms.Button butt_addBodyMaster;
        private System.Windows.Forms.Button butt_createMotor;
        private System.Windows.Forms.Button butt_addMarker;
        private System.Windows.Forms.GroupBox gp_motorConfig;
        private System.Windows.Forms.GroupBox gb_entitySelection;
        private System.Windows.Forms.TextBox txt_markerSelected;
        private System.Windows.Forms.TextBox txt_bodyMasterSelected;
        private System.Windows.Forms.TextBox txt_bodySlaveSelected;
        private System.Windows.Forms.Button butt_bodySlaveSelected;
        private System.Windows.Forms.TextBox txt_motorName;
        private System.Windows.Forms.Label lab_motorName;
        private System.Windows.Forms.Label lab_motorConstraints;
        private System.Windows.Forms.CheckBox chb_motorConstraint;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_motlawInputs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbMasterGround;
    }
}