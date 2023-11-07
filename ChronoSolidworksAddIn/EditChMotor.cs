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
                if ((SolidWorks.Interop.sldworks.Attribute)((Entity)swFeat).FindAttribute(m_SWintegration.defattr_chlink, 0) != null)
                {
                    SolidWorks.Interop.sldworks.Attribute motorAttribute = (SolidWorks.Interop.sldworks.Attribute)((Entity)swFeat).FindAttribute(m_SWintegration.defattr_chlink, 0);

                    string motorName = ((Parameter)motorAttribute.GetParameter("motor_name")).GetStringValue();
                    string motorType = ((Parameter)motorAttribute.GetParameter("motor_type")).GetStringValue();
                    string motorMotionlaw = ((Parameter)motorAttribute.GetParameter("motor_motionlaw")).GetStringValue();
                    string motorConstraints = ((Parameter)motorAttribute.GetParameter("motor_constraints")).GetStringValue();
                    string motorMarker = ((Parameter)motorAttribute.GetParameter("motor_marker")).GetStringValue();
                    string motorBody1 = ((Parameter)motorAttribute.GetParameter("motor_body1")).GetStringValue();
                    string motorBody2 = ((Parameter)motorAttribute.GetParameter("motor_body2")).GetStringValue();

                    ModelDoc2 swModel = (ModelDoc2)m_SWintegration.mSWApplication.ActiveDoc;
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
                    System.Windows.Forms.MessageBox.Show("Error in part selection.");
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
                    System.Windows.Forms.MessageBox.Show("Error in part selection.");
                }
            }
        }

        private void butt_createMotor_Click(object sender, EventArgs e)
        {
            ModelDoc2 swModel = (ModelDoc2)m_SWintegration.mSWApplication.ActiveDoc;

            byte[] motorMarkerRef = (byte[])swModel.Extension.GetPersistReference3(m_selectedMarker);
            byte[] motorBody1Ref = (byte[])swModel.Extension.GetPersistReference3(m_selectedBody1);
            byte[] motorBody2Ref = (byte[])swModel.Extension.GetPersistReference3(m_selectedBody2);

            string motorName        = txt_motorName.Text;
            string motorType        = cb_motorType.SelectedItem.ToString();
            string motorMotionlaw   = cb_motionLaw.SelectedItem.ToString();
            string motorConstraint  = chb_motorConstraint.Checked.ToString();
            string motorMarker      = GetStringFromID(swModel, motorMarkerRef);
            string motorBody1       = GetStringFromID(swModel, motorBody1Ref);
            string motorBody2       = GetStringFromID(swModel, motorBody2Ref);

            // If selected marker has no attributes, create them; otherwise, overwrite
            SolidWorks.Interop.sldworks.Attribute motorAttribute;
            if ((SolidWorks.Interop.sldworks.Attribute)((Entity)m_selectedMarker).FindAttribute(m_SWintegration.defattr_chlink, 0) == null)
            {
                motorAttribute = m_SWintegration.defattr_chlink.CreateInstance5(swModel, m_selectedMarker, "chrono_motor_data", 0, (int)swInConfigurationOpts_e.swAllConfiguration);
            }
            else
            { 
                motorAttribute = (SolidWorks.Interop.sldworks.Attribute)((Entity)m_selectedMarker).FindAttribute(m_SWintegration.defattr_chlink, 0); 
            }

            ((Parameter)motorAttribute.GetParameter("motor_name")).SetStringValue(motorName);
            ((Parameter)motorAttribute.GetParameter("motor_type")).SetStringValue(motorType);
            ((Parameter)motorAttribute.GetParameter("motor_motionlaw")).SetStringValue(motorMotionlaw);
            ((Parameter)motorAttribute.GetParameter("motor_constraints")).SetStringValue(motorConstraint);
            ((Parameter)motorAttribute.GetParameter("motor_marker")).SetStringValue(motorMarker);
            ((Parameter)motorAttribute.GetParameter("motor_body1")).SetStringValue(motorBody1);
            ((Parameter)motorAttribute.GetParameter("motor_body2")).SetStringValue(motorBody2);

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


    } // end form
} // end namespace
