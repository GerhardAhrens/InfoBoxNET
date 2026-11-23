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
    using System.ComponentModel;
    using System.Data.SQLite;
    using System.Diagnostics;

    [DebuggerDisplay("{this.FullName}")]
    [DataTable("TAB_Inventar")]
    public sealed partial class Inventars
    {
        private DateTime? _GekauftAm;

        public Inventars() { }

        public Inventars(string title, DateTime gekauftAm, decimal kaufBetrag)
        {
            this.Id = Guid.NewGuid();
            this.Title = title;
            this.KaufBetrag = kaufBetrag;
            this.IsAktiv = true;
            this.GekauftAm = gekauftAm;
            this.InventarAlter = this.GetAge(gekauftAm);
            this.CreatedBy = UserInfo.TS().CurrentUser;
            this.CreatedOn = UserInfo.TS().CurrentTime;
        }

        [PrimaryKey]
        [TableColumn(SQLiteDataType.Guid)]
        public Guid Id { get; set; }

        [TableColumn(SQLiteDataType.Text, 50)]
        public string Title { get; set; }

        [TableColumn(SQLiteDataType.Text, 250)]
        public string Description { get; set; }

        [TableColumn(SQLiteDataType.Text, 250)]
        public string InventarInfo { get; set; }

        [ColumnIndex("InventarTyp","InvTyp",ListSortDirection.Ascending)]
        [TableColumn(SQLiteDataType.Integer)]
        public int InventarTyp { get; set; }

        [TableColumn(SQLiteDataType.Text, 50)]
        public string Ablageort { get; set; }

        [ColumnIndex("VerkauftTyp", "VerTyp", ListSortDirection.Ascending)]
        [TableColumn(SQLiteDataType.Integer)]
        public int VerkauftTyp { get; set; }

        [TableColumn(SQLiteDataType.Text, 250)]
        public string Schlagworte { get; set; }

        [TableColumn(SQLiteDataType.Boolean)]
        public bool IsAktiv { get; set; }

        [TableColumn(SQLiteDataType.DateTime)]
        public DateTime? GekauftAm
        {
            get { return this._GekauftAm; }
            set
            { 
                this._GekauftAm = value;
                this.InventarAlter = this.GetAge(value == null ? new DateTime(1900,1,1) : (DateTime)value);
            }
        }

        [TableColumn(SQLiteDataType.Integer)]
        public int InventarAlter { get; private set; }

        [TableColumn(SQLiteDataType.Decimal, 8, 2)]
        public decimal KaufBetrag { get; set; }

        [TableColumn(SQLiteDataType.Text, 100)]
        public string KaufBeleg { get; set; }

        [TableColumn(SQLiteDataType.Text, 50)]
        public string CreatedBy { get; set; }

        [TableColumn(SQLiteDataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [TableColumn(SQLiteDataType.Text, 50)]
        public string ModifiedBy { get; set; }

        [TableColumn(SQLiteDataType.DateTime)]
        public DateTime ModifiedOn { get; set; }

        private int GetAge(DateTime dateOfBirth)
        {
            if (dateOfBirth == new DateTime(1900, 1, 1))
            {
                return 0;
            }

            var today = DateTime.Today;

            var a = (today.Year * 100 + today.Month) * 100 + today.Day;
            var b = (dateOfBirth.Year * 100 + dateOfBirth.Month) * 100 + dateOfBirth.Day;

            return (a - b) / 10000;
        }
    }
}
