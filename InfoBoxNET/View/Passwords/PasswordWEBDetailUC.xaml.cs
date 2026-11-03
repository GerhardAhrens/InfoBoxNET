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
    using System.Data.SQLite;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using InfoBoxNET.Core;
    using InfoBoxNET.Core.Enums;
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
            this.RowAction = rowAction;

            this.DataContext = this;
        }

        #region Properties

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
            set => base.SetValue(value, this.ChangeValue);
        }

        public string Website
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public Dictionary<Guid, string> UsernameSource
        {
            get => base.GetValue<Dictionary<Guid, string>>();
            set => base.SetValue(value);
        }

        public Brush SelectedBackgroundColor
        {
            get => base.GetValue<Brush>();
            set => base.SetValue(value,this.ChangeValue);
        }

        public byte[] Photo
        {
            get => base.GetValue<byte[]>();
            set => base.SetValue(value,this.ChangeValue);
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

        public PasswordPin CurrentRow
        {
            get => base.GetValue<PasswordPin>();
            set => base.SetValue(value);
        }

        public DataRowAction RowAction { get; set; }
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
                using (DatabaseService ds = new DatabaseService(App.DatabasePath))
                {
                    ds.OpenConnection();
                    if (ds.Connection == null)
                    {
                        return;
                    }

                    this.RegionSource = ds.Connection.RecordSet<List<Region>>(ds.SelectRows<Region>()).Get().Result;

                    var usernamesAll = ds.Connection.RecordSet<List<PasswordPin>>("SELECT Username, AccessTyp FROM TAB_PasswordPin").Get().Result;
                    if (usernamesAll != null)
                    {
                        this.UsernameSource = usernamesAll
                            .Where(w => w.AccessTyp == AccessTyp.Website)
                            .OrderBy(o => o.Username)
                            .DistinctBy(d => d.Username)
                            .ToDictionary(k => Guid.CreateVersion7(), v => v.Username);
                    }

                    this.Photo = ds.Connection.RecordSet<byte[]>($"SELECT Content FROM TAB_Attachment WHERE ObjectId = '{this.CurrentRow.Id.ToString()}'").Get().Result;
                }

                this.HorizontalImageList.SelectedValue = this.CurrentRow.Symbol+1;
                this.HorizontalImageList.ScrollIntoView(this.CurrentRow.Symbol+1);
                this.HorizontalImageList.UpdateLayout();
                this.SelectedBackgroundColor = ColorConverters.ConvertNameToBrush(this.CurrentRow.Background);
                this.cbUsername.IsDropDownOpen = false;
                this.TitelField.PART_TextBox.Focus();
                this.TitelField.PART_TextBox.CaretIndex = 0;
            }
            else if (this.CurrentRow != null && this.RowAction == DataRowAction.Add)
            {
                using (DatabaseService ds = new DatabaseService(App.DatabasePath))
                {
                    ds.OpenConnection();
                    if (ds.Connection == null)
                    {
                        return;
                    }

                    this.RegionSource = ds.Connection.RecordSet<List<Region>>(ds.SelectRows<Region>()).Get().Result;

                    var usernamesAll = ds.Connection.RecordSet<List<PasswordPin>>("SELECT Username, AccessTyp FROM TAB_PasswordPin").Get().Result;
                    if (usernamesAll != null)
                    {
                        this.UsernameSource = usernamesAll
                            .Where(w => w.AccessTyp == AccessTyp.Website)
                            .OrderBy(o => o.Username)
                            .DistinctBy(d => d.Username)
                            .ToDictionary(k => Guid.CreateVersion7(), v => v.Username);
                    }

                    this.HorizontalImageList.SelectedValue = 0;
                    this.HorizontalImageList.ScrollIntoView(0);
                    this.HorizontalImageList.UpdateLayout();
                    this.SelectedBackgroundColor = ColorConverters.ConvertNameToBrush();
                }
            }
        }
        #endregion Windows Events

        #region Command Events
        #endregion Command Events
    }
}
