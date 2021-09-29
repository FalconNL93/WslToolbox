using System;

namespace WslToolbox.Gui.Exceptions
{
    internal class ConfigurationNotSaved : Exception
    {
        public ConfigurationNotSaved()
        {
        }

        public ConfigurationNotSaved(string message)
            : base(message)
        {
        }

        public ConfigurationNotSaved(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}