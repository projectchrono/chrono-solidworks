using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swcommands;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using SolidWorksTools;
/*
using SldWorks;
using SwCommands;
using SwConst;
using SWPublished;
*/

namespace ChronoEngine_SwAddin
{

[ComVisible(true)]
public class SWIntegration : ISwAddin
{
    public SldWorks mSWApplication;
    private int mSWCookie;
    private TaskpaneView mTaskpaneView;
    private SWTaskpaneHost mTaskpaneHost;
    
    public AttributeDef defattr_chbody = default(AttributeDef);
    //public AttributeDef defattr_chconveyor = default(AttributeDef);
    public AttributeDef defattr_test = default(AttributeDef);
    public AttributeDef defattr_chlink = default(AttributeDef);

    public bool ConnectToSW(object ThisSW, int Cookie)
    {
        //System.Windows.Forms.MessageBox.Show("Add-in: ConnectToSW");
        try
        {
            mSWApplication = (SldWorks)ThisSW;
            mSWCookie = Cookie;
            bool result = mSWApplication.SetAddinCallbackInfo(0, this, Cookie);

            // Attributes register          

            defattr_chbody = (AttributeDef)mSWApplication.DefineAttribute("chrono_ChBody");
            defattr_chbody.AddParameter("friction",             (int)swParamType_e.swParamTypeDouble, 0.6, 0);
            defattr_chbody.AddParameter("rolling_friction",     (int)swParamType_e.swParamTypeDouble, 0, 0);
            defattr_chbody.AddParameter("spinning_friction",    (int)swParamType_e.swParamTypeDouble, 0, 0);
            defattr_chbody.AddParameter("restitution",          (int)swParamType_e.swParamTypeDouble, 0, 0);
            defattr_chbody.AddParameter("collision_on",         (int)swParamType_e.swParamTypeDouble, 1, 0);
            defattr_chbody.AddParameter("collision_margin",     (int)swParamType_e.swParamTypeDouble, 0.01, 0);
            defattr_chbody.AddParameter("collision_envelope",   (int)swParamType_e.swParamTypeDouble, 0.03, 0);
            defattr_chbody.AddParameter("collision_family",     (int)swParamType_e.swParamTypeDouble, 0, 0);
            defattr_chbody.Register();

           
            defattr_chlink = (AttributeDef)mSWApplication.DefineAttribute("chrono_ChLink");
            defattr_chlink.AddParameter("motor_name", (int)swParamType_e.swParamTypeString, 0.0, 0);
            defattr_chlink.AddParameter("motor_type", (int)swParamType_e.swParamTypeString, 0.0, 0);
            defattr_chlink.AddParameter("motor_motionlaw", (int)swParamType_e.swParamTypeString, 0.0, 0);
            defattr_chlink.AddParameter("motor_constraints", (int)swParamType_e.swParamTypeString, 0.0, 0);
            defattr_chlink.AddParameter("motor_marker", (int)swParamType_e.swParamTypeString, 0.0, 0);
            defattr_chlink.AddParameter("motor_body1", (int)swParamType_e.swParamTypeString, 0.0, 0); // slave
            defattr_chlink.AddParameter("motor_body2", (int)swParamType_e.swParamTypeString, 0.0, 0); // master
            defattr_chlink.Register();

            // Register the taskpane
            this.UISetup();

            // Event register: here is an example of how to do...
            SldWorks moSWApplication = (SldWorks)mSWApplication;
            moSWApplication.ActiveDocChangeNotify += new DSldWorksEvents_ActiveDocChangeNotifyEventHandler(test_event_ActiveDocChangeNotify);
            moSWApplication.ActiveModelDocChangeNotify += new DSldWorksEvents_ActiveModelDocChangeNotifyEventHandler(test_event_ActiveModelDocChangeNotify);
        }
        catch (Exception ex)
        {
            System.Windows.Forms.MessageBox.Show("ConnectToSW failed! " + ex.Message);
        }

        return true;
 
    }

    public bool DisconnectFromSW()
    {
        this.UITeardown();
        return true;
    }
    private void UISetup()
    {

        string addinPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string[] imagePaths = new string[6]
        {
            addinPath+"/icons/ChronoEngineAddIn_20.png",
            addinPath+"/icons/ChronoEngineAddIn_32.png",
            addinPath+"/icons/ChronoEngineAddIn_40.png",
            addinPath+"/icons/ChronoEngineAddIn_64.png",
            addinPath+"/icons/ChronoEngineAddIn_96.png",
            addinPath+"/icons/ChronoEngineAddIn_128.png"
        };

        mTaskpaneView = mSWApplication.CreateTaskpaneView3(imagePaths, "Chrono::Engine tools");
        mTaskpaneHost = (SWTaskpaneHost)mTaskpaneView.AddControl(SWTaskpaneHost.SWTASKPANE_PROGID,"");
        mTaskpaneHost.mSWApplication = this.mSWApplication;
        mTaskpaneHost.mSWintegration = this;
    }
    private void UITeardown()
    {
        mTaskpaneHost = null;
        mTaskpaneView.DeleteView();
        Marshal.ReleaseComObject(mTaskpaneView);
        mTaskpaneView = null;
    }

    [ComRegisterFunction()]
    private static void ComRegister(Type t)
    {
        //System.Windows.Forms.MessageBox.Show("Add-in: ComRegister()");
        string keyPath = String.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);
        using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(keyPath))
        {
            rk.SetValue(null, 1); // Load at startup
            rk.SetValue("Title", "ChronoEngine SwAddin"); // Title
            rk.SetValue("Description", "Add-in for modeling Chrono::Engine system with SolidWorks"); // Description
        }
    }

    [ComUnregisterFunction()]
    private static void ComUnregister(Type t)
    {
        string keyPath = String.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);
        Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(keyPath);
    }


    int test_event_ActiveDocChangeNotify()
    {
        //System.Windows.Forms.MessageBox.Show("ActiveDocChangeNotify");
        return 0;
    }
    int test_event_ActiveModelDocChangeNotify()
    {
        //System.Windows.Forms.MessageBox.Show("ActiveDocChangeNotify");
        return 0;
    }


    
}


} // end namespace