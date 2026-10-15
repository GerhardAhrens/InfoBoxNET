//-----------------------------------------------------------------------
// <copyright file="FixedPageLineExtensions.cs" company="Lifeprojects.de">
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
    using System.Windows.Media;
    using System.Windows.Shapes;

    public static partial class FixedPageExtensions
    {
        public static void Line(this FixedPage @this, Brush foreground, double top, double startLeft, double pageWidth, double height)
        {
            Line line = new Line();
            line.Stroke = foreground;
            line.X1 = startLeft;
            line.Y1 = top;
            line.X2 = pageWidth - startLeft;
            line.Y2 = top;
            line.HorizontalAlignment = HorizontalAlignment.Left;
            line.VerticalAlignment = VerticalAlignment.Center;
            line.StrokeThickness = height;
            @this.Children.Add(line);
        }

        public static void Line(this FixedPage @this, double top, double left, double width, double height)
        {
            Line line = new Line();
            line.Stroke = Brushes.Black;
            line.X1 = left;
            line.Y1 = top;
            line.X2 = width - left;
            line.Y2 = top;
            line.HorizontalAlignment = HorizontalAlignment.Left;
            line.VerticalAlignment = VerticalAlignment.Center;
            line.StrokeThickness = height;
            @this.Children.Add(line);
        }
    }
}
