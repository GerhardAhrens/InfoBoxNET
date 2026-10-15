//-----------------------------------------------------------------------
// <copyright file="FixedPageFooterExtensions.cs" company="Lifeprojects.de">
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
        public static void AddFooterLeft(this FixedPage @this, string text)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.FontSize = RConvert.PointsToPixels(10.0);
            textBlock.Margin = new Thickness(45, @this.Height - 40, 0, 0);
            @this.Children.Add(textBlock);
        }

        public static void AddFooterLeft(this FixedPage @this, string text, double fontSize, Brush foreground = null)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.FontSize = RConvert.PointsToPixels(fontSize);
            textBlock.Foreground = foreground ?? Brushes.Black;
            textBlock.Margin = new Thickness(45, @this.Height - 40, 0, 0);
            @this.Children.Add(textBlock);
        }

        public static void AddFooterCenter(this FixedPage @this, string text)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.FontSize = RConvert.PointsToPixels(10.0);
            textBlock.Margin = new Thickness((@this.Width / 2) - 50, @this.Height - 40, 0, 0);
            @this.Children.Add(textBlock);
        }

        public static void AddFooterCenter(this FixedPage @this, string text, double fontSize, Brush foreground = null)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.FontSize = RConvert.PointsToPixels(fontSize);
            textBlock.Foreground = foreground ?? Brushes.Black;
            textBlock.Margin = new Thickness((@this.Width / 2) - 50, @this.Height - 40, 0, 0);
            @this.Children.Add(textBlock);
        }

        public static void AddFooterRight(this FixedPage @this, string text)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.FontSize = RConvert.PointsToPixels(10.0);
            textBlock.Margin = new Thickness(@this.Width - 100, @this.Height - 40, 0, 0);
            @this.Children.Add(textBlock);
        }

        public static void AddFooterRight(this FixedPage @this, string text, double fontSize, Brush foreground = null)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.FontSize = RConvert.PointsToPixels(fontSize);
            textBlock.Foreground = foreground ?? Brushes.Black;
            textBlock.Margin = new Thickness(@this.Width - 100, @this.Height - 40, 0, 0);
            @this.Children.Add(textBlock);
        }
    }
}
