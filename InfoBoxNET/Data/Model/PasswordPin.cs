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

namespace PasswortNET.Model
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
        public AccessTyp AccessTyp { get; set; }

        [TableColumn(SQLiteDataType.VarChar)]
        public string Title { get; set; }

        [TableColumn(SQLiteDataType.VarChar)]
        public string Description { get; set; }

        [TableColumn(SQLiteDataType.Boolean)]
        public bool IsAttachment { get; set; }

        [TableColumn(SQLiteDataType.Boolean)]
        public bool ShowDescription { get; set; }

        public string Username { get; set; }

        public string Passwort { get; set; }

        public string Pin { get; set; }

        public string Website { get; set; }

        public int Symbol { get; set; }

        public string Background { get; set; }

        public Guid CompanyId { get; set; }

        public string Company { get; set; }

        public string CompanyInfoMail { get; set; }

        public string LicenseName { get; set; }

        public string LicenseKey { get; set; }

        [TableColumn(SQLiteDataType.Boolean)]
        public bool IsLicenseAbo { get; set; }

        public DateTime LicenseValid { get; set; }

        public string Region { get; set; }

        public DateTime LastExport { get; set; }

        public DateTime ShowLast { get; set; }

        [TableColumn(SQLiteDataType.Boolean)]
        public bool IsShowLast { get; set; }

        public SyncItemStatus SyncItemStatus { get; set; }

        public string SyncHash { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
