//-----------------------------------------------------------------------
// <copyright file="FixedPageLabelExtensions.cs" company="Lifeprojects.de">
//     Class: FixedPageExtensions
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>17.10.2025 07:30:01</date>
//
// <summary>
// Extension Class for 
// </summary>
//-----------------------------------------------------------------------

namespace System.Windows.Documents
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using RConvert = System.Windows.Documents.Convert;

    public static partial class FixedPageExtensions
    {
        public static void ReportLabel(this FixedPage @this, string text, Brush foreground, double fontSize, FontWeight fontWeight, Thickness position)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.Foreground = foreground;
            textBlock.FontSize = RConvert.PointsToPixels(fontSize);
            textBlock.FontWeight = fontWeight;
            textBlock.Margin = position;
            @this.Children.Add(textBlock);
        }

        public static void ReportLabel(this FixedPage @this,string text, Brush foreground, double fontSize, Thickness position)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.Foreground = foreground;
            textBlock.FontSize = RConvert.PointsToPixels(fontSize);
            textBlock.FontWeight = FontWeights.Normal;
            textBlock.Margin = position;
            @this.Children.Add(textBlock);
        }

        public static void ReportLabel(this FixedPage @this, string text, Brush foreground, Thickness position)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.Foreground = foreground;
            textBlock.FontSize = RConvert.PointsToPixels(12.0);
            textBlock.FontWeight = FontWeights.Normal;
            textBlock.Margin = position;
            @this.Children.Add(textBlock);
        }

        public static void ReportLabel(this FixedPage @this, string text, double fontSize, Thickness position)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.Foreground = Brushes.Black;
            textBlock.FontSize = RConvert.PointsToPixels(fontSize);
            textBlock.FontWeight = FontWeights.Normal;
            textBlock.Margin = position;
            @this.Children.Add(textBlock);
        }

        public static void ReportLabel(this FixedPage @this, string text, Thickness position)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.Foreground = Brushes.Black;
            textBlock.FontSize = RConvert.PointsToPixels(12.0);
            textBlock.FontWeight = FontWeights.Normal;
            textBlock.Margin = position;
            @this.Children.Add(textBlock);
        }
    }
}
