//-----------------------------------------------------------------------
// <copyright file="ReportPaperSize.cs" company="Lifeprojects.de">
//     Class: ReportPaperSize
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>16.10.2025 14:18:47</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace System.Windows.Documents
{
    using System;
    using System.Windows;

    public class ReportPaperSize
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportPaperSize"/> class.
        /// </summary>
        public ReportPaperSize()
        {
        }

        public static Size A4Landscape()
        {
            Size result;

            try
            {
                result = new Size((11.67 * 96), (8.27 * 96));
            }
            catch (Exception e)
            {
                string errorText = e.Message;
                throw;
            }

            return result;
        }

        public static Size A4Portrait()
        {
            Size result;

            try
            {
                result = new Size((8.27 * 96), (11.67 * 96));
            }
            catch (Exception e)
            {
                string errorText = e.Message;
                throw;
            }

            return result;
        }
    }
}
