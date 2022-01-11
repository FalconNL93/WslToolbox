!define DNFIVE "https://download.visualstudio.microsoft.com/download/pr/fccf43d2-3e62-4ede-b5a5-592a7ccded7b/6339f1fdfe3317df5b09adf65f0261ab/dotnet-runtime-5.0.13-win-x64.exe"
LangString StrDotNetFive ${LANG_ENGLISH} "Install the .NET 5.0 Runtime"

Section /o ".NET 5.0 Runtime" DotNetFive
    Var /GLOBAL downloadDotNetFive
    Var /GLOBAL commandParameters
    Var /GLOBAL EXIT_CODE

    IfSilent is_quiet is_not_quiet

    is_quiet:
        StrCpy $commandParameters "/q /norestart"
    is_not_quiet:
        StrCpy $commandParameters "/showrmui /passive /norestart"

    NSISdl::download "${DNFIVE}" "$TEMP\dotnet-runtime-5-win-x64.exe" $downloadDotNetFive

    StrCmp $downloadDotNetFive success fail

    success:
        ExecWait '"$TEMP\dotnet-runtime-5-win-x64.exe" $commandParameters' $EXIT_CODE
        GoTo finished
    fail:
        MessageBox MB_OK|MB_ICONEXCLAMATION "Could not download .NET 5, setup will continue without"
    finished:
        SetRebootFlag false

SectionEnd