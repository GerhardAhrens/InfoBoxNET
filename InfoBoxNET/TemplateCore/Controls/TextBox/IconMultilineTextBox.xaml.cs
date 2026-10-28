namespace System.Windows.Controls
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Interaktionslogik für IconMultilineTextBox.xaml
    /// </summary>
    public partial class IconMultilineTextBox : UserControl
    {
        public IconMultilineTextBox()
        {
            InitializeComponent();
        }

        #region Icon

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(
                nameof(Icon),
                typeof(DrawingImage),
                typeof(IconMultilineTextBox),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Das anzuzeigende Icon.
        /// Ist kein Icon gesetzt, wird das Standardsymbol T angezeigt.
        /// </summary>
        public DrawingImage Icon
        {
            get => (DrawingImage)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        #endregion

        #region IconBackground

        public static readonly DependencyProperty IconBackgroundProperty =
            DependencyProperty.Register(
                nameof(IconBackground),
                typeof(Brush),
                typeof(IconMultilineTextBox),
                new FrameworkPropertyMetadata(Brushes.LightBlue));

        /// <summary>
        /// Die Hintergrundfarbe des Symbolbereichs.
        /// Standard: LightBlue.
        /// </summary>
        public Brush IconBackground
        {
            get => (Brush)GetValue(IconBackgroundProperty);
            set => SetValue(IconBackgroundProperty, value);
        }

        #endregion

        #region Text

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(IconMultilineTextBox),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnTextChanged));

        /// <summary>
        /// Der eingegebene Text.
        /// Unterstützt die Two-Way-Datenbindung.
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private static void OnTextChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is IconMultilineTextBox control)
            {
                string text = e.NewValue as string ?? string.Empty;

                control.SetValue(
                    CharacterCountPropertyKey,
                    text.Length);
            }
        }

        #endregion

        #region Watermark

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register(
                nameof(Watermark),
                typeof(string),
                typeof(IconMultilineTextBox),
                new FrameworkPropertyMetadata("Beschreibung"));

        /// <summary>
        /// Der Wasserzeichentext.
        /// Standard: Beschreibung.
        /// </summary>
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
                typeof(IconMultilineTextBox),
                new FrameworkPropertyMetadata(Brushes.Gray));

        /// <summary>
        /// Die Farbe des Wasserzeichens.
        /// Standard: Grau.
        /// </summary>
        public Brush WatermarkForeground
        {
            get => (Brush)GetValue(WatermarkForegroundProperty);
            set => SetValue(WatermarkForegroundProperty, value);
        }

        #endregion

        #region MaxLength

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register(
                nameof(MaxLength),
                typeof(int),
                typeof(IconMultilineTextBox),
                new FrameworkPropertyMetadata(1000));

        /// <summary>
        /// Die maximal zulässige Zeichenanzahl.
        /// Standard: 1000.
        /// </summary>
        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        #endregion

        #region VisibleLines

        public static readonly DependencyProperty VisibleLinesProperty =
            DependencyProperty.Register(
                nameof(VisibleLines),
                typeof(int),
                typeof(IconMultilineTextBox),
                new FrameworkPropertyMetadata(3));

        /// <summary>
        /// Die Anzahl der sichtbaren Textzeilen.
        /// Standard: 3.
        /// Bei weiteren Zeilen wird bei Bedarf gescrollt.
        /// </summary>
        public int VisibleLines
        {
            get => (int)GetValue(VisibleLinesProperty);
            set => SetValue(VisibleLinesProperty, value);
        }

        #endregion

        #region CharacterCount

        private static readonly DependencyPropertyKey CharacterCountPropertyKey =
            DependencyProperty.RegisterReadOnly(
                nameof(CharacterCount),
                typeof(int),
                typeof(IconMultilineTextBox),
                new FrameworkPropertyMetadata(0));

        public static readonly DependencyProperty CharacterCountProperty =
            CharacterCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Die aktuelle Anzahl der Zeichen im Text.
        /// Wird automatisch aktualisiert.
        /// </summary>
        public int CharacterCount
        {
            get => (int)GetValue(CharacterCountProperty);
        }

        #endregion
    }
}
