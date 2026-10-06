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
    using System.Data.SQLite;
    using System.Windows;
    using System.Windows.Controls;

    using InfoBoxNET.Core;

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

            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }

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
        #endregion Command Events

        private void LoadDataHandler(bool isRefresh = false)
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

                    this.RegionDataSource = ds.Connection.RecordSet<List<InfoBoxNET.Model.Region>>("SELECT Id, Name FROM TAB_Region").Get().Result;

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
