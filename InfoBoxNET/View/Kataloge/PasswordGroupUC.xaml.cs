//-----------------------------------------------------------------------
// <copyright file="PasswordGroupUC.cs" company="Lifeprojects.de">
//     Class: PasswordGroupUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>03.10.2026</date>
//
// <summary>
// Template für eine neues UserControl
// </summary>
//-----------------------------------------------------------------------

namespace InfoBoxNET.View
{
    using System.ComponentModel;
    using System.Data;
    using System.Data.SQLite;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Xml.Linq;

    using InfoBoxNET.Core;

    using static System.Windows.Forms.VisualStyles.VisualStyleElement;

    /// <summary>
    /// Interaktionslogik für PasswordGroupUC.xaml
    /// </summary>
    public partial class PasswordGroupUC : UserControlBase
    {
        public PasswordGroupUC(ChangeViewEventArgs args) : base(typeof(PasswordGroupUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentCtorArgs = args;

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);
            this.SelectedGruppeItem = new CommandBase(commandParam => this.OnSelectedGruppe(commandParam), () => true);
            this.ActionDeleteCommand = new CommandBase(commandParam => this.OnActionDelete(commandParam), () => true);
            this.AddGruppeCommand = new CommandBase(commandParam => this.OnAddGruppe(commandParam), () => true);
            this.UpdateGruppeCommand = new CommandBase(commandParam => this.OnUpdateGruppe(commandParam), () => true);

            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase SelectedGruppeItem { get; private set; }
        public CommandBase ActionDeleteCommand { get; private set; }
        public CommandBase AddGruppeCommand { get; private set; }
        public CommandBase UpdateGruppeCommand { get; private set; }

        public IEnumerable<InfoBoxNET.Model.Region> RegionDataSource
        {
            get => base.GetValue<IEnumerable<InfoBoxNET.Model.Region>>();
            set => base.SetValue(value);
        }

        public InfoBoxNET.Model.Region SelectedRegionItem
        {
            get => base.GetValue<InfoBoxNET.Model.Region>();
            set => base.SetValue(value);
        }

        public Brush SelectedBrush
        {
            get => base.GetValue<Brush>();
            set => base.SetValue(value);
        }

        public string GruppenText
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        private ChangeViewEventArgs CurrentCtorArgs { get; set; }
        #endregion Properties

        #region Windows Events

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue) == false)
            {
                this.LoadDataHandler();
            }

            if (App.EventAgg.IsSubscription<StatusEvent>() == true)
            {
                await App.EventAgg.PublishAsync(new StatusEvent("Bereit"));
            }
        }
        #endregion Windows Events

        #region Command Events
        private async void OnGoBack(object commandParam)
        {
            if (commandParam is CommandButtons button && button == CommandButtons.GoBack)
            {
                ChangeViewEventArgs args = new();
                args.MenuButton = CommandButtons.Home;
                args.FromPage = this.CurrentCtorArgs.MenuButton;
                if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                {
                    await App.EventAgg.PublishAsync(args);
                }
            }
        }

        private void OnSelectedGruppe(object commandParam)
        {
            if (commandParam is SelectionChangedInfo region)
            {
                if (region.SelectedItem != null && region.SelectedItems.Any() == true)
                {
                    InfoBoxNET.Model.Region gruppe = (InfoBoxNET.Model.Region)region.SelectedItems[0];

                    this.GruppenText = gruppe.Name;
                    if (string.IsNullOrEmpty(gruppe.Background) == false)
                    {
                        BrushConverter converter = new BrushConverter();
                        Brush gruppenItemFarbe = (Brush)converter.ConvertFromString(gruppe.Background);
                        this.SelectedBrush = gruppenItemFarbe;
                    }
                }
            }
        }

        private void OnActionDelete(object commandParam)
        {
            if (commandParam is InfoBoxNET.Model.Region region)
            {
                /* Eintrag löschen */
                ID id = new ID(region.Id);
                this.DeleteGruppe(id);
            }
        }

        private void OnAddGruppe(object commandParam)
        {
        }

        private void OnUpdateGruppe(object commandParam)
        {
        }

        #endregion Command Events

        private void LoadDataHandler()
        {
            try
            {
                using (DatabaseService ds = new DatabaseService(App.DatabasePath))
                {
                    ds.OpenConnection();
                    if (ds.Connection == null)
                    {
                        return;
                    }

                    this.RegionDataSource = ds.Connection.RecordSet<List<InfoBoxNET.Model.Region>>("SELECT Id, Name, Background FROM TAB_Region").Get().Result;

                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        private void DeleteGruppe(ID id)
        {
            try
            {
                using (DatabaseService ds = new DatabaseService(App.DatabasePath))
                {
                    ds.OpenConnection();
                    if (ds.Connection == null)
                    {
                        return;
                    }

                    string sql = "DELETE FROM TAB_Region WHERE Id=@Id";
                    Dictionary<string, object> parameterCollection = new();
                    parameterCollection.Add("@Id", id.ToString());
                    int rowsAffected = ds.Connection.RecordSet<int>(sql, parameterCollection).Execute().Result;
                    if (rowsAffected > 0)
                    {
                        this.LoadDataHandler();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }
    }
}
