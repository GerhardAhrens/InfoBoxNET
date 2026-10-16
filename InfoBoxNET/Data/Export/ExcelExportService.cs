namespace InfoBoxNET.Data
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using ClosedXML.Excel;

    using InfoBoxNET.View;

    public static class ExcelExportService
    {
        /// <summary>
        /// Exportiert eine ICollectionView in eine Excel-Datei.
        /// Exportiert werden ausschließlich Properties mit ExportAttribute.
        /// </summary>
        public static void Export<T>(ICollectionView collectionView, string filePath) where T : class
        {
            if (collectionView == null)
                throw new ArgumentNullException(nameof(collectionView));

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Der Dateipfad darf nicht leer sein.", nameof(filePath));

            // Nur Properties mit ExportAttribute berücksichtigen.
            var properties = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead
                    && p.GetIndexParameters().Length == 0
                    && Attribute.IsDefined(p, typeof(ExportAttribute))).ToArray();

            if (properties.Length == 0)
                throw new InvalidOperationException($"Die Klasse {typeof(T).Name} enthält keine Properties mit ExportAttribute.");

            // ICollectionView in eine stabile Liste übernehmen.
            var items = collectionView.Cast<T>().ToList();

            App.DoEvents();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Export");

                // Kopfzeile erstellen.
                for (int column = 0; column < properties.Length; column++)
                {
                    var cell = worksheet.Cell(1, column + 1);
                    cell.Value = properties[column].Name;
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.FontColor = XLColor.Black;
                    cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                }

                // Datenzeilen schreiben.
                for (int row = 0; row < items.Count; row++)
                {
                    App.DoEvents();

                    var item = items[row];

                    for (int column = 0; column < properties.Length; column++)
                    {
                        var property = properties[column];
                        var cell = worksheet.Cell(row + 2, column + 1);
                        var value = property.GetValue(item);

                        WriteCell(cell, value, property.PropertyType);
                    }
                }

                // Kopfzeile und erste Spalte fixieren.
                worksheet.SheetView.Freeze(1, 1);

                // Kopfzeile beim Drucken wiederholen.
                worksheet.PageSetup.SetRowsToRepeatAtTop(1, 1);

                // Filter und Tabellenformat nur bei vorhandenen Daten.
                if (items.Count > 0)
                {
                    var lastRow = items.Count + 1;
                    var lastColumn = properties.Length;

                    var table = worksheet.Range(1, 1, lastRow, lastColumn).CreateTable();

                    table.Theme = XLTableTheme.TableStyleMedium2;
                }

                // Breiten an Inhalte anpassen.
                worksheet.Columns(1, properties.Length).AdjustToContents();

                // Überbreite Spalten begrenzen.
                foreach (var column in worksheet.Columns(1, properties.Length))
                {
                    if (column.Width > 50)
                    {
                        column.Width = 50;
                    }
                }

                workbook.SaveAs(filePath);
            }
        }

        private static void WriteCell(IXLCell cell, object value, Type propertyType)
        {
            if (value == null)
            {
                cell.Clear(XLClearOptions.Contents);
                return;
            }

            var type = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            if (type == typeof(string))
            {
                cell.Value = Convert.ToString(value, CultureInfo.CurrentCulture) ?? string.Empty;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            }
            else if (type == typeof(DateTime))
            {
                cell.Value = (DateTime)value;
                cell.Style.DateFormat.Format = "dd.MM.yyyy HH:mm";
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            }
            else if (type == typeof(bool))
            {
                cell.Value = (bool)value;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }
            else if (type.IsEnum)
            {
                // Lesbare Enum-Bezeichnung exportieren.
                cell.Value = Enum.GetName(type, value) ?? value.ToString();

                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            }
            else if (type == typeof(Guid))
            {
                cell.Value = value.ToString();
                cell.Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Left;
            }
            else if (type == typeof(byte)
                || type == typeof(sbyte)
                || type == typeof(short)
                || type == typeof(ushort)
                || type == typeof(int)
                || type == typeof(uint)
                || type == typeof(long)
                || type == typeof(ulong)
                || type == typeof(float)
                || type == typeof(double)
                || type == typeof(decimal))
            {
                cell.Value = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            }
            else
            {
                // Fallback für weitere Typen.
                cell.Value = Convert.ToString(value, CultureInfo.CurrentCulture) ?? string.Empty;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            }
        }
    }
}
