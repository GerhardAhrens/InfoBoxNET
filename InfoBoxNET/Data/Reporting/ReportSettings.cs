namespace InfoBoxNET.Data
{
    using System;
    using System.Globalization;
    using System.Windows;

    public enum ReportOrientation
    {
        Portrait,
        Landscape
    }

    public sealed class ReportSettings
    {
        // A4 bei 96 DPI
        public Size PaperSize { get; set; } = new Size(793.7008, 1122.5197);

        public ReportOrientation Orientation { get; set; } = ReportOrientation.Portrait;

        public Thickness Margins { get; set; } = new Thickness(40);

        public double HeaderHeight { get; set; } = 65;

        public double FooterHeight { get; set; } = 30;

        public double ColumnHeaderHeight { get; set; } = 28;

        public double RowHeight { get; set; } = 27;

        public string Title { get; set; } = "Report";

        public string NullText { get; set; } = "";

        public CultureInfo Culture { get; set; } = CultureInfo.GetCultureInfo("de-DE");

        public string PrintJobName { get; set; } = "Report";

        public Brush HeaderBackground { get; set; } = Brushes.LightSteelBlue;

        public Brush GridBrush { get; set; } = Brushes.LightGray;

        public Brush TextBrush { get; set; } = Brushes.Black;

        public Brush HeaderTextBrush { get; set; } = Brushes.Black;

        public double FontSize { get; set; } = 10;

        public double HeaderFontSize { get; set; } = 10;

        public double TitleFontSize { get; set; } = 18;

        // Parameter: aktuelle Seite, Gesamtseitenzahl.
        // Pro Seite muss eine neue UIElement-Instanz erzeugt werden.
        public Func<int, int, UIElement> HeaderFactory { get; set; }

        public Func<int, int, UIElement> FooterFactory { get; set; }

        public Size EffectivePaperSize
        {
            get
            {
                var size = PaperSize;

                return Orientation == ReportOrientation.Landscape
                    ? new Size(
                        Math.Max(size.Width, size.Height),
                        Math.Min(size.Width, size.Height))
                    : new Size(
                        Math.Min(size.Width, size.Height),
                        Math.Max(size.Width, size.Height));
            }
        }
    }
}
