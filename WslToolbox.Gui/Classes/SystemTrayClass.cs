using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Drawing;
using System.Reflection;

namespace WslToolbox.Gui.Classes
{
    public class SystemTrayClass : IDisposable
    {
        public TaskbarIcon Tray { get; set; }

        public void Dispose()
        {
            Tray?.Dispose();
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