namespace InfoBoxNET.Data.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class ReportValueFormatter
    {
        public static string Format(object value, ReportColumn column, ReportSettings settings)
        {
            if (value == null)
            {
                return settings.NullText;
            }

            var type = column.PropertyType;
            var culture = settings.Culture;
            var format = column.Format;

            if (type == typeof(bool))
            {
                if (value is bool boolean)
                {
                    return boolean ? "Ja" : "Nein";
                }
            }

            if (type.IsEnum)
            {
                return value.ToString() ?? string.Empty;
            }

            if (value is DateTime dateTime)
            {
                return dateTime.ToString(format ?? "dd.MM.yyyy HH:mm", culture);
            }

            if (value is DateOnly dateOnly)
            {
                return dateOnly.ToString(format ?? "dd.MM.yyyy", culture);
            }

            if (value is TimeOnly timeOnly)
            {
                return timeOnly.ToString(format ?? "HH:mm", culture);
            }

            if (value is IFormattable formattable)
            {
                return formattable.ToString(format, culture) ?? string.Empty;
            }

            return value.ToString() ?? string.Empty;
        }
    }
}
