using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WslToolbox.Gui.Configurations
{
    public static class ThemeConfiguration
    {
        public enum Styles
        {
            Auto = 0,
            Light = 1,
            Dark = 2
        }

        public static string Light = "Light.Blue";
        public static string Dark = "Dark.Steel";
    }
}
