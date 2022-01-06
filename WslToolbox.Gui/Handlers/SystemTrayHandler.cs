using System;
using System.Drawing;
using System.Reflection;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using WslToolbox.Gui.Configurations;

namespace WslToolbox.Gui.Handlers
{
    public class SystemTrayHandler : IDisposable
    {
        public TaskbarIcon Tray { get; private set; }

        public void Dispose()
        {
            if (Tray is null) return;

            Tray.Visibility = Visibility.Hidden;
            Tray.Icon.Dispose();
            Tray.Dispose();
        }

        public void Initialize(Visibility visibility)
        {
            TaskbarIcon toolboxIcon = new();
            toolboxIcon.ToolTipText = AppConfiguration.AppName;
            toolboxIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            Tray = toolboxIcon;
            Tray.Visibility = visibility;
        }

        public void ShowNotification(string title, string message, BalloonIcon symbol = BalloonIcon.None)
        {
            if (Tray.IsDisposed) return;
            if (Tray.Visibility != Visibility.Visible)
            {
                Tray.Visibility = Visibility.Visible;
                Tray.TrayBalloonTipClosed += (_, _) => { Tray.Visibility = Visibility.Hidden; };
            }

            Tray.ShowBalloonTip(title, message, symbol);
        }
    }
}