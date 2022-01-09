;-------------------------------------------------------------------------------
; Includes
!include "MUI2.nsh"
!include "LogicLib.nsh"
!include "WinVer.nsh"
!include "x64.nsh"
!include "FileFunc.nsh"

;-------------------------------------------------------------------------------
; Constants
!define ROOTDIR "${ROOT_FOLDER}"
!define COPYRIGHT "Copyright © 2021 FalconNL93"
!define SETUP_VERSION ${PRODUCT_VERSION}
!define EXECUTABLE "WslToolbox.Gui.exe"
!define APR "Software\Microsoft\Windows\CurrentVersion\Uninstall\{${Uuid}}"

;-------------------------------------------------------------------------------
; Attributes
Name "${PRODUCT_NAME}"
Caption "${DIALOG_CAPTION}"
OutFile "${EXECUTABLE_NAME}.exe"
InstallDir "$APPDATA\${PRODUCT_NAME}"
RequestExecutionLevel user
BrandingText "${BRANDING}"

;-------------------------------------------------------------------------------
; Version Info
VIProductVersion "${PRODUCT_VERSION}.0"
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

;-------------------------------------------------------------------------------
; Finish page
!define MUI_FINISHPAGE_NOAUTOCLOSE
!define MUI_FINISHPAGE_RUN
!define MUI_FINISHPAGE_RUN_NOTCHECKED
!define MUI_FINISHPAGE_RUN_TEXT "Launch ${PRODUCT_NAME}"
!define MUI_FINISHPAGE_RUN_FUNCTION "LaunchApp"

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

;-------------------------------------------------------------------------------
; Localization
!insertmacro MUI_LANGUAGE "English"
!include "${ROOTDIR}\lang.nsh"

;-------------------------------------------------------------------------------
; Installer Sections
Section "!${PRODUCT_NAME}" DefaultSection
    ; Attributes
    SectionIn RO
    SetOutPath $INSTDIR
    File /r "..\WslToolbox.Gui\bin\${PRODUCT_ENVIRONMENT}\${TARGET_DIRECTORY}\"

    ; Registry
    WriteRegStr HKCU "${APR}" "DisplayName" "${PRODUCT_NAME}"
    WriteRegStr HKCU "${APR}" "DisplayIcon" "$\"$INSTDIR\${EXECUTABLE}$\""
    WriteRegStr HKCU "${APR}" "DisplayVersion" "${PRODUCT_VERSION}"
    WriteRegStr HKCU "${APR}" "Publisher" "FalconNL93"
    WriteRegStr HKCU "${APR}" "NoRepair" "1"
    WriteRegStr HKCU "${APR}" "NoModify" "1"
    WriteRegStr HKCU "${APR}" "UninstallString" "$\"$INSTDIR\uninstall.exe$\""
    WriteRegStr HKCU "${APR}" "QuietUninstallString" "$\"$INSTDIR\uninstall.exe$\" /S"
    WriteRegStr HKCU "${APR}" "URLInfoAbout" "https://github.com/FalconNL93/WslToolbox"

    ; Write uninstaller
    WriteUninstaller "$INSTDIR\uninstall.exe"

    ; Calculate size
    ${GetSize} "$INSTDIR" "/S=0K" $0 $1 $2
    IntFmt $0 "0x%08X" $0
    WriteRegDWORD HKCU "${APR}" "EstimatedSize" "$0"
SectionEnd

;-------------------------------------------------------------------------------
; Desktop shortcut Section
Section /o "Desktop shortcut" DesktopShortcut
    CreateShortcut "$DESKTOP\${PRODUCT_NAME}.lnk" "$INSTDIR\${EXECUTABLE}"
SectionEnd

;-------------------------------------------------------------------------------
; Start menu Section
Section /o "Start menu shortcut" StartShortcut
    CreateShortcut "$SMPROGRAMS\${PRODUCT_NAME}.lnk" "$INSTDIR\${EXECUTABLE}"
SectionEnd

;-------------------------------------------------------------------------------
; Uninstaller Section
Section "Uninstall"  
    ; Remove shortcuts
    Delete "$DESKTOP\${PRODUCT_NAME}.lnk"
    Delete "$SMPROGRAMS\${PRODUCT_NAME}.lnk"

    ; Remove installation directory
    RmDir /r "$INSTDIR"

    ; Remove uninstaller
    Delete $INSTDIR\uninstall.exe

    ; Registry
    DeleteRegKey HKCU "${APR}"
SectionEnd

;-------------------------------------------------------------------------------
; Custom functions
Function LaunchApp
    ExecShell "" "$INSTDIR\${EXECUTABLE}"
FunctionEnd

Function .onInit
    ${IfNot} ${AtleastWin10}
        MessageBox MB_ICONEXCLAMATION "$(StrWarningOperatingSystem)" /SD IDOK
    ${EndIf}
FunctionEnd

;-------------------------------------------------------------------------------
; Section localization
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
!insertmacro MUI_DESCRIPTION_TEXT ${DefaultSection} $(StrDefaultSection)
!insertmacro MUI_DESCRIPTION_TEXT ${DesktopShortcut} $(StrDesktopShortcut)
!insertmacro MUI_DESCRIPTION_TEXT ${StartShortcut} $(StrStartShortcut)
!insertmacro MUI_FUNCTION_DESCRIPTION_END