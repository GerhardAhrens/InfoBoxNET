//-----------------------------------------------------------------------
// <copyright file="History.cs" company="www.lifeprojects.de">
//     Class: History
//     Copyright © www.lifeprojects.de 2026
// </copyright>
//
// <author>Gerhard Ahrens - www.lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>20.11.2026 15:02:30</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace InfoBoxNET.Model
{
    using System;
    using System.Data.SQLite;
    using System.Diagnostics;

    using InfoBoxNET.Core;

    [DebuggerDisplay("Name={this.Name}")]
    [DataTable("TAB_History")]
    public partial class History
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="History"/> class.
        /// </summary>
        public History()
        {
            this.Id = Guid.CreateVersion7();
            this.CreatedBy = Environment.UserName;
            this.CreatedOn = DateTime.Now;
        }

        public History(Guid objectId, string objectName = "PasswordPin")
        {
            this.Id = Guid.CreateVersion7();
            this.ObjectName = objectName;
            this.ObjectId = objectId;
            this.CreatedBy = Environment.UserName;
            this.CreatedOn = DateTime.Now;
        }

        [PrimaryKey]
        [TableColumn(SQLiteDataType.Guid)]
        public Guid Id { get; set; }

        [TableColumn(SQLiteDataType.Guid)]
        public Guid ObjectId { get; set; }

        [TableColumn(SQLiteDataType.VarChar)]
        public string ObjectName { get; set; }

        [TableColumn(SQLiteDataType.VarChar,100)]
        public string PropertyName { get; set; }

        [TableColumn(SQLiteDataType.VarChar, 50)]
        public string OriginalValue { get; set; }

        [TableColumn(SQLiteDataType.VarChar, 50)]
        public string ModifiedValue { get; set; }

        [TableColumn(SQLiteDataType.VarChar, 20)]
        public string PropertyType { get; set; }


        [TableColumn(SQLiteDataType.VarChar)]
        public string CreatedBy { get; set; }

        [TableColumn(SQLiteDataType.DateTime)]
        public DateTime CreatedOn { get; set; }
    }
}
