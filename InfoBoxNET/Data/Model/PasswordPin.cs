//-----------------------------------------------------------------------
// <copyright file="PasswordPin.cs" company="www.lifeprojects.de">
//     Class: PasswordPin
//     Copyright © www.lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - www.lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>27.04.2022 14:17:30</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace InfoBoxNET.Model
{
    using System;
    using System.Diagnostics;

    using InfoBoxNET.Core.Enums;

    using System.Data.SQLite;

    [DebuggerDisplay("Title={this.Title};Username={this.Username};Symbol={this.Symbol};Background={this.Background}")]
    [DataTable("TAB_PasswordPin")]
    public partial class PasswordPin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordPin"/> class.
        /// </summary>
        public PasswordPin()
        {
            this.LastExport = DateTime.Today.DefaultDate();
            this.CreatedBy = UserInfo.TS().CurrentUser;
            this.CreatedOn = UserInfo.TS().CurrentTime;
        }

        [PrimaryKey]
        [TableColumn(SQLiteDataType.Guid)]
        public Guid Id { get; set; }

        [TableColumn(SQLiteDataType.Integer)]
        public int Version { get; set; }

        [TableColumn(SQLiteDataType.Integer)]
        public AccessTyp AccessTyp { get; set; }

        [TableColumn(SQLiteDataType.VarChar)]
        public string Title { get; set; }

        [TableColumn(SQLiteDataType.VarChar,100)]
        public string Description { get; set; }

        [TableColumn(SQLiteDataType.Boolean)]
        public bool IsAttachment { get; set; }

        [TableColumn(SQLiteDataType.Boolean)]
        public bool ShowDescription { get; set; }

        [TableColumn(SQLiteDataType.VarChar)]
        public string Username { get; set; }

        [TableColumn(SQLiteDataType.VarChar)]
        public string Passwort { get; set; }

        [TableColumn(SQLiteDataType.VarChar,20)]
        public string Pin { get; set; }

        [TableColumn(SQLiteDataType.VarChar,100)]
        public string Website { get; set; }

        [TableColumn(SQLiteDataType.Integer)]
        public int Symbol { get; set; }

        [TableColumn(SQLiteDataType.VarChar,10)]
        public string Background { get; set; }

        [TableColumn(SQLiteDataType.Guid)]
        public Guid CompanyId { get; set; }

        [TableColumn(SQLiteDataType.VarChar)]
        public string Company { get; set; }

        [TableColumn(SQLiteDataType.VarChar,100)]
        public string CompanyInfoMail { get; set; }

        [TableColumn(SQLiteDataType.VarChar)]
        public string LicenseName { get; set; }

        [TableColumn(SQLiteDataType.VarChar)]
        public string LicenseKey { get; set; }

        [TableColumn(SQLiteDataType.Boolean)]
        public bool IsLicenseAbo { get; set; }

        [TableColumn(SQLiteDataType.DateTime)]
        public DateTime LicenseValid { get; set; }

        [TableColumn(SQLiteDataType.VarChar)]
        public string Region { get; set; }

        [TableColumn(SQLiteDataType.DateTime)]
        public DateTime LastExport { get; set; }

        [TableColumn(SQLiteDataType.DateTime)]
        public DateTime ShowLast { get; set; }

        [TableColumn(SQLiteDataType.Boolean)]
        public bool IsShowLast { get; set; }

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
