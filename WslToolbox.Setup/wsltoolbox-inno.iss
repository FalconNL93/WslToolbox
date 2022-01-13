#define ProductOwner "FalconNL93"
#define ProductUrl "https://github.com/FalconNL93/WslToolbox"
#define DefaultExecutable "WslToolbox.Gui.exe"

[Setup]
AppId={{{#ProductUuid}}
AppName={#ProductName}
AppVersion={#ProductVersion}
AppVerName={#ProductName} {#ProductVersion}
AppPublisher={#ProductOwner}
AppPublisherURL={#ProductUrl}
AppSupportURL={#ProductUrl}
AppUpdatesURL={#ProductUrl}

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
OutputBaseFilename={#OutputFile}

; Pages
DisableWelcomePage=no
DisableProgramGroupPage=auto

; Style
WizardStyle=classic
WizardImageFile=compiler:WizClassicImage.bmp
WizardSmallImageFile=compiler:WizClassicSmallImage.bmp

; Settings
DefaultDirName={userappdata}\{#ProductName}
SetupIconFile=compiler:SetupClassicIcon.ico
UninstallDisplayName={#ProductName}
UninstallDisplayIcon={app}\{#DefaultExecutable}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "startmenuicon"; Description: "{cm:CreateStartMenuIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\WslToolbox.Gui\bin\{#ProductEnvironment}\net5.0-windows10.0.19041.0\*.*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{autodesktop}\{#ProductName}"; Filename: "{app}\{#DefaultExecutable}"; Tasks: desktopicon
Name: "{autoprograms}\{#ProductName}"; Filename: "{app}\{#DefaultExecutable}"; Tasks: startmenuicon

[Run]
Filename: "{app}\{#DefaultExecutable}"; Description: "{cm:LaunchProgram,{#StringChange(ProductName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[CustomMessages]
CreateDesktopIcon=Create a &desktop icon
CreateStartMenuIcon=Create a start menu icon