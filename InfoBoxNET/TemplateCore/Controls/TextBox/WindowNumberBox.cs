namespace System.Windows.Controls
{
    using System.Globalization;
    using System.Windows.Input;
    using System.Windows.Media;

    public enum WindowNumberBoxType
    {
        Integer,
        Decimal,
        Currency
    }

    [TemplatePart(Name = "PART_ContentHost", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "PART_ClearButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_CurrencySymbol", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_Icon", Type = typeof(Image))]
    public class WindowNumberBox : TextBox
    {
        private TextBlock _currencySymbolText;
        private Image _iconImage;
        private Button _clearButton;
        private bool _updatingText;
        private bool _updatingValue;

        static WindowNumberBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowNumberBox), new FrameworkPropertyMetadata(typeof(WindowNumberBox)));
        }

        public WindowNumberBox()
        {
            Loaded += WindowNumberBox_Loaded;
            PreviewTextInput += WindowNumberBox_PreviewTextInput;
            LostFocus += WindowNumberBox_LostFocus;
            this.FontWeight = FontWeights.Normal;

            DataObject.AddPastingHandler(this, OnPaste);
        }

        #region Value

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(decimal?),
                typeof(WindowNumberBox),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnValueChanged));

        public decimal? Value
        {
            get => (decimal?)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WindowNumberBox box)
            {
                box.UpdateTextFromValue();
            }
        }

        #endregion

        #region NumberType

        public static readonly DependencyProperty NumberTypeProperty =
            DependencyProperty.Register(
                nameof(NumberType),
                typeof(WindowNumberBoxType),
                typeof(WindowNumberBox),
                new FrameworkPropertyMetadata(
                    WindowNumberBoxType.Decimal,
                    OnNumberTypeChanged));

        public WindowNumberBoxType NumberType
        {
            get => (WindowNumberBoxType)GetValue(NumberTypeProperty);
            set => SetValue(NumberTypeProperty, value);
        }

        private static void OnNumberTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WindowNumberBox box)
            {
                box.UpdateCurrencySymbol();
                box.UpdateTextFromValue();
            }
        }

        #endregion

        #region DecimalPlaces

        public static readonly DependencyProperty DecimalPlacesProperty =
            DependencyProperty.Register(
                nameof(DecimalPlaces),
                typeof(int),
                typeof(WindowNumberBox),
                new FrameworkPropertyMetadata(
                    2,
                    OnFormatChanged),
                value => (int)value >= 0 && (int)value <= 28);

        public int DecimalPlaces
        {
            get => (int)GetValue(DecimalPlacesProperty);
            set => SetValue(DecimalPlacesProperty, value);
        }

        #endregion

        #region FormatString

        public static readonly DependencyProperty FormatStringProperty =
            DependencyProperty.Register(
                nameof(FormatString),
                typeof(string),
                typeof(WindowNumberBox),
                new FrameworkPropertyMetadata(
                    null,
                    OnFormatChanged));

        public string FormatString
        {
            get => (string)GetValue(FormatStringProperty);
            set => SetValue(FormatStringProperty, value);
        }

        private static void OnFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WindowNumberBox box)
            {
                box.UpdateTextFromValue();
            }
        }

        #endregion

        #region Icon

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(
                nameof(Icon),
                typeof(DrawingImage),
                typeof(WindowNumberBox),
                new FrameworkPropertyMetadata(null));

        public DrawingImage Icon
        {
            get => (DrawingImage)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        private void UpdateIconVisibility()
        {
            if (_iconImage == null)
                return;

            _iconImage.Visibility = Icon != null
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
        #endregion

        #region CurrencySymbol

        public static readonly DependencyProperty CurrencySymbolProperty =
            DependencyProperty.Register(
                nameof(CurrencySymbol),
                typeof(string),
                typeof(WindowNumberBox),
                new FrameworkPropertyMetadata(string.Empty));

        public string CurrencySymbol
        {
            get => (string)GetValue(CurrencySymbolProperty);
            private set => SetValue(CurrencySymbolProperty, value);
        }

        private void UpdateCurrencySymbol()
        {
            if (_currencySymbolText == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(CurrencySymbol) == true)
            {
                var nfi = CultureInfo.CurrentCulture.NumberFormat;
                CurrencySymbol = nfi.CurrencySymbol;
            }

            _currencySymbolText.Text = CurrencySymbol ?? string.Empty;

            _currencySymbolText.Visibility = NumberType == WindowNumberBoxType.Currency ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

        #region PlaceholderText

        internal static readonly DependencyProperty PlaceholderVisibilityProperty =
            DependencyProperty.Register(
                nameof(PlaceholderVisibility),
                typeof(Visibility),
                typeof(WindowNumberBox),
                new FrameworkPropertyMetadata(Visibility.Visible));

        internal Visibility PlaceholderVisibility
        {
            get => (Visibility)GetValue(PlaceholderVisibilityProperty);
            set => SetValue(PlaceholderVisibilityProperty, value);
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register(
                nameof(PlaceholderText),
                typeof(string),
                typeof(WindowNumberBox),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    OnPlaceholderTextChanged));

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        private static void OnPlaceholderTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WindowNumberBox box)
            {
                box.UpdatePlaceholder();
            }
        }

        private void UpdatePlaceholder()
        {
            PlaceholderVisibility =
                string.IsNullOrEmpty(Text) &&
                !string.IsNullOrEmpty(PlaceholderText)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
        }

        #endregion

        #region PlaceholderForeground

        public static readonly DependencyProperty PlaceholderForegroundProperty =
            DependencyProperty.Register(
                nameof(PlaceholderForeground),
                typeof(Brush),
                typeof(WindowNumberBox),
                new FrameworkPropertyMetadata(
                    Brushes.LightGray,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush PlaceholderForeground
        {
            get => (Brush)GetValue(PlaceholderForegroundProperty);
            set => SetValue(PlaceholderForegroundProperty, value);
        }

        #endregion

        #region CornerRadius

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(WindowNumberBox),
                new FrameworkPropertyMetadata(new CornerRadius(5)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion

        #region ShowClearButton

        public static readonly DependencyProperty ShowClearButtonProperty =
            DependencyProperty.Register(
                nameof(ShowClearButton),
                typeof(bool),
                typeof(WindowNumberBox),
                new FrameworkPropertyMetadata(true));

        public bool ShowClearButton
        {
            get => (bool)GetValue(ShowClearButtonProperty);
            set => SetValue(ShowClearButtonProperty, value);
        }

        #endregion

        #region ClearButtonVisibility

        public static readonly DependencyProperty ClearButtonVisibilityProperty =
            DependencyProperty.Register(
                nameof(ClearButtonVisibility),
                typeof(Visibility),
                typeof(WindowNumberBox),
                new FrameworkPropertyMetadata(Visibility.Visible));

        public Visibility ClearButtonVisibility
        {
            get => (Visibility)GetValue(ClearButtonVisibilityProperty);
            set => SetValue(ClearButtonVisibilityProperty, value);
        }

        #endregion

        #region SelectAllOnFocus

        public static readonly DependencyProperty SelectAllOnFocusProperty =
            DependencyProperty.Register(
                nameof(SelectAllOnFocus),
                typeof(bool),
                typeof(WindowNumberBox),
                new FrameworkPropertyMetadata(false));

        public bool SelectAllOnFocus
        {
            get => (bool)GetValue(SelectAllOnFocusProperty);
            set => SetValue(SelectAllOnFocusProperty, value);
        }

        #endregion

        #region ClearButtonClicked

        public static readonly RoutedEvent ClearButtonClickedEvent =
            EventManager.RegisterRoutedEvent(
                nameof(ClearButtonClicked),
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(WindowNumberBox));

        public event RoutedEventHandler ClearButtonClicked
        {
            add => AddHandler(ClearButtonClickedEvent, value);
            remove => RemoveHandler(ClearButtonClickedEvent, value);
        }

        #endregion

        #region Template

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _clearButton = GetTemplateChild("PART_ClearButton") as Button;

            _currencySymbolText = GetTemplateChild("PART_CurrencySymbol") as TextBlock;

            _iconImage = GetTemplateChild("PART_Icon") as Image;

            if (_clearButton != null)
            {
                _clearButton.Click -= ClearButton_Click;
                _clearButton.Click += ClearButton_Click;
            }

            UpdateClearButton();
            UpdateCurrencySymbol();
            UpdateIconVisibility();
        }

        #endregion

        #region Loaded

        private void WindowNumberBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateClearButton();
            this.UpdateCurrencySymbol();
        }

        #endregion

        #region TextChanged

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            this.UpdateClearButton();
            this.UpdatePlaceholder();

            if (this._updatingText)
            {
                return;
            }

            this._updatingValue = true;

            try
            {
                if (string.IsNullOrWhiteSpace(Text))
                {
                    this.SetCurrentValue(ValueProperty, null);
                    return;
                }

                if (TryParseText(Text, out decimal result))
                {
                    this.SetCurrentValue(ValueProperty, result);
                }
            }
            finally
            {
                this._updatingValue = false;
            }
        }

        #endregion

        #region Clear

        new public void Clear()
        {
            if (IsReadOnly)
            {
                return;
            }

            RaiseEvent(new RoutedEventArgs(ClearButtonClickedEvent, this));

            Text = string.Empty;
            CaretIndex = 0;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Clear();
            e.Handled = true;
        }

        #endregion

        #region ClearButton

        private void UpdateClearButton()
        {
            if (_clearButton == null)
                return;

            bool visible =
                ShowClearButton &&
                ClearButtonVisibility == Visibility.Visible &&
                !IsReadOnly &&
                IsEnabled &&
                !string.IsNullOrEmpty(Text);

            _clearButton.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == IsEnabledProperty ||
                e.Property == IsReadOnlyProperty ||
                e.Property == ShowClearButtonProperty ||
                e.Property == ClearButtonVisibilityProperty)
            {
                UpdateClearButton();
            }
        }

        #endregion

        #region Focus

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);

            if (SelectAllOnFocus)
            {
                Dispatcher.BeginInvoke(new Action(SelectAll), System.Windows.Threading.DispatcherPriority.Input);
            }
        }

        #endregion

        #region Keyboard

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape && ShowClearButton && !string.IsNullOrEmpty(Text) && !IsReadOnly)
            {
                Clear();
                e.Handled = true;
                return;
            }

            base.OnKeyDown(e);
        }

        #endregion

        #region NumericInput

        private void WindowNumberBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsReadOnly)
            {
                return;
            }

            string newText = GetTextAfterInput(e.Text);

            e.Handled = !IsValidInput(newText);
        }

        private string GetTextAfterInput(string input)
        {
            string current = Text ?? string.Empty;

            return current
                .Remove(SelectionStart, SelectionLength)
                .Insert(SelectionStart, input);
        }

        private bool IsValidInput(string text)
        {
            if (string.IsNullOrEmpty(text))
                return true;

            var nfi = CultureInfo.CurrentCulture.NumberFormat;

            string decimalSeparator = nfi.NumberDecimalSeparator;
            string negativeSign = nfi.NegativeSign;

            if (text == negativeSign)
                return true;

            if (text.StartsWith(negativeSign))
                text = text.Substring(negativeSign.Length);

            if (text.Contains(negativeSign))
                return false;

            if (NumberType == WindowNumberBoxType.Integer)
                return IsDigitsOnly(text);

            int separatorIndex = text.IndexOf(decimalSeparator);

            if (separatorIndex >= 0)
            {
                if (text.IndexOf(
                    decimalSeparator,
                    separatorIndex + decimalSeparator.Length) >= 0)
                {
                    return false;
                }

                string fractionalPart =
                    text.Substring(
                        separatorIndex + decimalSeparator.Length);

                if (fractionalPart.Length > DecimalPlaces)
                    return false;
            }

            string normalized =
                text.Replace(decimalSeparator, string.Empty);

            return IsDigitsOnly(normalized);
        }

        private static bool IsDigitsOnly(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsDigit(c))
                    return false;
            }

            return true;
        }

        private void OnPaste(
            object sender,
            DataObjectPastingEventArgs e)
        {
            if (IsReadOnly ||
                !e.DataObject.GetDataPresent(DataFormats.Text))
            {
                e.CancelCommand();
                return;
            }

            string pastedText =
                e.DataObject.GetData(DataFormats.Text)?.ToString()
                ?? string.Empty;

            string newText = GetTextAfterInput(pastedText);

            if (!IsValidInput(newText))
                e.CancelCommand();
        }

        #endregion

        #region Parsing / Formatting

        private bool TryParseText(string text, out decimal result)
        {
            NumberStyles styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;

            if (NumberType == WindowNumberBoxType.Integer)
            {
                styles = NumberStyles.AllowLeadingSign;
            }

            return decimal.TryParse(text, styles, CultureInfo.CurrentCulture, out result);
        }

        private void UpdateTextFromValue()
        {
            if (_updatingValue || IsKeyboardFocused)
                return;

            _updatingText = true;

            try
            {
                if (Value == null)
                {
                    Text = string.Empty;
                    return;
                }

                Text = Value.Value.ToString(
                    GetEffectiveFormat(),
                    CultureInfo.CurrentCulture);
            }
            finally
            {
                _updatingText = false;
            }
        }

        private string GetEffectiveFormat()
        {
            if (!string.IsNullOrWhiteSpace(FormatString))
                return FormatString;

            return NumberType switch
            {
                WindowNumberBoxType.Integer => "N0",
                WindowNumberBoxType.Decimal => "N" + DecimalPlaces,
                WindowNumberBoxType.Currency => "N" + DecimalPlaces,
                _ => "N" + DecimalPlaces
            };
        }

        private void WindowNumberBox_LostFocus(
            object sender,
            RoutedEventArgs e)
        {
            UpdateTextFromValue();
        }

        #endregion
    }
}
