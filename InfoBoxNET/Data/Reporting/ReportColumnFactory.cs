namespace InfoBoxNET.Data.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public static class ReportColumnFactory
    {
        public static IReadOnlyList<ReportColumn> GetColumns<T>()
        {
            return typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(property => new
                {
                    Property = property, Attribute = property.GetCustomAttribute <PrintAttribute>(inherit: true)
                })
                .Where(x => x.Attribute != null && x.Property.CanRead && x.Property.GetIndexParameters().Length == 0)
                .Select(x => new ReportColumn
                {
                    Property = x.Property,
                    Header = x.Attribute!.Header
                        ?? x.Property.Name,
                    Order = x.Attribute.Order,
                    Width = x.Attribute.Width,
                    Format = x.Attribute.Format
                })
                .OrderBy(x => x.Order).ThenBy(x => x.Property.MetadataToken).ToList();
        }
    }
}
