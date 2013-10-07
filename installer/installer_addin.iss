//#include "ModifyPath.iss"

#define MyAppName "ChronoEngine SW Add-In"
#define MyAppVersion "v1.70"
#define MyAppPublisher "Alessandro Tasora"
#define MyAppURL "http://www.chronoengine.info"
#define MySolidWorksDir "C:\Program Files\SolidWorks Corp\SolidWorks"

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
DefaultDirName={pf}\
DefaultGroupName={#MyAppName}
WizardImageFile=SetupModern20.bmp
WizardSmallImageFile=SetupModernSmall26.bmp
PrivilegesRequired=admin
;Compression=none
OutputDir=c:\tasora\lavori\data_chrono
OutputBaseFilename=ChronoEngine_SolidWorks_{#MyAppVersion}
ArchitecturesInstallIn64BitMode=x64

[Files]
Source: {#MySolidWorksDir}\ChronoEngineAddIn.dll; DestDir: {code:myGetPathSolidWorks};  Flags: "sharedfile uninsnosharedfileprompt";  Check: myFoundSolidWorks; 
Source: ..\to_put_in_SW_dir\chronoengine\*; Excludes: "*\.svn"; DestDir: {code:myGetPathSolidWorks}\chronoengine;  Flags: recursesubdirs createallsubdirs;  Check: myFoundSolidWorks; 

[Run]
Filename:"{dotnet20}\RegAsm.exe"; Parameters: /codebase ChronoEngineAddIn.dll;WorkingDir: {code:myGetPathSolidWorks}; StatusMsg: "Registering controls ..."; Flags: runhidden;
 
[UninstallRun]
Filename:"{dotnet20}\RegAsm.exe"; Parameters: /unregister ChronoEngineAddIn.dll;WorkingDir: {code:myGetPathSolidWorks}; StatusMsg: "Unegistering controls ..."; Flags: runhidden;


[Icons]
Name: "{group}\Getting started"; Filename: "http://www.chronoengine.info"
Name: "{group}\Uninstall"; Filename: "{uninstallexe}"


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

  myvers := '11';

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

    if RegQueryStringValue(HKEY_LOCAL_MACHINE,
                      'SOFTWARE\SolidWorks\SolidWorks 2012\Setup',
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
           MsgBox('WARNING!'#13#13+
               'The installer was not able to detect SolidWorks 2011 64 bit, or later versions,'+
               'on your system.'#13#13+
               'Maybe your SolidWorks is not yet installed, or not properly installed?'#13+
               '(If so,please install SolidWorks 2011 64 bit, or later, before installing this plug-in).', mbError, MB_OK);

           Abort();
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

  if (PageID = wpSelectDir) then
    Result := True

  if (PageID = wpSelectDir) then
    Result := True
end;


function UpdateReadyMemo(Space, NewLine, MemoUserInfoInfo, MemoDirInfo, MemoTypeInfo,
  MemoComponentsInfo, MemoGroupInfo, MemoTasksInfo: String): String;
var
  S: String;
begin
  { Fill the 'Ready Memo' with the normal settings and the custom settings }
  S := '';

  if (mFoundSolidWorks = 1) then begin
    S := S + 'The SolidWorks dll directory is:' + NewLine;
    S := S + Space + mPathSolidWorks + NewLine;
    S := S + 'so the Chrono::Engine 64 bit add-in module for SolidWorks will be installed in:' + NewLine;
    S := S + Space + mPathSolidWorks + NewLine;
  end

  if ((mFoundSolidWorks = 0) ) then begin
    S := S + 'No installation of SolidWorks 2011 64bit, or later, has been detected on this system.' + NewLine;
    S := S + 'The Chrono::Engine add-in for SolidWorks CANNOT BE INSTALLED!' + NewLine;
  end

  Result := S;
end;


