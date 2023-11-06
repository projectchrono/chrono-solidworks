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
            this.cb_motorType = new System.Windows.Forms.ComboBox();
            this.cb_motorControl = new System.Windows.Forms.ComboBox();
            this.lst_bodiesSelected = new System.Windows.Forms.ListView();
            this.cb_motionLaw = new System.Windows.Forms.ComboBox();
            this.lab_motionlaw = new System.Windows.Forms.Label();
            this.butt_addBody = new System.Windows.Forms.Button();
            this.butt_createMotor = new System.Windows.Forms.Button();
            this.butt_addMarker = new System.Windows.Forms.Button();
            this.lst_markerSelected = new System.Windows.Forms.ListView();
            this.gp_motorConfig = new System.Windows.Forms.GroupBox();
            this.gb_entitySelection = new System.Windows.Forms.GroupBox();
            this.gp_motorConfig.SuspendLayout();
            this.gb_entitySelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // lab_motorType
            // 
            this.lab_motorType.AutoSize = true;
            this.lab_motorType.Location = new System.Drawing.Point(8, 30);
            this.lab_motorType.Name = "lab_motorType";
            this.lab_motorType.Size = new System.Drawing.Size(60, 13);
            this.lab_motorType.TabIndex = 0;
            this.lab_motorType.Text = "Motor type:";
            // 
            // lab_motorControl
            // 
            this.lab_motorControl.AutoSize = true;
            this.lab_motorControl.Location = new System.Drawing.Point(8, 70);
            this.lab_motorControl.Name = "lab_motorControl";
            this.lab_motorControl.Size = new System.Drawing.Size(72, 13);
            this.lab_motorControl.TabIndex = 1;
            this.lab_motorControl.Text = "Motor control:";
            // 
            // cb_motorType
            // 
            this.cb_motorType.FormattingEnabled = true;
            this.cb_motorType.Items.AddRange(new object[] {
            "Rotation",
            "Translation"});
            this.cb_motorType.Location = new System.Drawing.Point(113, 27);
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
            this.cb_motorControl.Location = new System.Drawing.Point(113, 67);
            this.cb_motorControl.Name = "cb_motorControl";
            this.cb_motorControl.Size = new System.Drawing.Size(191, 21);
            this.cb_motorControl.TabIndex = 4;
            this.cb_motorControl.Text = "Position";
            // 
            // lst_bodiesSelected
            // 
            this.lst_bodiesSelected.HideSelection = false;
            this.lst_bodiesSelected.Location = new System.Drawing.Point(116, 61);
            this.lst_bodiesSelected.Name = "lst_bodiesSelected";
            this.lst_bodiesSelected.Size = new System.Drawing.Size(191, 56);
            this.lst_bodiesSelected.TabIndex = 5;
            this.lst_bodiesSelected.UseCompatibleStateImageBehavior = false;
            this.lst_bodiesSelected.View = System.Windows.Forms.View.List;
            // 
            // cb_motionLaw
            // 
            this.cb_motionLaw.FormattingEnabled = true;
            this.cb_motionLaw.Items.AddRange(new object[] {
            "Constant",
            "Constant acceleration",
            "Cycloidal",
            "Double-S",
            "Poly345",
            "Setpoint",
            "Sine"});
            this.cb_motionLaw.Location = new System.Drawing.Point(113, 110);
            this.cb_motionLaw.Name = "cb_motionLaw";
            this.cb_motionLaw.Size = new System.Drawing.Size(191, 21);
            this.cb_motionLaw.TabIndex = 7;
            this.cb_motionLaw.Text = "Setpoint";
            // 
            // lab_motionlaw
            // 
            this.lab_motionlaw.AutoSize = true;
            this.lab_motionlaw.Location = new System.Drawing.Point(8, 113);
            this.lab_motionlaw.Name = "lab_motionlaw";
            this.lab_motionlaw.Size = new System.Drawing.Size(61, 13);
            this.lab_motionlaw.TabIndex = 6;
            this.lab_motionlaw.Text = "Motion law:";
            // 
            // butt_addBody
            // 
            this.butt_addBody.Location = new System.Drawing.Point(6, 82);
            this.butt_addBody.Name = "butt_addBody";
            this.butt_addBody.Size = new System.Drawing.Size(85, 23);
            this.butt_addBody.TabIndex = 8;
            this.butt_addBody.Text = "Add body";
            this.butt_addBody.UseVisualStyleBackColor = true;
            this.butt_addBody.Click += new System.EventHandler(this.butt_addBody_Click);
            // 
            // butt_createMotor
            // 
            this.butt_createMotor.Location = new System.Drawing.Point(128, 315);
            this.butt_createMotor.Name = "butt_createMotor";
            this.butt_createMotor.Size = new System.Drawing.Size(95, 26);
            this.butt_createMotor.TabIndex = 9;
            this.butt_createMotor.Text = "Create motor";
            this.butt_createMotor.UseVisualStyleBackColor = true;
            this.butt_createMotor.Click += new System.EventHandler(this.butt_createMotor_Click);
            // 
            // butt_addMarker
            // 
            this.butt_addMarker.Location = new System.Drawing.Point(6, 32);
            this.butt_addMarker.Name = "butt_addMarker";
            this.butt_addMarker.Size = new System.Drawing.Size(85, 23);
            this.butt_addMarker.TabIndex = 12;
            this.butt_addMarker.Text = "Add marker";
            this.butt_addMarker.UseVisualStyleBackColor = true;
            this.butt_addMarker.Click += new System.EventHandler(this.butt_addMarker_Click);
            // 
            // lst_markerSelected
            // 
            this.lst_markerSelected.HideSelection = false;
            this.lst_markerSelected.Location = new System.Drawing.Point(116, 29);
            this.lst_markerSelected.Name = "lst_markerSelected";
            this.lst_markerSelected.Size = new System.Drawing.Size(191, 26);
            this.lst_markerSelected.TabIndex = 11;
            this.lst_markerSelected.UseCompatibleStateImageBehavior = false;
            this.lst_markerSelected.View = System.Windows.Forms.View.List;
            // 
            // gp_motorConfig
            // 
            this.gp_motorConfig.Controls.Add(this.cb_motionLaw);
            this.gp_motorConfig.Controls.Add(this.lab_motorType);
            this.gp_motorConfig.Controls.Add(this.lab_motorControl);
            this.gp_motorConfig.Controls.Add(this.cb_motorType);
            this.gp_motorConfig.Controls.Add(this.cb_motorControl);
            this.gp_motorConfig.Controls.Add(this.lab_motionlaw);
            this.gp_motorConfig.Location = new System.Drawing.Point(15, 12);
            this.gp_motorConfig.Name = "gp_motorConfig";
            this.gp_motorConfig.Size = new System.Drawing.Size(310, 137);
            this.gp_motorConfig.TabIndex = 13;
            this.gp_motorConfig.TabStop = false;
            this.gp_motorConfig.Text = "Motor configuration";
            // 
            // gb_entitySelection
            // 
            this.gb_entitySelection.Controls.Add(this.butt_addMarker);
            this.gb_entitySelection.Controls.Add(this.lst_markerSelected);
            this.gb_entitySelection.Controls.Add(this.lst_bodiesSelected);
            this.gb_entitySelection.Controls.Add(this.butt_addBody);
            this.gb_entitySelection.Location = new System.Drawing.Point(12, 155);
            this.gb_entitySelection.Name = "gb_entitySelection";
            this.gb_entitySelection.Size = new System.Drawing.Size(312, 140);
            this.gb_entitySelection.TabIndex = 14;
            this.gb_entitySelection.TabStop = false;
            this.gb_entitySelection.Text = "Entity selection";
            // 
            // EditChLink
            // 
            this.ClientSize = new System.Drawing.Size(337, 353);
            this.Controls.Add(this.butt_createMotor);
            this.Controls.Add(this.gp_motorConfig);
            this.Controls.Add(this.gb_entitySelection);
            this.Name = "EditChLink";
            this.Text = "Chrono link properties";
            this.TopMost = true;
            this.gp_motorConfig.ResumeLayout(false);
            this.gp_motorConfig.PerformLayout();
            this.gb_entitySelection.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.Label lab_motorType;
        private System.Windows.Forms.Label lab_motorControl;
        private System.Windows.Forms.ComboBox cb_motorType;
        private System.Windows.Forms.ComboBox cb_motorControl;
        private System.Windows.Forms.ListView lst_bodiesSelected;
        private System.Windows.Forms.ComboBox cb_motionLaw;
        private System.Windows.Forms.Label lab_motionlaw;
        private System.Windows.Forms.Button butt_addBody;
        private System.Windows.Forms.Button butt_createMotor;
        private System.Windows.Forms.Button butt_addMarker;
        private System.Windows.Forms.ListView lst_markerSelected;
        private System.Windows.Forms.GroupBox gp_motorConfig;
        private System.Windows.Forms.GroupBox gb_entitySelection;
    }
}