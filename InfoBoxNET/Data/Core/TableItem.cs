namespace InfoBoxNET.Data.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class TableItem
    {
        public TableItem(string table, string column, string columnType)
        {
            this.Table = table;
            this.Column = column;
            this.ColumnType = columnType;
        }

        public string Table { get; private set; }
        public string Column { get; private set; }
        public string ColumnType { get; private set; }
    }
}
