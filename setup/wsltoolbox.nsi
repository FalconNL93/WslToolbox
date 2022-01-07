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
!define SETUP_VERSION ${PRODUCT_VERSION}

;-------------------------------------------------------------------------------
; Attributes
Name "${PRODUCT_NAME}"
OutFile "${EXECUTABLE_NAME}.exe"
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
VIAddVersionKey "FileVersion" "${PRODUCT_VERSION}"

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
LangString component_default ${LANG_ENGLISH} "Required files for WSL Toolbox"
LangString component_start_menu_shortcut ${LANG_ENGLISH} "Create a shortcut on the start menu"
LangString component_desktop_shortcut ${LANG_ENGLISH} "Create a shortcut on the desktop"

;-------------------------------------------------------------------------------
; Installer Sections
Section "WSL Toolbox" WslToolbox
    SetOutPath $INSTDIR
    File /r "..\WslToolbox.Gui\bin\Release\net5.0-windows10.0.19041.0"
    WriteUninstaller "$INSTDIR\uninstall.exe"
SectionEnd

;-------------------------------------------------------------------------------
; Uninstaller Section
section "Uninstall"  
    RMDir /r "$INSTDIR"
    Delete $INSTDIR\uninstall.exe
    DeleteRegKey HKCU "Software\FalconNL93\${PRODUCT_NAME}"
sectionEnd

;-------------------------------------------------------------------------------
; Section localization
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
!insertmacro MUI_DESCRIPTION_TEXT ${WslToolbox} $(component_default)
!insertmacro MUI_FUNCTION_DESCRIPTION_END