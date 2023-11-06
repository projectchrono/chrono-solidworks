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
    public partial class EditChLink : Form
    {
        //public ISldWorks m_SWApplication;
        SelectionMgr m_swSelMgr;
        ChronoEngine_SwAddin.SWIntegration m_SWintegration;
        //public bool m_isMotor;

        //string m_motorMarkerName;
        //string m_motorBody1Name;
        //string m_motorBody2Name;
        SolidWorks.Interop.sldworks.Feature m_selectedMarker;
        SolidWorks.Interop.sldworks.Component2 m_selectedBody1;
        SolidWorks.Interop.sldworks.Component2 m_selectedBody2;


        ////////////////////////////////////////////////////////////////////////


        public EditChLink(ref SelectionMgr swSelMgr, ref ChronoEngine_SwAddin.SWIntegration SWintegration)
        {
            InitializeComponent();
            m_swSelMgr = swSelMgr;
            m_SWintegration = SWintegration;
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


                    lst_bodiesSelected.Items.Add(feat.Name);



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
                }
            }
        }

        public void StoreToSelection(ref SelectionMgr swSelMgr, ref ChronoEngine_SwAddin.SWIntegration SWintegration) // ref AttributeDef mdefattr_chlink
        {
            int isel = 1; // start from 1 not from 0!
            Feature feat = m_swSelMgr.GetSelectedObject6(isel, -1);

            Component2 swPart = m_swSelMgr.GetSelectedObjectsComponent3(isel, -1);

            SolidWorks.Interop.sldworks.Attribute myattr = null;
            if (swPart != null)
                myattr = (SolidWorks.Interop.sldworks.Attribute)swPart.FindAttribute(SWintegration.defattr_chlink, 0);

            if (myattr == null)
            {

                //////////////////////////    
                //TODO
                //////////////////////////

                ModelDoc2 swPartModel = (ModelDoc2)swPart.GetModelDoc2();

                // if not already added to part, create and attach it
                myattr = SWintegration.defattr_chlink.CreateInstance5(swPartModel, swPart, "Chrono::ChBody_data", 0, (int)swInConfigurationOpts_e.swAllConfiguration);

                if (myattr == null)
                {
                    System.Windows.Forms.MessageBox.Show("Error in setting link attribute.");
                }
                //swPartModel.ForceRebuild3(false); // needed, but does not work...
                //swPartModel.Rebuild((int)swRebuildOptions_e.swRebuildAll); // needed but does not work...


                //Set_collision_on(Convert.ToBoolean(((Parameter)myattr.GetParameter("collision_on")).GetDoubleValue()));
            }
        }

        private void butt_addMarker_Click(object sender, EventArgs e)
        {
            if ((swSelectType_e)m_swSelMgr.GetSelectedObjectType3(1, -1) == swSelectType_e.swSelCOORDSYS)
            {
                Feature feat = m_swSelMgr.GetSelectedObject6(1, -1);
                m_selectedMarker = feat;
                lst_markerSelected.Items.Add(feat.Name);
            }
        }

        private void butt_addBody_Click(object sender, EventArgs e)
        {
            for (int isel = 1; isel <= m_swSelMgr.GetSelectedObjectCount2(-1); isel++)
            {
                if ((swSelectType_e)m_swSelMgr.GetSelectedObjectType3(isel, -1) == swSelectType_e.swSelCOMPONENTS)
                {
                    Component2 swPart = m_swSelMgr.GetSelectedObjectsComponent3(isel, -1);
                    if (swPart == null)
                    {
                        System.Windows.Forms.MessageBox.Show("swPart == null");
                        return;
                    }
                    else
                    {
                        if (isel == 1)                                  // WRONG SELECTION: TO FIX (works only if two bodies selected together) -> replace with two selection boxes or simil
                        {                                               // WRONG SELECTION: TO FIX (works only if two bodies selected together) -> replace with two selection boxes or simil
                            m_selectedBody1 = swPart;                   // WRONG SELECTION: TO FIX (works only if two bodies selected together) -> replace with two selection boxes or simil
                            lst_bodiesSelected.Items.Add(swPart.Name);  // WRONG SELECTION: TO FIX (works only if two bodies selected together) -> replace with two selection boxes or simil
                        }                                               // WRONG SELECTION: TO FIX (works only if two bodies selected together) -> replace with two selection boxes or simil
                        else if (isel == 2)                             // WRONG SELECTION: TO FIX (works only if two bodies selected together) -> replace with two selection boxes or simil
                        {                                               // WRONG SELECTION: TO FIX (works only if two bodies selected together) -> replace with two selection boxes or simil
                            m_selectedBody2 = swPart;                   // WRONG SELECTION: TO FIX (works only if two bodies selected together) -> replace with two selection boxes or simil
                            lst_bodiesSelected.Items.Add(swPart.Name);  // WRONG SELECTION: TO FIX (works only if two bodies selected together) -> replace with two selection boxes or simil
                        }
                    }
                }
            }
        }

        //public void SetMotorBody1Name(string bodyName)
        //{
        //    m_motorBody1Name = bodyName;
        //    //this.numeric_friction.Value = (decimal)m_friction;
        //}

        //public void SetMotorBody2Name(string bodyName)
        //{
        //    m_motorBody2Name = bodyName;
        //}

        //public void SetMotorMarkerName(string markerName)
        //{
        //    m_motorMarkerName = markerName;
        //}

        private void butt_createMotor_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("enum: " + (swSelectType_e)m_swSelMgr.GetSelectedObjectType3(1, -1));

            ModelDoc2 swModel = (ModelDoc2)m_SWintegration.mSWApplication.ActiveDoc;

            byte[] motorMarkerRef = (byte[])swModel.Extension.GetPersistReference3(m_selectedMarker);
            byte[] motorBody1Ref = (byte[])swModel.Extension.GetPersistReference3(m_selectedBody1);
            byte[] motorBody2Ref = (byte[])swModel.Extension.GetPersistReference3(m_selectedBody2);

            string motorMarker      = GetStringFromID(swModel, motorMarkerRef);
            string motorBody1       = GetStringFromID(swModel, motorBody1Ref);
            string motorBody2       = GetStringFromID(swModel, motorBody2Ref);
            string motorType        = cb_motorType.SelectedItem.ToString();
            string motorControl     = cb_motorControl.SelectedItem.ToString();
            string motorMotionlaw   = cb_motionLaw.SelectedItem.ToString();

            //string selectedMarkerString = GetStringFromID(swModel, selectedMarkerRef);
            //string selectedBodyString1 = GetStringFromID(swModel, selectedBodyRef1);
            //string selectedBodyString2 = GetStringFromID(swModel, selectedBodyRef2);



            SolidWorks.Interop.sldworks.Attribute motorAttribute;
            motorAttribute = m_SWintegration.defattr_chlink.CreateInstance5(swModel, m_selectedMarker, "chrono_motor_data", 0, (int)swInConfigurationOpts_e.swAllConfiguration);

            ((Parameter)motorAttribute.GetParameter("motor_marker")).SetStringValue(motorMarker);
            ((Parameter)motorAttribute.GetParameter("motor_body1")).SetStringValue(motorBody1);
            ((Parameter)motorAttribute.GetParameter("motor_body2")).SetStringValue(motorBody2);
            ((Parameter)motorAttribute.GetParameter("motor_type")).SetStringValue(motorType);
            ((Parameter)motorAttribute.GetParameter("motor_control")).SetStringValue(motorControl);
            ((Parameter)motorAttribute.GetParameter("motor_motionlaw")).SetStringValue(motorMotionlaw);

            swModel.ForceRebuild3(false); // needed, but does not work...
            swModel.Rebuild((int)swRebuildOptions_e.swRebuildAll); // needed but does not work...

            string debugString = "";
            debugString += "WRITING ATTRIBUTES\n";
            debugString += "motor_marker: " + motorMarker + "\n";
            debugString += "motor_body1: " + motorBody1 + "\n";
            debugString += "motor_body2: " + motorBody2 + "\n";
            debugString += "motor_type: " + motorType + "\n";
            debugString += "motor_control: " + motorControl + "\n";
            debugString += "motor_motionlaw: " + motorMotionlaw + "\n";
            System.Windows.Forms.MessageBox.Show(debugString);
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
