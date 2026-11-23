//-----------------------------------------------------------------------
// <copyright file="VerkaeuferPart.cs" company="Lifeprojects.de">
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
    using System.Data.SQLite;

    public sealed partial class VerkaeuferTyp
    {
        public string FullName
        {
            get
            {
                return $"{this.Title}-{this.Typ}-{this.IsAktiv}";
            }
        }

        public string Timestamp
        {
            get
            {
                string result = string.Empty;

                TimeStamp ts = new TimeStamp();
                result = ts.MaxEntry(this.CreatedOn, this.CreatedBy, this.ModifiedOn, this.ModifiedBy);

                return result;
            }
        }
    }
}
