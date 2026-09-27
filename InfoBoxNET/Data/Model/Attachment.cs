//-----------------------------------------------------------------------
// <copyright file="Attachment.cs" company="www.lifeprojects.de">
//     Class: Attachment
//     Copyright © www.lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - www.Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>30.06.2022 13:53:09</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace InfoBoxNET.Model
{
    using System;
    using System.Data.SQLite;

    using InfoBoxNET.Core.Enums;

    [DataTable("TAB_Attachment")]
    public sealed partial class Attachment 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffortProject"/> class.
        /// </summary>
        public Attachment()
        {
            this.Id = Guid.NewGuid();
            this.CreatedBy = UserInfo.TS().CurrentUser;
            this.CreatedOn = UserInfo.TS().CurrentTime;
        }

        [PrimaryKey]
        [TableColumn(SQLiteDataType.Guid)]
        public Guid Id { get; set; }

        [TableColumn(SQLiteDataType.Guid)]
        public Guid ObjectId { get; set; }

        [TableColumn(SQLiteDataType.VarChar)]
        public string ObjectName { get; set; }

        [TableColumn(SQLiteDataType.BLOB)]
        public byte[] Content { get; set; }

        [TableColumn(SQLiteDataType.VarChar,100)]
        public string Filename { get; set; }

        [TableColumn(SQLiteDataType.VarChar,5)]
        public string FileExtension { get; set; }

        [TableColumn(SQLiteDataType.DateTime)]
        public DateTime FileDateTime { get; set; }

        [TableColumn(SQLiteDataType.Real)]
        public long FileSize { get; set; }

        [TableColumn(SQLiteDataType.Integer)]
        public SyncItemStatus SyncItemStatus { get; set; }

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
