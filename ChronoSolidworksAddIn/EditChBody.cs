using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swcommands;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;


namespace ChronoEngineAddin
{
    public partial class EditChBody : Form
    {
        public bool m_collide;
        public double m_friction;
        public double m_rolling_friction;
        public double m_spinning_friction;
        public double m_restitution;
        public double m_collision_margin;
        public double m_collision_envelope;
        public int    m_collision_family;

        //bool show_conveyor_params;
        public double m_conveyor_speed;

        public EditChBody()
        {
            //show_conveyor_params = false;
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            m_collide = this.checkBox_collide.Checked;
            m_friction = (double)this.numeric_friction.Value;
            m_rolling_friction = (double)this.numeric_rolling_friction.Value;
            m_spinning_friction = (double)this.numeric_spinning_friction.Value;
            m_restitution = (double)this.numeric_restitution.Value;
            m_collision_margin = (double)this.numeric_collision_margin.Value;
            m_collision_envelope = (double)this.numeric_collision_envelope.Value;
            m_collision_family = this.comboBox_collision_family.SelectedIndex;
        }

        public void Set_collision_on(bool mval)
        {
            m_collide = mval;
            this.checkBox_collide.Checked = m_collide;

            // ghost collision-related stuff if collision not enabled
            this.numeric_friction.Enabled = mval;
            this.numeric_rolling_friction.Enabled = mval;
            this.numeric_spinning_friction.Enabled = mval;
            this.numeric_restitution.Enabled = mval;
            this.numeric_collision_margin.Enabled = mval;
            this.numeric_collision_envelope.Enabled = mval;
            this.comboBox_collision_family.Enabled = mval;
        }

        public void  Set_friction(double mval)
        {
            m_friction = mval;
            this.numeric_friction.Value = (decimal)m_friction;
        }

        public void Set_rolling_friction(double mval)
        {
            m_rolling_friction = mval;
            this.numeric_rolling_friction.Value = (decimal)m_rolling_friction;
        }

        public void Set_spinning_friction(double mval)
        {
            m_spinning_friction = mval;
            this.numeric_spinning_friction.Value = (decimal)m_spinning_friction;
        }

        public void Set_restitution(double mval)
        {
            m_restitution = mval;
            this.numeric_restitution.Value = (decimal)m_restitution;
        }

        public void Set_collision_margin(double mval)
        {
            m_collision_margin = mval;
            this.numeric_collision_margin.Value = (decimal)m_collision_margin;
        }

        public void Set_collision_envelope(double mval)
        {
            m_collision_envelope = mval;
            this.numeric_collision_envelope.Value = (decimal)m_collision_envelope;
        }

        public void Set_collision_family(int mval)
        {
            m_collision_family = mval;
            this.comboBox_collision_family.SelectedIndex = m_collision_family;
        }

        public void Set_conveyor_speed(double mval)
        {
            m_conveyor_speed = mval;
            this.numeric_conveyor_speed.Value = (decimal)m_conveyor_speed;
        }

        public bool UpdateFromSelection(SelectionMgr swSelMgr, ref AttributeDef mdefattr_chbody)//, ref AttributeDef defattr_chconveyor)
        {
            // Fetch current properties from the selected part(s) (i.e. ChBody in C::E)
            for (int isel = 1; isel <= swSelMgr.GetSelectedObjectCount2(-1); isel++)
                if ((swSelectType_e)swSelMgr.GetSelectedObjectType3(isel, -1) == swSelectType_e.swSelCOMPONENTS)
                {
                    Component2 swPart = swSelMgr.GetSelectedObjectsComponent3(isel, -1);
                    if (swPart == null)
                    {
                        System.Windows.Forms.MessageBox.Show("swPart == null");
                        return false;
                    }
                    
                    ModelDoc2 swPartModel = (ModelDoc2)swPart.GetModelDoc2();
                    if (swPartModel == null)
                    {
                        System.Windows.Forms.MessageBox.Show("swPartModel == null");
                        return false;
                    }

                    if (swPartModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) 
                    {
                        if (swPart.Solving == (int)swComponentSolvingOption_e.swComponentFlexibleSolving)
                        {
                            System.Windows.Forms.MessageBox.Show("Fexible assemblies not supported as ChBody (set as Rigid?)");
                            return false;
                        }
                        if (swPart.Solving == (int)swComponentSolvingOption_e.swComponentRigidSolving)
                        {
                            // System.Windows.Forms.MessageBox.Show("Setting props to rigid assembly as ChBody");
                            AssemblyDoc swAssemblyDoc = (AssemblyDoc)swPartModel;
                            swPart.Select(false);
                            swAssemblyDoc.EditAssembly();
                            swAssemblyDoc.EditRebuild();
                            //return;
                        }  
                    }

                    // fetch SW attribute with Chrono parameters for ChBody
                    SolidWorks.Interop.sldworks.Attribute myattr = null;
                    if (swPart != null)
                        myattr = (SolidWorks.Interop.sldworks.Attribute)swPart.FindAttribute(mdefattr_chbody, 0);

                    if (myattr == null)
                    {
                        // if not already added to part, create and attach it
                        //System.Windows.Forms.MessageBox.Show("Create data");
                        myattr = mdefattr_chbody.CreateInstance5(swPartModel, swPart, "Chrono::ChBody_data", 0, (int)swInConfigurationOpts_e.swAllConfiguration);

                        if (myattr == null)
                        {
                            System.Windows.Forms.MessageBox.Show("Probably you selected a part which is one of N instances in assembly. \n Please close window and use this function on another instance for whom you already have set the properties.\n [Sorry for the inconvenience, this issue will be removed in future]");
                            return false;
                        }
                        swPartModel.ForceRebuild3(false); // needed, but does not work...
                        swPartModel.Rebuild((int)swRebuildOptions_e.swRebuildAll); // needed but does not work...

                        if (myattr.GetEntityState((int)swAssociatedEntityStates_e.swIsEntityInvalid))
                            System.Windows.Forms.MessageBox.Show("swIsEntityInvalid!");
                        if (myattr.GetEntityState((int)swAssociatedEntityStates_e.swIsEntitySuppressed))
                            System.Windows.Forms.MessageBox.Show("swIsEntitySuppressed!");
                        if (myattr.GetEntityState((int)swAssociatedEntityStates_e.swIsEntityAmbiguous))
                            System.Windows.Forms.MessageBox.Show("swIsEntityAmbiguous!");
                        if (myattr.GetEntityState((int)swAssociatedEntityStates_e.swIsEntityDeleted))
                            System.Windows.Forms.MessageBox.Show("swIsEntityDeleted!");  
                    }

                    Set_collision_on(Convert.ToBoolean(((Parameter)myattr.GetParameter(
                                        "collision_on")).GetDoubleValue()));
                    Set_friction(((Parameter)myattr.GetParameter(
                                        "friction")).GetDoubleValue());
                    Set_rolling_friction(((Parameter)myattr.GetParameter(
                                        "rolling_friction")).GetDoubleValue());
                    Set_spinning_friction(((Parameter)myattr.GetParameter(
                                        "spinning_friction")).GetDoubleValue());
                    Set_restitution(((Parameter)myattr.GetParameter(
                                        "restitution")).GetDoubleValue());
                    Set_collision_envelope(((Parameter)myattr.GetParameter(
                                        "collision_envelope")).GetDoubleValue());
                    Set_collision_margin(((Parameter)myattr.GetParameter(
                                        "collision_margin")).GetDoubleValue());
                    Set_collision_family((int)((Parameter)myattr.GetParameter(
                                        "collision_family")).GetDoubleValue());
   

                    // fetch SW attribute with Chrono parameters for ChConveyor
                    /*
                    SolidWorks.Interop.sldworks.Attribute myattr_conv = (SolidWorks.Interop.sldworks.Attribute)swPart.FindAttribute(defattr_chconveyor, 0);
                    if (myattr_conv == null)
                    {
                        // if not already added to part, create and attach it
                        //myattr_conv = defattr_chconveyor.CreateInstance5(swPartModel, swPart, "Chrono::ChConveyor_data", 0, (int)swInConfigurationOpts_e.swThisConfiguration);
                    }
                    */
                    /*
                    // fetch SW attribute with Chrono parameters for ChConveyor (if any!)
                    SolidWorks.Interop.sldworks.Attribute myattr_conveyor = (SolidWorks.Interop.sldworks.Attribute)swPart.FindAttribute(defattr_chconveyor, 0);
                    if (myattr_conveyor != null)
                    {
                        show_conveyor_params = true;

                        Set_conveyor_speed(((Parameter)myattr_conveyor.GetParameter(
                                        "conveyor_speed")).GetDoubleValue());
                    }
                    */
                }
            return true;
        }


        public void StoreToSelection(SelectionMgr swSelMgr, ref AttributeDef mdefattr_chbody)//, ref AttributeDef defattr_chconveyor)
        {
            //System.Windows.Forms.MessageBox.Show("StoreToSelection()");

            // If user pressed OK, apply settings to all selected parts (i.e. ChBody in C::E):
            for (int isel = 1; isel <= swSelMgr.GetSelectedObjectCount2(-1); isel++)
                if ((swSelectType_e)swSelMgr.GetSelectedObjectType3(isel, -1) == swSelectType_e.swSelCOMPONENTS)
                {
                    //Component2 swPart = (Component2)swSelMgr.GetSelectedObject6(isel, -1);
                    Component2 swPart = swSelMgr.GetSelectedObjectsComponent3(isel, -1);
                    ModelDoc2  swPartModel = (ModelDoc2)swPart.GetModelDoc2();
                      //Component2 swPartcorr = swPart;
                      //Component2 swPartcorr = swPartModel.Extension.GetCorresponding(swPart);// ***TODO*** for instanced parts? does not work...

                    // fetch SW attribute with Chrono parameters for ChBody
                    SolidWorks.Interop.sldworks.Attribute myattr = (SolidWorks.Interop.sldworks.Attribute)swPart.FindAttribute(mdefattr_chbody, 0);
                    if (myattr == null)
                    {
                        // if not already added to part, create and attach it
                        System.Windows.Forms.MessageBox.Show("[store settings failed - should not happen]");
                        return;
                    }

                    ((Parameter)myattr.GetParameter("collision_on")).SetDoubleValue2(
                                  Convert.ToDouble(m_collide), (int)swInConfigurationOpts_e.swThisConfiguration, "");

                    ((Parameter)myattr.GetParameter("friction")).SetDoubleValue2(
                                  m_friction, (int)swInConfigurationOpts_e.swThisConfiguration, "");

                    ((Parameter)myattr.GetParameter("rolling_friction")).SetDoubleValue2(
                                  m_rolling_friction, (int)swInConfigurationOpts_e.swThisConfiguration, "");

                    ((Parameter)myattr.GetParameter("spinning_friction")).SetDoubleValue2(
                                  m_spinning_friction, (int)swInConfigurationOpts_e.swThisConfiguration, "");

                    ((Parameter)myattr.GetParameter("restitution")).SetDoubleValue2(
                                  m_restitution, (int)swInConfigurationOpts_e.swThisConfiguration, "");

                    ((Parameter)myattr.GetParameter("collision_margin")).SetDoubleValue2(
                                  m_collision_margin, (int)swInConfigurationOpts_e.swThisConfiguration, "");

                    ((Parameter)myattr.GetParameter("collision_envelope")).SetDoubleValue2(
                                  m_collision_envelope, (int)swInConfigurationOpts_e.swThisConfiguration, "");

                    ((Parameter)myattr.GetParameter("collision_family")).SetDoubleValue2(
                                  (double)m_collision_family, (int)swInConfigurationOpts_e.swThisConfiguration, "");
                    /*
                    // fetch SW attribute with Chrono parameters for ChConveyor
                    SolidWorks.Interop.sldworks.Attribute myattr_conveyor = (SolidWorks.Interop.sldworks.Attribute)swPart.FindAttribute(defattr_chconveyor, 0);
                    if (myattr_conveyor == null)
                    {
                        // if not already added to part, create and attach it
                        myattr_conveyor = defattr_chconveyor.CreateInstance5(swPartModel, swPart, "Chrono ChConveyor data", 0, (int)swInConfigurationOpts_e.swThisConfiguration);
                        if (myattr_conveyor == null) 
                            System.Windows.Forms.MessageBox.Show("myattr null in setting!!");
                    }

                    ((Parameter)myattr_conveyor.GetParameter("conveyor_speed")).SetDoubleValue2(
                                  m_conveyor_speed, (int)swInConfigurationOpts_e.swThisConfiguration, "");
                    */
                    swPartModel.ForceRebuild3(false);
                } 
        }



    }
}
