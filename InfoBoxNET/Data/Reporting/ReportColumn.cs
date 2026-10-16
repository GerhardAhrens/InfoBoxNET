namespace InfoBoxNET.Data
{
    using System;
    using System.Reflection;

    public sealed class ReportColumn
    {
        public required PropertyInfo Property { get; init; }

        public required string Header { get; init; }

        public int Order { get; init; }

        public double Width { get; init; }

        public string Format { get; init; }

        public Type PropertyType => Nullable.GetUnderlyingType(Property.PropertyType) ?? Property.PropertyType;

        public object GetValue(object item) => Property.GetValue(item);
    }
}
