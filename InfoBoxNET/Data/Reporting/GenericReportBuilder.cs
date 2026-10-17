namespace InfoBoxNET.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    using InfoBoxNET.Data.Reporting;

    public static class GenericReportBuilder
    {
        public static FixedDocument Build<T>(ICollectionView view, ReportSettings settings = null)
        {
            ArgumentNullException.ThrowIfNull(view);

            settings ??= new ReportSettings();

            ValidateSettings(settings);

            var columns = ReportColumnFactory.GetColumns<T>();

            if (columns.Count == 0)
            {
                throw new InvalidOperationException($"Der Typ {typeof(T).Name} enthält keine druckbaren Properties mit PrintAttribute.");
            }

            // Momentaufnahme der aktuellen View.
            var items = view.Cast<T>().ToList();

            var paper = settings.EffectivePaperSize;
            var margins = settings.Margins;

            double contentWidth = paper.Width - margins.Left - margins.Right;

            double contentHeight = paper.Height - margins.Top - margins.Bottom;

            if (contentWidth <= 0 || contentHeight <= 0)
            {
                throw new InvalidOperationException("Papierformat und Seitenränder ergeben keine nutzbare Druckfläche.");
            }

            double availableRowsHeight = contentHeight - settings.HeaderHeight - settings.FooterHeight - settings.ColumnHeaderHeight;

            if (availableRowsHeight < settings.RowHeight)
            {
                throw new InvalidOperationException("Der verfügbare Seitenbereich ist zu klein für mindestens eine Tabellenzeile.");
            }

            int rowsPerPage = Math.Max(1, (int)Math.Floor(availableRowsHeight / settings.RowHeight));

            // Auch für eine leere Datenmenge eine Seite erzeugen.
            int pageCount = Math.Max(1, (int)Math.Ceiling(items.Count / (double)rowsPerPage));

            double[] widths = CalculateColumnWidths(columns, contentWidth);

            var document = new FixedDocument();

            document.DocumentPaginator.PageSize = paper;

            for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
            {
                var page = new FixedPage
                {
                    Width = paper.Width,
                    Height = paper.Height
                };

                // Kopfbereich
                FrameworkElement header = settings.HeaderFactory?.Invoke(
                    pageIndex + 1, pageCount)
                    ?? CreateDefaultHeader(settings, contentWidth);

                AddElement(page, header, margins.Left, margins.Top, contentWidth, settings.HeaderHeight);

                // Tabellenkopf
                double tableTop = margins.Top + settings.HeaderHeight;

                AddTableHeader(page, columns, widths, margins.Left, tableTop, settings);

                // Datenzeilen
                int startIndex = pageIndex * rowsPerPage;

                int endIndex = Math.Min(startIndex + rowsPerPage, items.Count);

                double y = tableTop + settings.ColumnHeaderHeight;

                for (int rowIndex = startIndex; rowIndex < endIndex; rowIndex++)
                {
                    var item = items[rowIndex]!;

                    for (int colIndex = 0; colIndex < columns.Count; colIndex++)
                    {
                        var column = columns[colIndex];

                        object value = column.GetValue(item);

                        string text = ReportValueFormatter.Format(value, column, settings);

                        var cell = CreateCell(text, widths[colIndex], settings.RowHeight, settings, column.PropertyType, isHeader: false);

                        AddElement(page, cell, GetColumnX(margins.Left, widths, colIndex), y, widths[colIndex], settings.RowHeight);
                    }

                    y += settings.RowHeight;
                }

                // Fußbereich
                double footerTop =
                    paper.Height - margins.Bottom
                    - settings.FooterHeight;

                FrameworkElement footer = settings.FooterFactory?.Invoke(
                    pageIndex + 1, pageCount)
                    ?? CreateDefaultFooter(
                        pageIndex + 1, pageCount, contentWidth);

                AddElement(
                    page,
                    footer,
                    margins.Left,
                    footerTop,
                    contentWidth,
                    settings.FooterHeight);

                var pageContent = new PageContent
                {
                    Child = page
                };

                document.Pages.Add(pageContent);
            }

            return document;
        }

        private static void ValidateSettings(ReportSettings settings)
        {
            if (settings.PaperSize.Width <= 0 || settings.PaperSize.Height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(settings.PaperSize));
            }

            if (settings.Margins.Left < 0 || settings.Margins.Right < 0 || settings.Margins.Top < 0 || settings.Margins.Bottom < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(settings.Margins));
            }

            if (settings.HeaderHeight < 0 || settings.FooterHeight < 0 || settings.ColumnHeaderHeight <= 0 || settings.RowHeight <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(settings), "Die Höhen der Reportbereiche sind ungültig.");
            }
        }

        private static double[] CalculateColumnWidths(IReadOnlyList<ReportColumn> columns, double availableWidth)
        {
            int count = columns.Count;

            var widths = new double[count];

            var automatic = Enumerable.Range(0, count).Where(i => columns[i].Width <= 0).ToList();

            double fixedWidth = columns.Where(c => c.Width > 0).Sum(c => c.Width);

            // Wenn feste Breiten allein schon zu groß sind,
            // alle Spalten proportional anpassen.
            if (fixedWidth >= availableWidth)
            {
                double factor = availableWidth / fixedWidth;

                for (int i = 0; i < count; i++)
                {
                    widths[i] = columns[i].Width > 0 ? columns[i].Width * factor : 0;
                }

                if (automatic.Count > 0)
                {
                    // Keine Spalte soll Breite 0 erhalten.
                    return Enumerable.Repeat(availableWidth / count, count).ToArray();
                }

                return widths;
            }

            if (automatic.Count == 0)
            {
                // Nur feste Breiten: den verbleibenden Platz
                // proportional verteilen.
                double factor = availableWidth / fixedWidth;

                return columns
                    .Select(c => c.Width * factor)
                    .ToArray();
            }

            double automaticWidth = (availableWidth - fixedWidth) / automatic.Count;

            for (int i = 0; i < count; i++)
            {
                widths[i] = columns[i].Width > 0 ? columns[i].Width : automaticWidth;
            }

            return widths;
        }

        private static double GetColumnX(double startX, double[] widths, int columnIndex)
        {
            double x = startX;

            for (int i = 0; i < columnIndex; i++)
                x += widths[i];

            return x;
        }

        private static void AddTableHeader(FixedPage page, IReadOnlyList<ReportColumn> columns, double[] widths, double x, double y, ReportSettings settings)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                var cell = CreateCell(
                    columns[i].Header,
                    widths[i],
                    settings.ColumnHeaderHeight,
                    settings,
                    columns[i].PropertyType,
                    isHeader: true);

                AddElement(
                    page,
                    cell,
                    GetColumnX(x, widths, i),
                    y,
                    widths[i],
                    settings.ColumnHeaderHeight);
            }
        }

        private static Border CreateCell(string text, double width, double height, ReportSettings settings, Type propertyType, bool isHeader)
        {
            TextAlignment alignment =
                propertyType == typeof(bool)
                    ? TextAlignment.Center
                    : IsNumericType(propertyType)
                        ? TextAlignment.Right
                        : TextAlignment.Left;

            var textBlock = new TextBlock
            {
                Text = text,
                FontSize = isHeader
                    ? settings.HeaderFontSize
                    : settings.FontSize,
                FontWeight = isHeader
                    ? FontWeights.Bold
                    : FontWeights.Normal,
                Foreground = isHeader
                    ? settings.HeaderTextBrush
                    : settings.TextBrush,
                TextAlignment = alignment,
                TextWrapping = TextWrapping.NoWrap,
                TextTrimming = TextTrimming.CharacterEllipsis,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(4, 0, 4, 0)
            };

            return new Border
            {
                Width = width,
                Height = height,
                Background = isHeader
                    ? settings.HeaderBackground
                    : Brushes.White,
                BorderBrush = settings.GridBrush,
                BorderThickness = new Thickness(0, 0, 0.5, 0.5),
                Child = textBlock,
                ClipToBounds = true
            };
        }

        private static bool IsNumericType(Type type)
        {
            return Type.GetTypeCode(type) switch
            {
                TypeCode.Byte or
                TypeCode.SByte or
                TypeCode.Int16 or
                TypeCode.UInt16 or
                TypeCode.Int32 or
                TypeCode.UInt32 or
                TypeCode.Int64 or
                TypeCode.UInt64 or
                TypeCode.Single or
                TypeCode.Double or
                TypeCode.Decimal => true,

                _ => false
            };
        }

        private static FrameworkElement CreateDefaultHeader(ReportSettings settings, double width)
        {
            return new TextBlock
            {
                Text = settings.Title,
                Width = width,
                FontSize = settings.TitleFontSize,
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        private static FrameworkElement CreateDefaultFooter(int page, int totalPages,double width)
        {
            return new TextBlock
            {
                Text = $"Seite {page} von {totalPages}",
                Width = width,
                TextAlignment = TextAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 9
            };
        }

        private static void AddElement(FixedPage page, FrameworkElement element, double x, double y, double width, double height)
        {
            element.Width = width;
            element.Height = height;

            FixedPage.SetLeft(element, x);
            FixedPage.SetTop(element, y);

            page.Children.Add(element);
        }
    }
}
