# WSL Toolbox

WSL Toolbox allows you to manage your WSL Distributions through an easy to use interface.

WSL Toolbox is currently in development. Some features may not work as expected.

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

## Future features and improvements

- Install distribution by ISO
- Auto updater
- Code improvements
- UI improvements
- Improve service status on distribution commands
- Better feedback to UI

## Known issues

- You cannot install an already installed distribution, this is actually a restriction in WSL itself.
    - To bypass this, you can export the current installed distribution and importing it with a unique name.
- Status/state may not report correctly while executing tasks, to update status manually press F5 or the the "Refresh"
  button.
- UI may have some render issues.

## Requirements

- Windows 10 build 19041 or later
- Windows 11
- [.NET 5 Runtime](https://dotnet.microsoft.com/download/dotnet/5.0/runtime)

## Screenshots

![Main Window](./docs/images/screenshots/tb_1.png?raw=true "Main Window")

![Service Window](./docs/images/screenshots/tb_2.png?raw=true "Service Window")

![Other Window](./docs/images/screenshots/tb_3.png?raw=true "Other Window")

![Context Menu](./docs/images/screenshots/tb_4.png?raw=true "Context Menu")

![Settings Window](./docs/images/screenshots/tb_5.png?raw=true "Settings Window")

![Settings Grid Window](./docs/images/screenshots/tb_6.png?raw=true "Settings Grid Window")

![Settings Theme Window](./docs/images/screenshots/tb_7.png?raw=true "Settings Theme Window")

![Settings Other Window](./docs/images/screenshots/tb_8.png?raw=true "Settings Other Window")

![Import Status](./docs/images/screenshots/tb_9.png?raw=true "Import Status")

![Export Status](./docs/images/screenshots/tb_10.png?raw=true "Export Status")