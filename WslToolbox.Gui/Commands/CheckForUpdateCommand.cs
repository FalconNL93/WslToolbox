using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Handlers;

namespace WslToolbox.Gui.Commands
{
    public class CheckForUpdateCommand : GenericCommand
    {
        private readonly UpdateHandler _updateHandler;

        public CheckForUpdateCommand(UpdateHandler updateHandler)
        {
            _updateHandler = updateHandler;

            IsExecutable = _ => UpdateHandler.IsAvailable();
        }

        private static bool ValidUpdateFile()
        {
            var statusCode = UpdateHandler.HttpResponseCode(AppConfiguration.AppConfigurationUpdateXml);

            return statusCode == HttpStatusCode.OK;
        }

        public override void Execute(object parameter)
        {
            var showPrompt = (bool) parameter;

            if (!UpdateHandler.IsAvailable()) return;

            IsExecutable = _ => false;
            _updateHandler.CheckForUpdates(showPrompt);
            IsExecutable = _ => true;
        }
    }
}