namespace System.Windows.Controls
{

    [TemplatePart(Name = "PART_DecreaseButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_IncreaseButton", Type = typeof(Button))]
    public class ZoomControl : Control
    {
        static ZoomControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZoomControl), new FrameworkPropertyMetadata(typeof(ZoomControl)));
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(double),
                typeof(ZoomControl),
                new FrameworkPropertyMetadata(
                    100.0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnValueChanged,
                    CoerceValue));

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register(
                nameof(MinValue),
                typeof(double),
                typeof(ZoomControl),
                new PropertyMetadata(25.0, OnRangeChanged));

        public double MinValue
        {
            get => (double)GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register(
                nameof(MaxValue),
                typeof(double),
                typeof(ZoomControl),
                new PropertyMetadata(200.0, OnRangeChanged));

        public double MaxValue
        {
            get => (double)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register(
                nameof(Step),
                typeof(double),
                typeof(ZoomControl),
                new PropertyMetadata(25.0));

        public double Step
        {
            get => (double)GetValue(StepProperty);
            set => SetValue(StepProperty, value);
        }

        public static readonly DependencyProperty IsDecreaseEnabledProperty =
            DependencyProperty.Register(
                nameof(IsDecreaseEnabled),
                typeof(bool),
                typeof(ZoomControl),
                new PropertyMetadata(true));

        public bool IsDecreaseEnabled
        {
            get => (bool)GetValue(IsDecreaseEnabledProperty);
            private set => SetValue(IsDecreaseEnabledProperty, value);
        }

        public static readonly DependencyProperty IsIncreaseEnabledProperty =
            DependencyProperty.Register(
                nameof(IsIncreaseEnabled),
                typeof(bool),
                typeof(ZoomControl),
                new PropertyMetadata(true));

        public bool IsIncreaseEnabled
        {
            get => (bool)GetValue(IsIncreaseEnabledProperty);
            private set => SetValue(IsIncreaseEnabledProperty, value);
        }

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("PART_DecreaseButton") is Button decreaseButton)
            {
                decreaseButton.Click -= DecreaseButton_Click;
                decreaseButton.Click += DecreaseButton_Click;
            }

            if (GetTemplateChild("PART_IncreaseButton") is Button increaseButton)
            {
                increaseButton.Click -= IncreaseButton_Click;
                increaseButton.Click += IncreaseButton_Click;
            }

            base.OnApplyTemplate();

            UpdateButtonStates();
        }

        private static object CoerceValue(DependencyObject d, object baseValue)
        {
            var control = (ZoomControl)d;

            double value = (double)baseValue;
            double min = control.MinValue;
            double max = control.MaxValue;

            if (min > max)
                return value;

            return Math.Max(min, Math.Min(max, value));
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ZoomControl)d).UpdateButtonStates();
        }

        private static void OnRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ZoomControl)d;

            control.CoerceValue(ValueProperty);
            control.UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            IsDecreaseEnabled = Value > MinValue;
            IsIncreaseEnabled = Value < MaxValue;
        }

        public void Increase()
        {
            if (Step <= 0 || Value >= MaxValue)
                return;

            Value = Math.Min(MaxValue, Value + Step);
        }

        public void Decrease()
        {
            if (Step <= 0 || Value <= MinValue)
                return;

            Value = Math.Max(MinValue, Value - Step);
        }

        private void DecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            Decrease();
        }

        private void IncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            Increase();
        }
    }
}
