namespace System.Windows.Controls
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    public partial class IconWebTextBox : UserControl
    {
        public IconWebTextBox()
        {
            this.InitializeComponent();
            this.Background = Brushes.White;
        }

        #region Icon (links)

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(
                nameof(Icon),
                typeof(DrawingImage),
                typeof(IconWebTextBox),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Das linke Symbol.
        /// Ist kein Icon gesetzt, wird das Standardsymbol T angezeigt.
        /// </summary>
        public DrawingImage Icon
        {
            get => (DrawingImage)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        #endregion

        #region IconBackground (links)

        public static readonly DependencyProperty IconBackgroundProperty =
            DependencyProperty.Register(
                nameof(IconBackground),
                typeof(Brush),
                typeof(IconWebTextBox),
                new FrameworkPropertyMetadata(Brushes.LightBlue));

        /// <summary>
        /// Hintergrundfarbe des linken Symbolbereichs.
        /// </summary>
        public Brush IconBackground
        {
            get => (Brush)GetValue(IconBackgroundProperty);
            set => SetValue(IconBackgroundProperty, value);
        }

        #endregion

        #region WebIcon (rechts)

        public static readonly DependencyProperty WebIconProperty =
            DependencyProperty.Register(
                nameof(WebIcon),
                typeof(DrawingImage),
                typeof(IconWebTextBox),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Das rechte Symbol zum Öffnen einer Webadresse.
        /// Ist kein Icon gesetzt, wird das Standardsymbol ↗ angezeigt.
        /// </summary>
        public DrawingImage WebIcon
        {
            get => (DrawingImage)GetValue(WebIconProperty);
            set => SetValue(WebIconProperty, value);
        }

        #endregion

        #region WebIconBackground (rechts)

        public static readonly DependencyProperty WebIconBackgroundProperty =
            DependencyProperty.Register(
                nameof(WebIconBackground),
                typeof(Brush),
                typeof(IconWebTextBox),
                new FrameworkPropertyMetadata(Brushes.LightBlue));

        /// <summary>
        /// Hintergrundfarbe des rechten Symbolbereichs.
        /// </summary>
        public Brush WebIconBackground
        {
            get => (Brush)GetValue(WebIconBackgroundProperty);
            set => SetValue(WebIconBackgroundProperty, value);
        }

        #endregion

        #region WebIconForeground (rechts)

        public static readonly DependencyProperty WebIconForegroundProperty =
            DependencyProperty.Register(
                nameof(WebIconForeground),
                typeof(Brush),
                typeof(IconWebTextBox),
                new FrameworkPropertyMetadata(Brushes.Gray));

        /// <summary>
        /// Vordergrundfarbe des rechten Standardsymbols ↗.
        /// Wird nicht zum Einfärben eines DrawingImage verwendet.
        /// </summary>
        public Brush WebIconForeground
        {
            get => (Brush)GetValue(WebIconForegroundProperty);
            set => SetValue(WebIconForegroundProperty, value);
        }

        #endregion

        #region Text

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(IconWebTextBox),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Der eingegebene Text beziehungsweise die Webadresse.
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        #endregion

        #region Watermark

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register(
                nameof(Watermark),
                typeof(string),
                typeof(IconWebTextBox),
                new FrameworkPropertyMetadata("Webadresse"));

        /// <summary>
        /// Der Wasserzeichentext.
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
                typeof(IconWebTextBox),
                new FrameworkPropertyMetadata(Brushes.Gray));

        /// <summary>
        /// Die Farbe des Wasserzeichens.
        /// </summary>
        public Brush WatermarkForeground
        {
            get => (Brush)GetValue(WatermarkForegroundProperty);
            set => SetValue(WatermarkForegroundProperty, value);
        }

        #endregion

        #region Webadresse öffnen

        /// <summary>
        /// Öffnet eine gültige HTTP- oder HTTPS-Webadresse
        /// im Standardbrowser.
        /// Unterstützt Adressen mit www., http:// und https://.
        /// </summary>
        private void OpenWebAddress()
        {
            string address = Text?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(address))
            {
                return;
            }

            // Falls kein Protokoll angegeben wurde, HTTPS ergänzen.
            if (!address.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !address.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                address = $"https://{address}";
            }

            // Adresse validieren.
            if (!Uri.TryCreate(address, UriKind.Absolute, out Uri uri))
            {
                return;
            }

            // Ausschließlich HTTP und HTTPS zulassen.
            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            {
                return;
            }

            // Host muss vorhanden sein und einen Punkt enthalten.
            if (string.IsNullOrWhiteSpace(uri.Host) || !uri.Host.Contains("."))
                return;

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = uri.AbsoluteUri,
                    UseShellExecute = true
                });
            }
            catch (Win32Exception)
            {
                // Der Browser konnte nicht gestartet werden.
            }
            catch (InvalidOperationException)
            {
                // Der Startvorgang konnte nicht ausgeführt werden.
            }
            catch (ArgumentException)
            {
                // Ungültige Argumente für den Startvorgang.
            }
        }

        /// <summary>
        /// Reagiert auf einen Doppelklick auf das rechte Symbol.
        /// </summary>
        private void WebIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2)
            {
                return;
            }

            OpenWebAddress();

            e.Handled = true;
        }

        #endregion
    }
}