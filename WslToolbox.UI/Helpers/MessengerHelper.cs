using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Core.Services;
using WslToolbox.UI.Messengers;
using WslToolbox.UI.Models;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Helpers;

public static class MessengerHelper
{
    public static void ShowInfoBar(this IMessenger messenger, string? message = null, InfoBarSeverity severity = InfoBarSeverity.Informational, string title = "")
    {
        messenger.Send(new InfoBarChangedMessage(new InfoBarModel
        {
            IsOpen = true,
            IsClosable = true,
            IsIconVisible = true,
            Severity = severity,
            Title = title,
            Message = message
        }));
    }

    public static void ProgressChanged(this IMessenger messenger, UserProgressChangedEventArgs userProgress)
    {
        messenger.Send(new ProgressChangedMessage(userProgress));
    }

    public static void HideInfoBar(this IMessenger messenger)
    {
        messenger.Send(new InfoBarChangedMessage(new InfoBarModel {IsOpen = false}));
    }

    public static async Task<ContentDialogResult> ShowDialog(this IMessenger messenger, SimpleDialogModel viewModel)
    {
        return await messenger.Send(new SimpleDialogShowMessage(viewModel));
    }

    public static async Task<ContentDialogResult> ShowDialog(this IMessenger messenger,
        string title,
        string message,
        string primaryButtonText = "OK",
        string secondaryButtonText = ""
    )
    {
        return await messenger.Send(new SimpleDialogShowMessage(new SimpleDialogModel
        {
            Message = message,
            Title = title,
            PrimaryButtonText = primaryButtonText,
            SecondaryButtonText = secondaryButtonText,
            PrimaryButtonCommand = null,
            SecondaryButtonCommand = null
        }));
    }

    public static async Task<InputDialogModel> ShowInputDialog(this IMessenger messenger,
        string title,
        string message,
        string initialValue = "",
        string primaryButtonText = "OK",
        string secondaryButtonText = ""
    )
    {
        var dialog = await messenger.Send(new InputDialogMessage(new InputDialogModel
        {
            Message = message,
            Title = title,
            Result = initialValue,
            PrimaryButtonText = primaryButtonText,
            SecondaryButtonText = secondaryButtonText
        }));

        return dialog;
    }

    public static async Task ShowStartupDialogAsync(this IMessenger messenger, StartupDialogViewModel viewModel)
    {
        await messenger.Send(new StartupDialogShowMessage(viewModel));
    }

    public static async Task<ContentDialogResult> ShowUpdateDialog(this IMessenger messenger, UpdateViewModel viewModel)
    {
        return await messenger.Send(new UpdateDialogMessage(viewModel));
    }

    public static void ShowUpdateInfoBar(this IMessenger messenger, string message, string title = "", InfoBarSeverity severity = InfoBarSeverity.Informational)
    {
        messenger.Send(new UpdateInfoBarChangedMessage(new InfoBarModel
        {
            IsOpen = true,
            IsClosable = true,
            IsIconVisible = true,
            Severity = severity,
            Title = title,
            Message = message
        }));
    }

    public static async Task<ImportDialogViewModel> ShowImportDialog(this IMessenger messenger, string name, string directory)
    {
        var dialog = await messenger.Send(new ImportDialogMessage(new ImportDialogViewModel
        {
            Name = name,
            Directory = directory
        }));

        return dialog;
    }

    public static async Task<MoveDialogViewModel> ShowMoveDialog(this IMessenger messenger, Distribution distribution)
    {
        var dialog = await messenger.Send(new MoveDialogMessage(new MoveDialogViewModel
        {
            Distribution = distribution
        }));

        return dialog;
    }
}