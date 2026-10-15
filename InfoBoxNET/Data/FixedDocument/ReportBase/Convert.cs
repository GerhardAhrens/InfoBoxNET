//-----------------------------------------------------------------------
// <copyright file="Convert.cs" company="Lifeprojects.de">
//     Class: Convert
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>16.10.2025 14:24:44</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace System.Windows.Documents
{
    public class Convert
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Convert"/> class.
        /// </summary>
        public Convert()
        {
        }

        public static double PointsToPixels(double points)
        {
            return points * (96.0 / 72.0);
        }
    }
}
