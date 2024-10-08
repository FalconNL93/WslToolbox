# WSL Toolbox

WSL Toolbox allows you to manage your WSL Distributions through an easy to use interface for free.

## Beta

WSL Toolbox is currently in beta, some functionality may not work like expected or are missing/yet to be implemented.

Please report any bugs you encounter or create a pull request if you would like to contribute. Applies for feature requests too.

The UI is being rewritten to WinUI3 starting from version 0.6, the old UI (version 0.5.x) is still available from the releases page.

## Features

* Distribution overview
* Distribution control (start, stop and restart)
* Install new distributions
* Remove or edit distributions
* Import and export distributions
* Light/Dark mode
* Version check

Configuration for WSL Toolbox is saved to a JSON file.

## Install

* Download from [releases](https://github.com/FalconNL93/WslToolbox/releases)
* Winget:
  ``` winget install FalconNL93.WslToolbox ```

## Screenshots

![Main window](/assets/images/scr1.png?raw=true "Main Window")
![Add distribution](/assets/images/scr2.png?raw=true "Add distribution")
![Settings window](/assets/images/scr3.png?raw=true "Settings Window")

## Requirements

* [Windows Subsystem for Linux](https://www.microsoft.com/store/productId/9P9TQF7MRM4R)
* [.NET Desktop Runtime 8.0.6](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* Windows 11 (x64) **OR** Windows 10 x64 20H1 (19041) or newer.
