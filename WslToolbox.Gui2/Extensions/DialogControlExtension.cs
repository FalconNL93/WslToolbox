using System;
using System.Threading.Tasks;
using Wpf.Ui.Controls.Interfaces;

namespace WslToolbox.Gui2.Extensions;

public static class DialogControlExtension
{
    public static async Task ShowAndWaitAsync
    (
        this IDialogControl dialogControl,
        string message = "",
        string title = "",
        object? content = null,
        Action? buttonRightAction = null,
        Action? buttonLeftAction = null,
        string buttonRightName = "Cancel",
        string buttonLeftName = "OK"
    )
    {
        dialogControl.ButtonRightName = buttonRightName;
        dialogControl.ButtonLeftName = buttonLeftName;
        dialogControl.Message = message;
        dialogControl.Title = title;
        dialogControl.Content = content;

        if (buttonLeftAction != null)
        {
            dialogControl.ButtonLeftClick += (_, _) => buttonLeftAction();
        }
        else
        {
            dialogControl.ButtonLeftClick += (_, _) => dialogControl.Hide();
        }

        if (buttonRightAction != null)
        {
            dialogControl.ButtonRightClick += (_, _) => buttonRightAction();
        }
        else
        {
            dialogControl.ButtonRightClick += (_, _) => dialogControl.Hide();
        }

        await dialogControl.ShowAndWaitAsync();
    }
}