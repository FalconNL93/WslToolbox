using System;
using System.Threading.Tasks;
using Wpf.Ui.Controls.Interfaces;
using WslToolbox.Gui2.Models;

namespace WslToolbox.Gui2.Extensions;

public static class DialogControlExtension
{
    public static async Task ShowAndWaitAsync(
        this IDialogControl dialogControl,
        DialogControlModel model,
        Action? primaryButtonAction = null,
        Action? secondaryButtonAction = null
    )
    {
        dialogControl.ButtonLeftName = model.PrimaryButtonName;
        dialogControl.ButtonRightName = model.SecondaryButtonName;
        dialogControl.Content = model.Content;

        if (primaryButtonAction != null)
        {
            dialogControl.ButtonLeftClick += (_, _) =>
            {
                primaryButtonAction();
                dialogControl.Hide();
            };
        }
        else
        {
            dialogControl.ButtonLeftClick += (_, _) => dialogControl.Hide();
        }

        if (secondaryButtonAction != null)
        {
            dialogControl.ButtonRightClick += (_, _) =>
            {
                secondaryButtonAction();
                dialogControl.Hide();
            };
        }
        else
        {
            dialogControl.ButtonRightClick += (_, _) => dialogControl.Hide();
        }

        await dialogControl.ShowAndWaitAsync();
    }
}