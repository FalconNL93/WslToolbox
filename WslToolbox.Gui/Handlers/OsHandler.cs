using System;
using System.Collections.Generic;

namespace WslToolbox.Gui.Handlers
{
    public static class OsHandler
    {
        public static bool Supported()
        {
            return OsList().Exists(
                x => x.Major == Environment.OSVersion.Version.Major);
        }

        private static List<OsObject> OsList()
        {
            var osDictionary = new List<OsObject>
            {
                new(10, true)
            };

            return osDictionary;
        }
    }

    public class OsObject
    {
        public readonly int Major;
        private bool _supported;

        public OsObject(int major, bool supported)
        {
            Major = major;
            _supported = supported;
        }
    }
}