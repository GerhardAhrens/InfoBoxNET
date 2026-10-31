namespace System.Windows.Controls
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Microsoft.Win32;

    public partial class ImageBox : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty ImageBytesProperty =
            DependencyProperty.Register(
                nameof(ImageBytes),
                typeof(byte[]),
                typeof(ImageBox),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnImageChanged));

        public byte[] ImageBytes
        {
            get => (byte[])GetValue(ImageBytesProperty);
            set => SetValue(ImageBytesProperty, value);
        }

        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.Register(
                nameof(Symbol),
                typeof(DrawingImage),
                typeof(ImageBox),
                new FrameworkPropertyMetadata(
                    null,
                    OnImageChanged));

        public DrawingImage Symbol
        {
            get => (DrawingImage)GetValue(SymbolProperty);
            set => SetValue(SymbolProperty, value);
        }

        #endregion

        public ImageBox()
        {
            InitializeComponent();
            UpdateDisplay();
        }

        #region Display

        private static void OnImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageBox control)
                control.UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (DisplayImage == null)
                return;

            ImageSource source = null;

            // Bild hat Vorrang vor Symbol
            if (ImageBytes != null && ImageBytes.Length > 0)
            {
                try
                {
                    source = LoadBitmap(ImageBytes);
                }
                catch
                {
                    source = null;
                }
            }
            else if (Symbol != null)
            {
                source = Symbol;
            }

            DisplayImage.Source = source;

            Placeholder.Visibility =
                source == null
                    ? Visibility.Visible
                    : Visibility.Collapsed;
        }

        private static BitmapImage LoadBitmap(byte[] data)
        {
            using var stream = new MemoryStream(data);

            var bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = stream;
            bitmap.EndInit();

            bitmap.Freeze();

            return bitmap;
        }

        #endregion

        #region Context Menu

        private void ImageContextMenu_Opening(object sender, RoutedEventArgs e)
        {
            if (sender is not ContextMenu menu)
                return;

            bool hasImage =
                ImageBytes != null && ImageBytes.Length > 0;

            bool hasSymbol = Symbol != null;

            // Entfernen
            ((MenuItem)menu.Items[0]).IsEnabled = hasImage || hasSymbol;

            // Aus Zwischenablage einfügen
            ((MenuItem)menu.Items[2]).IsEnabled = Clipboard.ContainsImage();

            // In Zwischenablage kopieren
            ((MenuItem)menu.Items[3]).IsEnabled = hasImage;

            // Bild aus Datei laden
            ((MenuItem)menu.Items[5]).IsEnabled = true;
        }

        #endregion

        #region Remove

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            ImageBytes = null;
            Symbol = null;
        }

        #endregion

        #region Clipboard - Paste

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Clipboard.ContainsImage())
                    return;

                BitmapSource bitmap = Clipboard.GetImage();

                if (bitmap == null)
                    return;

                ImageBytes = ConvertToPng(bitmap);
                Symbol = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Das Bild konnte nicht eingefügt werden.\n\n{ex.Message}",
                    "ImageBox",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        #endregion

        #region Clipboard - Copy

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ImageBytes == null || ImageBytes.Length == 0)
                    return;

                BitmapSource bitmap = LoadBitmap(ImageBytes);

                Clipboard.SetImage(bitmap);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Das Bild konnte nicht kopiert werden.\n\n{ex.Message}", "ImageBox", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #endregion

        #region Load Image File

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Bild auswählen",
                Filter = "Bilddateien|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.tif;*.tiff|Alle Dateien|*.*",

                CheckFileExists = true,
                Multiselect = false
            };

            if (dialog.ShowDialog() != true)
                return;

            try
            {
                byte[] data = File.ReadAllBytes(dialog.FileName);

                // Bild vor dem Übernehmen validieren
                LoadBitmap(data);

                ImageBytes = data;
                Symbol = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Die Bilddatei konnte nicht geladen werden.\n\n{ex.Message}", "ImageBox", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #endregion

        #region Helper

        private static byte[] ConvertToPng(BitmapSource bitmap)
        {
            var encoder = new PngBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using var stream = new MemoryStream();

            encoder.Save(stream);

            return stream.ToArray();
        }

        #endregion
    }
}
