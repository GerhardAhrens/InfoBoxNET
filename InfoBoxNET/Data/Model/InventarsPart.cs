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
    using System.Data.SQLite;

    public sealed partial class Inventars
    {
        public string FullName
        {
            get
            {
                if (this.GekauftAm == null)
                {
                    return $"{this.Title}-{this.InventarTyp}-{this.KaufBetrag.ToString("#.00")}";
                }
                else
                {
                    return $"{this.Title}-{this.InventarTyp}-{((DateTime)this.GekauftAm).ToShortDateString()}-{this.KaufBetrag.ToString("#.00")}";
                }
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
