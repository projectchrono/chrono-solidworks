using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


//using ChronoEngineAddIn;

namespace ChronoEngineAddin
{
    public partial class EditChLink : Form
    {
        //public ISldWorks m_SWApplication;
        SelectionMgr m_swSelMgr;
        bool butt_addMate_clicked = false;



        public EditChLink(ref SelectionMgr swSelMgr)
        {
            InitializeComponent();
            m_swSelMgr = swSelMgr;
        }

        public void AddMate() // , ref AttributeDef mdefattr_chbody
        {
            //System.Windows.Forms.MessageBox.Show("StoreToSelection()");

            // If user pressed OK, apply settings to all selected parts (i.e. ChLink in C::E):
            for (int isel = 1; isel <= m_swSelMgr.GetSelectedObjectCount2(-1); isel++)
            {
                if ((swSelectType_e)m_swSelMgr.GetSelectedObjectType3(isel, -1) == swSelectType_e.swSelMATES)
                {
                    // coincident: 0
                    // concentric: 1
                    // distance: 5

                    Feature feat = m_swSelMgr.GetSelectedObject6(isel, -1);
                    //Mate2 swMate = (Mate2)m_swSelMgr.GetSelectedObjectsComponent3(isel, -1);
                    ////MateEntity2 swMate = (MateEntity2)m_swSelMgr.GetSelectedObjectsComponent3(isel, -1);


                    lst_matesSelected.Items.Add(feat.Name);
                    //


                    //Mate2 swMate = (Mate2)swMateFeature.GetSpecificFeature2();
                    //// Get the mated parts
                    //MateEntity2 swEntityA = swMate.MateEntity(0);
                    //MateEntity2 swEntityB = swMate.MateEntity(1);
                    //Component2 swCompA = swEntityA.ReferenceComponent;
                    //Component2 swCompB = swEntityB.ReferenceComponent;
                    //double[] paramsA = (double[])swEntityA.EntityParams;
                    //double[] paramsB = (double[])swEntityB.EntityParams;
                    //// this is needed because parts might reside in subassemblies, and mate params are expressed in parent subassembly
                    //MathTransform invroottrasf = (MathTransform)roottrasf.Inverse();
                    //MathTransform trA = roottrasf;
                    //MathTransform trB = roottrasf;

                    //System.Windows.Forms.MessageBox.Show("Mate type: " + feat.Name); //+ swMate.GetType().Name




                    //swMate.GetConcentricAlignmentType

                    //Feature swMateFeature = (Feature)m_swSelMgr.GetSelectedObjectsComponent3(isel, -1);
                    //string name = swMateFeature.Name;
                    //System.Windows.Forms.MessageBox.Show(name);

                    ////Component2 swPart = (Component2)swSelMgr.GetSelectedObject6(isel, -1);
                    //Component2 swPart = swSelMgr.GetSelectedObjectsComponent3(isel, -1);
                    //ModelDoc2 swPartModel = (ModelDoc2)swPart.GetModelDoc2();
                    ////Component2 swPartcorr = swPart;
                    ////Component2 swPartcorr = swPartModel.Extension.GetCorresponding(swPart);// ***TODO*** for instanced parts? does not work...

                    //// fetch SW attribute with Chrono parameters for ChBody
                    //SolidWorks.Interop.sldworks.Attribute myattr = (SolidWorks.Interop.sldworks.Attribute)swPart.FindAttribute(mdefattr_chbody, 0);
                    //if (myattr == null)
                    //{
                    //    // if not already added to part, create and attach it
                    //    System.Windows.Forms.MessageBox.Show("[store settings failed - should not happen]");
                    //    return;
                    //}

                    //((Parameter)myattr.GetParameter("collision_on")).SetDoubleValue2(
                    //              Convert.ToDouble(m_collide), (int)swInConfigurationOpts_e.swThisConfiguration, "");

                    //((Parameter)myattr.GetParameter("friction")).SetDoubleValue2(
                    //              m_friction, (int)swInConfigurationOpts_e.swThisConfiguration, "");

                    //((Parameter)myattr.GetParameter("rolling_friction")).SetDoubleValue2(
                    //              m_rolling_friction, (int)swInConfigurationOpts_e.swThisConfiguration, "");

                    //((Parameter)myattr.GetParameter("spinning_friction")).SetDoubleValue2(
                    //              m_spinning_friction, (int)swInConfigurationOpts_e.swThisConfiguration, "");

                    //((Parameter)myattr.GetParameter("restitution")).SetDoubleValue2(
                    //              m_restitution, (int)swInConfigurationOpts_e.swThisConfiguration, "");

                    //((Parameter)myattr.GetParameter("collision_margin")).SetDoubleValue2(
                    //              m_collision_margin, (int)swInConfigurationOpts_e.swThisConfiguration, "");

                    //((Parameter)myattr.GetParameter("collision_envelope")).SetDoubleValue2(
                    //              m_collision_envelope, (int)swInConfigurationOpts_e.swThisConfiguration, "");

                    //((Parameter)myattr.GetParameter("collision_family")).SetDoubleValue2(
                    //              (double)m_collision_family, (int)swInConfigurationOpts_e.swThisConfiguration, "");
                    ///*
                    //// fetch SW attribute with Chrono parameters for ChConveyor
                    //SolidWorks.Interop.sldworks.Attribute myattr_conveyor = (SolidWorks.Interop.sldworks.Attribute)swPart.FindAttribute(defattr_chconveyor, 0);
                    //if (myattr_conveyor == null)
                    //{
                    //    // if not already added to part, create and attach it
                    //    myattr_conveyor = defattr_chconveyor.CreateInstance5(swPartModel, swPart, "Chrono ChConveyor data", 0, (int)swInConfigurationOpts_e.swThisConfiguration);
                    //    if (myattr_conveyor == null) 
                    //        System.Windows.Forms.MessageBox.Show("myattr null in setting!!");
                    //}

                    //((Parameter)myattr_conveyor.GetParameter("conveyor_speed")).SetDoubleValue2(
                    //              m_conveyor_speed, (int)swInConfigurationOpts_e.swThisConfiguration, "");
                    //*/
                    //swPartModel.ForceRebuild3(false);
                }
            }
        }


        private void butt_addMate_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("pressed AddMate button");
            AddMate();
        }

        private void EditChLink_Load(object sender, EventArgs e)
        {

        }

    }
}
