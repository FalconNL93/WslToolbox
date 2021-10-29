using System;
using System.Windows;

namespace WslToolbox.Gui.Commands
{
    public class CopyToClipboardCommand : GenericCommand
    {
        public CopyToClipboardCommand()
        {
            IsExecutable = _ => true;
        }

        public override void Execute(object parameter)
        {
            try
            {
                Clipboard.SetText((string) parameter);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}