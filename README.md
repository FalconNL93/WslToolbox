# WSL Toolbox

WSL Toolbox allows you to manage your WSL Distributions through an easy to use interface.

WSL Toolbox is currently in development. Some features may not work as expected.

## Note

I am currently developing a new GUI for WSL Toolbox, this GUI will be maintained in the "new-gui" branch and will eventually replace the current GUI.

## Install

Download WSL Toolbox via the [releases page](https://github.com/FalconNL93/WslToolbox/releases). Either choose the
ZIP-file or the setup file.

## Build

| Branch   |                                                                                                      Build Status                                                                                                       |
|:---------|:-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------:|
| Beta     | [![Build Status](https://dev.azure.com/FalconNL93/WSL%20Toolbox/_apis/build/status/FalconNL93.WslToolbox?branchName=beta)](https://dev.azure.com/FalconNL93/WSL%20Toolbox/_build/latest?definitionId=1&branchName=beta) |

## Features

- Grid overview of installed distributions
- Distribution control
    - Start / Stop / Restart
    - Install
    - Remove
    - Export
    - Import
    - Rename
    - Convert WSL1 to WSL2

- WSL Service control
    - Start / Stop / Restart
    - Update kernel

- Interface features
    - Light / Dark mode
    - Run application on startup
    - System tray
    - Minimize to system tray
    - Minimize to tray on startup
    - Config customizable through GUI and JSON

## Shortcuts

| Shortcut |        Action         |
|:---------|:---------------------:|
| F5       | Refresh distributions |
| CTRL + , |     Open settings     |

## Requirements

- Windows 10 build 19041 or later
- Windows 11
- [.NET 5 Runtime](https://dotnet.microsoft.com/download/dotnet/5.0/runtime)

## Screenshots

![Main Window](./docs/images/screenshots/mw1.png?raw=true "Main Window")
![Main Window 2](./docs/images/screenshots/mw2.png?raw=true "Main Window 2")
![Main Window 3](./docs/images/screenshots/mw3.png?raw=true "Main Window 3")
![Main Window 4](./docs/images/screenshots/mw4.png?raw=true "Main Window 4")
![Settings Window 4](./docs/images/screenshots/sw1.png?raw=true "Settings Window")

## Special thanks

**Jetbrains:**

I'm using [Jetbrains](https://www.jetbrains.com/) [Rider](https://www.jetbrains.com/rider/) to learn and develop C#,
they provided me with an open source license.

## Dependencies

**WSL Toolbox is using the following packages:**

- ModernWPF UI Library - https://github.com/Kinnara/ModernWpf
- Command Line Parser Library - https://github.com/commandlineparser/commandline
- AutoUpdater.NET - https://github.com/ravibpatel/AutoUpdater.NET
- Hardcodet NotifyIcon for WPF - https://github.com/hardcodet/wpf-notifyicon
