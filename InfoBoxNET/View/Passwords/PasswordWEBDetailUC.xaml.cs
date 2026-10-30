//-----------------------------------------------------------------------
// <copyright file="PasswordWEBDetailUC.cs" company="Lifeprojects.de">
//     Class: PasswordWEBDetailUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>26.10.2026</date>
//
// <summary>
// Erstellen und Ändern von Zugangsdaten für Webseiten.
// Die Klasse PasswordWEBDetailUC ist eine Benutzeroberfläche (UserControl) in einer WPF-Anwendung,
// die es dem Benutzer ermöglicht, Details zu Web-Passwörtern anzuzeigen und zu bearbeiten.
// Sie enthält Logik für das Laden der Ansicht, das Zurücknavigieren und die Verwaltung von Befehlen.
// </summary>
//-----------------------------------------------------------------------

namespace InfoBoxNET.View
{
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;

    using InfoBoxNET.Core;
    using InfoBoxNET.Model;

    /// <summary>
    /// Interaktionslogik für PasswordWEBDetailUC.xaml
    /// </summary>
    public partial class PasswordWEBDetailUC : UserControlBase
    {
        public PasswordWEBDetailUC(PasswordPin rowView, DataRowAction rowAction = DataRowAction.Change) : base(typeof(PasswordWEBDetailUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentRow = rowView;
            this.OriginalRow = rowView;
            this.RowAction = rowAction;
            this.Background = System.Windows.Media.Brushes.LightGreen;

            this.DataContext = this;
        }

        #region Properties

        public string Title
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public bool ShowDescription
        {
            get => base.GetValue<bool>();
            set => base.SetValue(value);
        }

        public string Description
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public Dictionary<int, string> SymbolSource
        {
            get => base.GetValue<Dictionary<int, string>>();
            set => base.SetValue(value);
        }

        public int SelectedSymbol
        {
            get => base.GetValue<int>();
            set => base.SetValue(value);
        }

        public string Website
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public DataRowAction RowAction { get; set; }
        public PasswordPin OriginalRow { get; set; }
        public PasswordPin CurrentRow { get; set; }
        private MessageBase Message { get; } = new MessageBase();

        #endregion Properties

        #region Windows Events

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            using (SymbolsList sl = new SymbolsList())
            {
                this.SymbolSource = sl.GetSymbols();
            }

            if (this.CurrentRow != null && this.RowAction == DataRowAction.Change)
            {
                this.Title = this.CurrentRow.Title;
                this.ShowDescription = this.CurrentRow.ShowDescription;
                this.Description = this.CurrentRow.Description;
                this.SelectedSymbol = this.CurrentRow.Symbol;
                this.HorizontalImageList.SelectedValue = this.CurrentRow.Symbol;
                this.HorizontalImageList.ScrollIntoView(this.CurrentRow.Symbol);
                this.HorizontalImageList.UpdateLayout();
                this.Website = this.CurrentRow.Website;
            }
        }
        #endregion Windows Events

        #region Command Events
        #endregion Command Events

    }
}
