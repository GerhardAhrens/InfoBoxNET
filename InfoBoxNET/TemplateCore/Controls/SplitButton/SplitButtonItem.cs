namespace System.Windows.Controls
{
    using System.Windows;
    using System.Windows.Media;

    public class SplitButtonItem : Button
    {
        static SplitButtonItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SplitButtonItem),
                new FrameworkPropertyMetadata(
                    typeof(SplitButtonItem)));
        }

        #region Icon

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(
                nameof(Icon),
                typeof(DrawingImage),
                typeof(SplitButtonItem),
                new PropertyMetadata(null));

        public DrawingImage Icon
        {
            get => (DrawingImage)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        #endregion

        #region Description

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(
                nameof(Description),
                typeof(string),
                typeof(SplitButtonItem),
                new PropertyMetadata(null));

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        #endregion
    }
}
