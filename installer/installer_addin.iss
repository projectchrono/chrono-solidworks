//#include "ModifyPath.iss"

#define MyAppName "ChronoEngine SW Add-In"
#define MyAppVersion "v9.1"
#define MyAppPublisher "Alessandro Tasora"
#define MyAppURL "http://www.chronoengine.info"

[Setup]
ShowLanguageDialog=yes
UserInfoPage=no
AppCopyright=A.Tasora
AppName={#MyAppName}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultGroupName={#MyAppName}
DefaultDirName={autopf}\ChronoSolidworks
WizardImageFile=SetupModern20.bmp
;WizardSmallImageFile=../to_put_in_app_dir/ChronoEngineAddIn.bmp
PrivilegesRequired=admin
;Compression=none
OutputDir=.
OutputBaseFilename=ChronoEngine_SolidWorks_{#MyAppVersion}
ArchitecturesInstallIn64BitMode=x64

[Files]
Source: ..\chrono-solidworks_install\ChronoEngineAddIn.dll; DestDir: {app};  Flags: "sharedfile uninsnosharedfileprompt";
Source: ..\chrono-solidworks_install\hacd_CLI.dll; DestDir: {app};  Flags: "sharedfile uninsnosharedfileprompt";
Source: ..\chrono-solidworks_install\Newtonsoft.Json.dll; DestDir: {app};  Flags: "sharedfile uninsnosharedfileprompt";
Source: ..\chrono-solidworks_install\*; Excludes: "*\.svn,*.git,*.dll"; DestDir: {app};  Flags: recursesubdirs createallsubdirs;  

[Run]
Filename:"{dotnet40}\RegAsm.exe"; Parameters: /codebase ChronoEngineAddIn.dll; WorkingDir: {app}; StatusMsg: "Registering controls ..."; Flags: runhidden;
 
[UninstallRun]
Filename:"{dotnet40}\RegAsm.exe"; Parameters: /unregister ChronoEngineAddIn.dll; WorkingDir: {app}; StatusMsg: "Unegistering controls ..."; Flags: runhidden; RunOnceId: "ChronoEngineAddInUninstallTag"



[Icons]
Name: "{group}\Getting started"; Filename: "http://www.projectchrono.org"
Name: "{group}\Uninstall"; Filename: "{uninstallexe}"

[Registry]
Root: HKLM; Subkey: Software\ChronoSolidworks; ValueType: string; ValueName: InstallPath; ValueData: {app}

[Code]
var
  mFoundSolidWorks: Integer;
  mPathSolidWorks: String;
  
  

function myFoundSolidWorks: Boolean;
begin
  Result := mFoundSolidWorks =1;
end;
function myGetPathSolidWorks(Param: String): String;
begin
  Result := mPathSolidWorks;
end;


procedure InitializeWizard;
var
  mTitleSWdir: String;
  mallDirSolidWorks: String;
  mCut: Integer;
  myvers: String;
begin


  
  // CHECK PYTHON INSTALLATION
  mFoundSolidWorks := 0;

  // ***NOTE**** the following code (that detects the directory where SW is 
  // installed) is NOT necessary anymore because in latest revisions this
  // add-in installer does not need such information (it just registers the 
  // ChronoEngineAddIn.dll in the COM system)

  //***BEGIN OF NOT USED CODE*** Maybe to remove in future. Just for reference. 

  if (IsWin64) then
  begin
    // CASE OF 64 BIT PLATFORM



    // find 64 bit SW v.20:
    if RegQueryStringValue(HKEY_LOCAL_MACHINE,
                      'SOFTWARE\SolidWorks\SolidWorks 2020\Setup',
                      'SolidWorks Folder',
                      mallDirSolidWorks) then
    begin
            mPathSolidWorks := mallDirSolidWorks; 
            mFoundSolidWorks := 1;
    end;

    // find 64 bit SW v.21:
    if RegQueryStringValue(HKEY_LOCAL_MACHINE,
                      'SOFTWARE\SolidWorks\SolidWorks 2021\Setup',
                      'SolidWorks Folder',
                      mallDirSolidWorks) then
    begin
            mPathSolidWorks := mallDirSolidWorks; 
            mFoundSolidWorks := 1;
    end;

    // find 64 bit SW v.22:
    if RegQueryStringValue(HKEY_LOCAL_MACHINE,
                      'SOFTWARE\SolidWorks\SolidWorks 2022\Setup',
                      'SolidWorks Folder',
                      mallDirSolidWorks) then
    begin
            mPathSolidWorks := mallDirSolidWorks; 
            mFoundSolidWorks := 1;
    end;

    // find 64 bit SW v.23:
    if RegQueryStringValue(HKEY_LOCAL_MACHINE,
                      'SOFTWARE\SolidWorks\SolidWorks 2023\Setup',
                      'SolidWorks Folder',
                      mallDirSolidWorks) then
    begin
            mPathSolidWorks := mallDirSolidWorks; 
            mFoundSolidWorks := 1;
    end;

    // find 64 bit SW v.24:
    if RegQueryStringValue(HKEY_LOCAL_MACHINE,
                      'SOFTWARE\SolidWorks\SolidWorks 2024\Setup',
                      'SolidWorks Folder',
                      mallDirSolidWorks) then
    begin
            mPathSolidWorks := mallDirSolidWorks; 
            mFoundSolidWorks := 1;
    end;

    // find 64 bit SW v.25:
    if RegQueryStringValue(HKEY_LOCAL_MACHINE,
                      'SOFTWARE\SolidWorks\SolidWorks 2025\Setup',
                      'SolidWorks Folder',
                      mallDirSolidWorks) then
    begin
            mPathSolidWorks := mallDirSolidWorks; 
            mFoundSolidWorks := 1;
    end;
                
  end 
  else
  begin
    // CASE OF 32 BIT PLATFORM
          mFoundSolidWorks := 0;
  end;


  if mFoundSolidWorks = 0 then 
  begin 
           //MsgBox('WARNING!'#13#13+
           //    'The installer was not able to detect SolidWorks 64 bit'+
           //    'on your system.'#13#13+
           //    'Maybe your SolidWorks is not yet installed, or not properly installed?'#13+
           //    '(If so,please install SolidWorks 64 bit before installing this plug-in).', mbError, MB_OK);
           //
           //Abort();
  end;

  //***END OF NOT USED CODE***                               

end;


function NextButtonClick(CurPageID: Integer): Boolean;
var
  I: Integer;
  mfilesize: Integer;
begin

      Result := True;
end;


function ShouldSkipPage(PageID: Integer): Boolean;
begin
  Result := False;
  
  { Skip pages that shouldn't be shown }

  //if (PageID = wpSelectDir) then
  //  Result := True
end;


function UpdateReadyMemo(Space, NewLine, MemoUserInfoInfo, MemoDirInfo, MemoTypeInfo,
  MemoComponentsInfo, MemoGroupInfo, MemoTasksInfo: String): String;
var
  S: String;
begin
  { Fill the 'Ready Memo' with the normal settings and the custom settings }
  S :=     'Assuming NET 4.0 framework is installed.' + NewLine + NewLine;
  S := S + 'After the installation do this: launch SolidWorks,' + NewLine; 
  S := S + 'then check that the Chrono add-in is activated ' + NewLine;
  S := S + 'by using the Tools/Add-in... menu. '  + NewLine;
  S := S + 'In the Add-ins panel you should find an item called ' + NewLine;
  S := S + 'ChronoEngine SwAddin; set it as active by flagging it.' + NewLine;

  //if (mFoundSolidWorks = 1) then begin
  //  S := S + 'The SolidWorks dll directory is:' + NewLine;
  //  S := S + Space + mPathSolidWorks + NewLine;
  //  S := S + 'so the Chrono::Engine 64 bit add-in module for SolidWorks will be installed in:' + NewLine;
  //  S := S + Space + mPathSolidWorks + 'chronoengine' + NewLine;
  //end

  //if ((mFoundSolidWorks = 0) ) then begin
  //  S := S + 'No installation of SolidWorks 64bit has been detected on this system.' + NewLine;
  //  S := S + 'The Chrono::Engine add-in for SolidWorks CANNOT BE INSTALLED!' + NewLine;
  //end

  Result := S;
end;


