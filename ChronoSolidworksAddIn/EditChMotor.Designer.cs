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
            this.chb_motorConstraint = new System.Windows.Forms.CheckBox();
            this.lab_motorConstraints = new System.Windows.Forms.Label();
            this.txt_motorName = new System.Windows.Forms.TextBox();
            this.lab_motorName = new System.Windows.Forms.Label();
            this.gb_entitySelection = new System.Windows.Forms.GroupBox();
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
            this.cb_motorType.Location = new System.Drawing.Point(125, 57);
            this.cb_motorType.Name = "cb_motorType";
            this.cb_motorType.Size = new System.Drawing.Size(179, 21);
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
            this.cb_motionLaw.Location = new System.Drawing.Point(125, 93);
            this.cb_motionLaw.Name = "cb_motionLaw";
            this.cb_motionLaw.Size = new System.Drawing.Size(179, 21);
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
            this.butt_addBodyMaster.Location = new System.Drawing.Point(6, 102);
            this.butt_addBodyMaster.Name = "butt_addBodyMaster";
            this.butt_addBodyMaster.Size = new System.Drawing.Size(104, 23);
            this.butt_addBodyMaster.TabIndex = 8;
            this.butt_addBodyMaster.Text = "Add master body";
            this.butt_addBodyMaster.UseVisualStyleBackColor = true;
            this.butt_addBodyMaster.Click += new System.EventHandler(this.butt_addBodyMaster_Click);
            // 
            // butt_createMotor
            // 
            this.butt_createMotor.Location = new System.Drawing.Point(122, 315);
            this.butt_createMotor.Name = "butt_createMotor";
            this.butt_createMotor.Size = new System.Drawing.Size(95, 26);
            this.butt_createMotor.TabIndex = 9;
            this.butt_createMotor.Text = "Create motor";
            this.butt_createMotor.UseVisualStyleBackColor = true;
            this.butt_createMotor.Click += new System.EventHandler(this.butt_createMotor_Click);
            // 
            // butt_addMarker
            // 
            this.butt_addMarker.Location = new System.Drawing.Point(6, 30);
            this.butt_addMarker.Name = "butt_addMarker";
            this.butt_addMarker.Size = new System.Drawing.Size(104, 23);
            this.butt_addMarker.TabIndex = 12;
            this.butt_addMarker.Text = "Add marker";
            this.butt_addMarker.UseVisualStyleBackColor = true;
            this.butt_addMarker.Click += new System.EventHandler(this.butt_addMarker_Click);
            // 
            // gp_motorConfig
            // 
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
            this.gp_motorConfig.Size = new System.Drawing.Size(310, 151);
            this.gp_motorConfig.TabIndex = 13;
            this.gp_motorConfig.TabStop = false;
            this.gp_motorConfig.Text = "Motor configuration";
            // 
            // chb_motorConstraint
            // 
            this.chb_motorConstraint.AutoSize = true;
            this.chb_motorConstraint.Location = new System.Drawing.Point(125, 128);
            this.chb_motorConstraint.Name = "chb_motorConstraint";
            this.chb_motorConstraint.Size = new System.Drawing.Size(64, 17);
            this.chb_motorConstraint.TabIndex = 18;
            this.chb_motorConstraint.Text = "enabled";
            this.chb_motorConstraint.UseVisualStyleBackColor = true;
            // 
            // lab_motorConstraints
            // 
            this.lab_motorConstraints.AutoSize = true;
            this.lab_motorConstraints.Location = new System.Drawing.Point(8, 128);
            this.lab_motorConstraints.Name = "lab_motorConstraints";
            this.lab_motorConstraints.Size = new System.Drawing.Size(91, 13);
            this.lab_motorConstraints.TabIndex = 17;
            this.lab_motorConstraints.Text = "Motor constraints:";
            // 
            // txt_motorName
            // 
            this.txt_motorName.Location = new System.Drawing.Point(125, 22);
            this.txt_motorName.Name = "txt_motorName";
            this.txt_motorName.Size = new System.Drawing.Size(179, 20);
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
            this.gb_entitySelection.Controls.Add(this.butt_bodySlaveSelected);
            this.gb_entitySelection.Controls.Add(this.txt_bodyMasterSelected);
            this.gb_entitySelection.Controls.Add(this.txt_bodySlaveSelected);
            this.gb_entitySelection.Controls.Add(this.txt_markerSelected);
            this.gb_entitySelection.Controls.Add(this.butt_addMarker);
            this.gb_entitySelection.Controls.Add(this.butt_addBodyMaster);
            this.gb_entitySelection.Location = new System.Drawing.Point(12, 169);
            this.gb_entitySelection.Name = "gb_entitySelection";
            this.gb_entitySelection.Size = new System.Drawing.Size(312, 140);
            this.gb_entitySelection.TabIndex = 14;
            this.gb_entitySelection.TabStop = false;
            this.gb_entitySelection.Text = "Entity selection";
            // 
            // butt_bodySlaveSelected
            // 
            this.butt_bodySlaveSelected.Location = new System.Drawing.Point(6, 67);
            this.butt_bodySlaveSelected.Name = "butt_bodySlaveSelected";
            this.butt_bodySlaveSelected.Size = new System.Drawing.Size(104, 23);
            this.butt_bodySlaveSelected.TabIndex = 15;
            this.butt_bodySlaveSelected.Text = "Add slave body";
            this.butt_bodySlaveSelected.UseVisualStyleBackColor = true;
            this.butt_bodySlaveSelected.Click += new System.EventHandler(this.butt_bodySlaveSelected_Click);
            // 
            // txt_bodyMasterSelected
            // 
            this.txt_bodyMasterSelected.Location = new System.Drawing.Point(128, 105);
            this.txt_bodyMasterSelected.Name = "txt_bodyMasterSelected";
            this.txt_bodyMasterSelected.ReadOnly = true;
            this.txt_bodyMasterSelected.Size = new System.Drawing.Size(179, 20);
            this.txt_bodyMasterSelected.TabIndex = 14;
            // 
            // txt_bodySlaveSelected
            // 
            this.txt_bodySlaveSelected.Location = new System.Drawing.Point(128, 69);
            this.txt_bodySlaveSelected.Name = "txt_bodySlaveSelected";
            this.txt_bodySlaveSelected.ReadOnly = true;
            this.txt_bodySlaveSelected.Size = new System.Drawing.Size(179, 20);
            this.txt_bodySlaveSelected.TabIndex = 13;
            // 
            // txt_markerSelected
            // 
            this.txt_markerSelected.Location = new System.Drawing.Point(128, 32);
            this.txt_markerSelected.Name = "txt_markerSelected";
            this.txt_markerSelected.ReadOnly = true;
            this.txt_markerSelected.Size = new System.Drawing.Size(179, 20);
            this.txt_markerSelected.TabIndex = 0;
            // 
            // EditChMotor
            // 
            this.ClientSize = new System.Drawing.Size(337, 353);
            this.Controls.Add(this.butt_createMotor);
            this.Controls.Add(this.gp_motorConfig);
            this.Controls.Add(this.gb_entitySelection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "EditChMotor";
            this.Text = "Chrono motor properties";
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
    }
}