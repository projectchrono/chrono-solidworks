namespace ChronoEngineAddin
{
    partial class EditChSDA
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
            this.butt_selectMasterBody = new System.Windows.Forms.Button();
            this.butt_createSDA = new System.Windows.Forms.Button();
            this.butt_selectSlaveMarker = new System.Windows.Forms.Button();
            this.gb_entitySelection = new System.Windows.Forms.GroupBox();
            this.txt_markerMasterSelected = new System.Windows.Forms.TextBox();
            this.but_selectMasterMarker = new System.Windows.Forms.Button();
            this.cbMasterGround = new System.Windows.Forms.CheckBox();
            this.butt_selectSlaveBody = new System.Windows.Forms.Button();
            this.txt_bodyMasterSelected = new System.Windows.Forms.TextBox();
            this.txt_bodySlaveSelected = new System.Windows.Forms.TextBox();
            this.txt_markerSlaveSelected = new System.Windows.Forms.TextBox();
            this.cb_sdaType = new System.Windows.Forms.ComboBox();
            this.lab_motorType = new System.Windows.Forms.Label();
            this.lab_motorName = new System.Windows.Forms.Label();
            this.txt_sdaName = new System.Windows.Forms.TextBox();
            this.gp_motorConfig = new System.Windows.Forms.GroupBox();
            this.txt_restLength = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_actuatorForce = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_dampingCoeff = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_springCoeff = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gb_entitySelection.SuspendLayout();
            this.gp_motorConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // butt_selectMasterBody
            // 
            this.butt_selectMasterBody.Location = new System.Drawing.Point(11, 127);
            this.butt_selectMasterBody.Name = "butt_selectMasterBody";
            this.butt_selectMasterBody.Size = new System.Drawing.Size(140, 30);
            this.butt_selectMasterBody.TabIndex = 8;
            this.butt_selectMasterBody.Text = "Select Master Body";
            this.butt_selectMasterBody.UseVisualStyleBackColor = true;
            this.butt_selectMasterBody.Click += new System.EventHandler(this.butt_selectMasterBody_Click);
            // 
            // butt_createSDA
            // 
            this.butt_createSDA.Location = new System.Drawing.Point(121, 452);
            this.butt_createSDA.Name = "butt_createSDA";
            this.butt_createSDA.Size = new System.Drawing.Size(140, 30);
            this.butt_createSDA.TabIndex = 9;
            this.butt_createSDA.Text = "Create SDA";
            this.butt_createSDA.UseVisualStyleBackColor = true;
            this.butt_createSDA.Click += new System.EventHandler(this.butt_createSDA_Click);
            // 
            // butt_selectSlaveMarker
            // 
            this.butt_selectSlaveMarker.Location = new System.Drawing.Point(11, 19);
            this.butt_selectSlaveMarker.Name = "butt_selectSlaveMarker";
            this.butt_selectSlaveMarker.Size = new System.Drawing.Size(140, 30);
            this.butt_selectSlaveMarker.TabIndex = 12;
            this.butt_selectSlaveMarker.Text = "Select Slave Marker";
            this.butt_selectSlaveMarker.UseVisualStyleBackColor = true;
            this.butt_selectSlaveMarker.Click += new System.EventHandler(this.butt_selectSlaveMarker_Click);
            // 
            // gb_entitySelection
            // 
            this.gb_entitySelection.Controls.Add(this.txt_markerMasterSelected);
            this.gb_entitySelection.Controls.Add(this.but_selectMasterMarker);
            this.gb_entitySelection.Controls.Add(this.cbMasterGround);
            this.gb_entitySelection.Controls.Add(this.butt_selectSlaveBody);
            this.gb_entitySelection.Controls.Add(this.txt_bodyMasterSelected);
            this.gb_entitySelection.Controls.Add(this.txt_bodySlaveSelected);
            this.gb_entitySelection.Controls.Add(this.txt_markerSlaveSelected);
            this.gb_entitySelection.Controls.Add(this.butt_selectSlaveMarker);
            this.gb_entitySelection.Controls.Add(this.butt_selectMasterBody);
            this.gb_entitySelection.Location = new System.Drawing.Point(15, 256);
            this.gb_entitySelection.Name = "gb_entitySelection";
            this.gb_entitySelection.Size = new System.Drawing.Size(356, 189);
            this.gb_entitySelection.TabIndex = 14;
            this.gb_entitySelection.TabStop = false;
            this.gb_entitySelection.Text = "Entity Selection";
            // 
            // txt_markerMasterSelected
            // 
            this.txt_markerMasterSelected.Location = new System.Drawing.Point(171, 61);
            this.txt_markerMasterSelected.Name = "txt_markerMasterSelected";
            this.txt_markerMasterSelected.ReadOnly = true;
            this.txt_markerMasterSelected.Size = new System.Drawing.Size(179, 20);
            this.txt_markerMasterSelected.TabIndex = 18;
            // 
            // but_selectMasterMarker
            // 
            this.but_selectMasterMarker.Location = new System.Drawing.Point(11, 55);
            this.but_selectMasterMarker.Name = "but_selectMasterMarker";
            this.but_selectMasterMarker.Size = new System.Drawing.Size(140, 30);
            this.but_selectMasterMarker.TabIndex = 19;
            this.but_selectMasterMarker.Text = "Select Master Marker";
            this.but_selectMasterMarker.UseVisualStyleBackColor = true;
            this.but_selectMasterMarker.Click += new System.EventHandler(this.but_selectMasterMarker_Click);
            // 
            // cbMasterGround
            // 
            this.cbMasterGround.AutoSize = true;
            this.cbMasterGround.Location = new System.Drawing.Point(172, 159);
            this.cbMasterGround.Name = "cbMasterGround";
            this.cbMasterGround.Size = new System.Drawing.Size(154, 17);
            this.cbMasterGround.TabIndex = 17;
            this.cbMasterGround.Text = "Set ground as Master Body";
            this.cbMasterGround.UseVisualStyleBackColor = true;
            this.cbMasterGround.CheckedChanged += new System.EventHandler(this.cbMasterGround_CheckedChanged);
            // 
            // butt_selectSlaveBody
            // 
            this.butt_selectSlaveBody.Location = new System.Drawing.Point(11, 91);
            this.butt_selectSlaveBody.Name = "butt_selectSlaveBody";
            this.butt_selectSlaveBody.Size = new System.Drawing.Size(140, 30);
            this.butt_selectSlaveBody.TabIndex = 15;
            this.butt_selectSlaveBody.Text = "Select Slave Body";
            this.butt_selectSlaveBody.UseVisualStyleBackColor = true;
            this.butt_selectSlaveBody.Click += new System.EventHandler(this.butt_selectSlaveBody_Click);
            // 
            // txt_bodyMasterSelected
            // 
            this.txt_bodyMasterSelected.Location = new System.Drawing.Point(172, 133);
            this.txt_bodyMasterSelected.Name = "txt_bodyMasterSelected";
            this.txt_bodyMasterSelected.ReadOnly = true;
            this.txt_bodyMasterSelected.Size = new System.Drawing.Size(179, 20);
            this.txt_bodyMasterSelected.TabIndex = 14;
            // 
            // txt_bodySlaveSelected
            // 
            this.txt_bodySlaveSelected.Location = new System.Drawing.Point(170, 97);
            this.txt_bodySlaveSelected.Name = "txt_bodySlaveSelected";
            this.txt_bodySlaveSelected.ReadOnly = true;
            this.txt_bodySlaveSelected.Size = new System.Drawing.Size(179, 20);
            this.txt_bodySlaveSelected.TabIndex = 13;
            // 
            // txt_markerSlaveSelected
            // 
            this.txt_markerSlaveSelected.Location = new System.Drawing.Point(171, 25);
            this.txt_markerSlaveSelected.Name = "txt_markerSlaveSelected";
            this.txt_markerSlaveSelected.ReadOnly = true;
            this.txt_markerSlaveSelected.Size = new System.Drawing.Size(179, 20);
            this.txt_markerSlaveSelected.TabIndex = 0;
            // 
            // cb_sdaType
            // 
            this.cb_sdaType.FormattingEnabled = true;
            this.cb_sdaType.Items.AddRange(new object[] {
            "Translational",
            "Rotational"});
            this.cb_sdaType.Location = new System.Drawing.Point(170, 57);
            this.cb_sdaType.Name = "cb_sdaType";
            this.cb_sdaType.Size = new System.Drawing.Size(180, 21);
            this.cb_sdaType.TabIndex = 3;
            this.cb_sdaType.Text = "Translational";
            // 
            // lab_motorType
            // 
            this.lab_motorType.AutoSize = true;
            this.lab_motorType.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lab_motorType.Location = new System.Drawing.Point(8, 60);
            this.lab_motorType.Name = "lab_motorType";
            this.lab_motorType.Size = new System.Drawing.Size(55, 13);
            this.lab_motorType.TabIndex = 0;
            this.lab_motorType.Text = "SDA type:";
            // 
            // lab_motorName
            // 
            this.lab_motorName.AutoSize = true;
            this.lab_motorName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lab_motorName.Location = new System.Drawing.Point(8, 25);
            this.lab_motorName.Name = "lab_motorName";
            this.lab_motorName.Size = new System.Drawing.Size(61, 13);
            this.lab_motorName.TabIndex = 8;
            this.lab_motorName.Text = "SDA name:";
            // 
            // txt_sdaName
            // 
            this.txt_sdaName.Location = new System.Drawing.Point(170, 22);
            this.txt_sdaName.Name = "txt_sdaName";
            this.txt_sdaName.Size = new System.Drawing.Size(180, 20);
            this.txt_sdaName.TabIndex = 16;
            // 
            // gp_motorConfig
            // 
            this.gp_motorConfig.Controls.Add(this.label2);
            this.gp_motorConfig.Controls.Add(this.txt_restLength);
            this.gp_motorConfig.Controls.Add(this.label1);
            this.gp_motorConfig.Controls.Add(this.txt_actuatorForce);
            this.gp_motorConfig.Controls.Add(this.label5);
            this.gp_motorConfig.Controls.Add(this.txt_dampingCoeff);
            this.gp_motorConfig.Controls.Add(this.label4);
            this.gp_motorConfig.Controls.Add(this.txt_springCoeff);
            this.gp_motorConfig.Controls.Add(this.label3);
            this.gp_motorConfig.Controls.Add(this.txt_sdaName);
            this.gp_motorConfig.Controls.Add(this.lab_motorName);
            this.gp_motorConfig.Controls.Add(this.lab_motorType);
            this.gp_motorConfig.Controls.Add(this.cb_sdaType);
            this.gp_motorConfig.Location = new System.Drawing.Point(15, 12);
            this.gp_motorConfig.Name = "gp_motorConfig";
            this.gp_motorConfig.Size = new System.Drawing.Size(356, 238);
            this.gp_motorConfig.TabIndex = 13;
            this.gp_motorConfig.TabStop = false;
            this.gp_motorConfig.Text = "SDA Configuration";
            // 
            // txt_restLength
            // 
            this.txt_restLength.Location = new System.Drawing.Point(169, 204);
            this.txt_restLength.Name = "txt_restLength";
            this.txt_restLength.Size = new System.Drawing.Size(180, 20);
            this.txt_restLength.TabIndex = 29;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(8, 207);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Rest length:";
            // 
            // txt_actuatorForce
            // 
            this.txt_actuatorForce.Location = new System.Drawing.Point(169, 146);
            this.txt_actuatorForce.Name = "txt_actuatorForce";
            this.txt_actuatorForce.Size = new System.Drawing.Size(180, 20);
            this.txt_actuatorForce.TabIndex = 27;
            this.txt_actuatorForce.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label5.Location = new System.Drawing.Point(7, 149);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "Actuator force:";
            // 
            // txt_dampingCoeff
            // 
            this.txt_dampingCoeff.Location = new System.Drawing.Point(169, 120);
            this.txt_dampingCoeff.Name = "txt_dampingCoeff";
            this.txt_dampingCoeff.Size = new System.Drawing.Size(180, 20);
            this.txt_dampingCoeff.TabIndex = 25;
            this.txt_dampingCoeff.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Location = new System.Drawing.Point(7, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Damping coefficient:";
            // 
            // txt_springCoeff
            // 
            this.txt_springCoeff.Location = new System.Drawing.Point(169, 94);
            this.txt_springCoeff.Name = "txt_springCoeff";
            this.txt_springCoeff.Size = new System.Drawing.Size(180, 20);
            this.txt_springCoeff.TabIndex = 23;
            this.txt_springCoeff.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(7, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Spring coefficient:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(8, 185);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(300, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Optional: if empty, automatically calculated from initial positions";
            // 
            // EditChSDA
            // 
            this.ClientSize = new System.Drawing.Size(387, 494);
            this.Controls.Add(this.butt_createSDA);
            this.Controls.Add(this.gp_motorConfig);
            this.Controls.Add(this.gb_entitySelection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "EditChSDA";
            this.Text = "Chrono Spring-Damper-Actuator";
            this.TopMost = true;
            this.gb_entitySelection.ResumeLayout(false);
            this.gb_entitySelection.PerformLayout();
            this.gp_motorConfig.ResumeLayout(false);
            this.gp_motorConfig.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion
        private System.Windows.Forms.Button butt_selectMasterBody;
        private System.Windows.Forms.Button butt_createSDA;
        private System.Windows.Forms.Button butt_selectSlaveMarker;
        private System.Windows.Forms.GroupBox gb_entitySelection;
        private System.Windows.Forms.TextBox txt_markerSlaveSelected;
        private System.Windows.Forms.TextBox txt_bodyMasterSelected;
        private System.Windows.Forms.TextBox txt_bodySlaveSelected;
        private System.Windows.Forms.Button butt_selectSlaveBody;
        private System.Windows.Forms.CheckBox cbMasterGround;
        private System.Windows.Forms.TextBox txt_markerMasterSelected;
        private System.Windows.Forms.Button but_selectMasterMarker;
        private System.Windows.Forms.ComboBox cb_sdaType;
        private System.Windows.Forms.Label lab_motorType;
        private System.Windows.Forms.Label lab_motorName;
        private System.Windows.Forms.TextBox txt_sdaName;
        private System.Windows.Forms.GroupBox gp_motorConfig;
        private System.Windows.Forms.TextBox txt_dampingCoeff;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_springCoeff;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_actuatorForce;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_restLength;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}