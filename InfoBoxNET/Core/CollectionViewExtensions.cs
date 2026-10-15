//-----------------------------------------------------------------------
// <copyright file="PasswordsUC.cs" company="Lifeprojects.de">
//     Class: PasswordsUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.09.2026</date>
//
// <summary>
// Template für eine neues UserControl
// </summary>
//-----------------------------------------------------------------------

namespace InfoBoxNET.Core
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;
    using System.Reflection;
    using System.Text.Json.Serialization;

    public static class CollectionViewExtensions
    {
        /// <summary>
        /// Konvertiert eine ICollectionView in eine DataTable unter Beibehaltung der Datentypen.
        /// Unterstützt Filterung nach Export- und Print-Attributen sowie den Ausschluss ignorierter Properties.
        /// </summary>
        /// <param name="forExport">Wenn true, werden NUR Properties mit [Export] einbezogen.</param>
        /// <param name="forPrint">Wenn true, werden NUR Properties mit [Print] einbezogen.</param>
        public static DataTable ToDataTable<T>(this ICollectionView collectionView, bool forExport = false, bool forPrint = false)
        {
            DataTable dataTable = new DataTable();

            if (collectionView == null)
                return dataTable;

            // Spaltenstruktur ermitteln
            Type targetType = typeof(T);
            if (targetType == typeof(object))
            {
                object firstItem = collectionView.Cast<object>().FirstOrDefault();
                if (firstItem != null)
                {
                    targetType = firstItem.GetType();
                }
                else
                {
                    return dataTable;
                }
            }

            // Alle öffentlichen Instanz-Properties abrufen
            var allProperties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Filter-Logik anwenden
            var filteredProperties = allProperties.Where(prop =>
            {
                // Modus 1: Gezielter Export (White-Listing)
                if (forExport && !prop.IsDefined(typeof(ExportAttribute), true))
                    return false;

                // Modus 2: Gezielter Druck (White-Listing)
                if (forPrint && !prop.IsDefined(typeof(PrintAttribute), true))
                    return false;

                // Modus 3: Standard-Ausschluss (Black-Listing), falls kein White-Listing aktiv ist
                if (!forExport && !forPrint)
                {
                    if (prop.IsDefined(typeof(NotMappedAttribute), true) ||
                        prop.IsDefined(typeof(JsonIgnoreAttribute), true))
                        return false;
                }

                return true;
            }).ToArray();

            // Spalten in der DataTable anlegen
            foreach (PropertyInfo prop in filteredProperties)
            {
                Type columnType = prop.PropertyType;

                if (columnType.IsGenericType && columnType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    columnType = Nullable.GetUnderlyingType(columnType);
                }

                dataTable.Columns.Add(prop.Name, columnType);
            }

            // Daten zeilenweise befüllen
            foreach (object item in collectionView)
            {
                if (item == null)
                    continue;

                DataRow row = dataTable.NewRow();
                foreach (PropertyInfo prop in filteredProperties)
                {
                    object value = prop.GetValue(item, null);
                    row[prop.Name] = value ?? DBNull.Value;
                }

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}
