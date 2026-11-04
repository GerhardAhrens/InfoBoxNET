namespace InfoBoxNET.Core
{
    using System.Reflection;

    public class PropertyComparer
    {
        /// <summary>
        /// Vergleicht zwei Objekte auf Property-Ebene und gibt die Unterschiede zurück.
        /// </summary>
        public static IEnumerable<PropertyDifference> Compare<T>(T original, T modified) where T : class
        {
            if (original == null) throw new ArgumentNullException(nameof(original));
            if (modified == null) throw new ArgumentNullException(nameof(modified));

            // Holt alle öffentlichen Instanz-Properties des Typs
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                // Indizierte Properties (z.B. Item[int]) überspringen
                if (property.GetIndexParameters().Length > 0) continue;

                object originalValue = property.GetValue(original);
                object modifiedValue = property.GetValue(modified);

                // Vergleich unter Berücksichtigung von Null-Werten
                if (!Equals(originalValue, modifiedValue))
                {
                    yield return new PropertyDifference
                    {
                        PropertyName = property.Name,
                        OriginalValue = originalValue,
                        ModifiedValue = modifiedValue,
                        PropertyType = property.PropertyType
                    };
                }
            }
        }
    }

    public class PropertyDifference
    {
        public string PropertyName { get; init; }
        public object OriginalValue { get; init; }
        public object ModifiedValue { get; init; }
        public Type PropertyType { get; init; }
    }
}
