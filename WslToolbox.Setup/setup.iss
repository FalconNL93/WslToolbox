[Setup]
AppId={#ProductUuid}
AppName={#ProductName}
AppVersion={#ProductVersion}
AppVerName={#ProductName} {#ProductVersion}
AppPublisher={#ProductOwner}
AppPublisherURL={#ProductUrl}
AppSupportURL={#ProductUrl}
AppUpdatesURL={#ProductUrl}
WizardStyle=modern

; Executable information
VersionInfoDescription={#ProductDescription}
VersionInfoProductTextVersion={#ProductVersion}
VersionInfoCompany={#ProductOwner}
VersionInfoVersion={#ProductVersion}

; Permissions
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=commandline

; Compiler
OutputDir="{#OutputDirectory}"
OutputBaseFilename="{#OutputFile}-setup"

; Pages
DisableWelcomePage=no
DisableProgramGroupPage=yes

; Settings
DefaultDirName={userappdata}\{#ProductName}
UninstallDisplayName={#ProductName}
UninstallDisplayIcon={app}\{#DefaultExecutable}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "startmenuicon"; Description: "{cm:CreateStartMenuIcon}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
Source: "{#BinariesDirectory}\*.*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{autodesktop}\{#ProductName}"; Filename: "{app}\{#DefaultExecutable}"; Tasks: desktopicon
Name: "{autoprograms}\{#ProductName}"; Filename: "{app}\{#DefaultExecutable}"; Tasks: startmenuicon

[Run]
Filename: "{app}\{#DefaultExecutable}"; Description: "{cm:LaunchProgram,{#StringChange(ProductName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Messages]
SetupAppTitle = {#ProductName}
SetupWindowTitle = Setup - {#ProductName}
BeveledLabel = {#ProductVersion}

[CustomMessages]
CreateDesktopIcon=Create a &desktop icon
CreateStartMenuIcon=Create a start menu icon

[Code]
{ ///////////////////////////////////////////////////////////////////// }
function GetUninstallString(): String;
var
  sUnInstPath: String;
  sUnInstallString: String;
begin
  sUnInstPath := ExpandConstant('Software\Microsoft\Windows\CurrentVersion\Uninstall\{#emit SetupSetting("AppId")}_is1');
  sUnInstallString := '';
  if not RegQueryStringValue(HKLM, sUnInstPath, 'UninstallString', sUnInstallString) then
    RegQueryStringValue(HKCU, sUnInstPath, 'UninstallString', sUnInstallString);
  Result := sUnInstallString;
end;


{ ///////////////////////////////////////////////////////////////////// }
function IsUpgrade(): Boolean;
begin
  Result := (GetUninstallString() <> '');
end;


{ ///////////////////////////////////////////////////////////////////// }
function UnInstallOldVersion(): Integer;
var
  sUnInstallString: String;
  iResultCode: Integer;
begin
{ Return Values: }
{ 1 - uninstall string is empty }
{ 2 - error executing the UnInstallString }
{ 3 - successfully executed the UnInstallString }

  { default return value }
  Result := 0;

  { get the uninstall string of the old app }
  sUnInstallString := GetUninstallString();
  if sUnInstallString <> '' then begin
    sUnInstallString := RemoveQuotes(sUnInstallString);
    if Exec(sUnInstallString, '/SILENT /NORESTART /SUPPRESSMSGBOXES','', SW_HIDE, ewWaitUntilTerminated, iResultCode) then
      Result := 3
    else
      Result := 2;
  end else
    Result := 1;
end;

{ ///////////////////////////////////////////////////////////////////// }
procedure CurStepChanged(CurStep: TSetupStep);
begin
  if (CurStep=ssInstall) then
  begin
    if (IsUpgrade()) then
    begin
      UnInstallOldVersion();
    end;
  end;
end;

{ ///////////////////////////////////////////////////////////////////// }
function ShouldSkipPage(PageID: Integer): Boolean;
begin
    if IsUpgrade then
        if PageID = wpSelectTasks then
            Result := true;
            
end;