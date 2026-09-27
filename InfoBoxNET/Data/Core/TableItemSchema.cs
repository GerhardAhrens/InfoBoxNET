namespace InfoBoxNET.Data.Core
{
    public class TableItemSchema
    {
        public TableItemSchema(string table, string column, string columnType)
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
