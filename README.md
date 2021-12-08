# WSL Toolbox

WSL Toolbox allows you to manage your WSL Distributions through an easy to use interface.

WSL Toolbox is currently in development. Some features may not work as expected.

[![Build status](https://ci.appveyor.com/api/projects/status/hks2bqstbxs3asnt?svg=true)](https://ci.appveyor.com/project/FalconNL93/wsltoolbox)

## Install
Download WSL Toolbox via the [releases page](https://github.com/FalconNL93/WslToolbox/releases), extract and run `WslToolbox.Gui.exe`.

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

| Shortcut  |  Action
|:----------|:--------:
| F5        | Refresh distributions
| CTRL + ,  | Open settings

## Requirements

- Windows 10 build 19041 or later
- Windows 11
- [.NET 5 Runtime](https://dotnet.microsoft.com/download/dotnet/5.0/runtime)

## Screenshots

![Main Window](./docs/images/screenshots/tb1.png?raw=true "Main Window")
![Main Window](./docs/images/screenshots/tb3.png?raw=true "Main Window")
![Main Window](./docs/images/screenshots/tb2.png?raw=true "Settings Window")
![Main Window](./docs/images/screenshots/tb4.png?raw=true "Settings Window")

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
