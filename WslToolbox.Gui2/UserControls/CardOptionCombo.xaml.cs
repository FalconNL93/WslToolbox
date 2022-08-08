using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WslToolbox.Gui2.UserControls;

public partial class CardOptionCombo : UserControl
{
    public static readonly DependencyProperty CardOptionOptions = DependencyProperty.Register(nameof(Options),
        typeof(IEnumerable<string>), typeof(CardOptionCombo),
        new UIPropertyMetadata(default(List<string>)));

    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register(nameof(HeaderContent), nameof(HeaderContent).GetType(), typeof(CardOptionCombo));

    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register(nameof(DescriptionContent), nameof(DescriptionContent).GetType(),
            typeof(CardOptionCombo));

    private readonly DependencyProperty _cardOptionProperty = DependencyProperty.Register(nameof(Value),
        nameof(Value).GetType(), typeof(CardOptionCombo), new FrameworkPropertyMetadata {BindsTwoWayByDefault = true});

    public CardOptionCombo()
    {
        InitializeComponent();
    }

    public string Value
    {
        get => (string) GetValue(_cardOptionProperty);
        set => SetValue(_cardOptionProperty, value);
    }

    public IEnumerable<string> Options
    {
        get => (IEnumerable<string>) GetValue(CardOptionOptions);
        set => SetValue(CardOptionOptions, value);
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