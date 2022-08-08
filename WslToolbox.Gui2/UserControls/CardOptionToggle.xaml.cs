using System.Windows;
using System.Windows.Controls;

namespace WslToolbox.Gui2.UserControls;

public partial class CardOptionToggle : UserControl
{
    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register(nameof(HeaderContent), nameof(HeaderContent).GetType(), typeof(CardOptionToggle));

    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register(nameof(DescriptionContent), nameof(DescriptionContent).GetType(),
            typeof(CardOptionToggle));

    private readonly DependencyProperty _cardOptionProperty = DependencyProperty.Register(nameof(Value),
        nameof(Value).GetType(), typeof(CardOptionToggle), new FrameworkPropertyMetadata {BindsTwoWayByDefault = true});

    public CardOptionToggle()
    {
        InitializeComponent();
    }

    public bool Value
    {
        get => (bool) GetValue(_cardOptionProperty);
        set => SetValue(_cardOptionProperty, value);
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