using System;
using System.Drawing;
using System.Reflection;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace WslToolbox.Gui.Classes
{
    public class SystemTrayClass : IDisposable
    {
        public TaskbarIcon Tray { get; private set; }

        public void Dispose()
        {
            if (Tray is null) return;

            Tray.Visibility = Visibility.Hidden;
            Tray.Icon.Dispose();
            Tray.Dispose();
        }

        public void Show()
        {
            TaskbarIcon toolboxIcon = new();
            toolboxIcon.ToolTipText = Assembly.GetExecutingAssembly().GetName().Name;
            toolboxIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            Tray = toolboxIcon;
        }

        public void ShowBalloonTip(string title, string message, BalloonIcon symbol = BalloonIcon.None)
        {
            Tray.ShowBalloonTip(title, message, symbol);
        }
    }
}