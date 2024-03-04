using System;
using System.Windows.Forms;
using System.Windows.Interop;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


//using ChronoEngineAddIn;

namespace ChronoEngineAddin
{
    public partial class EditChSDA : Form
    {
        //public ISldWorks m_SWApplication;
        SelectionMgr m_swSelMgr;
        ChronoEngine_SwAddin.SWIntegration m_SWintegration;

        Feature m_selectedMarker1;
        Feature m_selectedMarker2;
        Component2 m_selectedBody1;
        Component2 m_selectedBody2;
        int m_ctr = 0; // global counter for inserted attributes


        ////////////////////////////////////////////////////////////////////////

        public EditChSDA(ref SelectionMgr swSelMgr, ref ChronoEngine_SwAddin.SWIntegration SWintegration)
        {
            InitializeComponent();
            m_swSelMgr = swSelMgr;
            m_SWintegration = SWintegration;
        }

        private void butt_selectSlaveMarker_Click(object sender, EventArgs e)
        {
            if ((swSelectType_e)m_swSelMgr.GetSelectedObjectType3(1, -1) == swSelectType_e.swSelCOORDSYS)
            {
                Feature swFeat = m_swSelMgr.GetSelectedObject6(1, -1);
                m_selectedMarker1 = swFeat;
                txt_markerSlaveSelected.Text = swFeat.Name;
            }
        }

        private void but_selectMasterMarker_Click(object sender, EventArgs e)
        {
            if ((swSelectType_e)m_swSelMgr.GetSelectedObjectType3(1, -1) == swSelectType_e.swSelCOORDSYS)
            {
                Feature swFeat = m_swSelMgr.GetSelectedObject6(1, -1);
                m_selectedMarker2 = swFeat;
                txt_markerMasterSelected.Text = swFeat.Name;

                // If attributes are already present in selected CoordSys, populate Form items
                if ((SolidWorks.Interop.sldworks.Attribute)((Entity)swFeat).FindAttribute(m_SWintegration.defattr_chsda, 0) != null)
                {
                    MessageBox.Show("TODO REIMPLEMENT");

                    //SolidWorks.Interop.sldworks.Attribute sdaAttribute = (SolidWorks.Interop.sldworks.Attribute)((Entity)swFeat).FindAttribute(m_SWintegration.defattr_chsda, 0);

                    //string sdaName = ((Parameter)sdaAttribute.GetParameter("sda_name")).GetStringValue();
                    //string sdaType = ((Parameter)sdaAttribute.GetParameter("sda_type")).GetStringValue();
                    //string sdaSpringCoeff = ((Parameter)sdaAttribute.GetParameter("sda_spring_coeff")).GetStringValue();
                    //string sdaDampingCoeff = ((Parameter)sdaAttribute.GetParameter("sda_damping_coeff")).GetStringValue();
                    //string sdaActuatorForce = ((Parameter)sdaAttribute.GetParameter("sda_actuator_force")).GetStringValue();
                    //string sdaRestLength = ((Parameter)sdaAttribute.GetParameter("sda_rest_length")).GetStringValue();
                    //string sdaMarker1 = ((Parameter)sdaAttribute.GetParameter("sda_marker1")).GetStringValue();
                    //string sdaMarker2 = ((Parameter)sdaAttribute.GetParameter("sda_marker2")).GetStringValue();
                    //string sdaBody1 = ((Parameter)sdaAttribute.GetParameter("sda_body1")).GetStringValue();
                    //string sdaBody2 = ((Parameter)sdaAttribute.GetParameter("sda_body2")).GetStringValue();

                    //ModelDoc2 swModel = (ModelDoc2)m_SWintegration.m_swApplication.ActiveDoc;
                    //byte[] selBody1Ref = (byte[])GetIDFromString(swModel, sdaBody1);
                    //byte[] selBody2Ref = (byte[])GetIDFromString(swModel, sdaBody2);
                    //byte[] selMarker1Ref = (byte[])GetIDFromString(swModel, sdaMarker1);

                    //Component2 selectedBody1 = (Component2)GetObjectFromID(swModel, selBody1Ref);
                    //Component2 selectedBody2 = (Component2)GetObjectFromID(swModel, selBody2Ref);
                    //Feature selectedMarker1 = (Feature)GetObjectFromID(swModel, selMarker1Ref);

                    //m_selectedBody1 = selectedBody1;
                    //m_selectedBody2 = selectedBody2;

                    //txt_sdaName.Text = sdaName;
                    //cb_sdaType.Text = sdaType;
                    //txt_springCoeff.Text = sdaSpringCoeff;
                    //txt_dampingCoeff.Text = sdaDampingCoeff;
                    //txt_actuatorForce.Text = sdaActuatorForce;
                    //txt_restLength.Text = sdaRestLength;
                    //txt_markerSlaveSelected.Text = selectedMarker1.Name;
                    //txt_bodySlaveSelected.Text = selectedBody1.Name;
                    //txt_bodyMasterSelected.Text = selectedBody2.Name;
                }
            }
        }

        private void butt_selectSlaveBody_Click(object sender, EventArgs e)
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
                    MessageBox.Show("Error in Part selection.");
                }
            }
        }

        private void butt_selectMasterBody_Click(object sender, EventArgs e)
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
                    MessageBox.Show("Error in Part selection.");
                }
            }
        }

        private void cbMasterGround_CheckedChanged(object sender, EventArgs e)
        {
            // Use default assembly 'ground' body as master
            if (cbMasterGround.Checked)
            {
                txt_bodyMasterSelected.Text = "ground";
                txt_bodyMasterSelected.Enabled = false;
                butt_selectMasterBody.Enabled = false;
            }
            else
            {
                txt_bodyMasterSelected.Text = "";
                txt_bodyMasterSelected.Enabled = true;
                butt_selectMasterBody.Enabled = true;
            }
        }

        private bool checkInputSanity()
        {
            bool isInputSane = false;

            double tmp = 0.0;
            if (Double.TryParse(txt_springCoeff.Text, out tmp) && Double.TryParse(txt_dampingCoeff.Text, out tmp) && Double.TryParse(txt_actuatorForce.Text, out tmp))
            {
                if (Double.TryParse(txt_restLength.Text, out tmp) || txt_restLength.Text == "")
                {
                    isInputSane = true;
                }
            }

            return isInputSane;
        }

        private void butt_createSDA_Click(object sender, EventArgs e)
        {
            if (!checkInputSanity()) // proceed only if numeric input is valid
            {
                MessageBox.Show("SDA not created: numeric input is invalid.");
                return;
            }

            ModelDoc2 swModel = (ModelDoc2)m_SWintegration.m_swApplication.ActiveDoc;

            byte[] sdaMarker1Ref = (byte[])swModel.Extension.GetPersistReference3(m_selectedMarker1);
            byte[] sdaMarker2Ref = (byte[])swModel.Extension.GetPersistReference3(m_selectedMarker2);
            byte[] sdaBody1Ref = (byte[])swModel.Extension.GetPersistReference3(m_selectedBody1);

            string sdaName          = txt_sdaName.Text;
            string sdaType          = cb_sdaType.SelectedItem.ToString();
            string sdaSpringCoeff   = txt_springCoeff.Text;
            string sdaDampingCoeff  = txt_dampingCoeff.Text;
            string sdaActuatorForce = txt_actuatorForce.Text;
            string sdaRestLength    = txt_restLength.Text;
            string sdaMarker1       = GetStringFromID(swModel, sdaMarker1Ref);
            string sdaMarker2       = GetStringFromID(swModel, sdaMarker2Ref);
            string sdaBody1         = GetStringFromID(swModel, sdaBody1Ref);

            string sdaBody2;
            if (cbMasterGround.Checked)
            {
                sdaBody2 = "ground";
            }
            else
            {
                byte[] sdaBody2Ref = (byte[])swModel.Extension.GetPersistReference3(m_selectedBody2);
                sdaBody2 = GetStringFromID(swModel, sdaBody2Ref);
            }

            // If selected marker has no attributes, create them; otherwise, overwrite
            SolidWorks.Interop.sldworks.Attribute sdaAttribute1;
            SolidWorks.Interop.sldworks.Attribute sdaAttribute2;
            //if ((SolidWorks.Interop.sldworks.Attribute)((Entity)m_selectedMarker2).FindAttribute(m_SWintegration.defattr_chsda, 0) == null)
            //{
                sdaAttribute1 = m_SWintegration.defattr_chsda.CreateInstance5(swModel, m_selectedMarker1, sdaName + "_" + m_ctr, 0, (int)swInConfigurationOpts_e.swAllConfiguration);
                ++m_ctr;
                sdaAttribute2 = m_SWintegration.defattr_chsda.CreateInstance5(swModel, m_selectedMarker2, sdaName + "_" + m_ctr, 0, (int)swInConfigurationOpts_e.swAllConfiguration);
                ++m_ctr;
            //}
            //else
            //{
            //    MessageBox.Show("TODO REIMPLEMENT");
            //    sdaAttribute1 = (SolidWorks.Interop.sldworks.Attribute)((Entity)m_selectedMarker1).FindAttribute(m_SWintegration.defattr_chsda, 0);
            //    sdaAttribute2 = (SolidWorks.Interop.sldworks.Attribute)((Entity)m_selectedMarker2).FindAttribute(m_SWintegration.defattr_chsda, 0);
            //}

            ((Parameter)sdaAttribute1.GetParameter("sda_name")).SetStringValue(sdaName);
            ((Parameter)sdaAttribute1.GetParameter("sda_type")).SetStringValue(sdaType);
            ((Parameter)sdaAttribute1.GetParameter("sda_spring_coeff")).SetStringValue(sdaSpringCoeff);
            ((Parameter)sdaAttribute1.GetParameter("sda_damping_coeff")).SetStringValue(sdaDampingCoeff);
            ((Parameter)sdaAttribute1.GetParameter("sda_actuator_force")).SetStringValue(sdaActuatorForce);
            ((Parameter)sdaAttribute1.GetParameter("sda_rest_length")).SetStringValue(sdaRestLength);
            ((Parameter)sdaAttribute1.GetParameter("sda_marker1")).SetStringValue(sdaMarker1);
            ((Parameter)sdaAttribute1.GetParameter("sda_marker2")).SetStringValue(sdaMarker2);
            ((Parameter)sdaAttribute1.GetParameter("sda_body1")).SetStringValue(sdaBody1);
            ((Parameter)sdaAttribute1.GetParameter("sda_body2")).SetStringValue(sdaBody2);

            ((Parameter)sdaAttribute2.GetParameter("sda_name")).SetStringValue(sdaName);
            ((Parameter)sdaAttribute2.GetParameter("sda_type")).SetStringValue(sdaType);
            ((Parameter)sdaAttribute2.GetParameter("sda_spring_coeff")).SetStringValue(sdaSpringCoeff);
            ((Parameter)sdaAttribute2.GetParameter("sda_damping_coeff")).SetStringValue(sdaDampingCoeff);
            ((Parameter)sdaAttribute2.GetParameter("sda_actuator_force")).SetStringValue(sdaActuatorForce);
            ((Parameter)sdaAttribute2.GetParameter("sda_rest_length")).SetStringValue(sdaRestLength);
            ((Parameter)sdaAttribute2.GetParameter("sda_marker1")).SetStringValue(sdaMarker1);
            ((Parameter)sdaAttribute2.GetParameter("sda_marker2")).SetStringValue(sdaMarker2);
            ((Parameter)sdaAttribute2.GetParameter("sda_body1")).SetStringValue(sdaBody1);
            ((Parameter)sdaAttribute2.GetParameter("sda_body2")).SetStringValue(sdaBody2);

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
