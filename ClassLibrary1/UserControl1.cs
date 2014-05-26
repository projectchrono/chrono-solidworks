using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Windows.Media.Media3D;
using System.IO;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swcommands;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using SolidWorksTools;
using ChronoEngineAddin;

//using ConvertToCollisionShapes;



namespace ChronoEngine_SwAddin
{


    [ComVisible(true)]
    [ProgId(SWTASKPANE_PROGID)]
    public partial class SWTaskpaneHost : UserControl
    {
        public const string SWTASKPANE_PROGID = "ChronoEngine.Taskpane";
        public ISldWorks mSWApplication;
        internal SaveFileDialog SaveFileDialog1;
        internal int num_comp;
        internal string save_dir_shapes = "";
        internal string save_filename = "";
        internal UserProgressBar swProgress;
        internal Hashtable saved_parts;
        internal Hashtable saved_shapes;


        class myBytearrayHashComparer : IEqualityComparer
        {
            public new bool Equals(object x, object y)
            {
                byte[] a1 = (byte[])x;
                byte[] a2 = (byte[])y;
                if (a1.Length != a2.Length)
                    return false;

                for (int i = 0; i < a1.Length; i++)
                    if (a1[i] != a2[i])
                        return false;

                return true;
            }

            public int GetHashCode(object x)
            {
                byte[] obj = (byte[])x;
                if (obj == null || obj.Length == 0)
                    return 0;
                var hashCode = 0;
                for (var i = 0; i < obj.Length; i++)
                    // Rotate by 3 bits and XOR the new value.
                    hashCode = (hashCode << 3) | (hashCode >> (29)) ^ obj[i];
                return hashCode;
            }

        }


        public SWTaskpaneHost()
        {
            InitializeComponent();
            this.SaveFileDialog1 = new SaveFileDialog();
            this.saved_parts  = new Hashtable(new myBytearrayHashComparer());
            this.saved_shapes = new Hashtable();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChScale.L = (double)this.numeric_scale_L.Value;
            ChScale.M = (double)this.numeric_scale_M.Value;
            ChScale.T = (double)this.numeric_scale_T.Value;

            ModelDoc2 swModel;
            swModel = (ModelDoc2)this.mSWApplication.ActiveDoc;
            if (swModel == null)
            {
                System.Windows.Forms.MessageBox.Show("Please open an assembly!");
                return;
            }
            if (swModel.GetType() != (int)swDocumentTypes_e.swDocASSEMBLY)
            {
                System.Windows.Forms.MessageBox.Show("Please use this command when an assembly is open.");
                return;
            }
            
            this.SaveFileDialog1.Filter = "C::E Python script (*.py)|*.py";
            this.SaveFileDialog1.DefaultExt = "py";
            //this.SaveFileDialog1.FileName = "mechanism";
            this.SaveFileDialog1.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            DialogResult result = SaveFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                string asciitext = ""; 
                
                this.save_filename = SaveFileDialog1.FileName;

                if (this.checkBox_surfaces.Checked)
                {
                    string save_directory = System.IO.Path.GetDirectoryName(SaveFileDialog1.FileName);
                    this.save_dir_shapes = save_directory + "\\" + System.IO.Path.GetFileNameWithoutExtension(this.save_filename) + "_shapes";
                    //System.Windows.Forms.MessageBox.Show("DIRECTORY FOR SHAPES: " + save_dir_shapes);
                    DirectoryInfo mi = System.IO.Directory.CreateDirectory(this.save_dir_shapes);
                    if (mi.Exists == false) 
                        System.Windows.Forms.MessageBox.Show("ERROR. Can't create directory for .obj surfaces: " + this.save_dir_shapes);

                    // ***TEST*** Dump also hierarchy for test
                    string asciidump = "";
                    this.ExportToDump(ref asciidump);
                    FileStream dumpstream = new FileStream(this.save_dir_shapes + "\\" + "dump_log.txt", FileMode.Create, FileAccess.ReadWrite);
                    StreamWriter dumpwriter = new StreamWriter(dumpstream);
                    dumpwriter.Write(asciidump);
                    dumpwriter.Flush();
                    dumpstream.Close();
                }

                // Do the conversion into a Python text block!!!
                // This will scan the low level hierarchy of the assembly
                // and its mating constraints and create the proper Chrono::Engine
                // links.

                this.ExportToPython(ref asciitext);


                byte[] byteArray = Encoding.ASCII.GetBytes(asciitext);
                MemoryStream stream = new MemoryStream(byteArray);

                Stream fileStream;
                fileStream = SaveFileDialog1.OpenFile();
                stream.Position = 0;
                stream.WriteTo(fileStream);
                fileStream.Close();

                if (this.checkBox_savetest.Checked)
                {
                    string save_directory = System.IO.Path.GetDirectoryName(SaveFileDialog1.FileName);
                    try
                    {

                        //System.Windows.Forms.MessageBox.Show("GetCurrentWorkingDirectory: " + this.mSWApplication.GetCurrentWorkingDirectory());
                        //System.Windows.Forms.MessageBox.Show("GetExecutablePath: " + this.mSWApplication.GetExecutablePath());
                        //System.Windows.Forms.MessageBox.Show("GetDataFolder: " + this.mSWApplication.GetDataFolder(true));
 
                        if (!System.IO.File.Exists(save_directory + "\\run_test.py"))
                            System.IO.File.Copy(this.mSWApplication.GetExecutablePath() + "\\chronoengine\\run_test.py", save_directory + "\\run_test.py");
                        if (!System.IO.File.Exists(save_directory + "\\_template_POV.pov"))
                            System.IO.File.Copy(this.mSWApplication.GetExecutablePath() + "\\chronoengine\\_template_POV.pov", save_directory + "\\_template_POV.pov");
                    }
                    catch (Exception exc)
                    {
                        System.Windows.Forms.MessageBox.Show("Cannot write the test Python program. \n Make sure that the template chronoengine\\run_test.py is in your SolidWorks directory.\n\n" + exc.Message);
                    }
                }
            }

            // System.Windows.Forms.MessageBox.Show("Ok, saved");

        }
        

        //
        // Traverse for Python dumping
        //

        public void ExportToPython(ref string asciitext)
        {
            ModelDoc2 swModel;
            ConfigurationManager swConfMgr;
            Configuration swConf;
            Component2 swRootComp;

            this.saved_parts.Clear();
            this.saved_shapes.Clear();

            swModel = (ModelDoc2)this.mSWApplication.ActiveDoc;
            if (swModel == null) return;
            swConfMgr = (ConfigurationManager)swModel.ConfigurationManager;
            swConf = (Configuration)swConfMgr.ActiveConfiguration;
            swRootComp = (Component2)swConf.GetRootComponent3(true);
            
            this.mSWApplication.GetUserProgressBar(out this.swProgress);
            if (this.swProgress != null)
                this.swProgress.Start(0, 5, "Exporting to Python");

            num_comp =0;

            asciitext = "# Chrono::Engine Python script from SolidWorks \n" +
                        "# Assembly: " + swModel.GetPathName() + "\n\n\n";

            asciitext += "import ChronoEngine_PYTHON_core as chrono \n";
            asciitext += "import builtins \n\n";

            asciitext += "shapes_dir = '" + System.IO.Path.GetFileNameWithoutExtension(this.save_filename) + "_shapes" + "/' \n\n";
            
            asciitext += "if hasattr(builtins, 'exported_system_relpath'): \n";
            asciitext += "    shapes_dir = builtins.exported_system_relpath + shapes_dir \n\n";

            asciitext += "exported_items = [] \n\n";

            asciitext += "body_0= chrono.ChBodyAuxRefShared()\n" +
                         "body_0.SetName('ground')\n" +
                         "body_0.SetBodyFixed(1)\n" +
                         "exported_items.append(body_0)\n\n";

            
                    
            if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
            {
                // Write down all parts

                PythonTraverseComponent_for_ChBody (swRootComp, 1, ref asciitext, -1);


                // Write down all constraints

                MathTransform roottrasf = swRootComp.GetTotalTransform(true);
                if (roottrasf == null)
                {
                    IMathUtility swMath = (IMathUtility)this.mSWApplication.GetMathUtility();
                    double[] nulltr = new double[] {1,0,0,0,1,0,0,0,1, 0,0,0, 1, 0,0,0};
                    roottrasf = (MathTransform)swMath.CreateTransform(nulltr);
                }                

                Feature swFeat = (Feature)swModel.FirstFeature();
                PythonTraverseFeatures_for_links(swFeat, 1, ref asciitext, ref roottrasf, ref swRootComp);

                PythonTraverseComponent_for_links(swRootComp, 1, ref asciitext, ref roottrasf);


                // Write down all markers in assembly (that are not in sub parts, so they belong to 'ground' object)

                swFeat = (Feature)swModel.FirstFeature();
                PythonTraverseFeatures_for_markers(swFeat, 1, ref asciitext, 0, roottrasf);
                
            }

            if (this.swProgress != null)
                this.swProgress.End();
        }


        //
        // LINK EXPORTING FUNCTIONS
        // 

        public void PythonTraverseComponent_for_links(Component2 swComp, long nLevel, ref  string asciitext, ref MathTransform roottrasf)
        {
            // Scan assembly features and save mating info

            if (nLevel > 1)
            {
                Feature swFeat = (Feature)swComp.FirstFeature();
                PythonTraverseFeatures_for_links(swFeat, nLevel, ref asciitext, ref roottrasf, ref swComp);
            }

            // Recursive scan of subassemblies

            object[] vChildComp;
            Component2 swChildComp;

            vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];
                              
                if (swChildComp.Solving == (int)swComponentSolvingOption_e.swComponentFlexibleSolving)
                    PythonTraverseComponent_for_links(swChildComp, nLevel + 1, ref asciitext, ref roottrasf);
            }
        }

        public void PythonTraverseFeatures_for_links(Feature swFeat, long nLevel, ref  string asciitext, ref MathTransform roottrasf, ref Component2 assemblyofmates)
        {
            Feature swSubFeat;

            int num_link = 0;

            while ((swFeat != null))
            {
                // Export mates as constraints

                if ((swFeat.GetTypeName2() == "MateGroup") && 
                    (this.checkBox_constraints.Checked))
                {
                    swSubFeat = (Feature)swFeat.GetFirstSubFeature();
            
                    while ((swSubFeat != null))
                    {
                        ConvertMates.ConvertMateToPython(ref swSubFeat, ref asciitext, ref mSWApplication, ref saved_parts, ref num_link, ref roottrasf, ref assemblyofmates);
                        
                        swSubFeat = (Feature)swSubFeat.GetNextSubFeature();
                    
                    } // end while loop on subfeatures mates

                } // end if mate group

                swFeat = (Feature)swFeat.GetNextFeature();
            
            } // end while loop on features

        }



        //
        // BODY EXPORTING FUNCTIONS
        // 

        public void PythonTraverseComponent_for_markers(Component2 swComp, long nLevel, ref  string asciitext, int nbody)
        {
            // Look if component contains markers
            Feature swFeat = (Feature)swComp.FirstFeature();
            MathTransform swCompTotalTrasf = swComp.GetTotalTransform(true);
            PythonTraverseFeatures_for_markers(swFeat, nLevel, ref asciitext, nbody, swCompTotalTrasf);

            // Recursive scan of subcomponents

            Component2 swChildComp;
            object[] vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                PythonTraverseComponent_for_markers(swChildComp, nLevel + 1, ref asciitext, nbody);
            }
        }

        public void PythonTraverseFeatures_for_markers(Feature swFeat, long nLevel, ref  string asciitext, int nbody, MathTransform swCompTotalTrasf)
        {
            CultureInfo bz = new CultureInfo("en-BZ");
            
            int nmarker = 0;

            String bodyname = "body_" + nbody;

            while ((swFeat != null))
            {
                // asciitext += "# feature: " + swFeat.Name + " [" + swFeat.GetTypeName2() + "]" + "\n";

                // Export markers, if any (as coordinate systems)
                if (swFeat.GetTypeName2() == "CoordSys")
                {
                    nmarker++;
                    CoordinateSystemFeatureData swCoordSys = (CoordinateSystemFeatureData)swFeat.GetDefinition();
                    MathTransform tr = swCoordSys.Transform;

                    MathTransform tr_part = swCompTotalTrasf;
                    MathTransform tr_abs = tr.IMultiply(tr_part);  // row-ordered transf. -> reverse mult.order!

                    double[] quat = GetQuaternionFromMatrix(ref tr_abs);
                    double[] amatr = (double[])tr_abs.ArrayData;
                    String markername = "marker_" + nbody + "_" + nmarker;
                    asciitext += "\n# Auxiliary marker (coordinate system feature)\n";
                    asciitext += String.Format(bz, "{0} =chrono.ChMarkerShared()\n", markername);
                    asciitext += String.Format(bz, "{0}.SetName('{1}')" + "\n", markername, swFeat.Name);
                    asciitext += String.Format(bz, "{0}.AddMarker({1})\n", bodyname, markername);
                    asciitext += String.Format(bz, "{0}.Impose_Abs_Coord(chrono.ChCoordsysD(chrono.ChVectorD({1},{2},{3}),chrono.ChQuaternionD({4},{5},{6},{7})))\n",
                               markername,
                               amatr[9]*ChScale.L, 
                               amatr[10]*ChScale.L, 
                               amatr[11]*ChScale.L,
                               quat[0], quat[1], quat[2], quat[3]);
                }

                swFeat = (Feature)swFeat.GetNextFeature();
            }
        }


        public void PythonTraverseComponent_for_countingmassbodies(Component2 swComp, ref int valid_bodies)
        {
            // Add bodies of this component to the list
            object[] bodies;
            object bodyInfo;
            bodies = (object[])swComp.GetBodies3((int)swBodyType_e.swAllBodies, out bodyInfo);

            if (bodies != null)
            {
                // note: some bodies might be collision shapes and must not enter the mass computation:
                for (int ib = 0; ib < bodies.Length; ib++)
                {
                    Body2 abody = (Body2)bodies[ib];
                    if (!(abody.Name.StartsWith("COLL.")))
                    {
                        valid_bodies += 1;
                    }
                }
            }

            // Recursive scan of subcomponents

            Component2 swChildComp;
            object[] vChildComp = (object[])swComp.GetChildren();
            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];
                PythonTraverseComponent_for_countingmassbodies(swChildComp, ref valid_bodies);
            }
        }

        public void PythonTraverseComponent_for_massbodies(Component2 swComp, ref object[] obodies, ref int addedb)
        {
            // Add bodies of this component to the list
            object[] bodies;
            object bodyInfo;
            bodies = (object[])swComp.GetBodies3((int)swBodyType_e.swAllBodies, out bodyInfo);

            if (bodies != null)
            {
                // note: some bodies might be collision shapes and must not enter the mass computation:
                for (int ib = 0; ib < bodies.Length; ib++)
                {
                    Body2 abody = (Body2)bodies[ib];
                    if (!(abody.Name.StartsWith("COLL.")))
                    {
                        obodies[addedb] = bodies[ib];
                        addedb += 1;
                    }
                }

            }

            // Recursive scan of subcomponents

            Component2 swChildComp;
            object[] vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                PythonTraverseComponent_for_massbodies(swChildComp, ref obodies, ref addedb);
            }
        }


        public void PythonTraverseComponent_for_visualizationshapes(Component2 swComp, long nLevel, ref string asciitext, int nbody, ref int nvisshape, Component2 chbody_comp)
        {
            CultureInfo bz = new CultureInfo("en-BZ");
            object[] bodies;
            object bodyInfo;
            bodies = (object[])swComp.GetBodies3((int)swBodyType_e.swAllBodies, out bodyInfo);

            if (bodies != null)
              if (bodies.Length > 0)
            {
                // Export the component shape to a .OBJ file representing its SW body(s)
                nvisshape += 1;
                string bodyname  = "body_" + nbody;
                string shapename = "body_" + nbody + "_" + nvisshape;
                string obj_filename = this.save_dir_shapes + "\\" + shapename + ".obj";

                ModelDoc2 swCompModel = (ModelDoc2)swComp.GetModelDoc();
                if (!this.saved_shapes.ContainsKey(swCompModel.GetPathName()))
                {
                    try
                    {
                        FileStream ostream = new FileStream(obj_filename, FileMode.Create, FileAccess.ReadWrite);
                        StreamWriter writer = new StreamWriter(ostream); //, new UnicodeEncoding());
                        string asciiobj = "";
                        if (this.swProgress != null)
                            this.swProgress.UpdateTitle("Exporting " + swComp.Name2 + " (tesselate) ...");
                        // Write the OBJ converted visualization shapes:
                        TesselateToObj.Convert(swComp, ref asciiobj, this.checkBox_saveUV.Checked, ref this.swProgress);
                        writer.Write(asciiobj);
                        writer.Flush();
                        ostream.Close();

                        this.saved_shapes.Add(swCompModel.GetPathName(), shapename);
                    }
                    catch (Exception)
                    {
                        System.Windows.Forms.MessageBox.Show("Cannot write to file: " + obj_filename);
                    }
                }
                else
                {
                    // reuse the already-saved shape name
                    shapename = (String)saved_shapes[swCompModel.GetPathName()];
                }

                asciitext += String.Format(bz, "\n# Visualization shape \n", bodyname);
                asciitext += String.Format(bz, "{0}_shape = chrono.ChObjShapeFileShared() \n", shapename);
                asciitext += String.Format(bz, "{0}_shape.SetFilename(shapes_dir +'{0}.obj') \n", shapename);

                object foo = null;
                double[] vMatProperties = (double[])swComp.GetMaterialPropertyValues2((int)swInConfigurationOpts_e.swThisConfiguration, foo);

                if (vMatProperties != null)
                    if (vMatProperties[0] != -1)
                    {
                        asciitext += String.Format(bz, "{0}_shape.SetColor(chrono.ChColor({1},{2},{3})) \n", shapename,
                            vMatProperties[0], vMatProperties[1], vMatProperties[2]);
                        asciitext += String.Format(bz, "{0}_shape.SetFading({1}) \n", shapename, vMatProperties[7]);
                    }

                MathTransform absframe_chbody = chbody_comp.GetTotalTransform(true);
                MathTransform absframe_shape  = swComp.GetTotalTransform(true);
                MathTransform absframe_chbody_inv = absframe_chbody.IInverse();
                MathTransform relframe_shape = absframe_shape.IMultiply(absframe_chbody_inv);  // row-ordered transf. -> reverse mult.order!
                double[] amatr = (double[])relframe_shape.ArrayData;
                double[] quat  = GetQuaternionFromMatrix(ref relframe_shape);

                asciitext += String.Format(bz, "{0}_level = chrono.ChAssetLevelShared() \n", shapename);
                asciitext += String.Format(bz, "{0}_level.GetFrame().SetPos(chrono.ChVectorD({1},{2},{3})) \n", shapename, 
                    amatr[9] *ChScale.L,
                    amatr[10]*ChScale.L,
                    amatr[11]*ChScale.L);
                asciitext += String.Format(bz, "{0}_level.GetFrame().SetRot(chrono.ChQuaternionD({1},{2},{3},{4})) \n", shapename, quat[0], quat[1], quat[2], quat[3]);
                asciitext += String.Format(bz, "{0}_level.GetAssets().push_back({0}_shape) \n", shapename);

                asciitext += String.Format(bz, "{0}.GetAssets().push_back({1}_level) \n", bodyname, shapename);

                
            }



            // Recursive scan of subcomponents

            Component2 swChildComp;
            object[] vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                if (swChildComp.Visible == (int)swComponentVisibilityState_e.swComponentVisible)
                    PythonTraverseComponent_for_visualizationshapes(swChildComp, nLevel + 1, ref asciitext, nbody, ref nvisshape, chbody_comp);
            }
        }

       


        public void PythonTraverseComponent_for_collshapes(Component2 swComp, long nLevel, ref  string asciitext, int nbody, ref MathTransform chbodytransform, ref bool found_collisionshapes)
        {
            // Look if component contains collision shapes (customized SW solid bodies):
            PythonTraverseFeatures_for_collshapes(swComp, nLevel, ref asciitext, nbody, ref chbodytransform,  ref found_collisionshapes);

            // Recursive scan of subcomponents

            Component2 swChildComp;
            object[] vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                PythonTraverseComponent_for_collshapes(swChildComp, nLevel + 1, ref asciitext, nbody, ref chbodytransform,  ref found_collisionshapes);
            }
        }

        public void PythonTraverseFeatures_for_collshapes(Component2 swComp, long nLevel, ref  string asciitext, int nbody, ref MathTransform chbodytransform, ref bool found_collisionshapes)
        {
            CultureInfo bz = new CultureInfo("en-BZ");
            Feature swFeat;
            swFeat = (Feature)swComp.FirstFeature();    

            String bodyname = "body_" + nbody;

            MathTransform subcomp_transform = swComp.GetTotalTransform(true);
            MathTransform invchbody_trasform = (MathTransform)chbodytransform.Inverse();
            MathTransform collshape_subcomp_transform = subcomp_transform.IMultiply(invchbody_trasform); // row-ordered transf. -> reverse mult.order!

            // Export collision shapes
            if (this.checkBox_collshapes.Checked)
            {   
                object[] bodies;
                object bodyInfo;
                bodies = (object[])swComp.GetBodies3((int)swBodyType_e.swAllBodies, out bodyInfo);

                if (bodies != null)
                {
                    // see if it contains some collision shape
                    bool build_collision_model = false;
                    for (int ib = 0; ib < bodies.Length; ib++)
                    {
                        Body2 swBody = (Body2)bodies[ib];
                        if (swBody.Name.StartsWith("COLL."))
                            build_collision_model = true;
                    }

                    if (build_collision_model)
                    {
                        if (!found_collisionshapes)
                        {
                            found_collisionshapes = true;
                            // clear model only at 1st subcomponent where coll shapes are found in features:
                            asciitext += "\n# Collision shape(s) \n";
                            asciitext += String.Format(bz, "{0}.GetCollisionModel().ClearModel()\n", bodyname);
                        }

                        for (int ib = 0; ib < bodies.Length; ib++)
                        {
                            Body2 swBody = (Body2)bodies[ib];

                            if (swBody.Name.StartsWith("COLL."))
                            {
                                bool rbody_converted = false;
                                if (ConvertToCollisionShapes.SWbodyToSphere(swBody))
                                {
                                    Point3D center_l = new Point3D(); // in local subcomponent
                                    double rad = 0;
                                    ConvertToCollisionShapes.SWbodyToSphere(swBody, ref rad, ref center_l);
                                    Point3D center = SWTaskpaneHost.PointTransform(center_l, ref collshape_subcomp_transform);
                                    asciitext += String.Format(bz, "{0}.GetCollisionModel().AddSphere({1}, chrono.ChVectorD({2},{3},{4}))\n", bodyname, 
                                        rad * ChScale.L,
                                        center.X * ChScale.L,
                                        center.Y * ChScale.L,
                                        center.Z * ChScale.L);
                                    rbody_converted = true;
                                }
                                if (ConvertToCollisionShapes.SWbodyToBox(swBody))
                                {
                                    Point3D  vC_l = new Point3D();
                                    Vector3D eX_l = new Vector3D(); Vector3D eY_l = new Vector3D(); Vector3D eZ_l = new Vector3D();
                                    ConvertToCollisionShapes.SWbodyToBox(swBody, ref vC_l, ref eX_l, ref eY_l, ref eZ_l);
                                    Point3D  vC = SWTaskpaneHost.PointTransform(vC_l, ref collshape_subcomp_transform);
                                    Vector3D eX = SWTaskpaneHost.DirTransform(eX_l, ref collshape_subcomp_transform);
                                    Vector3D eY = SWTaskpaneHost.DirTransform(eY_l, ref collshape_subcomp_transform);
                                    Vector3D eZ = SWTaskpaneHost.DirTransform(eZ_l, ref collshape_subcomp_transform);
                                    double hsX = eX.Length * 0.5;
                                    double hsY = eY.Length * 0.5;
                                    double hsZ = eZ.Length * 0.5;
                                    Point3D  vO = vC + 0.5 * eX + 0.5 * eY + 0.5 * eZ;
                                    Vector3D Dx = eX; Dx.Normalize();
                                    Vector3D Dy = eY; Dy.Normalize();
                                    Vector3D Dz = Vector3D.CrossProduct(Dx, Dy);
                                    asciitext += String.Format(bz, "mr = chrono.ChMatrix33D()\n");
                                    asciitext += String.Format(bz, "mr[0,0]={0}; mr[1,0]={1}; mr[2,0]={2} \n", Dx.X, Dx.Y, Dx.Z);
                                    asciitext += String.Format(bz, "mr[0,1]={0}; mr[1,1]={1}; mr[2,1]={2} \n", Dy.X, Dy.Y, Dy.Z);
                                    asciitext += String.Format(bz, "mr[0,2]={0}; mr[1,2]={1}; mr[2,2]={2} \n", Dz.X, Dz.Y, Dz.Z);
                                    asciitext += String.Format(bz, "{0}.GetCollisionModel().AddBox({1},{2},{3},chrono.ChVectorD({4},{5},{6}),mr)\n", bodyname,
                                        hsX * ChScale.L,
                                        hsY * ChScale.L,
                                        hsZ * ChScale.L,
                                        vO.X * ChScale.L,
                                        vO.Y * ChScale.L,
                                        vO.Z * ChScale.L);
                                    rbody_converted = true;
                                }
                                if (ConvertToCollisionShapes.SWbodyToCylinder(swBody))
                                {
                                    Point3D p1_l = new Point3D();
                                    Point3D p2_l = new Point3D();
                                    double rad = 0;
                                    ConvertToCollisionShapes.SWbodyToCylinder(swBody, ref p1_l, ref p2_l, ref rad);
                                    Point3D p1 = SWTaskpaneHost.PointTransform(p1_l, ref collshape_subcomp_transform);
                                    Point3D p2 = SWTaskpaneHost.PointTransform(p2_l, ref collshape_subcomp_transform);
                                    Vector3D Dy = p1 - p2; Dy.Normalize();
                                    double hsY = (p1 - p2).Length * 0.5;
                                    double hsZ = rad;
                                    double hsX = rad;
                                    Point3D  vO = p1 + 0.5 * (p2 - p1);
                                    Vector3D Dx = new Vector3D();
                                    if (Dy.X < 0.9)
                                    {
                                        Vector3D Dtst = new Vector3D(1, 0, 0);
                                        Dx = Vector3D.CrossProduct(Dtst, Dy);
                                    }
                                    else
                                    {
                                        Vector3D Dtst = new Vector3D(0, 1, 0);
                                        Dx = Vector3D.CrossProduct(Dtst, Dy);
                                    }
                                    Vector3D Dz = Vector3D.CrossProduct(Dx, Dy);
                                    asciitext += String.Format(bz, "mr = chrono.ChMatrix33D()\n");
                                    asciitext += String.Format(bz, "mr[0,0]={0}; mr[1,0]={1}; mr[2,0]={2} \n", Dx.X, Dx.Y, Dx.Z);
                                    asciitext += String.Format(bz, "mr[0,1]={0}; mr[1,1]={1}; mr[2,1]={2} \n", Dy.X, Dy.Y, Dy.Z);
                                    asciitext += String.Format(bz, "mr[0,2]={0}; mr[1,2]={1}; mr[2,2]={2} \n", Dz.X, Dz.Y, Dz.Z);
                                    asciitext += String.Format(bz, "{0}.GetCollisionModel().AddCylinder({1},{2},{3},chrono.ChVectorD({4},{5},{6}),mr)\n", bodyname,
                                        hsX * ChScale.L,
                                        hsZ * ChScale.L,
                                        hsY * ChScale.L,
                                        vO.X * ChScale.L,
                                        vO.Y * ChScale.L,
                                        vO.Z * ChScale.L); // note order hsX-Z-Y
                                    rbody_converted = true;
                                }

                                if (ConvertToCollisionShapes.SWbodyToConvexHull(swBody, 30) && !rbody_converted)
                                {
                                    Point3D[] vertexes = new Point3D[1]; // will be resized by SWbodyToConvexHull
                                    ConvertToCollisionShapes.SWbodyToConvexHull(swBody, ref vertexes, 30);
                                    if (vertexes.Length > 0)
                                    {
                                        asciitext += String.Format(bz, "pt_vect = chrono.vector_ChVectorD()\n");
                                        for (int iv = 0; iv < vertexes.Length; iv++)
                                        {
                                            Point3D vert_l = vertexes[iv];
                                            Point3D vert   = SWTaskpaneHost.PointTransform(vert_l, ref collshape_subcomp_transform);
                                            asciitext += String.Format(bz, "pt_vect.push_back(chrono.ChVectorD({0},{1},{2}))\n",
                                                vert.X * ChScale.L,
                                                vert.Y * ChScale.L,
                                                vert.Z * ChScale.L);
                                        }
                                        asciitext += String.Format(bz, "{0}.GetCollisionModel().AddConvexHull(pt_vect)\n", bodyname);
                                    }
                                }


                            } // end dealing with a collision shape

                        } // end solid bodies traversal for converting to coll.shapes

                        

                    } // end if build_collision_model
                }

            } // end collision shapes export

        }


        public void PythonTraverseComponent_for_ChBody(Component2 swComp, long nLevel, ref  string asciitext,  int nbody)
        {
            CultureInfo bz = new CultureInfo("en-BZ");
            object[] vmyChildComp = (object[])swComp.GetChildren();
            bool found_chbody_equivalent = false;

            if (nLevel > 1)
                if (nbody == -1)
                    if ((swComp.Solving == (int)swComponentSolvingOption_e.swComponentRigidSolving) || 
                        (vmyChildComp.Length == 0))
            {
                // OK! this is a 'leaf' of the tree of ChBody equivalents (a SDW subassebly or part)

                found_chbody_equivalent = true;

                this.num_comp++;

                nbody = this.num_comp;  // mark the rest of recursion as 'n-th body found'

                if (this.swProgress != null)
                {
                    this.swProgress.UpdateTitle("Exporting " + swComp.Name2 + " ...");
                    this.swProgress.UpdateProgress(this.num_comp % 5);
                }

                MathTransform chbodytransform = swComp.GetTotalTransform(true);
                double[] amatr;
                amatr = (double[])chbodytransform.ArrayData;
                string bodyname = "body_" + this.num_comp;

                // Write create body
                asciitext += "# Rigid body part\n";
                asciitext += bodyname + "= chrono.ChBodyAuxRefShared()" + "\n";

                // Write name
                asciitext += bodyname + ".SetName('" + swComp.Name2 + "')" + "\n";

                // Write position
                asciitext += bodyname + ".SetPos(chrono.ChVectorD("
                           + (amatr[9] * ChScale.L).ToString("g", bz) + ","
                           + (amatr[10]* ChScale.L).ToString("g", bz) + ","
                           + (amatr[11]* ChScale.L).ToString("g", bz) + "))" + "\n";

                // Write rotation
                double[] quat = GetQuaternionFromMatrix(ref chbodytransform);
                asciitext += String.Format(bz, "{0}.SetRot(chrono.ChQuaternionD({1:g},{2:g},{3:g},{4:g}))\n",
                           bodyname, quat[0], quat[1], quat[2], quat[3]);

                // Compute mass

                int nvalid_bodies = 0;
                PythonTraverseComponent_for_countingmassbodies(swComp, ref nvalid_bodies);

                int addedb = 0;
                object[] bodies_nocollshapes = new object[nvalid_bodies];
                PythonTraverseComponent_for_massbodies(swComp, ref bodies_nocollshapes, ref addedb);
                
                MassProperty swMass;
                swMass = (MassProperty)swComp.IGetModelDoc().Extension.CreateMassProperty();
                bool boolstatus = false;
                boolstatus = swMass.AddBodies((object[])bodies_nocollshapes);
                swMass.SetCoordinateSystem(chbodytransform);
                swMass.UseSystemUnits = true;
                //note: do not set here the COG-to-REF position because here SW express it in absolute coords
                // double cogX = ((double[])swMass.CenterOfMass)[0];
                // double cogY = ((double[])swMass.CenterOfMass)[1];
                // double cogZ = ((double[])swMass.CenterOfMass)[2];
                double mass = swMass.Mass;
                double[] Itensor = (double[])swMass.GetMomentOfInertia((int)swMassPropertyMoment_e.swMassPropertyMomentAboutCenterOfMass);
                double Ixx = Itensor[0];
                double Iyy = Itensor[4];
                double Izz = Itensor[8];
                double Ixy = Itensor[1];
                double Izx = Itensor[2];
                double Iyz = Itensor[5];

                MassProperty swMassb;
                swMassb = (MassProperty)swComp.IGetModelDoc().Extension.CreateMassProperty();
                bool boolstatusb = false;
                boolstatusb = swMassb.AddBodies(bodies_nocollshapes);
                swMassb.UseSystemUnits = true;
                double cogXb = ((double[])swMassb.CenterOfMass)[0];
                double cogYb = ((double[])swMassb.CenterOfMass)[1];
                double cogZb = ((double[])swMassb.CenterOfMass)[2];

                asciitext += String.Format(bz, "{0}.SetMass({1:g})\n",
                           bodyname,
                           mass * ChScale.M);

                // Write inertia tensor 
                asciitext += String.Format(bz, "{0}.SetInertiaXX(chrono.ChVectorD({1:g},{2:g},{3:g}))\n",
                           bodyname, 
                           Ixx * ChScale.M * ChScale.L * ChScale.L,
                           Iyy * ChScale.M * ChScale.L * ChScale.L,
                           Izz * ChScale.M * ChScale.L * ChScale.L);
                // Note: C::E assumes that's up to you to put a 'minus' sign in values of Ixy, Iyz, Izx
                asciitext += String.Format(bz, "{0}.SetInertiaXY(chrono.ChVectorD({1:g},{2:g},{3:g}))\n",
                           bodyname,
                           -Ixy * ChScale.M * ChScale.L * ChScale.L,
                           -Izx * ChScale.M * ChScale.L * ChScale.L,
                           -Iyz * ChScale.M * ChScale.L * ChScale.L);

                // Write the position of the COG respect to the REF
                asciitext += String.Format(bz, "{0}.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD({1:g},{2:g},{3:g}),chrono.ChQuaternionD(1,0,0,0)))\n",
                            bodyname, 
                            cogXb * ChScale.L,
                            cogYb * ChScale.L,
                            cogZb * ChScale.L);

                // Write 'fixed' state
                if (swComp.IsFixed())
                    asciitext += String.Format(bz, "{0}.SetBodyFixed(1)\n", bodyname);


                // Write shapes (saving also Wavefront files .obj)
                if (this.checkBox_surfaces.Checked)
                {
                    int nvisshape = 0;

                    if (swComp.Visible == (int)swComponentVisibilityState_e.swComponentVisible)
                        PythonTraverseComponent_for_visualizationshapes(swComp, nLevel, ref asciitext, nbody, ref nvisshape, swComp);        
                } 

                // Write markers (SW coordsystems) contained in this component or subcomponents
                // if any.
                PythonTraverseComponent_for_markers(swComp, nLevel, ref asciitext, nbody);


                // Write collision shapes (customized SW solid bodies) contained in this component or subcomponents
                // if any.
                bool found_collisionshapes = false;
                PythonTraverseComponent_for_collshapes(swComp, nLevel, ref asciitext, nbody, ref chbodytransform, ref found_collisionshapes);
                if (found_collisionshapes)
                {
                    asciitext += String.Format(bz, "{0}.GetCollisionModel().BuildModel()\n", bodyname);
                    asciitext += String.Format(bz, "{0}.SetCollide(1)\n", bodyname);
                }

                // Insert to a list of exported items
                asciitext += String.Format(bz, "\n" +"exported_items.append({0})\n", bodyname);

                // End writing body in Python-
                asciitext += "\n\n\n"; 


            } // end if ChBody equivalent (tree leaf or non-flexible assembly)


            // Things to do also for sub-components of 'non flexible' assemblies: 
            //

                // store in hashtable, will be useful later when adding constraints
            if ((nLevel > 1) && (nbody != -1))
            try
            {
                string bodyname = "body_" + this.num_comp;

                ModelDocExtension swModelDocExt = default(ModelDocExtension);
                ModelDoc2 swModel = (ModelDoc2)this.mSWApplication.ActiveDoc;
                //if (swModel != null)
                swModelDocExt = swModel.Extension;
                this.saved_parts.Add(swModelDocExt.GetPersistReference3(swComp), bodyname);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Cannot add part to hashtable?");
            }


            // Traverse all children, proceeding to subassemblies and parts, if any
            // 

            object[] vChildComp;
            Component2 swChildComp;

            vChildComp = (object[])swComp.GetChildren();

            for (long i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                PythonTraverseComponent_for_ChBody(swChildComp, nLevel + 1, ref asciitext, nbody);
            }


        }





        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




        //
        // Traverse everything
        //

        public void ExportToDump(ref string asciitext)
        {
            ModelDoc2 swModel;
            ConfigurationManager swConfMgr;
            Configuration swConf;
            Component2 swRootComp;

            swModel = (ModelDoc2)this.mSWApplication.ActiveDoc;
            swConfMgr = (ConfigurationManager)swModel.ConfigurationManager;
            swConf = (Configuration)swConfMgr.ActiveConfiguration;
            swRootComp = (Component2)swConf.GetRootComponent3(true);

            asciitext = "# Dump hierarchy from SolidWorks \n" + 
                        "# Assembly: " + swModel.GetPathName() + "\n\n\n";

            // The root component (root assembly) cannot work in DumpTraverseComponent() 
            // cause SW api limit, so call feature traversal using this custom step:
            Feature swFeat = (Feature)swModel.FirstFeature();
            DumpTraverseFeatures(swFeat, 1, ref asciitext);

            // Traverse all sub components
            if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
            {
                DumpTraverseComponent(swRootComp, 1, ref asciitext);
            }
        }

        public void DumpTraverseFeatures(Feature swFeat, long nLevel, ref  string asciitext)
        {
            Feature swSubFeat;
            string sPadStr = " ";
            long i = 0;

            for (i = 0; i <= nLevel; i++)
            {
                sPadStr = sPadStr + "  ";
            }

            while ((swFeat != null))
            {
                asciitext += sPadStr + "    -" + swFeat.Name + " [" + swFeat.GetTypeName2() + "]" + "\n";
                swSubFeat = (Feature)swFeat.GetFirstSubFeature();
                if ((swSubFeat != null))
                {
                    DumpTraverseFeatures(swSubFeat, nLevel + 1, ref asciitext);
                }
                if (nLevel == 1)
                {
                    swFeat = (Feature)swFeat.GetNextFeature();
                }
                else
                {
                    swFeat = (Feature)swFeat.GetNextSubFeature();
                }
            }
        }
        

        public void DumpTraverseComponent(Component2 swComp, long nLevel, ref  string asciitext)
        {
            // *** SCAN THE COMPONENT FEATURES

            if (!swComp.IsRoot())
            {
                Feature swFeat;
                swFeat = (Feature)swComp.FirstFeature();
                DumpTraverseFeatures(swFeat, nLevel, ref asciitext);
            }

            // *** RECURSIVE SCAN CHILDREN COMPONENTS

            object[] vChildComp;
            Component2 swChildComp;
            string sPadStr = " ";
            long i = 0;

            for (i = 0; i <= nLevel - 1; i++)
            {
                sPadStr = sPadStr + "  ";
            }

            vChildComp = (object[])swComp.GetChildren();

            for (i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                asciitext += sPadStr + "+" + swChildComp.Name2 + " <" + swChildComp.ReferencedConfiguration + ">" +"\n";

                // DumpTraverseComponentFeatures(swChildComp, nLevel, ref asciitext);

                DumpTraverseComponent(swChildComp, nLevel + 1, ref asciitext);
            }
        }





        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        public double[] GetQuaternionFromMatrix(ref MathTransform trasf)
        {
            double[] q = new double[4];
            double[] amatr = (double[])trasf.ArrayData;
            double s, tr;
										// for speed reasons: ..
            double m00 = amatr[0];
			double m01 = amatr[3];
			double m02 = amatr[6]; 
			double m10 = amatr[1]; 
			double m11 = amatr[4]; 
			double m12 = amatr[7]; 
			double m20 = amatr[2];
            double m21 = amatr[5];
            double m22 = amatr[8];

			tr=m00 + m11 + m22;		// diag sum

			if (tr >= 0)
			{
				s = Math.Sqrt(tr + 1.0);
				q[0] = 0.5 * s;
				s = 0.5 / s;
				q[1] = (m21 - m12) * s;
				q[2] = (m02 - m20) * s;
				q[3] = (m10 - m01) * s;
			}
			else
			{
				int i = 0;

				if (m11 > m00)
				{	
					i = 1;
					if (m22 > m11)	i = 2;
				}
				else
				{
					if (m22 > m00)  i = 2;
				}

				switch (i)
				{
				case 0:
					s = Math.Sqrt(m00 - m11 - m22 + 1);
					q[1] = 0.5 * s;
					s = 0.5 / s;
					q[2] = (m01 + m10) * s;
					q[3] = (m20 + m02) * s;
					q[0] = (m21 - m12) * s;
					break;
				case 1:
					s = Math.Sqrt(m11 - m22 - m00 + 1);
					q[2] = 0.5 * s;
					s = 0.5 / s;
					q[3] = (m12 + m21) * s;
					q[1] = (m01 + m10) * s;
					q[0] = (m02 - m20) * s;
					break;
				case 2:
					s = Math.Sqrt(m22 - m00 - m11 + 1);
					q[3] = 0.5 * s;
					s = 0.5 / s;
					q[1] = (m20 + m02) * s;
					q[2] = (m12 + m21) * s;
					q[0] = (m10 - m01) * s;
					break;
				}
			}
			return q;
        }

        public static Matrix3D GetMatrixFromMathTransform(ref MathTransform trasf)
        {
            double[] amatr = (double[])trasf.ArrayData;

            Matrix3D res = new Matrix3D(amatr[0], amatr[1], amatr[2], 0,
                                        amatr[3], amatr[4], amatr[5], 0,
                                        amatr[6], amatr[7], amatr[8], 0,
                                        amatr[9], amatr[10], amatr[11], 1);
            return res;
        }


        public static Point3D PointTransform(Point3D pt, ref MathTransform trasf)
        {
            //Point3D pp = new Point3D(pt.X,pt.Y,pt.Z);
            Matrix3D M = GetMatrixFromMathTransform(ref trasf);
            return M.Transform(pt);
        }

        public static Vector3D DirTransform(Vector3D dir, ref MathTransform trasf)
        {
            Matrix3D M = GetMatrixFromMathTransform(ref trasf);
            M.OffsetX = 0; M.OffsetY = 0; M.OffsetZ = 0;
            return M.Transform(dir);
        }


        private void checkBox_constraints_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button_setcollshape_Click(object sender, EventArgs e)
        {
            ModelDoc2 swModel;
            swModel = (ModelDoc2)this.mSWApplication.ActiveDoc;
            if (swModel == null)
            {
                System.Windows.Forms.MessageBox.Show("Please open a part and select a solid body!");
                return;
            }

            SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;

            if (swSelMgr.GetSelectedObjectCount2(-1) == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select one or more solid bodies!");
                return;
            }

            /* TEST
            // stub for code to try to convert whole body:
            ModelDoc2 selpart;
            Component2 selcomp;
            
            for (int isel = 1; isel <= swSelMgr.GetSelectedObjectCount2(-1); isel++ )
            {
                if ((swSelectType_e)swSelMgr.GetSelectedObjectType3(isel, -1) == swSelectType_e.swSelCOMPONENTS)
                {
                    selcomp = (Component2)swSelMgr.GetSelectedObject6(isel, -1);
                    if (selcomp != null)
                    {
                        selpart = (ModelDoc2)selcomp.GetModelDoc2();
                        String partinfo = "Component2! Part/assembly name:" + selcomp.Name + "\n" + "Config:" + selcomp.ReferencedConfiguration + "\n" + "Path:" + selpart.GetPathName();
                        System.Windows.Forms.MessageBox.Show(partinfo);
                    }
                }
            }
            */


            for (int isel = 1; isel <= swSelMgr.GetSelectedObjectCount2(-1); isel++ )
            {
                if ((swSelectType_e)swSelMgr.GetSelectedObjectType3(isel, -1) != swSelectType_e.swSelSOLIDBODIES)
                {
                    System.Windows.Forms.MessageBox.Show("This function can be applied only to solid bodies! Select one or more bodies before using it.");
                    return;
                }

                bool rbody_converted = false;
                Body2 swBody = (Body2)swSelMgr.GetSelectedObject6(isel, -1);


                // ----- Try to see if this is a sphere

                if (ConvertToCollisionShapes.SWbodyToSphere(swBody))
                {
                    string mname = swBody.Name;
                    mname.Replace("COLL.SPHERE-", "");
                    swBody.Name = "COLL.SPHERE-" + mname;
                    rbody_converted = true;
                }

                // ----- Try to see if this is a box

                if (ConvertToCollisionShapes.SWbodyToBox(swBody))
                {
                    string mname = swBody.Name;
                    mname.Replace("COLL.BOX-", "");
                    swBody.Name = "COLL.BOX-" + mname;
                    rbody_converted = true;
                }

                // ----- Try to see if this is a cylinder

                if (ConvertToCollisionShapes.SWbodyToCylinder(swBody))
                {
                    string mname = swBody.Name;
                    mname.Replace("COLL.CYLINDER-", "");
                    swBody.Name = "COLL.CYLINDER-" + mname;
                    rbody_converted = true;
                }

                // ----- Try to see if this is a convex hull (except if already converted bacause it's a box)

                if (ConvertToCollisionShapes.SWbodyToConvexHull(swBody, 30) && !rbody_converted)
                {
                    string mname = swBody.Name;
                    mname.Replace("COLL.CONV.HULL-", "");
                    swBody.Name = "COLL.CONV.HULL-" + mname;
                    rbody_converted = true;
                }


                // Change color of the body if it can be used as collision shape
                if (rbody_converted)
                {
                    //int materr = swBody.SetMaterialProperty("Default", "Materiali personalizzati.sldmat", "CollisionShapeMaterial");
                    int materr = swBody.SetMaterialProperty("Default", "", "Air");
                    if (materr !=1)
                        materr = swBody.SetMaterialProperty("Default", "", "Aria");
                    if (materr != 1)
                        materr = swBody.SetMaterialProperty("Default", "", "Luft");
                    if (materr != 1)
                        System.Windows.Forms.MessageBox.Show("Warning! could not assign 'air' material to the collision shape: "+ swBody.Name +"\n This can affect mass computations."); 

                    double[] mcolor = new double[9] { 1, 0, 0, 1, 1, 1, 0, 0.7, 0 }; //  R, G, B, Ambient, Diffuse, Specular, Shininess, Transparency, Emission 
                    swBody.MaterialPropertyValues2 = mcolor;
                    
                    // must force rebuild otherwise one cannot see in the design tree that an 'air' material has been added to collision bodies, etc.
                    //swModel.Rebuild((int)swRebuildOptions_e.swForceRebuildAll);
                    swModel.ForceRebuild3(false);
                }

            } // end loop on selected items

        }

        private void checkBox_surfaces_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox_surfaces.Checked)
            {
                this.checkBox_separateobj.Enabled = true;
                this.checkBox_saveUV.Enabled = true;
            }
            else
            {
                this.checkBox_separateobj.Enabled = false;
                this.checkBox_saveUV.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button_runtest_Click(object sender, EventArgs e)
        {
            CultureInfo bz = new CultureInfo("en-BZ");

            try
            {
                System.Diagnostics.ProcessStartInfo startInfo;
                System.Diagnostics.Process process;
                string save_directory = System.IO.Path.GetDirectoryName(this.save_filename);
                string directory = save_directory;
                string pyArgs = "";
                pyArgs += " -f " + System.IO.Path.GetFileName(this.save_filename); //was .GetFileNameWithoutExtension(this.save_filename);
                pyArgs += " -d " + this.numeric_dt.Value.ToString(bz);
                pyArgs += " -T " + this.numeric_length.Value.ToString(bz);
                string script = "run_test.py";
                startInfo = new System.Diagnostics.ProcessStartInfo("Python.exe");
                startInfo.WorkingDirectory = directory;
                startInfo.Arguments = script + " " + pyArgs;
               // startInfo.UseShellExecute = false;
               // startInfo.CreateNoWindow = false;
               // startInfo.RedirectStandardOutput = true;
               // startInfo.RedirectStandardError = true;

                process = new System.Diagnostics.Process();
                process.StartInfo = startInfo;
                process.Start();

              //  process.WaitForExit();
              //  process.Close();
            }
            catch (Exception myex)
            {
                System.Windows.Forms.MessageBox.Show("Cannot execute the test Python program. \n - Make sure that you already saved with 'save test.py' enabled; \n - Make sure you have Python installed \n\n\nException:\n" + myex.ToString());
            }
        }

        private void checkBox_scale_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox_scale.Checked)
            {
                this.numeric_scale_L.Enabled = true;
                this.numeric_scale_M.Enabled = true;
                this.numeric_scale_T.Enabled = true;
            }
            else
            {
                this.numeric_scale_L.Enabled = false;
                this.numeric_scale_M.Enabled = false;
                this.numeric_scale_T.Enabled = false;
            }
        }

        private void button_convexdecomp_Click(object sender, EventArgs e)
        {
            ModelDoc2 swModel;
            swModel = (ModelDoc2)this.mSWApplication.ActiveDoc;
            if (swModel == null)
            {
                System.Windows.Forms.MessageBox.Show("Please open a part and select a solid body!");
                return;
            }

            SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;

            if (swSelMgr.GetSelectedObjectCount2(-1) == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select one or more solid bodies!");
                return;
            }

            for (int isel = 1; isel <= swSelMgr.GetSelectedObjectCount2(-1); isel++)
            {
                if ((swSelectType_e)swSelMgr.GetSelectedObjectType3(isel, -1) != swSelectType_e.swSelSOLIDBODIES)
                {
                    System.Windows.Forms.MessageBox.Show("This function can be applied only to solid bodies! Select one or more bodies before using it.");
                    return;
                }

                bool rbody_converted = false;
                Body2 swBodyIn = (Body2)swSelMgr.GetSelectedObject6(isel, -1);

                // ----- tesselate
                Face2 swFace = null;
                Tessellation swTessellation = null;

                bool bResult = false;

                // Pass in null so the whole body will be tessellated
                swTessellation = (Tessellation)swBodyIn.GetTessellation(null);

                // Set up the Tessellation object
                swTessellation.NeedFaceFacetMap = true;
                swTessellation.NeedVertexParams = true;
                swTessellation.NeedVertexNormal = true;
                swTessellation.ImprovedQuality = true;

                int group_vstride = 0;

                // How to handle matches across common edges
                swTessellation.MatchType = (int)swTesselationMatchType_e.swTesselationMatchFacetTopology;

                // Do it
                if (swProgress != null)
                    swProgress.UpdateTitle("Tesselation process...");
                bResult = swTessellation.Tessellate();

                // Now get the facet data per face
                int[] aFacetIds;
                int iNumFacetIds;
                int[] aFinIds;
                int[] aVertexIds;
                double[] aVertexCoords1;

                int numv = swTessellation.GetVertexCount();
                int numf = swTessellation.GetFacetCount();
                int iface = 0;

                // Get all vertexes
                vhacd_CLI.Vect3D[] myvertexes = new vhacd_CLI.Vect3D[numv];
                vhacd_CLI.Triangle[] mytriangles = new vhacd_CLI.Triangle[numf];
 
                for (int iv = 0; iv < numv; iv++)
                {
                    if ((swProgress != null) && (iv % 200 == 0))
                        swProgress.UpdateTitle("Vertexes process: " + iv + "-th vertex ...");
                    aVertexCoords1 = (double[])swTessellation.GetVertexPoint(iv);
                    myvertexes[iv] = new vhacd_CLI.Vect3D(
                        aVertexCoords1[0],
                        aVertexCoords1[1],
                        aVertexCoords1[2] );
                }

                // Loop over faces
                swFace = (Face2)swBodyIn.GetFirstFace();

                while (swFace != null)
                {
                    aFacetIds = (int[])swTessellation.GetFaceFacets(swFace);

                    iNumFacetIds = aFacetIds.Length;

                    for (int iFacetIdIdx = 0; iFacetIdIdx < iNumFacetIds; iFacetIdIdx++)
                    {
                        if ((swProgress != null) && (iFacetIdIdx % 100 == 0))
                            swProgress.UpdateTitle("Faces process: " + iFacetIdIdx + "-th face ...");

                        aFinIds = (int[])swTessellation.GetFacetFins(aFacetIds[iFacetIdIdx]);

                        // There should always be three fins per facet
                        int iFinIdx = 0; 
                        aVertexIds = (int[])swTessellation.GetFinVertices(aFinIds[iFinIdx]);
                        int ip1 = aVertexIds[0] + group_vstride;
                        iFinIdx = 1; 
                        aVertexIds = (int[])swTessellation.GetFinVertices(aFinIds[iFinIdx]);
                        int ip2 = aVertexIds[0] + group_vstride;
                        iFinIdx = 2; 
                        aVertexIds = (int[])swTessellation.GetFinVertices(aFinIds[iFinIdx]);
                        int ip3 = aVertexIds[0] + group_vstride;
                        
                        mytriangles[iface] = new vhacd_CLI.Triangle(ip1, ip2, ip3);

                        iface++;
                    }
                    swFace = (Face2)swFace.GetNextFace();
                }

                group_vstride += swTessellation.GetVertexCount();
                
                // construct a new customer dialog
                ConvexDecomp2 myCustomerDialog = new ConvexDecomp2();
                myCustomerDialog.SetMeshInfo(numf, numv);
                // show the modal dialog
                if (myCustomerDialog.ShowDialog() == DialogResult.OK)
                {

                    // Perform convexdecomposition
                    vhacd_CLI.vhacd_CLI_wrapper myConvexDecomp = new vhacd_CLI.vhacd_CLI_wrapper();

                    myConvexDecomp.SetMesh(myvertexes, mytriangles);

                    myConvexDecomp.targetNTrianglesDecimatedMesh = myCustomerDialog.m_decimate;
                    myConvexDecomp.alpha = myCustomerDialog.m_alpha;
                    myConvexDecomp.concavity = myCustomerDialog.m_concavity;
                    myConvexDecomp.depth = myCustomerDialog.m_depht;
                    myConvexDecomp.posSampling = myCustomerDialog.m_positionsampling;
                    myConvexDecomp.angleSampling = myCustomerDialog.m_anglesampling;
                    myConvexDecomp.posRefine = myCustomerDialog.m_posrefine;
                    myConvexDecomp.angleRefine = myCustomerDialog.m_anglerefine;

                    int nhulls = myConvexDecomp.Compute(true, false);

                    Body2 myNewBody = null;
                    IPartDoc mpart = null;

                    if (myConvexDecomp.GetNClusters() > 0)
                    {
                        Configuration swConf;
                        Component2 swRootComp;

                        swConf = (Configuration)swModel.GetActiveConfiguration();
                        swRootComp = (Component2)swConf.GetRootComponent3(true);

                        if (swModel.GetType() == (int)swDocumentTypes_e.swDocPART)
                        {
                            mpart = (IPartDoc)swModel;
                        }
                    }

                    for (int nc = 0; nc < myConvexDecomp.GetNClusters(); nc++)
                    {
                        vhacd_CLI.Vect3D[] hull_vertexes = new vhacd_CLI.Vect3D[0];
                        vhacd_CLI.Triangle[] hull_triangles = new vhacd_CLI.Triangle[0];
                        int npoints = myConvexDecomp.GetClusterConvexHull(nc, ref hull_vertexes, ref hull_triangles);

                        if (mpart != null)
                        {
                            //myNewBody = (Body2)mpart.CreateNewBody();
                            myNewBody = (Body2)mpart.ICreateNewBody2();

                            for (int ihf = 0; ihf < hull_triangles.GetLength(0); ihf++)
                            {
                                double[] vPt = new double[9];
                                vPt[0] = hull_vertexes[hull_triangles[ihf].p1].X;
                                vPt[1] = hull_vertexes[hull_triangles[ihf].p1].Y;
                                vPt[2] = hull_vertexes[hull_triangles[ihf].p1].Z;
                                vPt[3] = hull_vertexes[hull_triangles[ihf].p2].X;
                                vPt[4] = hull_vertexes[hull_triangles[ihf].p2].Y;
                                vPt[5] = hull_vertexes[hull_triangles[ihf].p2].Z;
                                vPt[6] = hull_vertexes[hull_triangles[ihf].p3].X;
                                vPt[7] = hull_vertexes[hull_triangles[ihf].p3].Y;
                                vPt[8] = hull_vertexes[hull_triangles[ihf].p3].Z;
                                myNewBody.CreatePlanarTrimSurfaceDLL((Object)vPt, null);
                            }
                            bool created = myNewBody.CreateBodyFromSurfaces();
                            if (created)
                            {
                                ModelDocExtension mextension = swModel.Extension;
                                Feature mlastfeature = mextension.GetLastFeatureAdded();
                                mlastfeature.Select2(true, nc); // does not append?
                                /*  ***TO DO***
                                int materr = myNewBody.SetMaterialProperty("Default", "", "Air");
                                if (materr != 1)
                                    materr = myNewBody.SetMaterialProperty("Default", "", "Aria");
                                if (materr != 1)
                                    materr = myNewBody.SetMaterialProperty("Default", "", "Luft");
                                //if (materr != 1)
                                //    System.Windows.Forms.MessageBox.Show("Warning! could not assign 'air' material to the collision shape: " + myNewBody.Name + "\n This can affect mass computations.");
                                */
                                double[] mcolor = new double[9] { 1, 0, 0, 1, 1, 1, 0, 0.7, 0 }; //  R, G, B, Ambient, Diffuse, Specular, Shininess, Transparency, Emission 
                                myNewBody.MaterialPropertyValues2 = mcolor;  // it does not work
                            }
                        }
                    } // end loop on clusters
                    /* ***TO DO***
                    // all new 'imported body' features must be grouped into a folder to avoid confusion:
                    IFeatureManager swFeatMgr = swModel.FeatureManager;
                    Feature swFeat = swFeatMgr.InsertFeatureTreeFolder2((int)swFeatureTreeFolderType_e.swFeatureTreeFolder_Containing);
                    */
                    // must force rebuild otherwise one cannot see in the design tree that an 'air' material has been added to collision bodies, etc.
                    //swModel.Rebuild((int)swRebuildOptions_e.swForceRebuildAll);
                    swModel.ForceRebuild3(false);

                } // end if user choose OK to decompose
                
            } // end loop on selected items

        }








    }  // end class



}  // end namespace
