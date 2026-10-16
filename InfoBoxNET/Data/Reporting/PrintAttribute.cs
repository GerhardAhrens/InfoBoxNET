//-----------------------------------------------------------------------
// <copyright file="PrintAttribute.cs" company="Lifeprojects.de">
//     Class: PrintAttribute
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.09.2026</date>
//
// <summary>
// Attribut für Modelklassen, die für das Drucken ausgelesen werden.
// </summary>
//-----------------------------------------------------------------------

namespace InfoBoxNET.Data
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class PrintAttribute : Attribute
    {
        public string Header { get; set; }

        public int Order { get; set; }

        // Breite in WPF-DIPs; 1 DIP entspricht 1/96 Zoll.
        // 0 bedeutet: Breite automatisch bestimmen.
        public double Width { get; set; }

        // Optionales Format, z. B. dd.MM.yyyy oder N2.
        public string Format { get; set; }
    }
}
