
import-module au

$releases = 'https://github.com/FalconNL93/WslToolbox/releases'

function global:au_SearchReplace {
   @{
        ".\tools\chocolateyInstall.ps1" = @{
            "(?i)(^\s*url64bit\s*=\s*)('.*')"   = "`$1'$($Latest.URL64)'"
            "(?i)(^\s*checksum64\s*=\s*)('.*')" = "`$1'$($Latest.Checksum64)'"
        }
    }
}

function global:au_GetLatest {
    Write-Output "test";
    $download_page = Invoke-WebRequest -Uri $releases

    $url64   = $download_page.links | ? href -match '.exe$' | % href | select -First 1
    Write-Output $url64;
    $version = (Split-Path ( Split-Path $url64 ) -Leaf)

    @{
        URL64   = 'https://github.com' + $url64
        Version = $version
    }
}

update