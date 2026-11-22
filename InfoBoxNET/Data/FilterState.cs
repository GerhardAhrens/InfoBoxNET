namespace InfoBoxNET.Data
{
    using System;

    using InfoBoxNET.Core.Enums;

    public class FilterState
    {
        public Predicate<object> FilterPredicate { get; set; }
        public string SearchText { get; set; }
        public object SelectedItem { get; set; } // Bonus: Auswahl gleich mitmerken

        public AccessTyp AccessTyp { get; set; }

        public string TagFilter { get; set; }
    }
}
