#define MyAppName "WslToolbox"
#define MyAppVersion "1.0"
#define MyAppPublisher "FalconNL93"
#define MyAppURL "https://github.com/FalconNL93/WslToolbox"
#define MyAppExeName "WslToolbox.Gui.exe"

[Setup]
AppId={{E6549358-EF8F-4076-B986-5D6EFFFDE52A}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={localappdata}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=commandline
OutputDir=bin
OutputBaseFilename=WslToolbox-setup
Compression=lzma
SolidCompression=yes
WizardStyle=classic
DisableWelcomePage=no

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "{#SourcePath}\..\WslToolbox.Gui\bin\Release\Windows\*.*"; DestDir: "{app}"; Flags: ignoreversion external recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

