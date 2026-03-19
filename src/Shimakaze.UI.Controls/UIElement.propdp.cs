
namespace Shimakaze.UI;

public partial class UIElement
{
    /// <summary>
    /// 获取或设置元素的宽度。
    /// </summary>
    public float Width
    {
        get => (float)GetValue(WidthProperty)!;
        set => SetValue(WidthProperty, value);
    }
    /// <summary>
    /// 标识 Width 依赖属性。
    /// </summary>
    public static readonly DependencyProperty WidthProperty =
        DependencyProperty.Register(nameof(Width), typeof(float), typeof(UIElement), new PropertyMetadata(float.NaN));


    public float MinWidth
    {
        get => (float)GetValue(MinWidthProperty)!;
        set => SetValue(MinWidthProperty, value);
    }
    public static readonly DependencyProperty MinWidthProperty =
        DependencyProperty.Register(nameof(MinWidth), typeof(float), typeof(UIElement), new PropertyMetadata(0f));


    public float MaxWidth
    {
        get => (float)GetValue(MaxWidthProperty)!;
        set => SetValue(MaxWidthProperty, value);
    }

    public static readonly DependencyProperty MaxWidthProperty =
        DependencyProperty.Register(nameof(MaxWidth), typeof(float), typeof(UIElement), new PropertyMetadata(float.PositiveInfinity));


    /// <summary>
    /// 获取或设置元素的高度。
    /// </summary>
    public float Height
    {
        get => (float)GetValue(HeightProperty)!;
        set => SetValue(HeightProperty, value);
    }
    /// <summary>
    /// 标识 Height 依赖属性。
    /// </summary>
    public static readonly DependencyProperty HeightProperty =
        DependencyProperty.Register(nameof(Height), typeof(float), typeof(UIElement), new PropertyMetadata(float.NaN));

    public float MinHeight
    {
        get => (float)GetValue(MinHeightProperty)!;
        set => SetValue(MinHeightProperty, value);
    }
    public static readonly DependencyProperty MinHeightProperty =
        DependencyProperty.Register(nameof(MinHeight), typeof(float), typeof(UIElement), new PropertyMetadata(0f));


    public float MaxHeight
    {
        get => (float)GetValue(MaxHeightProperty)!;
        set => SetValue(MaxHeightProperty, value);
    }

    public static readonly DependencyProperty MaxHeightProperty =
        DependencyProperty.Register(nameof(MaxHeight), typeof(float), typeof(UIElement), new PropertyMetadata(float.PositiveInfinity));


    /// <summary>
    /// 获取或设置元素的外边距。
    /// </summary>
    public Thickness Margin
    {
        get => (Thickness)GetValue(MarginProperty)!;
        set => SetValue(MarginProperty, value);
    }

    /// <summary>
    /// 标识 Margin 依赖属性。
    /// </summary>
    public static readonly DependencyProperty MarginProperty =
        DependencyProperty.Register(nameof(Margin), typeof(Thickness), typeof(UIElement), new PropertyMetadata(new Thickness()));

    /// <summary>
    /// 获取或设置一个值，该值指示元素是否启用。
    /// </summary>
    public bool IsEnabled
    {
        get => (bool)GetValue(IsEnabledProperty)!;
        set => SetValue(IsEnabledProperty, value);
    }

    /// <summary>
    /// 标识 IsEnabled 依赖属性。
    /// </summary>
    public static readonly DependencyProperty IsEnabledProperty =
        DependencyProperty.Register(nameof(IsEnabled), typeof(bool), typeof(UIElement), new PropertyMetadata(true));

    public HorizontalAlignment HorizontalAlignment
    {
        get { return (HorizontalAlignment)GetValue(HorizontalAlignmentProperty)!; }
        set { SetValue(HorizontalAlignmentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for HorizontalAlignment.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty HorizontalAlignmentProperty =
        DependencyProperty.Register(nameof(HorizontalAlignment), typeof(HorizontalAlignment), typeof(UIElement), new PropertyMetadata(HorizontalAlignment.Stretch));

    public VerticalAlignment VerticalAlignment
    {
        get { return (VerticalAlignment)GetValue(VerticalAlignmentProperty)!; }
        set { SetValue(VerticalAlignmentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for VerticalAlignment.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty VerticalAlignmentProperty =
        DependencyProperty.Register(nameof(VerticalAlignment), typeof(VerticalAlignment), typeof(UIElement), new PropertyMetadata(VerticalAlignment.Stretch));


}