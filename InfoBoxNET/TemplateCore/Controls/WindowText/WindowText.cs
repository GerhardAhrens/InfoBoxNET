namespace System.Windows.Controls
{
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Input;

    [TemplatePart(Name = "PART_TextBlock", Type = typeof(TextBlock))]
    public class WindowText : Control
    {
        static WindowText()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowText), new FrameworkPropertyMetadata(typeof(WindowText)));
        }

        public WindowText()
        {
            this.Focusable = false;
        }

        #region Text

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(WindowText),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        #endregion

        #region Typography

        public static readonly DependencyProperty StyleTypeProperty =
            DependencyProperty.Register(
                nameof(StyleType),
                typeof(WindowTextStyle),
                typeof(WindowText),
                new FrameworkPropertyMetadata(WindowTextStyle.Default, OnStyleTypeChanged));

        public WindowTextStyle StyleType
        {
            get => (WindowTextStyle)GetValue(StyleTypeProperty);
            set => SetValue(StyleTypeProperty, value);
        }

        private static void OnStyleTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WindowText control)
            {
                control.UpdateHyperlinkBehavior();
            }
        }

        new public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register(
                nameof(FontSize),
                typeof(double),
                typeof(WindowText),
                new FrameworkPropertyMetadata(14.0));

        new public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        new public static readonly DependencyProperty FontWeightProperty =
            DependencyProperty.Register(
                nameof(FontWeight),
                typeof(FontWeight),
                typeof(WindowText),
                new FrameworkPropertyMetadata(FontWeights.Normal));

        new public FontWeight FontWeight
        {
            get => (FontWeight)GetValue(FontWeightProperty);
            set => SetValue(FontWeightProperty, value);
        }

        new public static readonly DependencyProperty FontStyleProperty =
            DependencyProperty.Register(
                nameof(FontStyle),
                typeof(FontStyle),
                typeof(WindowText),
                new FrameworkPropertyMetadata(FontStyles.Normal));

        new public FontStyle FontStyle
        {
            get => (FontStyle)GetValue(FontStyleProperty);
            set => SetValue(FontStyleProperty, value);
        }

        new public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register(
                nameof(Foreground),
                typeof(Brush),
                typeof(WindowText),
                new FrameworkPropertyMetadata(Brushes.Black));

        new public Brush Foreground
        {
            get => (Brush)GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        #endregion

        #region Text Layout

        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register(
                nameof(TextAlignment),
                typeof(TextAlignment),
                typeof(WindowText),
                new FrameworkPropertyMetadata(TextAlignment.Left));

        public TextAlignment TextAlignment
        {
            get => (TextAlignment)GetValue(TextAlignmentProperty);
            set => SetValue(TextAlignmentProperty, value);
        }

        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register(
                nameof(TextWrapping),
                typeof(TextWrapping),
                typeof(WindowText),
                new FrameworkPropertyMetadata(TextWrapping.Wrap, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public TextWrapping TextWrapping
        {
            get => (TextWrapping)GetValue(TextWrappingProperty);
            set => SetValue(TextWrappingProperty, value);
        }

        public static readonly DependencyProperty TextTrimmingProperty =
            DependencyProperty.Register(
                nameof(TextTrimming),
                typeof(TextTrimming),
                typeof(WindowText),
                new FrameworkPropertyMetadata(TextTrimming.None));

        public TextTrimming TextTrimming
        {
            get => (TextTrimming)GetValue(TextTrimmingProperty);
            set => SetValue(TextTrimmingProperty, value);
        }

        public static readonly DependencyProperty LineHeightProperty =
            DependencyProperty.Register(
                nameof(LineHeight),
                typeof(double),
                typeof(WindowText),
                new FrameworkPropertyMetadata(double.NaN));

        public double LineHeight
        {
            get => (double)GetValue(LineHeightProperty);
            set => SetValue(LineHeightProperty, value);
        }

        public static readonly DependencyProperty LineStackingStrategyProperty =
            DependencyProperty.Register(
                nameof(LineStackingStrategy),
                typeof(LineStackingStrategy),
                typeof(WindowText),
                new FrameworkPropertyMetadata(LineStackingStrategy.MaxHeight));

        public LineStackingStrategy LineStackingStrategy
        {
            get => (LineStackingStrategy)GetValue(LineStackingStrategyProperty);
            set => SetValue(LineStackingStrategyProperty, value);
        }

        public static readonly DependencyProperty MaxLinesProperty =
            DependencyProperty.Register(
                nameof(MaxLines),
                typeof(int),
                typeof(WindowText),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public int MaxLines
        {
            get => (int)GetValue(MaxLinesProperty);
            set => SetValue(MaxLinesProperty, value);
        }
        #endregion

        #region Hyperlink

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(WindowText),
                new FrameworkPropertyMetadata(null));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                nameof(CommandParameter),
                typeof(object),
                typeof(WindowText));

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register(
                nameof(CommandTarget),
                typeof(IInputElement),
                typeof(WindowText));

        public IInputElement CommandTarget
        {
            get => (IInputElement)GetValue(CommandTargetProperty);
            set => SetValue(CommandTargetProperty, value);
        }

        private void UpdateHyperlinkBehavior()
        {
            bool isHyperlink = StyleType == WindowTextStyle.Hyperlink;

            Focusable = isHyperlink;
            Cursor = isHyperlink ? Cursors.Hand : Cursors.Arrow;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (StyleType == WindowTextStyle.Hyperlink)
            {
                if (IsEnabled && IsMouseOver)
                {
                    ExecuteCommand();
                }

                e.Handled = true;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (StyleType != WindowTextStyle.Hyperlink)
            {
                return;
            }

            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                ExecuteCommand();
                e.Handled = true;
            }
        }

        private void ExecuteCommand()
        {
            if (!IsEnabled)
            {
                return;
            }

            if (Command is RoutedCommand routedCommand)
            {
                IInputElement target = CommandTarget ?? this;

                if (routedCommand.CanExecute(CommandParameter, target))
                {
                    routedCommand.Execute(CommandParameter, target);
                }
            }
            else if (Command?.CanExecute(CommandParameter) == true)
            {
                Command.Execute(CommandParameter);
            }
        }

        #endregion
    }

    public class WindowTextMaxHeightConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3 || values[0] is not int maxLines || values[1] is not double fontSize || values[2] is not double lineHeight)
            {
                return double.PositiveInfinity;
            }

            if (maxLines <= 0)
                return double.PositiveInfinity;

            double effectiveLineHeight =
                double.IsNaN(lineHeight) || lineHeight <= 0
                    ? fontSize * 1.5
                    : lineHeight;

            return maxLines * effectiveLineHeight;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
