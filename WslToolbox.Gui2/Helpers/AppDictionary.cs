using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace WslToolbox.Gui2.Helpers;

public class AppDictionary : ResourceDictionary
{
    private readonly List<string> _dictionaries = new()
    {
        "DefaultDictionary.xaml",
        "SettingsDictionary.xaml",
    };
    
    public AppDictionary()
    {
        foreach (var dictionary in _dictionaries)
        {
            try
            {
                MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri($"pack://application:,,,/WslToolbox.Gui2;component/Resources/Dictionaries/{dictionary}", UriKind.Absolute)
                });
            }
            catch (Exception e)
            {
                Debug.Write($"Could not add dictionary {dictionary}: {e.Message}");
            }
        }
    }
}