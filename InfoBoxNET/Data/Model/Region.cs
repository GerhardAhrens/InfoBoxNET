//-----------------------------------------------------------------------
// <copyright file="Region.cs" company="www.lifeprojects.de">
//     Class: Region
//     Copyright © www.lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - www.lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>30.05.2022 15:02:30</date>
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

    using InfoBoxNET.Core.Enums;

    [DebuggerDisplay("Name={this.Name}")]
    [DataTable("TAB_Region")]
    public partial class Region 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Region"/> class.
        /// </summary>
        public Region()
        {
            this.LastExport = DateTime.Today.DefaultDate();
            this.Id = Guid.CreateVersion7();
        }

        [PrimaryKey]
        [TableColumn(SQLiteDataType.Guid)]
        public Guid Id { get; set; }

        [TableColumn(SQLiteDataType.VarChar)]
        public string Name { get; set; }

        [TableColumn(SQLiteDataType.Integer)]
        public int ItemSorting { get; set; }

        [TableColumn(SQLiteDataType.VarChar,100)]
        public string Description { get; set; }

        [TableColumn(SQLiteDataType.VarChar,10)]
        public string Background { get; set; }

        [TableColumn(SQLiteDataType.Integer)]
        public int Symbol { get; set; }

        [TableColumn(SQLiteDataType.DateTime)]
        public DateTime LastExport { get; set; }

        [TableColumn(SQLiteDataType.Integer)]
        public SyncItemStatus SyncItemStatus { get; set; }

        [TableColumn(SQLiteDataType.VarChar)]
        public string SyncHash { get; set; }

        [TableColumn(SQLiteDataType.VarChar)]
        public string CreatedBy { get; set; }

        [TableColumn(SQLiteDataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [TableColumn(SQLiteDataType.VarChar)]
        public string ModifiedBy { get; set; }

        [TableColumn(SQLiteDataType.DateTime)]
        public DateTime ModifiedOn { get; set; }
    }
}
