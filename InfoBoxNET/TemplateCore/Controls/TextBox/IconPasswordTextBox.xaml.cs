namespace System.Windows.Controls
{
    using System.Windows.Media;

    public partial class IconPasswordTextBox : UserControl
    {
        private bool _isUpdating;

        public IconPasswordTextBox()
        {
            this.InitializeComponent();
        }

        #region Icon

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(
                nameof(Icon),
                typeof(DrawingImage),
                typeof(IconPasswordTextBox),
                new PropertyMetadata(null));

        public DrawingImage Icon
        {
            get => (DrawingImage)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        #endregion

        #region Password

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                nameof(Password),
                typeof(string),
                typeof(IconPasswordTextBox),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnPasswordChanged));

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not IconPasswordTextBox control)
            {
                return;
            }

            control.UpdatePasswordControls();
            control.UpdatePasswordStrength();
        }

        #endregion

        #region Watermark

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register(
                nameof(Watermark),
                typeof(string),
                typeof(IconPasswordTextBox),
                new PropertyMetadata("Passwort"));

        public string Watermark
        {
            get => (string)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        #endregion

        #region WatermarkForeground

        public static readonly DependencyProperty WatermarkForegroundProperty =
            DependencyProperty.Register(
                nameof(WatermarkForeground),
                typeof(Brush),
                typeof(IconPasswordTextBox),
                new PropertyMetadata(Brushes.Gray));

        public Brush WatermarkForeground
        {
            get => (Brush)GetValue(WatermarkForegroundProperty);
            set => SetValue(WatermarkForegroundProperty, value);
        }

        #endregion

        #region PasswordChar

        public static readonly DependencyProperty PasswordCharProperty =
            DependencyProperty.Register(
                nameof(PasswordChar),
                typeof(char),
                typeof(IconPasswordTextBox),
                new PropertyMetadata('*'));

        public char PasswordChar
        {
            get => (char)GetValue(PasswordCharProperty);
            set => SetValue(PasswordCharProperty, value);
        }

        #endregion

        #region IsPasswordVisible

        public static readonly DependencyProperty IsPasswordVisibleProperty =
            DependencyProperty.Register(
                nameof(IsPasswordVisible),
                typeof(bool),
                typeof(IconPasswordTextBox),
                new PropertyMetadata(false, OnIsPasswordVisibleChanged));

        public bool IsPasswordVisible
        {
            get => (bool)GetValue(IsPasswordVisibleProperty);
            set => SetValue(IsPasswordVisibleProperty, value);
        }

        private static void OnIsPasswordVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IconPasswordTextBox control)
            {
                control.UpdateVisibility();
            }
        }

        #endregion

        #region PasswordStrength

        public enum PasswordQuality
        {
            Weak,
            Medium,
            Strong
        }

        private static readonly DependencyPropertyKey PasswordStrengthPropertyKey =
            DependencyProperty.RegisterReadOnly(
                nameof(PasswordStrength),
                typeof(PasswordQuality),
                typeof(IconPasswordTextBox),
                new PropertyMetadata(PasswordQuality.Weak));

        public static readonly DependencyProperty PasswordStrengthProperty = PasswordStrengthPropertyKey.DependencyProperty;

        public PasswordQuality PasswordStrength => (PasswordQuality)GetValue(PasswordStrengthProperty);

        #endregion

        #region PasswordStrengthBrush

        private static readonly DependencyPropertyKey PasswordStrengthBrushPropertyKey =
            DependencyProperty.RegisterReadOnly(
                nameof(PasswordStrengthBrush),
                typeof(Brush),
                typeof(IconPasswordTextBox),
                new PropertyMetadata(Brushes.OrangeRed));

        public static readonly DependencyProperty PasswordStrengthBrushProperty = PasswordStrengthBrushPropertyKey.DependencyProperty;

        public Brush PasswordStrengthBrush => (Brush)GetValue(PasswordStrengthBrushProperty);

        #endregion

        #region PasswordChanged

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this._isUpdating == true)
            {
                return;
            }

            string password = PART_PasswordBox.Password;

            if (Password != password)
            {
                this.SetCurrentValue(PasswordProperty, password);
            }

            this.UpdatePasswordStrength();
        }

        #endregion

        #region PasswordControls

        private void UpdatePasswordControls()
        {
            if (this.PART_PasswordBox == null || this.PART_TextBox == null)
            {
                return;
            }

            string password = this.Password ?? string.Empty;

            _isUpdating = true;

            try
            {
                int caretIndex = this.PART_TextBox.CaretIndex;

                if (this.PART_PasswordBox.Password != password)
                {
                    this.PART_PasswordBox.Password = password;
                }

                if (this.PART_TextBox.Text != password)
                {
                    this.PART_TextBox.Text = password;
                }

                this.PART_TextBox.CaretIndex = Math.Min(caretIndex, this.PART_TextBox.Text.Length);
            }
            finally
            {
                this._isUpdating = false;
            }
        }

        #endregion

        #region Visibility

        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

        private void UpdateVisibility()
        {
            if (this.PART_PasswordBox == null || this.PART_TextBox == null)
            {
                return;
            }

            if (this.IsPasswordVisible == true)
            {
                this.PART_PasswordBox.Visibility = Visibility.Collapsed;
                this.PART_TextBox.Visibility = Visibility.Visible;

                this.PART_ToggleButton.Content = "◌";
                this.PART_ToggleButton.ToolTip = "Passwort verbergen";

                this.PART_TextBox.Focus();
                this.PART_TextBox.CaretIndex = this.PART_TextBox.Text.Length;
            }
            else
            {
                this.PART_PasswordBox.Visibility = Visibility.Visible;
                this.PART_TextBox.Visibility = Visibility.Collapsed;

                this.PART_ToggleButton.Content = "◉";
                this.PART_ToggleButton.ToolTip = "Passwort anzeigen";

                this.PART_PasswordBox.Focus();
            }
        }

        #endregion

        #region PasswordStrength

        private void UpdatePasswordStrength()
        {
            string password = this.Password ?? string.Empty;

            int score = 0;

            if (password.Length >= 10)
                score++;

            if (password.Any(char.IsDigit))
                score++;

            if (password.Any(char.IsUpper))
                score++;

            if (password.Any(char.IsLower))
                score++;

            if (password.Any(c => !char.IsLetterOrDigit(c)))
                score++;

            PasswordQuality quality;
            Brush brush;

            if (score == 5)
            {
                quality = PasswordQuality.Strong;
                brush = new SolidColorBrush(Color.FromRgb(22, 163, 74));
            }
            else if (score >= 3)
            {
                quality = PasswordQuality.Medium;
                brush = new SolidColorBrush(Color.FromRgb(229, 184, 46));
            }
            else
            {
                quality = PasswordQuality.Weak;
                brush = new SolidColorBrush(Color.FromRgb(232, 93, 53));
            }

            this.SetValue(PasswordStrengthPropertyKey, quality);
            this.SetValue(PasswordStrengthBrushPropertyKey, brush);
        }

        #endregion
    }
}
