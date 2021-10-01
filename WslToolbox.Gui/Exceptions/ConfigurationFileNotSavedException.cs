using System;

namespace WslToolbox.Gui.Exceptions
{
    internal class ConfigurationFileNotSavedException : Exception
    {
        public ConfigurationFileNotSavedException()
        {
        }

        public ConfigurationFileNotSavedException(string message)
            : base(message)
        {
        }

        public ConfigurationFileNotSavedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}