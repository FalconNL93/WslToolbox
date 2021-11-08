using System;
using System.IO;
using System.Linq;

namespace WslToolbox.Gui.Handlers
{
    public class OsHandler
    {
        public enum States
        {
            Unsupported,
            Minimum,
            Recommended
        }

        public const int MinimumOsBuild = 19041;
        public const int RecommendedOsBuild = 19043;
        public readonly int OsBuild = Environment.OSVersion.Version.Build;

        public readonly States State;

        public OsHandler()
        {
            State = OsBuild switch
            {
                >= RecommendedOsBuild => States.Recommended,
                >= MinimumOsBuild => States.Minimum,
                _ => States.Unsupported
            };
        }
    }
}