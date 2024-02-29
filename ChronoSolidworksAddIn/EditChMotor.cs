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
using ChronoEngine_SwAddin;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


//using ChronoEngineAddIn;

namespace ChronoEngineAddin
{
    public partial class EditChMotor : Form
    {
        //public ISldWorks m_SWApplication;
        SelectionMgr m_swSelMgr;
        ChronoEngine_SwAddin.SWIntegration m_SWintegration;

        SolidWorks.Interop.sldworks.Feature m_selectedMarker;
        SolidWorks.Interop.sldworks.Component2 m_selectedBody1;
        SolidWorks.Interop.sldworks.Component2 m_selectedBody2;


        ////////////////////////////////////////////////////////////////////////

        public EditChMotor(ref SelectionMgr swSelMgr, ref ChronoEngine_SwAddin.SWIntegration SWintegration)
        {
            InitializeComponent();
            m_swSelMgr = swSelMgr;
            m_SWintegration = SWintegration;
        }

        private void butt_addMarker_Click(object sender, EventArgs e)
        {
            if ((swSelectType_e)m_swSelMgr.GetSelectedObjectType3(1, -1) == swSelectType_e.swSelCOORDSYS)
            {
                Feature swFeat = m_swSelMgr.GetSelectedObject6(1, -1);
                m_selectedMarker = swFeat;
                txt_markerSelected.Text = swFeat.Name;

                // If attributes are already present in selected CoordSys, populate Form items
                if ((SolidWorks.Interop.sldworks.Attribute)((Entity)swFeat).FindAttribute(m_SWintegration.defattr_chmotor, 0) != null)
                {
                    SolidWorks.Interop.sldworks.Attribute motorAttribute = (SolidWorks.Interop.sldworks.Attribute)((Entity)swFeat).FindAttribute(m_SWintegration.defattr_chmotor, 0);

                    string motorName = ((Parameter)motorAttribute.GetParameter("motor_name")).GetStringValue();
                    string motorType = ((Parameter)motorAttribute.GetParameter("motor_type")).GetStringValue();
                    string motorMotionlaw = ((Parameter)motorAttribute.GetParameter("motor_motionlaw")).GetStringValue();
                    string motorConstraints = ((Parameter)motorAttribute.GetParameter("motor_constraints")).GetStringValue();
                    string motorMarker = ((Parameter)motorAttribute.GetParameter("motor_marker")).GetStringValue();
                    string motorBody1 = ((Parameter)motorAttribute.GetParameter("motor_body1")).GetStringValue();
                    string motorBody2 = ((Parameter)motorAttribute.GetParameter("motor_body2")).GetStringValue();
                    string motlawInputs = ((Parameter)motorAttribute.GetParameter("motor_motlaw_inputs")).GetStringValue();

                    ModelDoc2 swModel = (ModelDoc2)m_SWintegration.m_swApplication.ActiveDoc;
                    byte[] selBody1Ref = (byte[])GetIDFromString(swModel, motorBody1);
                    byte[] selBody2Ref = (byte[])GetIDFromString(swModel, motorBody2);

                    SolidWorks.Interop.sldworks.Component2 selectedBody1 = (Component2)GetObjectFromID(swModel, selBody1Ref);
                    SolidWorks.Interop.sldworks.Component2 selectedBody2 = (Component2)GetObjectFromID(swModel, selBody2Ref);

                    m_selectedBody1 = selectedBody1;
                    m_selectedBody2 = selectedBody2;

                    txt_motorName.Text = motorName;
                    cb_motorType.Text = motorType;
                    cb_motionLaw.Text = motorMotionlaw;
                    if (motorConstraints == "True")
                        chb_motorConstraint.CheckState = CheckState.Checked;
                    else
                        chb_motorConstraint.CheckState = CheckState.Unchecked;
                    txt_markerSelected.Text = swFeat.Name;
                    txt_bodySlaveSelected.Text = selectedBody1.Name;
                    txt_bodyMasterSelected.Text = selectedBody2.Name;
                    txt_motlawInputs.Text = motlawInputs;
                }
            }
        }

        private void butt_bodySlaveSelected_Click(object sender, EventArgs e)
        {
            if ((swSelectType_e)m_swSelMgr.GetSelectedObjectType3(1, -1) == swSelectType_e.swSelCOMPONENTS)
            {
                Component2 swPart = m_swSelMgr.GetSelectedObjectsComponent3(1, -1);
                if (swPart != null)
                {
                    m_selectedBody1 = swPart;
                    txt_bodySlaveSelected.Text = swPart.Name;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Error in Part selection.");
                }
            }
        }

        private void butt_addBodyMaster_Click(object sender, EventArgs e)
        {
            if ((swSelectType_e)m_swSelMgr.GetSelectedObjectType3(1, -1) == swSelectType_e.swSelCOMPONENTS)
            {
                Component2 swPart = m_swSelMgr.GetSelectedObjectsComponent3(1, -1);
                if (swPart != null)
                {
                    m_selectedBody2 = swPart;
                    txt_bodyMasterSelected.Text = swPart.Name;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Error in Part selection.");
                }
            }
        }

        private bool checkMotlawInputsSanity()
        {
            bool isInputSane = false;
            string motorMotionlaw = cb_motionLaw.SelectedItem.ToString();
            string motlawInputs = txt_motlawInputs.Text;

            // If empty inputs, acceptable
            if (String.IsNullOrEmpty(motlawInputs))
            {
                return true; 
            }

            // Try to parse inputs string into numeric array: if error, wrong inputs for sure
            double[] numericInputs;
            try
            {
                numericInputs = motlawInputs.Split(',').Select(r => Convert.ToDouble(r)).ToArray();
            }
            catch
            {
                MessageBox.Show("Given motion law inputs are not numeric. Motor not created.");
                return false;
            }

            // Check if input number is appropriate for given motion law
            switch (motorMotionlaw)
            {
                case "Const":
                    isInputSane = (numericInputs.Length == 1) ? true : false;
                    break;
                case "ConstAcc":
                    isInputSane = (numericInputs.Length == 4) ? true : false;
                    break;
                case "Cycloidal":
                    isInputSane = (numericInputs.Length == 2) ? true : false;
                    break;
                case "DoubleS":
                    isInputSane = (numericInputs.Length == 7) ? true : false;
                    break;
                case "Poly345":
                    isInputSane = (numericInputs.Length == 2) ? true : false;
                    break;
                case "ChFunction_Setpoint":
                    isInputSane = (numericInputs.Length == 2) ? true : false;
                    break;
                case "Sine":
                    isInputSane = (numericInputs.Length == 3) ? true : false;
                    break;
                default:
                    break;
            }

            if (!isInputSane)
            {
                string msg = "Wrong number of motion law inputs:\n"
                    + $"selected {motorMotionlaw} with {numericInputs.Length} inputs.\n\n"
                    + "Motor not created.";
                MessageBox.Show(msg);
            }

            return isInputSane;
        }

        private void butt_createMotor_Click(object sender, EventArgs e)
        {

            if (!checkMotlawInputsSanity()) // proceed only if given motion law inputs are appropriate
            {
                MessageBox.Show("Motor not created: inputs for motion law are invalid.");
                return;
            }

            ModelDoc2 swModel = (ModelDoc2)m_SWintegration.m_swApplication.ActiveDoc;

            byte[] motorMarkerRef = (byte[])swModel.Extension.GetPersistReference3(m_selectedMarker);
            byte[] motorBody1Ref = (byte[])swModel.Extension.GetPersistReference3(m_selectedBody1);

            string motorName        = txt_motorName.Text;
            string motorType        = cb_motorType.SelectedItem.ToString();
            string motorMotionlaw   = cb_motionLaw.SelectedItem.ToString();
            string motorConstraint  = chb_motorConstraint.Checked.ToString();
            string motlawInputs     = txt_motlawInputs.Text;
            string motorMarker      = GetStringFromID(swModel, motorMarkerRef);
            string motorBody1       = GetStringFromID(swModel, motorBody1Ref);

            string motorBody2;
            if (cbMasterGround.Checked)
            {
                motorBody2 = "ground";
            }
            else
            {
                byte[] motorBody2Ref = (byte[])swModel.Extension.GetPersistReference3(m_selectedBody2);
                motorBody2 = GetStringFromID(swModel, motorBody2Ref);
            }

            // If selected marker has no attributes, create them; otherwise, overwrite
            SolidWorks.Interop.sldworks.Attribute motorAttribute;
            if ((SolidWorks.Interop.sldworks.Attribute)((Entity)m_selectedMarker).FindAttribute(m_SWintegration.defattr_chmotor, 0) == null)
            {
                motorAttribute = m_SWintegration.defattr_chmotor.CreateInstance5(swModel, m_selectedMarker, "chrono_motor_data", 0, (int)swInConfigurationOpts_e.swAllConfiguration);
            }
            else
            {
                motorAttribute = (SolidWorks.Interop.sldworks.Attribute)((Entity)m_selectedMarker).FindAttribute(m_SWintegration.defattr_chmotor, 0);
            }

            ((Parameter)motorAttribute.GetParameter("motor_name")).SetStringValue(motorName);
            ((Parameter)motorAttribute.GetParameter("motor_type")).SetStringValue(motorType);
            ((Parameter)motorAttribute.GetParameter("motor_motionlaw")).SetStringValue(motorMotionlaw);
            ((Parameter)motorAttribute.GetParameter("motor_constraints")).SetStringValue(motorConstraint);
            ((Parameter)motorAttribute.GetParameter("motor_marker")).SetStringValue(motorMarker);
            ((Parameter)motorAttribute.GetParameter("motor_body1")).SetStringValue(motorBody1);
            ((Parameter)motorAttribute.GetParameter("motor_body2")).SetStringValue(motorBody2);
            ((Parameter)motorAttribute.GetParameter("motor_motlaw_inputs")).SetStringValue(motlawInputs);

            swModel.ForceRebuild3(false);
            swModel.Rebuild((int)swRebuildOptions_e.swRebuildAll);
        }

        public static string GetStringFromID(ModelDoc2 swModel, byte[] vPIDarr)
        {
            string functionReturnValue = null;

            foreach (int vPID in vPIDarr)
            {
                functionReturnValue = functionReturnValue + vPID.ToString("###000");
            }
            return functionReturnValue;

        }

        public static object GetIDFromString(ModelDoc2 swModel, string IDstring)
        {
            ModelDocExtension swModExt = default(ModelDocExtension);
            byte[] ByteStream = new byte[IDstring.Length / 3];
            object vPIDarr = null;

            swModExt = swModel.Extension;
            for (int i = 0; i <= IDstring.Length - 3; i += 3)
            {
                int j;
                j = i / 3;
                ByteStream[j] = Convert.ToByte(IDstring.Substring(i, 3));
            }

            vPIDarr = ByteStream;

            return vPIDarr; // TODO, why passing through an object instead of keeping byte[]?

            //object functionReturnValue = swModExt.GetObjectByPersistReference3((vPIDarr), out nRetval);

            //Debug.Assert((int)swPersistReferencedObjectStates_e.swPersistReferencedObject_Ok == nRetval);
            //Debug.Assert((functionReturnValue != null));
            //return functionReturnValue;

        }

        public static object GetObjectFromID(ModelDoc2 swModel, byte[] vPIDarr)
        {
            int nRetval = 0;
            object functionReturnValue = swModel.Extension.GetObjectByPersistReference3((vPIDarr), out nRetval);

            if (nRetval != (int)swPersistReferencedObjectStates_e.swPersistReferencedObject_Ok)
            {
                System.Windows.Forms.MessageBox.Show("GetObjectFromID failed");
            }

            return functionReturnValue;

        }

        private void cbMasterGround_CheckedChanged(object sender, EventArgs e)
        {
            if (cbMasterGround.Checked)
            {
                txt_bodyMasterSelected.Text = "ground";
                txt_bodyMasterSelected.Enabled = false;
                butt_addBodyMaster.Enabled = false;
            }
            else
            {
                txt_bodyMasterSelected.Text = "";
                txt_bodyMasterSelected.Enabled = true;
                butt_addBodyMaster.Enabled = true;
            }
        }
    } // end form
} // end namespace
