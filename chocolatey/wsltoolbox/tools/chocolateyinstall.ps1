$ErrorActionPreference = 'Stop'

$packageArgs = @{
  packageName            = 'wsltoolbox'
  fileType               = 'exe'
  url64bit               = 'https://github.com/FalconNL93/WslToolbox/releases/download/0.6.17.0/wsltoolbox_0.6.17.0_x64_setup.exe'
  checksum64             = '30ACE8713D478EB97541ED9AAC531FBCC5F46662D1F96176387AF73D83BF6E5A'
  checksumType64         = 'sha256'
  silentArgs             = '/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /SP-'
  validExitCodes         = @(0)
  softwareName           = 'WSL Toolbox'
}
Install-ChocolateyPackage @packageArgs