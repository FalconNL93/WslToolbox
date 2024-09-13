using WslToolbox.UI.Core.Contracts;
using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.Core.Configurations;

public abstract class WslSettingSectionBase : IWslSettingSection
{
    public abstract string SectionName { get; }
    public abstract List<WslSetting> Settings { get; }
}