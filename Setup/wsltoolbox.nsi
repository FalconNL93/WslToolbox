;-------------------------------------------------------------------------------
; Includes
!include "MUI2.nsh"
!include "LogicLib.nsh"
!include "WinVer.nsh"
!include "x64.nsh"

;-------------------------------------------------------------------------------
; Constants
!define PRODUCT_NAME "WSL Toolbox"
!define COPYRIGHT "Copyright © 2021 FalconNL93"
!define PRODUCT_VERSION "1.0.0.0"
!define SETUP_VERSION 1.0.0.0

;-------------------------------------------------------------------------------
; Attributes
Name "${PRODUCT_NAME}"
OutFile "wsl-toolbox.exe"
InstallDir "$APPDATA\${PRODUCT_NAME}"
InstallDirRegKey HKCU "Software\FalconNL93\${PRODUCT_NAME}" ""
RequestExecutionLevel user

;-------------------------------------------------------------------------------
; Version Info
VIProductVersion "${PRODUCT_VERSION}"
VIAddVersionKey "ProductName" "${PRODUCT_NAME}"
VIAddVersionKey "ProductVersion" "${PRODUCT_VERSION}"
VIAddVersionKey "FileDescription" "${PRODUCT_DESCRIPTION}"
VIAddVersionKey "LegalCopyright" "${COPYRIGHT}"
VIAddVersionKey "FileVersion" "${SETUP_VERSION}"

;-------------------------------------------------------------------------------
; Modern UI Appearance
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\nsis3-install.ico"
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP "${NSISDIR}\Contrib\Graphics\Header\nsis3-grey.bmp"
!define MUI_WELCOMEFINISHPAGE_BITMAP "${NSISDIR}\Contrib\Graphics\Wizard\nsis3-grey.bmp"
!define MUI_FINISHPAGE_NOAUTOCLOSE

;-------------------------------------------------------------------------------
; Installer Pages
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

;-------------------------------------------------------------------------------
; Uninstaller Pages
!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

;-------------------------------------------------------------------------------
; Languages
!insertmacro MUI_LANGUAGE "English"

;-------------------------------------------------------------------------------
; Installer Sections
Section "${PRODUCT_NAME}" WslToolbox
	SetOutPath $INSTDIR
    SectionIn RO
    File /r "..\WslToolbox.Gui\bin\Release\x64\*"
	WriteUninstaller "$INSTDIR\Uninstall.exe"
SectionEnd

;-------------------------------------------------------------------------------
; Uninstaller Sections
Section "Uninstall"
	Delete "$INSTDIR\Uninstall.exe"
	RMDir "$INSTDIR"
	DeleteRegKey /ifempty HKCU "Software\FalconNL93\${PRODUCT_NAME}"
SectionEnd

;-------------------------------------------------------------------------------
; Include localization
!include "lang\en.nsi"

;-------------------------------------------------------------------------------
; Section localization
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
!insertmacro MUI_DESCRIPTION_TEXT ${WslToolbox} $(component_default)
!insertmacro MUI_FUNCTION_DESCRIPTION_END