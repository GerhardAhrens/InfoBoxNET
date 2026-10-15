//-----------------------------------------------------------------------
// <copyright file="ReportLabel.cs" company="Lifeprojects.de">
//     Class: ReportLabel
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>16.10.2025 14:28:56</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace System.Windows.Documents
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using RConvert = System.Windows.Documents.Convert;

    public class ReportLabel : TextBlock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportLabel"/> class.
        /// </summary>
        public ReportLabel(string text, Brush foreground, double fontSize, FontWeight fontWeight, Thickness position)
        {
            this.Text = text;
            this.Foreground = foreground;
            this.FontSize = RConvert.PointsToPixels(fontSize);
            this.FontWeight = fontWeight;
            this.Margin = position;
        }

        public ReportLabel(string text, Brush foreground, double fontSize, Thickness position)
        {
            this.Text = text;
            this.Foreground = foreground;
            this.FontSize = RConvert.PointsToPixels(fontSize);
            this.FontWeight = FontWeights.Normal;
            this.Margin = position;
        }

        public ReportLabel(string text, Brush foreground, Thickness position)
        {
            this.Text = text;
            this.Foreground = foreground;
            this.FontSize = RConvert.PointsToPixels(12.0);
            this.FontWeight = FontWeights.Normal;
            this.Margin = position;
        }

        public ReportLabel(string text, Thickness position)
        {
            this.Text = text;
            this.Foreground = Brushes.Black;
            this.FontSize = RConvert.PointsToPixels(12.0);
            this.FontWeight = FontWeights.Normal;
            this.Margin = position;
        }
    }
}
