using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.Core.Contracts;

public interface IWslSettingSection
{
    List<WslSetting> Settings { get; }
}