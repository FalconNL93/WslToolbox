using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WslToolbox.Gui.Exceptions
{
    class ConfigurationNotSaved : Exception
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
