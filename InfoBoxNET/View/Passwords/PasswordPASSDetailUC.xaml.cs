//-----------------------------------------------------------------------
// <copyright file="PasswordPASSDetailUC.cs" company="Lifeprojects.de">
//     Class: PasswordPASSDetailUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>26.10.2026</date>
//
// <summary>
// Erstellen und Ändern von Passwörtern für Webseiten und Geräten.
// Die Klasse PasswordPASSDetailUC ist eine Benutzeroberfläche (UserControl) in einer WPF-Anwendung,
// die es dem Benutzer ermöglicht, Details zu Web-Passwörtern anzuzeigen und zu bearbeiten.
// Sie enthält Logik für das Laden der Ansicht, das Zurücknavigieren und die Verwaltung von Befehlen.
// </summary>
//-----------------------------------------------------------------------

namespace InfoBoxNET.View
{
    using System.Data;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Controls;

    using InfoBoxNET.Core;
    using InfoBoxNET.Model;

    /// <summary>
    /// Interaktionslogik für PasswordPASSDetailUC.xaml
    /// </summary>
    public partial class PasswordPASSDetailUC : UserControlBase
    {
        public PasswordPASSDetailUC(PasswordPin rowView, DataRowAction rowAction = DataRowAction.Change) : base(typeof(PasswordPASSDetailUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentRow = rowView;
            this.RowAction = rowAction;
            this.Background = System.Windows.Media.Brushes.Orange;

            this.DataContext = this;
        }

        #region Properties
        public CommandBase PasswordGeneratorCommand { get; private set; }

        public IEnumerable<Region> RegionSource
        {
            get => base.GetValue<IEnumerable<Region>>();
            set => base.SetValue(value);
        }

        public Region SelectedRegion
        {
            get => base.GetValue<Region>();
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
            set => base.SetValue(value, this.ChangeValue);
        }

        public Dictionary<Guid, string> UsernameSource
        {
            get => base.GetValue<Dictionary<Guid, string>>();
            set => base.SetValue(value);
        }

        public string Username
        {
            get => base.GetValue<string>();
            set => base.SetValue(value, this.ChangeValue);
        }

        public Brush SelectedBackgroundColor
        {
            get => base.GetValue<Brush>();
            set => base.SetValue(value, this.ChangeValue);
        }

        public byte[] Photo
        {
            get => base.GetValue<byte[]>();
            set => base.SetValue(value, this.ChangeValue);
        }

        private void ChangeValue(object arg1, string arg2)
        {
            if (arg2 == nameof(this.Photo))
            {
                this.CurrentRow.Photo = (byte[])arg1;
            }
            else if (arg2 == nameof(this.SelectedBackgroundColor))
            {
                this.CurrentRow.Background = ColorConverters.ConvertBrushToName((Brush)arg1);
            }
        }

        private void ChangeValue(int arg1, string arg2)
        {
            if (arg2 == nameof(this.SelectedSymbol))
            {
                this.CurrentRow.Symbol = arg1;
            }
        }

        private void ChangeValue(string arg1, string arg2)
        {
            if (arg2 == nameof(this.Username))
            {
                this.CurrentRow.Username = arg1;
            }
        }

        public PasswordPin CurrentRow
        {
            get => base.GetValue<PasswordPin>();
            set => base.SetValue(value);
        }

        public DataRowAction RowAction { get; set; }
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

            }
            else if (this.CurrentRow != null && this.RowAction == DataRowAction.Add)
            {

            }
        }
        #endregion Windows Events

        #region Command Events
        #endregion Command Events

    }
}
