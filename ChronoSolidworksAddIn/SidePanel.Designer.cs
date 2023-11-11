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
            this.button_settrimeshcoll = new System.Windows.Forms.Button();
            this.button_ExportToJson = new System.Windows.Forms.Button();
            this.button_ExportToCpp = new System.Windows.Forms.Button();
            this.butt_chronoMotors = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_dt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_length)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_scale_L)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_scale_M)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_scale_T)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_envelope)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_margin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_contactbreaking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_sphereswept)).BeginInit();
            this.SuspendLayout();
            // 
            // button_ExportToPython
            // 
            this.button_ExportToPython.Location = new System.Drawing.Point(17, 41);
            this.button_ExportToPython.Name = "button_ExportToPython";
            this.button_ExportToPython.Size = new System.Drawing.Size(200, 32);
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
            this.label1.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Chrono::Engine";
            // 
            // checkBox_surfaces
            // 
            this.checkBox_surfaces.AutoSize = true;
            this.checkBox_surfaces.Checked = true;
            this.checkBox_surfaces.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_surfaces.Location = new System.Drawing.Point(17, 157);
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
            this.checkBox_collshapes.Location = new System.Drawing.Point(17, 224);
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
            this.checkBox_constraints.Location = new System.Drawing.Point(17, 247);
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
            this.button_setcollshape.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_setcollshape.Location = new System.Drawing.Point(17, 463);
            this.button_setcollshape.Name = "button_setcollshape";
            this.button_setcollshape.Size = new System.Drawing.Size(68, 54);
            this.button_setcollshape.TabIndex = 5;
            this.button_setcollshape.Text = "Set body as primitive collision ";
            this.button_setcollshape.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTipSavePython.SetToolTip(this.button_setcollshape, resources.GetString("button_setcollshape.ToolTip"));
            this.button_setcollshape.UseVisualStyleBackColor = true;
            this.button_setcollshape.Click += new System.EventHandler(this.button_setcollshape_Click);
            // 
            // checkBox_separateobj
            // 
            this.checkBox_separateobj.AutoSize = true;
            this.checkBox_separateobj.Location = new System.Drawing.Point(33, 180);
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
            this.checkBox_saveUV.Location = new System.Drawing.Point(33, 201);
            this.checkBox_saveUV.Name = "checkBox_saveUV";
            this.checkBox_saveUV.Size = new System.Drawing.Size(149, 17);
            this.checkBox_saveUV.TabIndex = 7;
            this.checkBox_saveUV.Text = "Save UV map  information";
            this.toolTipSavePython.SetToolTip(this.checkBox_saveUV, resources.GetString("checkBox_saveUV.ToolTip"));
            this.checkBox_saveUV.UseVisualStyleBackColor = true;
            this.checkBox_saveUV.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button_runtest
            // 
            this.button_runtest.Location = new System.Drawing.Point(148, 364);
            this.button_runtest.Name = "button_runtest";
            this.button_runtest.Size = new System.Drawing.Size(69, 22);
            this.button_runtest.TabIndex = 8;
            this.button_runtest.Text = "Run test";
            this.toolTipSavePython.SetToolTip(this.button_runtest, "If you saved a test Python program, with the check button at \r\nthe left, then you" +
        " can also run the program directly by pressing\r\nthis button. \r\nNOTE: you must ha" +
        "ve Python installed.");
            this.button_runtest.UseVisualStyleBackColor = true;
            this.button_runtest.Click += new System.EventHandler(this.button_runtest_Click);
            // 
            // checkBox_savetest
            // 
            this.checkBox_savetest.AutoSize = true;
            this.checkBox_savetest.Location = new System.Drawing.Point(17, 368);
            this.checkBox_savetest.Name = "checkBox_savetest";
            this.checkBox_savetest.Size = new System.Drawing.Size(88, 17);
            this.checkBox_savetest.TabIndex = 9;
            this.checkBox_savetest.Text = "Save test .py";
            this.toolTipSavePython.SetToolTip(this.checkBox_savetest, resources.GetString("checkBox_savetest.ToolTip"));
            this.checkBox_savetest.UseVisualStyleBackColor = true;
            // 
            // numeric_dt
            // 
            this.numeric_dt.DecimalPlaces = 5;
            this.numeric_dt.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numeric_dt.Location = new System.Drawing.Point(150, 391);
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
            this.numeric_length.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numeric_length.Location = new System.Drawing.Point(150, 413);
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
            this.numeric_scale_L.Location = new System.Drawing.Point(150, 295);
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
            this.numeric_scale_M.Location = new System.Drawing.Point(150, 316);
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
            this.checkBox_scale.Location = new System.Drawing.Point(17, 276);
            this.checkBox_scale.Name = "checkBox_scale";
            this.checkBox_scale.Size = new System.Drawing.Size(126, 17);
            this.checkBox_scale.TabIndex = 18;
            this.checkBox_scale.Text = "Scale while exporting";
            this.toolTipSavePython.SetToolTip(this.checkBox_scale, "If checked, you can set a reduction or enlargement scale for lenghts (L), masses " +
        "(M) and times (T) when exporting output.");
            this.checkBox_scale.UseVisualStyleBackColor = true;
            this.checkBox_scale.CheckedChanged += new System.EventHandler(this.checkBox_scale_CheckedChanged);
            // 
            // numeric_scale_T
            // 
            this.numeric_scale_T.DecimalPlaces = 3;
            this.numeric_scale_T.Location = new System.Drawing.Point(150, 337);
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
            this.button_convexdecomp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_convexdecomp.Location = new System.Drawing.Point(150, 463);
            this.button_convexdecomp.Name = "button_convexdecomp";
            this.button_convexdecomp.Size = new System.Drawing.Size(67, 54);
            this.button_convexdecomp.TabIndex = 21;
            this.button_convexdecomp.Text = "Convex decomposition";
            this.button_convexdecomp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTipSavePython.SetToolTip(this.button_convexdecomp, "To use this function: \r\nselect a solid body and press the button");
            this.button_convexdecomp.UseVisualStyleBackColor = true;
            this.button_convexdecomp.Click += new System.EventHandler(this.button_convexdecomp_Click);
            // 
            // button_chrono_property
            // 
            this.button_chrono_property.Location = new System.Drawing.Point(17, 523);
            this.button_chrono_property.Name = "button_chrono_property";
            this.button_chrono_property.Size = new System.Drawing.Size(102, 25);
            this.button_chrono_property.TabIndex = 22;
            this.button_chrono_property.Text = "Chrono properties";
            this.button_chrono_property.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTipSavePython.SetToolTip(this.button_chrono_property, "Open the properties for Chrono rigid body");
            this.button_chrono_property.UseVisualStyleBackColor = true;
            this.button_chrono_property.Click += new System.EventHandler(this.button_chrono_property_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "irrlicht",
            "pov"});
            this.comboBox1.Location = new System.Drawing.Point(150, 436);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(67, 21);
            this.comboBox1.TabIndex = 23;
            this.toolTipSavePython.SetToolTip(this.comboBox1, "Choose the type of  visualization system used by the test python program");
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // numeric_envelope
            // 
            this.numeric_envelope.DecimalPlaces = 5;
            this.numeric_envelope.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numeric_envelope.Location = new System.Drawing.Point(150, 590);
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
            this.numeric_margin.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numeric_margin.Location = new System.Drawing.Point(150, 612);
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
            this.numeric_contactbreaking.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numeric_contactbreaking.Location = new System.Drawing.Point(150, 634);
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
            this.numeric_sphereswept.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numeric_sphereswept.Location = new System.Drawing.Point(150, 657);
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
            // button_settrimeshcoll
            // 
            this.button_settrimeshcoll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_settrimeshcoll.Location = new System.Drawing.Point(88, 463);
            this.button_settrimeshcoll.Name = "button_settrimeshcoll";
            this.button_settrimeshcoll.Size = new System.Drawing.Size(60, 54);
            this.button_settrimeshcoll.TabIndex = 34;
            this.button_settrimeshcoll.Text = "Set as mesh collision";
            this.button_settrimeshcoll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTipSavePython.SetToolTip(this.button_settrimeshcoll, resources.GetString("button_settrimeshcoll.ToolTip"));
            this.button_settrimeshcoll.UseVisualStyleBackColor = true;
            this.button_settrimeshcoll.Click += new System.EventHandler(this.button_settrimeshcoll_Click);
            // 
            // button_ExportToJson
            // 
            this.button_ExportToJson.Location = new System.Drawing.Point(17, 109);
            this.button_ExportToJson.Name = "button_ExportToJson";
            this.button_ExportToJson.Size = new System.Drawing.Size(200, 32);
            this.button_ExportToJson.TabIndex = 35;
            this.button_ExportToJson.Text = "Export to JSON";
            this.button_ExportToJson.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTipSavePython.SetToolTip(this.button_ExportToJson, resources.GetString("button_ExportToJson.ToolTip"));
            this.button_ExportToJson.UseVisualStyleBackColor = true;
            this.button_ExportToJson.Click += new System.EventHandler(this.ExportClick);
            // 
            // button_ExportToCpp
            // 
            this.button_ExportToCpp.Location = new System.Drawing.Point(17, 75);
            this.button_ExportToCpp.Name = "button_ExportToCpp";
            this.button_ExportToCpp.Size = new System.Drawing.Size(200, 32);
            this.button_ExportToCpp.TabIndex = 36;
            this.button_ExportToCpp.Text = "Export to C++";
            this.button_ExportToCpp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTipSavePython.SetToolTip(this.button_ExportToCpp, resources.GetString("button_ExportToCpp.ToolTip"));
            this.button_ExportToCpp.UseVisualStyleBackColor = true;
            this.button_ExportToCpp.Click += new System.EventHandler(this.ExportClick);
            // 
            // butt_chronoMotors
            // 
            this.butt_chronoMotors.Location = new System.Drawing.Point(125, 523);
            this.butt_chronoMotors.Name = "butt_chronoMotors";
            this.butt_chronoMotors.Size = new System.Drawing.Size(92, 25);
            this.butt_chronoMotors.TabIndex = 37;
            this.butt_chronoMotors.Text = "Chrono motors";
            this.butt_chronoMotors.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTipSavePython.SetToolTip(this.butt_chronoMotors, "Open the properties for Chrono motors");
            this.butt_chronoMotors.UseVisualStyleBackColor = true;
            this.butt_chronoMotors.Click += new System.EventHandler(this.butt_chronoMotors_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(128, 391);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "dt";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(104, 413);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Length";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(104, 297);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Scale L";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(101, 318);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Scale M";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(101, 339);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Scale T";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(77, 437);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Visualization";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 571);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Global settings";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(48, 592);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Collision envelope";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(61, 614);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 13);
            this.label10.TabIndex = 28;
            this.label10.Text = "Collision margin";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(52, 636);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(88, 13);
            this.label11.TabIndex = 30;
            this.label11.Text = "Contact breaking";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(27, 659);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(113, 13);
            this.label12.TabIndex = 32;
            this.label12.Text = "Trimesh sphereswept r";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // SWTaskpaneHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.butt_chronoMotors);
            this.Controls.Add(this.button_ExportToCpp);
            this.Controls.Add(this.button_ExportToJson);
            this.Controls.Add(this.button_settrimeshcoll);
            this.Controls.Add(this.numeric_sphereswept);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.numeric_contactbreaking);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.numeric_margin);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.numeric_envelope);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button_chrono_property);
            this.Controls.Add(this.button_convexdecomp);
            this.Controls.Add(this.numeric_scale_T);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkBox_scale);
            this.Controls.Add(this.numeric_scale_M);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numeric_scale_L);
            this.Controls.Add(this.numeric_length);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numeric_dt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBox_savetest);
            this.Controls.Add(this.button_runtest);
            this.Controls.Add(this.checkBox_saveUV);
            this.Controls.Add(this.checkBox_separateobj);
            this.Controls.Add(this.button_setcollshape);
            this.Controls.Add(this.checkBox_constraints);
            this.Controls.Add(this.checkBox_collshapes);
            this.Controls.Add(this.checkBox_surfaces);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_ExportToPython);
            this.Name = "SWTaskpaneHost";
            this.Size = new System.Drawing.Size(242, 691);
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
        private System.Windows.Forms.Label label8;
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
    }
}
