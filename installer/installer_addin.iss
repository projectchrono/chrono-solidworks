//#include "ModifyPath.iss"

#define MyAppName "ChronoEngine SW Add-In"
#define MyAppVersion "v4.00"
#define MyAppPublisher "Alessandro Tasora"
#define MyAppURL "http://www.chronoengine.info"
#define MySolidWorksDir "C:\Program Files\SolidWorks Corp\SolidWorks (2)"
#define MyAppSource "C:\Program Files\chrono_solidworks"

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
DefaultDirName={pf}\ChronoSolidworks
WizardImageFile=SetupModern20.bmp
WizardSmallImageFile=SetupModernSmall26.bmp
PrivilegesRequired=admin
;Compression=none
OutputDir=c:\tasora\lavori\data_chrono
OutputBaseFilename=ChronoEngine_SolidWorks_{#MyAppVersion}
ArchitecturesInstallIn64BitMode=x64

[Files]
Source: {#MyAppSource}\ChronoEngineAddIn.dll; DestDir: {app};  Flags: "sharedfile uninsnosharedfileprompt"; 
Source: {#MyAppSource}\hacd_CLI.dll; DestDir: {app};  Flags: "sharedfile uninsnosharedfileprompt"; 
Source: ..\to_put_in_SW_dir\*; Excludes: "*\.svn,*.git"; DestDir: {app};  Flags: recursesubdirs createallsubdirs; 
;Source: {#MySolidWorksDir}\chronoengine\*.dll; DestDir: {code:myGetPathSolidWorks}\chronoengine;  Check: myFoundSolidWorks; 

[Run]
Filename:"{dotnet40}\RegAsm.exe"; Parameters: /codebase ChronoEngineAddIn.dll;WorkingDir: {app}; StatusMsg: "Registering controls ..."; Flags: runhidden;
 
[UninstallRun]
Filename:"{dotnet40}\RegAsm.exe"; Parameters: /unregister ChronoEngineAddIn.dll;WorkingDir: {app}; StatusMsg: "Unegistering controls ..."; Flags: runhidden;


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


  if (IsWin64) then
  begin
    // CASE OF 64 BIT PLATFORM

    // find 64 bit SW v.11:
    if RegQueryStringValue(HKEY_LOCAL_MACHINE,
                      'SOFTWARE\SolidWorks\SolidWorks 2011\Setup',
                      'SolidWorks Folder',
                      mallDirSolidWorks) then
    begin
            mPathSolidWorks := mallDirSolidWorks; 
            mFoundSolidWorks := 1;
    end

    // find 64 bit SW v.12:
    if RegQueryStringValue(HKEY_LOCAL_MACHINE,
                      'SOFTWARE\SolidWorks\SolidWorks 2012\Setup',
                      'SolidWorks Folder',
                      mallDirSolidWorks) then
    begin
            mPathSolidWorks := mallDirSolidWorks; 
            mFoundSolidWorks := 1;
    end


    // find 64 bit SW v.13:
    if RegQueryStringValue(HKEY_LOCAL_MACHINE,
                      'SOFTWARE\SolidWorks\SolidWorks 2013\Setup',
                      'SolidWorks Folder',
                      mallDirSolidWorks) then
    begin
            mPathSolidWorks := mallDirSolidWorks; 
            mFoundSolidWorks := 1;
    end


    // find 64 bit SW v.14:
    if RegQueryStringValue(HKEY_LOCAL_MACHINE,
                      'SOFTWARE\SolidWorks\SolidWorks 2014\Setup',
                      'SolidWorks Folder',
                      mallDirSolidWorks) then
    begin
            mPathSolidWorks := mallDirSolidWorks; 
            mFoundSolidWorks := 1;
    end

    // find 64 bit SW v.15:
    if RegQueryStringValue(HKEY_LOCAL_MACHINE,
                      'SOFTWARE\SolidWorks\SolidWorks 2015\Setup',
                      'SolidWorks Folder',
                      mallDirSolidWorks) then
    begin
            mPathSolidWorks := mallDirSolidWorks; 
            mFoundSolidWorks := 1;
    end

    // find 64 bit SW v.16:
    if RegQueryStringValue(HKEY_LOCAL_MACHINE,
                      'SOFTWARE\SolidWorks\SolidWorks 2016\Setup',
                      'SolidWorks Folder',
                      mallDirSolidWorks) then
    begin
            mPathSolidWorks := mallDirSolidWorks; 
            mFoundSolidWorks := 1;
    end

    // find 64 bit SW v.17:
    if RegQueryStringValue(HKEY_LOCAL_MACHINE,
                      'SOFTWARE\SolidWorks\SolidWorks 2017\Setup',
                      'SolidWorks Folder',
                      mallDirSolidWorks) then
    begin
            mPathSolidWorks := mallDirSolidWorks; 
            mFoundSolidWorks := 1;
    end


    
  end 
  else
  begin
    // CASE OF 32 BIT PLATFORM
          mFoundSolidWorks := 0;
  end



  if mFoundSolidWorks = 0 then 
  begin 
           //MsgBox('WARNING!'#13#13+
           //    'The installer was not able to detect SolidWorks 64 bit'+
           //    'on your system.'#13#13+
           //    'Maybe your SolidWorks is not yet installed, or not properly installed?'#13+
           //    '(If so,please install SolidWorks 64 bit before installing this plug-in).', mbError, MB_OK);
           //
           //Abort();
  end 
                                   

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
  S :=     'Instaling the Chrono::SolidWorks add-in.'  + NewLine  + NewLine;
  S :=     'Assuming NET 4.0 framework is installed.';
  S := S + 'After the installation do this: launch SolidWorks, then check that the Chrono add-in is activated by using the Tools/Add-in... menu. '  + NewLine;
  S := S + 'In the Add-ins panel you should find an item called ChronoEngine SwAddin; set it as active by flagging it.' + NewLine;

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


