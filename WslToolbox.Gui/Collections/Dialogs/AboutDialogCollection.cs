using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ModernWpf.Controls;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Helpers;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections.Dialogs
{
    public static class AboutDialogCollection
    {
        public static IEnumerable<Control> Items(MainViewModel viewModel)
        {
            Control[] controls =
            {
                new Label {Content = AppConfiguration.AppName},
                new Label {Content = $"GUI Version: {AssemblyHelper.Version()} Build {AssemblyHelper.Build()}"},
                new Label {Content = $"Core Version: {Core.Helpers.AssemblyHelper.Version()}"},
                new HyperlinkButton
                {
                    Content = "Github",
                    NavigateUri = new Uri(AppConfiguration.GithubRepository)
                },
                new HyperlinkButton
                {
                    Content = "Docs",
                    NavigateUri = new Uri(AppConfiguration.GithubDocs)
                },
                new Label {Margin = new Thickness(0, 5, 0, 0), Content = $"{AppConfiguration.AppName} is using:"},
                new TextBox {Text = UsedPackages(), IsReadOnly = true}
            };

            return controls;
        }

        private static string UsedPackages()
        {
            var packages = new Dictionary<string, string>
            {
                {"AutoUpdater.NET", "https://github.com/ravibpatel/AutoUpdater.NET"},
                {"Command Line Parser Library", "https://github.com/commandlineparser/commandline"},
                {"Hardcodet NotifyIcon for WPF", "https://github.com/hardcodet/wpf-notifyicon"},
                {"ModernWPF UI Library", "https://github.com/Kinnara/ModernWpf"},
                {"Serilog", "https://github.com/serilog/serilog"}
            };

            return packages.Aggregate<KeyValuePair<string, string>, string>(null,
                (current, package) => $"{current}{package.Key} - {package.Value}\n");
        }
    }
}