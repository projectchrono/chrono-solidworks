namespace ChronoEngine_SwAddin
{
    partial class SWTaskpaneHost
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SWTaskpaneHost));
            this.button_ExportToPython = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_surfaces = new System.Windows.Forms.CheckBox();
            this.checkBox_collshapes = new System.Windows.Forms.CheckBox();
            this.checkBox_constraints = new System.Windows.Forms.CheckBox();
            this.toolTipSavePython = new System.Windows.Forms.ToolTip(this.components);
            this.button_setcollshape = new System.Windows.Forms.Button();
            this.checkBox_separateobj = new System.Windows.Forms.CheckBox();
            this.checkBox_saveUV = new System.Windows.Forms.CheckBox();
            this.button_runtest = new System.Windows.Forms.Button();
            this.checkBox_savetest = new System.Windows.Forms.CheckBox();
            this.numeric_dt = new System.Windows.Forms.NumericUpDown();
            this.numeric_length = new System.Windows.Forms.NumericUpDown();
            this.numeric_scale_L = new System.Windows.Forms.NumericUpDown();
            this.numeric_scale_M = new System.Windows.Forms.NumericUpDown();
            this.checkBox_scale = new System.Windows.Forms.CheckBox();
            this.numeric_scale_T = new System.Windows.Forms.NumericUpDown();
            this.button_convexdecomp = new System.Windows.Forms.Button();
            this.button_chrono_property = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.numeric_envelope = new System.Windows.Forms.NumericUpDown();
            this.numeric_margin = new System.Windows.Forms.NumericUpDown();
            this.numeric_contactbreaking = new System.Windows.Forms.NumericUpDown();
            this.numeric_sphereswept = new System.Windows.Forms.NumericUpDown();
            this.button_ExportToJson = new System.Windows.Forms.Button();
            this.button_ExportToCpp = new System.Windows.Forms.Button();
            this.butt_chronoMotors = new System.Windows.Forms.Button();
            this.button_settrimeshcoll = new System.Windows.Forms.Button();
            this.but_runSimulation = new System.Windows.Forms.Button();
            this.nud_numIterations = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_dt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_length)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_scale_L)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_scale_M)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_scale_T)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_envelope)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_margin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_contactbreaking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_sphereswept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_numIterations)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_ExportToPython
            // 
            this.button_ExportToPython.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.button_ExportToPython.Location = new System.Drawing.Point(9, 19);
            this.button_ExportToPython.Name = "button_ExportToPython";
            this.button_ExportToPython.Size = new System.Drawing.Size(224, 32);
            this.button_ExportToPython.TabIndex = 0;
            this.button_ExportToPython.Text = "Export to Python";
            this.button_ExportToPython.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTipSavePython.SetToolTip(this.button_ExportToPython, resources.GetString("button_ExportToPython.ToolTip"));
            this.button_ExportToPython.UseVisualStyleBackColor = true;
            this.button_ExportToPython.Click += new System.EventHandler(this.ExportClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Chrono::Engine";
            // 
            // checkBox_surfaces
            // 
            this.checkBox_surfaces.AutoSize = true;
            this.checkBox_surfaces.Checked = true;
            this.checkBox_surfaces.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_surfaces.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.checkBox_surfaces.Location = new System.Drawing.Point(11, 24);
            this.checkBox_surfaces.Name = "checkBox_surfaces";
            this.checkBox_surfaces.Size = new System.Drawing.Size(102, 17);
            this.checkBox_surfaces.TabIndex = 2;
            this.checkBox_surfaces.Text = "Export surfaces ";
            this.toolTipSavePython.SetToolTip(this.checkBox_surfaces, resources.GetString("checkBox_surfaces.ToolTip"));
            this.checkBox_surfaces.UseVisualStyleBackColor = true;
            this.checkBox_surfaces.CheckedChanged += new System.EventHandler(this.checkBox_surfaces_CheckedChanged);
            // 
            // checkBox_collshapes
            // 
            this.checkBox_collshapes.AutoSize = true;
            this.checkBox_collshapes.Checked = true;
            this.checkBox_collshapes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_collshapes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.checkBox_collshapes.Location = new System.Drawing.Point(11, 91);
            this.checkBox_collshapes.Name = "checkBox_collshapes";
            this.checkBox_collshapes.Size = new System.Drawing.Size(131, 17);
            this.checkBox_collshapes.TabIndex = 3;
            this.checkBox_collshapes.Text = "Export collison shapes";
            this.toolTipSavePython.SetToolTip(this.checkBox_collshapes, resources.GetString("checkBox_collshapes.ToolTip"));
            this.checkBox_collshapes.UseVisualStyleBackColor = true;
            // 
            // checkBox_constraints
            // 
            this.checkBox_constraints.AutoSize = true;
            this.checkBox_constraints.Checked = true;
            this.checkBox_constraints.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_constraints.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.checkBox_constraints.Location = new System.Drawing.Point(11, 114);
            this.checkBox_constraints.Name = "checkBox_constraints";
            this.checkBox_constraints.Size = new System.Drawing.Size(110, 17);
            this.checkBox_constraints.TabIndex = 4;
            this.checkBox_constraints.Text = "Export constraints";
            this.toolTipSavePython.SetToolTip(this.checkBox_constraints, resources.GetString("checkBox_constraints.ToolTip"));
            this.checkBox_constraints.UseVisualStyleBackColor = true;
            // 
            // toolTipSavePython
            // 
            this.toolTipSavePython.ToolTipTitle = "Press this button to create a .py script with C::E assets";
            // 
            // button_setcollshape
            // 
            this.button_setcollshape.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.button_setcollshape.Location = new System.Drawing.Point(9, 22);
            this.button_setcollshape.Name = "button_setcollshape";
            this.button_setcollshape.Size = new System.Drawing.Size(110, 30);
            this.button_setcollshape.TabIndex = 5;
            this.button_setcollshape.Text = "Primitive Shape";
            this.toolTipSavePython.SetToolTip(this.button_setcollshape, resources.GetString("button_setcollshape.ToolTip"));
            this.button_setcollshape.UseVisualStyleBackColor = true;
            this.button_setcollshape.Click += new System.EventHandler(this.button_setcollshape_Click);
            // 
            // checkBox_separateobj
            // 
            this.checkBox_separateobj.AutoSize = true;
            this.checkBox_separateobj.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.checkBox_separateobj.Location = new System.Drawing.Point(25, 47);
            this.checkBox_separateobj.Name = "checkBox_separateobj";
            this.checkBox_separateobj.Size = new System.Drawing.Size(172, 17);
            this.checkBox_separateobj.TabIndex = 6;
            this.checkBox_separateobj.Text = "Separate .obj per each subpart";
            this.toolTipSavePython.SetToolTip(this.checkBox_separateobj, resources.GetString("checkBox_separateobj.ToolTip"));
            this.checkBox_separateobj.UseVisualStyleBackColor = true;
            // 
            // checkBox_saveUV
            // 
            this.checkBox_saveUV.AutoSize = true;
            this.checkBox_saveUV.Checked = true;
            this.checkBox_saveUV.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_saveUV.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.checkBox_saveUV.Location = new System.Drawing.Point(25, 68);
            this.checkBox_saveUV.Name = "checkBox_saveUV";
            this.checkBox_saveUV.Size = new System.Drawing.Size(146, 17);
            this.checkBox_saveUV.TabIndex = 7;
            this.checkBox_saveUV.Text = "Save UV map information";
            this.toolTipSavePython.SetToolTip(this.checkBox_saveUV, resources.GetString("checkBox_saveUV.ToolTip"));
            this.checkBox_saveUV.UseVisualStyleBackColor = true;
            this.checkBox_saveUV.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button_runtest
            // 
            this.button_runtest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_runtest.Location = new System.Drawing.Point(139, 133);
            this.button_runtest.Name = "button_runtest";
            this.button_runtest.Size = new System.Drawing.Size(100, 25);
            this.button_runtest.TabIndex = 8;
            this.button_runtest.Text = "Run PY test";
            this.toolTipSavePython.SetToolTip(this.button_runtest, "If you saved a test Python program, with the check button at \r\nthe left, then you" +
        " can also run the program directly by pressing\r\nthis button. \r\nNOTE: you must ha" +
        "ve Python installed.");
            this.button_runtest.UseVisualStyleBackColor = true;
            this.button_runtest.Click += new System.EventHandler(this.button_runtest_Click);
            // 
            // checkBox_savetest
            // 
            this.checkBox_savetest.AutoSize = true;
            this.checkBox_savetest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_savetest.Location = new System.Drawing.Point(6, 141);
            this.checkBox_savetest.Name = "checkBox_savetest";
            this.checkBox_savetest.Size = new System.Drawing.Size(121, 17);
            this.checkBox_savetest.TabIndex = 9;
            this.checkBox_savetest.Text = "Save PY for Testing";
            this.toolTipSavePython.SetToolTip(this.checkBox_savetest, resources.GetString("checkBox_savetest.ToolTip"));
            this.checkBox_savetest.UseVisualStyleBackColor = true;
            // 
            // numeric_dt
            // 
            this.numeric_dt.DecimalPlaces = 5;
            this.numeric_dt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numeric_dt.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numeric_dt.Location = new System.Drawing.Point(171, 21);
            this.numeric_dt.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_dt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            589824});
            this.numeric_dt.Name = "numeric_dt";
            this.numeric_dt.Size = new System.Drawing.Size(67, 20);
            this.numeric_dt.TabIndex = 11;
            this.toolTipSavePython.SetToolTip(this.numeric_dt, "Timestep [s]");
            this.numeric_dt.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            // 
            // numeric_length
            // 
            this.numeric_length.DecimalPlaces = 2;
            this.numeric_length.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numeric_length.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numeric_length.Location = new System.Drawing.Point(171, 44);
            this.numeric_length.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numeric_length.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numeric_length.Name = "numeric_length";
            this.numeric_length.Size = new System.Drawing.Size(67, 20);
            this.numeric_length.TabIndex = 13;
            this.toolTipSavePython.SetToolTip(this.numeric_length, "Duration of the test simulation [s]");
            this.numeric_length.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numeric_scale_L
            // 
            this.numeric_scale_L.DecimalPlaces = 3;
            this.numeric_scale_L.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.numeric_scale_L.Location = new System.Drawing.Point(169, 136);
            this.numeric_scale_L.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numeric_scale_L.Name = "numeric_scale_L";
            this.numeric_scale_L.Size = new System.Drawing.Size(67, 20);
            this.numeric_scale_L.TabIndex = 14;
            this.toolTipSavePython.SetToolTip(this.numeric_scale_L, "Scale factor for all lengths. For example, if you set 0.1, an object that is 5m l" +
        "ong will be exported as 0.5m long.");
            this.numeric_scale_L.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numeric_scale_M
            // 
            this.numeric_scale_M.DecimalPlaces = 3;
            this.numeric_scale_M.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.numeric_scale_M.Location = new System.Drawing.Point(169, 157);
            this.numeric_scale_M.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numeric_scale_M.Name = "numeric_scale_M";
            this.numeric_scale_M.Size = new System.Drawing.Size(67, 20);
            this.numeric_scale_M.TabIndex = 17;
            this.toolTipSavePython.SetToolTip(this.numeric_scale_M, "Scale factor for all masses. For example, if you set 0.1, an object that is 5kg h" +
        "eavy will be exported as 0.5kg.");
            this.numeric_scale_M.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // checkBox_scale
            // 
            this.checkBox_scale.AutoSize = true;
            this.checkBox_scale.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.checkBox_scale.Location = new System.Drawing.Point(9, 157);
            this.checkBox_scale.Name = "checkBox_scale";
            this.checkBox_scale.Size = new System.Drawing.Size(85, 17);
            this.checkBox_scale.TabIndex = 18;
            this.checkBox_scale.Text = "Scale export";
            this.toolTipSavePython.SetToolTip(this.checkBox_scale, "If checked, you can set a reduction or enlargement scale for lenghts (L), masses " +
        "(M) and times (T) when exporting output.");
            this.checkBox_scale.UseVisualStyleBackColor = true;
            this.checkBox_scale.CheckedChanged += new System.EventHandler(this.checkBox_scale_CheckedChanged);
            // 
            // numeric_scale_T
            // 
            this.numeric_scale_T.DecimalPlaces = 3;
            this.numeric_scale_T.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.numeric_scale_T.Location = new System.Drawing.Point(169, 178);
            this.numeric_scale_T.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numeric_scale_T.Name = "numeric_scale_T";
            this.numeric_scale_T.Size = new System.Drawing.Size(67, 20);
            this.numeric_scale_T.TabIndex = 20;
            this.toolTipSavePython.SetToolTip(this.numeric_scale_T, "Scale factor for time, in measuring units of exported items (some exported quanti" +
        "ties depends on time scale, ex speed = [L]/[T] )");
            this.numeric_scale_T.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // button_convexdecomp
            // 
            this.button_convexdecomp.Enabled = false;
            this.button_convexdecomp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.button_convexdecomp.Location = new System.Drawing.Point(9, 55);
            this.button_convexdecomp.Name = "button_convexdecomp";
            this.button_convexdecomp.Size = new System.Drawing.Size(224, 30);
            this.button_convexdecomp.TabIndex = 21;
            this.button_convexdecomp.Text = "Convex Decomposition";
            this.toolTipSavePython.SetToolTip(this.button_convexdecomp, "To use this function: \r\nselect a solid body and press the button");
            this.button_convexdecomp.UseVisualStyleBackColor = true;
            this.button_convexdecomp.Click += new System.EventHandler(this.button_convexdecomp_Click);
            // 
            // button_chrono_property
            // 
            this.button_chrono_property.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.button_chrono_property.Location = new System.Drawing.Point(8, 19);
            this.button_chrono_property.Name = "button_chrono_property";
            this.button_chrono_property.Size = new System.Drawing.Size(110, 30);
            this.button_chrono_property.TabIndex = 22;
            this.button_chrono_property.Text = "Body Properties";
            this.toolTipSavePython.SetToolTip(this.button_chrono_property, "Open the Chrono properties associated to the selected body");
            this.button_chrono_property.UseVisualStyleBackColor = true;
            this.button_chrono_property.Click += new System.EventHandler(this.button_chrono_property_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Irrlicht",
            "PovRay"});
            this.comboBox1.Location = new System.Drawing.Point(171, 89);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(67, 21);
            this.comboBox1.TabIndex = 23;
            this.comboBox1.Text = "Irrlicht";
            this.toolTipSavePython.SetToolTip(this.comboBox1, "Choose the type of  visualization system used by the test python program");
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // numeric_envelope
            // 
            this.numeric_envelope.DecimalPlaces = 5;
            this.numeric_envelope.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.numeric_envelope.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numeric_envelope.Location = new System.Drawing.Point(171, 25);
            this.numeric_envelope.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numeric_envelope.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            589824});
            this.numeric_envelope.Name = "numeric_envelope";
            this.numeric_envelope.Size = new System.Drawing.Size(67, 20);
            this.numeric_envelope.TabIndex = 27;
            this.toolTipSavePython.SetToolTip(this.numeric_envelope, "Outward tolerance for detecting potential contacts. Too small, might miss contact" +
        "s. Too large, cause false positives and high CPU times.");
            this.numeric_envelope.Value = new decimal(new int[] {
            3,
            0,
            0,
            196608});
            // 
            // numeric_margin
            // 
            this.numeric_margin.DecimalPlaces = 5;
            this.numeric_margin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.numeric_margin.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numeric_margin.Location = new System.Drawing.Point(171, 47);
            this.numeric_margin.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numeric_margin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            589824});
            this.numeric_margin.Name = "numeric_margin";
            this.numeric_margin.Size = new System.Drawing.Size(67, 20);
            this.numeric_margin.TabIndex = 29;
            this.toolTipSavePython.SetToolTip(this.numeric_margin, "Inward tolerance for interpenetrating contacts. Too small might cause objects sin" +
        "king. Too large, cause false positives and high CPU times.");
            this.numeric_margin.Value = new decimal(new int[] {
            3,
            0,
            0,
            196608});
            // 
            // numeric_contactbreaking
            // 
            this.numeric_contactbreaking.DecimalPlaces = 5;
            this.numeric_contactbreaking.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.numeric_contactbreaking.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numeric_contactbreaking.Location = new System.Drawing.Point(171, 69);
            this.numeric_contactbreaking.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numeric_contactbreaking.Name = "numeric_contactbreaking";
            this.numeric_contactbreaking.Size = new System.Drawing.Size(67, 20);
            this.numeric_contactbreaking.TabIndex = 31;
            this.toolTipSavePython.SetToolTip(this.numeric_contactbreaking, "Nonzero value cause contact persistence between frames when possible, that can he" +
        "lp solver convergence. Too large, anyway, can lead to wrong contacts.");
            this.numeric_contactbreaking.Value = new decimal(new int[] {
            2,
            0,
            0,
            196608});
            // 
            // numeric_sphereswept
            // 
            this.numeric_sphereswept.DecimalPlaces = 5;
            this.numeric_sphereswept.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.numeric_sphereswept.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numeric_sphereswept.Location = new System.Drawing.Point(171, 92);
            this.numeric_sphereswept.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numeric_sphereswept.Name = "numeric_sphereswept";
            this.numeric_sphereswept.Size = new System.Drawing.Size(67, 20);
            this.numeric_sphereswept.TabIndex = 33;
            this.toolTipSavePython.SetToolTip(this.numeric_sphereswept, resources.GetString("numeric_sphereswept.ToolTip"));
            this.numeric_sphereswept.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // button_ExportToJson
            // 
            this.button_ExportToJson.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.button_ExportToJson.Location = new System.Drawing.Point(9, 89);
            this.button_ExportToJson.Name = "button_ExportToJson";
            this.button_ExportToJson.Size = new System.Drawing.Size(224, 32);
            this.button_ExportToJson.TabIndex = 35;
            this.button_ExportToJson.Text = "Export to JSON";
            this.button_ExportToJson.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTipSavePython.SetToolTip(this.button_ExportToJson, resources.GetString("button_ExportToJson.ToolTip"));
            this.button_ExportToJson.UseVisualStyleBackColor = true;
            this.button_ExportToJson.Click += new System.EventHandler(this.ExportClick);
            // 
            // button_ExportToCpp
            // 
            this.button_ExportToCpp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.button_ExportToCpp.Location = new System.Drawing.Point(9, 54);
            this.button_ExportToCpp.Name = "button_ExportToCpp";
            this.button_ExportToCpp.Size = new System.Drawing.Size(224, 32);
            this.button_ExportToCpp.TabIndex = 36;
            this.button_ExportToCpp.Text = "Export to C++";
            this.button_ExportToCpp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTipSavePython.SetToolTip(this.button_ExportToCpp, resources.GetString("button_ExportToCpp.ToolTip"));
            this.button_ExportToCpp.UseVisualStyleBackColor = true;
            this.button_ExportToCpp.Click += new System.EventHandler(this.ExportClick);
            // 
            // butt_chronoMotors
            // 
            this.butt_chronoMotors.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.butt_chronoMotors.Location = new System.Drawing.Point(123, 19);
            this.butt_chronoMotors.Name = "butt_chronoMotors";
            this.butt_chronoMotors.Size = new System.Drawing.Size(110, 30);
            this.butt_chronoMotors.TabIndex = 37;
            this.butt_chronoMotors.Text = "Motors";
            this.toolTipSavePython.SetToolTip(this.butt_chronoMotors, "Open the properties for Chrono motors");
            this.butt_chronoMotors.UseVisualStyleBackColor = true;
            this.butt_chronoMotors.Click += new System.EventHandler(this.butt_chronoMotors_Click);
            // 
            // button_settrimeshcoll
            // 
            this.button_settrimeshcoll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.button_settrimeshcoll.Location = new System.Drawing.Point(123, 22);
            this.button_settrimeshcoll.Name = "button_settrimeshcoll";
            this.button_settrimeshcoll.Size = new System.Drawing.Size(110, 30);
            this.button_settrimeshcoll.TabIndex = 34;
            this.button_settrimeshcoll.Text = "Mesh";
            this.toolTipSavePython.SetToolTip(this.button_settrimeshcoll, resources.GetString("button_settrimeshcoll.ToolTip"));
            this.button_settrimeshcoll.UseVisualStyleBackColor = true;
            this.button_settrimeshcoll.Click += new System.EventHandler(this.button_settrimeshcoll_Click);
            // 
            // but_runSimulation
            // 
            this.but_runSimulation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.but_runSimulation.Location = new System.Drawing.Point(8, 29);
            this.but_runSimulation.Name = "but_runSimulation";
            this.but_runSimulation.Size = new System.Drawing.Size(90, 30);
            this.but_runSimulation.TabIndex = 25;
            this.but_runSimulation.Text = "Run Simulation";
            this.toolTipSavePython.SetToolTip(this.but_runSimulation, "Run a Chrono simulation of this assembly");
            this.but_runSimulation.UseVisualStyleBackColor = true;
            this.but_runSimulation.Click += new System.EventHandler(this.but_runSimulation_Click);
            // 
            // nud_numIterations
            // 
            this.nud_numIterations.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_numIterations.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_numIterations.Location = new System.Drawing.Point(171, 66);
            this.nud_numIterations.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nud_numIterations.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_numIterations.Name = "nud_numIterations";
            this.nud_numIterations.Size = new System.Drawing.Size(67, 20);
            this.nud_numIterations.TabIndex = 27;
            this.toolTipSavePython.SetToolTip(this.nud_numIterations, "Duration of the test simulation [s]");
            this.nud_numIterations.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(115, 23);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Timestep";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(125, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Length";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label4.Location = new System.Drawing.Point(122, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Scale L";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label5.Location = new System.Drawing.Point(120, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Scale M";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label6.Location = new System.Drawing.Point(121, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Scale T";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(100, 92);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Visualization";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label9.Location = new System.Drawing.Point(74, 27);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Collision Envelope";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label10.Location = new System.Drawing.Point(87, 49);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 13);
            this.label10.TabIndex = 28;
            this.label10.Text = "Collision Margin";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label11.Location = new System.Drawing.Point(27, 71);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(139, 13);
            this.label11.TabIndex = 30;
            this.label11.Text = "Contact Breaking Threshold";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label12.Location = new System.Drawing.Point(24, 94);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(142, 13);
            this.label12.TabIndex = 32;
            this.label12.Text = "Mesh Sphere Sweep Radius";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nud_numIterations);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.but_runSimulation);
            this.groupBox1.Controls.Add(this.button_runtest);
            this.groupBox1.Controls.Add(this.checkBox_savetest);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numeric_length);
            this.groupBox1.Controls.Add(this.numeric_dt);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 682);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(245, 168);
            this.groupBox1.TabIndex = 45;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Simulation";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(92, 68);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Num Iterations";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.numeric_contactbreaking);
            this.groupBox2.Controls.Add(this.numeric_envelope);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.numeric_margin);
            this.groupBox2.Controls.Add(this.numeric_sphereswept);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(10, 555);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(245, 121);
            this.groupBox2.TabIndex = 46;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Collision Settings";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.numeric_scale_T);
            this.groupBox3.Controls.Add(this.checkBox_surfaces);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.checkBox_collshapes);
            this.groupBox3.Controls.Add(this.numeric_scale_M);
            this.groupBox3.Controls.Add(this.checkBox_constraints);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.checkBox_separateobj);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.checkBox_saveUV);
            this.groupBox3.Controls.Add(this.numeric_scale_L);
            this.groupBox3.Controls.Add(this.checkBox_scale);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(10, 342);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(245, 207);
            this.groupBox3.TabIndex = 47;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Export Settings";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button_chrono_property);
            this.groupBox4.Controls.Add(this.butt_chronoMotors);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(10, 277);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(245, 59);
            this.groupBox4.TabIndex = 48;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Chrono Properties";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button_setcollshape);
            this.groupBox5.Controls.Add(this.button_convexdecomp);
            this.groupBox5.Controls.Add(this.button_settrimeshcoll);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(10, 175);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(245, 96);
            this.groupBox5.TabIndex = 49;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Flag Body as Collision Shape";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.button_ExportToCpp);
            this.groupBox6.Controls.Add(this.button_ExportToJson);
            this.groupBox6.Controls.Add(this.button_ExportToPython);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(10, 41);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(245, 128);
            this.groupBox6.TabIndex = 50;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Export";
            // 
            // SWTaskpaneHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Name = "SWTaskpaneHost";
            this.Size = new System.Drawing.Size(268, 868);
            this.toolTipSavePython.SetToolTip(this, resources.GetString("$this.ToolTip"));
            ((System.ComponentModel.ISupportInitialize)(this.numeric_dt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_length)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_scale_L)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_scale_M)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_scale_T)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_envelope)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_margin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_contactbreaking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_sphereswept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_numIterations)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_ExportToPython;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_surfaces;
        private System.Windows.Forms.CheckBox checkBox_collshapes;
        private System.Windows.Forms.CheckBox checkBox_constraints;
        private System.Windows.Forms.ToolTip toolTipSavePython;
        private System.Windows.Forms.Button button_setcollshape;
        private System.Windows.Forms.CheckBox checkBox_separateobj;
        private System.Windows.Forms.CheckBox checkBox_saveUV;
        private System.Windows.Forms.Button button_runtest;
        private System.Windows.Forms.CheckBox checkBox_savetest;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numeric_dt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numeric_length;
        private System.Windows.Forms.NumericUpDown numeric_scale_L;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numeric_scale_M;
        private System.Windows.Forms.CheckBox checkBox_scale;
        private System.Windows.Forms.NumericUpDown numeric_scale_T;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button_convexdecomp;
        private System.Windows.Forms.Button button_chrono_property;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numeric_envelope;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numeric_margin;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numeric_contactbreaking;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numeric_sphereswept;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button_settrimeshcoll;
        private System.Windows.Forms.Button button_ExportToJson;
        private System.Windows.Forms.Button button_ExportToCpp;
        private System.Windows.Forms.Button butt_chronoMotors;
        private System.Windows.Forms.Button but_runSimulation;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.NumericUpDown nud_numIterations;
        private System.Windows.Forms.Label label8;
    }
}
