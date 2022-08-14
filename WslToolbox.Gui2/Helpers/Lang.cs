using System;
using WslToolbox.Gui2.Resources;

namespace WslToolbox.Gui2.Helpers;

public static class Lang
{
    public static string? GetString(string key)
    {
        try
        {
            return localization.ResourceManager.GetString(key);
        }
        catch (Exception e)
        {
            return key;
        }
    }
}