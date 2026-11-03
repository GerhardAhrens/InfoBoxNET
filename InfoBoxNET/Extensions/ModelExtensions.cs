namespace InfoBoxNET.Core
{
    using System;
    using System.Data;
    using System.Reflection;
    using System.Text.Json;

    public static class ModelExtensions
    {
        public static DataRow ToDataRow<T>(this T model, DataTable table = null) where T : class
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            // 1. Wenn keine DataTable übergeben wurde, erstellen wir eine mit passenden Spalten
            if (table == null)
            {
                table = new DataTable();
                PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo prop in properties)
                {
                    // Beachtet auch Nullable-Typen für die DataTable-Struktur
                    Type propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                    // Enums werden in der DataRow meist als Integer oder String gespeichert
                    if (propType.IsEnum)
                    {
                        propType = typeof(int);
                    }

                    table.Columns.Add(prop.Name, propType);
                }
            }

            // 2. Neue DataRow aus der Tabelle erzeugen
            DataRow row = table.NewRow();

            // 3. Werte via Reflection in die Spalten schreiben
            foreach (DataColumn column in table.Columns)
            {
                PropertyInfo prop = typeof(T).GetProperty(column.ColumnName);
                if (prop != null)
                {
                    object value = prop.GetValue(model, null);

                    // Enum-Werte zu int konvertieren, damit es zur Spalte passt
                    if (value != null && prop.PropertyType.IsEnum)
                    {
                        value = (int)value;
                    }

                    // DBNull.Value setzen, falls die Property null ist
                    row[column.ColumnName] = value ?? DBNull.Value;
                }
            }

            return row;
        }

        public static T DeepClone<T>(this T source) where T : class
        {
            if (source == null) return null;

            // Serialisiert das Objekt in einen JSON-String und deserialisiert es direkt wieder
            // Dadurch entsteht ein komplett unabhängiges Objekt im Speicher
            string json = JsonSerializer.Serialize(source);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
