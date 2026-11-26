namespace System.Windows.Controls
{
    using System.ComponentModel;
    using System.Data;
    using System.Globalization;
    using System.Reflection;
    using System.Text;
    using System.Windows.Data;

    internal class FilterManager
    {
        private readonly AdvancedListView _listView;

        private readonly Dictionary<AdvancedGridViewColumn, FilterInfo> _filters = new();

        public FilterManager(AdvancedListView listView)
        {
            this._listView = listView;
        }

        public bool IsFilterActive => this._filters.Values.Any(f => !string.IsNullOrWhiteSpace(f.FilterText));

        public int VisibleRowCount
        {
            get
            {
                if (_listView.ItemsSource == null)
                {
                    return 0;
                }

                ICollectionView view = CollectionViewSource.GetDefaultView(_listView.ItemsSource);

                if (view is BindingListCollectionView blcv && blcv.SourceCollection is DataView dv)
                {
                    return dv.Count;
                }
                else if (view is ListCollectionView collectionView)
                {
                    return collectionView.Count;
                }

                return 0;
            }
        }

        public int TotalRowCount
        {
            get
            {
                if (_listView.ItemsSource == null)
                {
                    return 0;
                }

                ICollectionView view = CollectionViewSource.GetDefaultView(_listView.ItemsSource);

                if (view is BindingListCollectionView blcv && blcv.SourceCollection is DataView dv)
                {
                    return dv.Table.Rows.Count;
                }
                else if (view is ListCollectionView collectionView)
                {
                    return collectionView.Count;
                }

                return 0;
            }
        }

        public IEnumerable<FilterInfo> Filters
        {
            get => this._filters.Values;
        }

        public void Clear()
        {
            this._filters.Clear();
        }

        public FilterInfo RegisterColumn(AdvancedGridViewColumn column, string propertyName)
        {
            if (this._filters.TryGetValue(column, out FilterInfo info))
            {
                return info;
            }

            info = new FilterInfo(column, propertyName);
            info.FilterType = ResolveFilterType(column, propertyName);
            this._filters.Add(column, info);

            return info;
        }

        private FilterType ResolveFilterType(AdvancedGridViewColumn column, string propertyName)
        {
            if (column.FilterType != FilterType.Auto)
            {
                return column.FilterType;
            }

            if (_listView.ItemsSource == null)
            {
                return FilterType.Text;
            }

            ICollectionView view = CollectionViewSource.GetDefaultView(_listView.ItemsSource);

            if (view is BindingListCollectionView blcv && blcv.SourceCollection is DataView dataView)
            {
                propertyName = propertyName.Trim('[', ']');

                Type type = dataView.Table.Columns[propertyName].DataType;

                if (type == typeof(string))
                    return FilterType.Text;

                if (type == typeof(DateTime))
                    return FilterType.Date;

                if (type == typeof(bool))
                    return FilterType.Boolean;

                if (type == typeof(byte) ||
                    type == typeof(short) ||
                    type == typeof(int) ||
                    type == typeof(long) ||
                    type == typeof(float) ||
                    type == typeof(double) ||
                    type == typeof(decimal))
                    return FilterType.Number;
            }
            else if (view is ListCollectionView collectionView)
            {
                var werte = view.Cast<object>().Select(item => item.GetType().GetProperty(propertyName)?.GetValue(item, null)).ToList();

                Type type = werte.FirstOrDefault()?.GetType();

                if (type == typeof(string))
                    return FilterType.Text;

                if (type == typeof(DateTime))
                    return FilterType.Date;

                if (type == typeof(bool))
                    return FilterType.Boolean;

                if (type == typeof(byte) ||
                    type == typeof(short) ||
                    type == typeof(int) ||
                    type == typeof(long) ||
                    type == typeof(float) ||
                    type == typeof(double) ||
                    type == typeof(decimal))
                    return FilterType.Number;
            }

            return FilterType.Text;
        }
        public void SetFilter(AdvancedGridViewColumn column, string value)
        {
            if (this._filters.TryGetValue(column, out FilterInfo info) == false)
            {
                return;
            }

            info.FilterText = value ?? string.Empty;

            this.Refresh();
        }

        public FilterInfo GetFilter(AdvancedGridViewColumn column)
        {
            this._filters.TryGetValue(column, out FilterInfo filter);
            return filter;
        }

        private void Refresh()
        {
            if (this._listView.ItemsSource == null)
            {
                return;
            }

            try
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(this._listView.ItemsSource);

                if (view is BindingListCollectionView blcv)
                {
                    if (blcv.SourceCollection is DataView dataView)
                    {
                        dataView.RowFilter = this.BuildRowFilter();
                    }
                }

                if (view is ICollectionView blcv1)
                {
                    if (blcv1.SourceCollection is IEnumerable<object>)
                    {
                        DataView dataView = this.ConvertToDataView(blcv1);
                        dataView.RowFilter = this.BuildRowFilter();
                    }
                }
            }
            catch (EvaluateException)
            {
                // Ungültiger Zwischenzustand während der Eingabe ignorieren.
            }
            catch (SyntaxErrorException)
            {
                // Ebenfalls ignorieren.
            }

            _listView.RaiseFilterChanged();
            _listView.RaiseStatusChanged();
        }
        private string BuildRowFilter()
        {
            StringBuilder sb = new();

            foreach (FilterInfo filter in this._filters.Values)
            {
                if (filter.IsEmpty)
                {
                    continue;
                }

                string expression = BuildExpression(filter);

                if (string.IsNullOrWhiteSpace(expression))
                {
                    continue;
                }

                if (sb.Length > 0)
                {
                    sb.Append(" AND ");
                }

                sb.Append(expression);
            }

            return sb.ToString();
        }

        #region Expression Filter Text, Nummerisch, Datum, Boolean
        private static string BuildExpression(FilterInfo filter)
        {
            return filter.FilterType switch
            {
                FilterType.Text => BuildTextExpression(filter),
                FilterType.Number => BuildNumberExpression(filter),
                FilterType.Date => BuildDateExpression(filter),
                FilterType.Boolean => BuildBooleanExpression(filter),
                _ => BuildTextExpression(filter)
            };
        }

        private static string BuildTextExpression(FilterInfo filter)
        {
            string field = $"[{filter.PropertyName}]";
            string value = filter.FilterText.Trim();

            if (string.IsNullOrWhiteSpace(value))
                return null;

            static string Escape(string s)
                => s.Replace("'", "''");

            if (value.StartsWith("=", StringComparison.OrdinalIgnoreCase))
            {
                string operand = value[1..].Trim();

                if (operand.Length == 0)
                    return null;

                return $"Convert({field}, 'System.String') = '{Escape(operand)}'";
            }

            if (value.StartsWith("<>", StringComparison.OrdinalIgnoreCase))
            {
                string operand = value[2..].Trim();

                if (operand.Length == 0)
                    return null;

                return $"Convert({field}, 'System.String') <> '{Escape(operand)}'";
            }

            if (value.StartsWith("*", StringComparison.OrdinalIgnoreCase))
            {
                string operand = value[1..];

                if (operand.Length == 0)
                    return null;

                return $"Convert({field}, 'System.String') LIKE '%{Escape(operand)}'";
            }

            if (value.EndsWith("*", StringComparison.OrdinalIgnoreCase))
            {
                string operand = value[..^1];

                if (operand.Length == 0)
                    return null;

                return $"Convert({field}, 'System.String') LIKE '{Escape(operand)}%'";
            }

            return $"Convert({field}, 'System.String') LIKE '%{Escape(value)}%'";
        }

        private static string BuildNumberExpression(FilterInfo filter)
        {
            string field = $"[{filter.PropertyName}]";
            string value = filter.FilterText.Trim();

            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            bool Parse(string text, out decimal number)
            {
                return decimal.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out number);
            }

            if (value.StartsWith(">=", StringComparison.OrdinalIgnoreCase))
            {
                if (!Parse(value[2..], out decimal n))
                {
                    return null;
                }

                return $"{field} >= {n.ToString(CultureInfo.CurrentCulture)}";
            }

            if (value.StartsWith("<=", StringComparison.OrdinalIgnoreCase))
            {
                if (!Parse(value[2..], out decimal n))
                {
                    return null;
                }

                return $"{field} <= {n.ToString(CultureInfo.CurrentCulture)}";
            }

            if (value.StartsWith("<>", StringComparison.OrdinalIgnoreCase))
            {
                if (!Parse(value[2..], out decimal n))
                {
                    return null;
                }

                return $"{field} <> {n.ToString(CultureInfo.CurrentCulture)}";
            }

            if (value.StartsWith(">", StringComparison.OrdinalIgnoreCase))
            {
                if (!Parse(value[1..], out decimal n))
                {
                    return null;
                }

                return $"{field} > {n.ToString(CultureInfo.CurrentCulture)}";
            }

            if (value.StartsWith("<", StringComparison.OrdinalIgnoreCase))
            {
                if (!Parse(value[1..], out decimal n))
                {
                    return null;
                }

                return $"{field} < {n.ToString(CultureInfo.CurrentCulture)}";
            }

            if (value.StartsWith("=", StringComparison.OrdinalIgnoreCase))
            {
                if (!Parse(value[1..], out decimal n))
                {
                    return null;
                }

                return $"{field} = {n.ToString(CultureInfo.CurrentCulture)}";
            }

            return $"Convert({field}, 'System.String') LIKE '%{value.Replace("'", "''")}%'";
        }

        private static string BuildDateExpression(FilterInfo filter)
        {
            string field = $"[{filter.PropertyName}]";
            string value = filter.FilterText.Trim();

            if (string.IsNullOrWhiteSpace(value))
                return null;

            string[] operators = { ">=", "<=", "<>", ">", "<", "=" };

            string op = "=";
            string operand = value;

            foreach (string candidate in operators)
            {
                if (value.StartsWith(candidate))
                {
                    op = candidate;
                    operand = value[candidate.Length..].Trim();
                    break;
                }
            }

            if (!DateTime.TryParse(operand, out DateTime date))
            {
                return null;
            }

            string dateLiteral = $"#{date:MM/dd/yyyy}#";

            return $"{field} {op} {dateLiteral}";
        }

        private static string BuildBooleanExpression(FilterInfo filter)
        {
            string field = $"[{filter.PropertyName}]";
            string value = filter.FilterText.Trim().ToLowerInvariant();

            if (value is "true" or "1" or "ja")
            {
                return $"{field} = true";
            }

            if (value is "false" or "0" or "nein")
            {
                return $"{field} = false";
            }

            return null;
        }
        #endregion Expression Filter Expression Filter Text, Nummerisch, Datum, Boolean

        public DataView ConvertToDataView(ICollectionView collectionView)
        {
            DataTable table = new DataTable();

            // Erstes sichtbares Element holen
            var firstItem = collectionView.Cast<object>().FirstOrDefault();
            if (firstItem == null)
                return table.DefaultView;

            // Sonderfall: Falls das IEnumerable<object> doch DataRowViews enthält
            if (firstItem is DataRowView firstRowView)
            {
                table = firstRowView.DataView.Table.Clone();
                foreach (DataRowView rv in collectionView)
                {
                    table.ImportRow(rv.Row);
                }
                return table.DefaultView;
            }

            // Standardfall: Normale Objekte oder anonyme Typen (z. B. aus LINQ Select)
            PropertyInfo[] props = firstItem.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                // Nullable-Typen (z. B. int?) in Basis-Typen (int) umwandeln, da DataTable kein Nullable unterstützt
                Type colType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                table.Columns.Add(prop.Name, colType);
            }

            // Alle (gefilterten/sortierten) Elemente der View in die DataTable schreiben
            foreach (var item in collectionView)
            {
                if (item == null) continue;

                DataRow row = table.NewRow();
                foreach (var prop in props)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table.DefaultView;
        }
    }
}
