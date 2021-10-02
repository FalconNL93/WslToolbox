using System;
using System.Collections.Generic;
using System.Linq;

namespace WslToolbox.Gui.Configurations
{
    public static class ThemeConfiguration
    {
        public enum Styles
        {
            Auto,
            Light,
            Dark
        }

        public const string Light = "Light.Blue";
        public const string Dark = "Dark.Steel";

        public static IEnumerable<Styles> GetValues()
        {
            return Enum.GetValues(typeof(Styles)).Cast<Styles>();
        }
    }
}