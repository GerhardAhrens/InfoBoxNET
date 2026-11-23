//-----------------------------------------------------------------------
// <copyright file="Inventar.cs" company="Lifeprojects.de">
//     Class: Inventar
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>22.05.2025 08:36:58</date>
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
    [DataTable("TAB_InventarTyp")]
    public sealed partial class InventarTyp
    {
        public InventarTyp()
        {
            this.Id = Guid.NewGuid();
            this.CreatedBy = UserInfo.TS().CurrentUser;
            this.CreatedOn = UserInfo.TS().CurrentTime;
        }

        public InventarTyp(string name, int typ)
        {
            this.Id = Guid.NewGuid();
            this.Title = name;
            this.Typ = typ;
            this.IsAktiv = true;
            this.CreatedBy = UserInfo.TS().CurrentUser;
            this.CreatedOn = UserInfo.TS().CurrentTime;
        }

        [PrimaryKey]
        [TableColumn(SQLiteDataType.Guid)]
        public Guid Id { get; private set; }

        [TableColumn(SQLiteDataType.Text, 50)]
        public string Title { get; set; }

        [TableColumn(SQLiteDataType.VarChar, 250)]
        public string Description { get; set; }

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
