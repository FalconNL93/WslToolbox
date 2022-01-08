;-------------------------------------------------------------------------------
; Includes
!include "MUI2.nsh"
!include "LogicLib.nsh"
!include "WinVer.nsh"
!include "x64.nsh"
!include "FileFunc.nsh"

;-------------------------------------------------------------------------------
; Constants
!define COPYRIGHT "Copyright © 2021 FalconNL93"
!define SETUP_VERSION ${PRODUCT_VERSION}
!define EXECUTABLE "WslToolbox.Gui.exe"
!define APR "Software\Microsoft\Windows\CurrentVersion\Uninstall\{${Uuid}}"

;-------------------------------------------------------------------------------
; Attributes
Name "${PRODUCT_NAME}"
OutFile "${EXECUTABLE_NAME}.exe"
InstallDir "$APPDATA\${PRODUCT_NAME}"
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
!define MUI_FINISHPAGE_RUN
!define MUI_FINISHPAGE_RUN_NOTCHECKED
!define MUI_FINISHPAGE_RUN_TEXT "Launch ${PRODUCT_NAME}"
!define MUI_FINISHPAGE_RUN_FUNCTION "LaunchApp"
!define MUI_FINISHPAGE_SHOWREADME
!define MUI_FINISHPAGE_SHOWREADME_NOTCHECKED
!define MUI_FINISHPAGE_SHOWREADME_TEXT "Create desktop shortcut"
!define MUI_FINISHPAGE_SHOWREADME_FUNCTION "CreateAppShortcut"

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
LangString component_default_start ${LANG_ENGLISH} "Create a start menu shortcut"

;-------------------------------------------------------------------------------
; Installer Sections
Section "WSL Toolbox" WslToolbox
    SectionIn RO
    SetOutPath $INSTDIR
    File /r "..\WslToolbox.Gui\bin\${PRODUCT_ENVIRONMENT}\${TARGET_DIRECTORY}\"

    ; Registry
    WriteRegStr HKCU "${APR}" "DisplayName" "${PRODUCT_NAME}"
    WriteRegStr HKCU "${APR}" "DisplayIcon" "$\"$INSTDIR\${EXECUTABLE}$\""
    WriteRegStr HKCU "${APR}" "DisplayVersion" "${DISPLAY_VERSION}"
    WriteRegStr HKCU "${APR}" "Publisher" "FalconNL93"
    WriteRegStr HKCU "${APR}" "NoRepair" "1"
    WriteRegStr HKCU "${APR}" "NoModify" "1"
    WriteRegStr HKCU "${APR}" "UninstallString" "$\"$INSTDIR\uninstall.exe$\""
    WriteRegStr HKCU "${APR}" "QuietUninstallString" "$\"$INSTDIR\uninstall.exe$\" /S"
    WriteRegStr HKCU "${APR}" "URLInfoAbout" "https://github.com/FalconNL93/WslToolbox"

    ; Calculate size
    ${GetSize} "$INSTDIR" "/S=0K" $0 $1 $2
    IntFmt $0 "0x%08X" $0
    WriteRegDWORD HKCU "${APR}" "EstimatedSize" "$0"

    WriteUninstaller "$INSTDIR\uninstall.exe"
SectionEnd

;-------------------------------------------------------------------------------
; Uninstaller Section
Section "Uninstall"  
    ; Shortcuts
    Delete "$DESKTOP\${PRODUCT_NAME}.lnk"
    Delete "$SMPROGRAMS\${PRODUCT_NAME}.lnk"

    ; Installation directory
    RmDir /r "$INSTDIR"

    ; Uninstaller
    Delete $INSTDIR\uninstall.exe

    ; Registry
    DeleteRegKey HKCU "${APR}"
SectionEnd

Section /o "Create start menu Shortcut" StartShortcuts
    CreateShortcut "$SMPROGRAMS\${PRODUCT_NAME}.lnk" "$INSTDIR\${EXECUTABLE}"
SectionEnd

;-------------------------------------------------------------------------------
; Section localization
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
!insertmacro MUI_DESCRIPTION_TEXT ${WslToolbox} $(component_default)
!insertmacro MUI_DESCRIPTION_TEXT ${StartShortcuts} $(component_default_start)
!insertmacro MUI_FUNCTION_DESCRIPTION_END

;-------------------------------------------------------------------------------
; Custom functions
Function LaunchApp
    ExecShell "" "$INSTDIR\${EXECUTABLE}"
FunctionEnd

Function CreateAppShortcut
    CreateShortcut "$DESKTOP\${PRODUCT_NAME}.lnk" "$INSTDIR\${EXECUTABLE}"
FunctionEnd

