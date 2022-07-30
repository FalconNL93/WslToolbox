using System.Windows;
using System.Windows.Controls;

namespace WslToolbox.Gui2.UserControls;

public partial class CardOption : UserControl
{
    public static readonly DependencyProperty CardOptionProperty =
        DependencyProperty.Register("Enabled", typeof(bool), typeof(CardOption), new FrameworkPropertyMetadata {BindsTwoWayByDefault = true});

    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register("HeaderContent", typeof(string), typeof(CardOption));

    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register("DescriptionContent", typeof(string), typeof(CardOption));

    public CardOption()
    {
        InitializeComponent();
    }

    public bool Enabled
    {
        get => (bool) GetValue(CardOptionProperty);
        set => SetValue(CardOptionProperty, value);
    }

    public string HeaderContent
    {
        get => (string) GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public string DescriptionContent
    {
        get => (string) GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }
}