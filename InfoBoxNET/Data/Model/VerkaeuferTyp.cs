//-----------------------------------------------------------------------
// <copyright file="Verkaeufer.cs" company="Lifeprojects.de">
//     Class: Verkaeufer
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>28.06.2025 14:24</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Inventar.Modell.Model
{
    using System;
    using System.Data.SQLite;
    using System.Diagnostics;

    [DebuggerDisplay("{this.FullName}")]
    [DataTable("TAB_VerkaeuferTyp")]
    public sealed partial class VerkaeuferTyp
    {
        public VerkaeuferTyp()
        {
            this.Id = Guid.NewGuid();
            this.CreatedBy = UserInfo.TS().CurrentUser;
            this.CreatedOn = UserInfo.TS().CurrentTime;
        }

        public VerkaeuferTyp(string name)
        {
            this.Id = Guid.NewGuid();
            this.Title = name;
            this.IsAktiv = true;
            this.CreatedBy = UserInfo.TS().CurrentUser;
            this.CreatedOn = UserInfo.TS().CurrentTime;
        }

        public VerkaeuferTyp(string name, string adresse)
        {
            this.Id = Guid.NewGuid();
            this.Title = name;
            this.Adresse = adresse;
            this.IsAktiv = true;
            this.CreatedBy = UserInfo.TS().CurrentUser;
            this.CreatedOn = UserInfo.TS().CurrentTime;
        }

        [PrimaryKey]
        [TableColumn(SQLiteDataType.Guid)]
        public Guid Id { get; private set; }

        [TableColumn(SQLiteDataType.Text, 50)]
        public string Title { get; set; }

        [TableColumn(SQLiteDataType.Text, 250)]
        public string Adresse { get; set; }

        [TableColumn(SQLiteDataType.Integer)]
        public int Typ { get; set; }

        [TableColumn(SQLiteDataType.Boolean)]
        public bool IsAktiv { get; set; }

        [TableColumn(SQLiteDataType.Text, 50)]
        public string CreatedBy { get; set; }

        [TableColumn(SQLiteDataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [TableColumn(SQLiteDataType.Text, 50)]
        public string ModifiedBy { get; set; }

        [TableColumn(SQLiteDataType.DateTime)]
        public DateTime ModifiedOn { get; set; }
    }
}
