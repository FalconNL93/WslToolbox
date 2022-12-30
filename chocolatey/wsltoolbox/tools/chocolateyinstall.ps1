$ErrorActionPreference = 'Stop' # stop on all errors
$toolsDir   = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$url64      = 'https://github.com/FalconNL93/WslToolbox/releases/download/0.6.17.0/wsltoolbox_0.6.17.0_x64_setup.exe'
$checksum64 = '30ACE8713D478EB97541ED9AAC531FBCC5F46662D1F96176387AF73D83BF6E5A'

$packageArgs = @{
  packageName   = $env:ChocolateyPackageName
  softwareName  = 'WSL Toolbox'
  unzipLocation = $toolsDir
  fileType      = 'exe'
  url64bit      = $url64
  checksum64    = $checksum64
  checksumType64= 'sha256'
  silentArgs   = '/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /SP-'
  validExitCodes= @(0)
}

Install-ChocolateyPackage @packageArgs