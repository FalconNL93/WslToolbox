using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WslToolbox.Classes
{
    public class SystemTrayClass : IDisposable
    {
        public TaskbarIcon Tray { get; set; }

        public void Dispose() => Tray?.Dispose();

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
