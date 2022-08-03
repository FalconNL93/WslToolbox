using System.Windows;
using System.Windows.Controls;

namespace WslToolbox.Gui2.UserControls;

public partial class CardOptionString : UserControl
{
    private readonly DependencyProperty _cardOptionProperty = DependencyProperty.Register(nameof(Value), nameof(Value).GetType(), typeof(CardOptionString), new FrameworkPropertyMetadata {BindsTwoWayByDefault = true});

    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register(nameof(HeaderContent), nameof(HeaderContent).GetType(), typeof(CardOptionString));

    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register(nameof(DescriptionContent), nameof(DescriptionContent).GetType(), typeof(CardOptionString));

    public CardOptionString()
    {
        InitializeComponent();
    }

    public string Value
    {
        get => (string) GetValue(_cardOptionProperty);
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