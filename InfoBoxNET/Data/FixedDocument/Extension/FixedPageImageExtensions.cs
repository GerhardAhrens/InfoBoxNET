//-----------------------------------------------------------------------
// <copyright file="FixedPageImageExtensions.cs" company="Lifeprojects.de">
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

public static partial class FixedPageExtensions
    {
        public static void Image(this FixedPage @this, ImageSource source, double size, Thickness position)
        {
            Image image = new Image
            {
                Source = source,
                Width = size,
                Height = size,
                Stretch = Stretch.Uniform
            };

            image.Margin = position;

            @this.Children.Add(image);
        }
    }
}
