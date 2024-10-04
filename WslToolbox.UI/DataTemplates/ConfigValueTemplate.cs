using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.DataTemplates
{
    public class ConfigValueTemplate : DataTemplateSelector
    {
        public DataTemplate StringTemplate { get; set; }
        public DataTemplate BoolTemplate { get; set; }

        public DataTemplate OptionsTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item == null)
            {
                return StringTemplate;
            }

            var wslSetting = (WslSetting) item;
            if (wslSetting.Options is not null)
            {
                return OptionsTemplate;
            }

            return wslSetting.Default is bool
                ? BoolTemplate
                : StringTemplate;
        }
    }
}