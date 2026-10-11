//-----------------------------------------------------------------------
// <copyright file="PasswordsUC.cs" company="Lifeprojects.de">
//     Class: PasswordsUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.09.2026</date>
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
    using InfoBoxNET.Model;

    /// <summary>
    /// Interaktionslogik für PasswordsUC.xaml
    /// </summary>
    public partial class PasswordsUC : UserControlBase
    {
        public PasswordsUC(ChangeViewEventArgs args) : base(typeof(PasswordsUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);
            this.ImpExportPasswordsCommand = new CommandBase(commandParam => this.OnImpExport(commandParam), () => true);
            this.CurrentCtorArgs = args;

            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase ImpExportPasswordsCommand { get; private set; }

        public ICollectionView PasswordPinDataSource
        {
            get => base.GetValue<ICollectionView>();
            set => base.SetValue(value);
        }

        public InfoBoxNET.Model.PasswordPin SelectedPasswordPinItem
        {
            get => base.GetValue<PasswordPin>();
            set => base.SetValue(value);
        }

        public string SearchText
        {
            get => base.GetValue<string>();
            set => base.SetValue(value,this.RefreschDataSource);
        }

        private int RowCount { get; set; }
        private ChangeViewEventArgs CurrentCtorArgs { get; set; }
        #endregion Properties

        #region Windows Events

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue) == false)
            {
                this.LoadDataHandler();
            }
        }
        #endregion Windows Events

        #region Command Events
        private async void OnGoBack(object commandParam)
        {
            if (commandParam != null && commandParam is CommandButtons button)
            {
                if (button == CommandButtons.GoBack)
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
        }

        private async void OnImpExport(object commandParam)
        {
            if (commandParam != null && commandParam is CommandButtons button)
            {
                if (button == CommandButtons.ImportExportPasswords)
                {
                    ChangeViewEventArgs args = new();
                    args.MenuButton = CommandButtons.ImportExportPasswords;
                    args.FromPage = this.CurrentCtorArgs.MenuButton;
                    if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                    {
                        await App.EventAgg.PublishAsync(args);
                    }
                }
            }
        }
        #endregion Command Events

        private async void LoadDataHandler()
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

                    string sql = ds.SelectRows<PasswordPin>(whereClause: "Version = 0");
                    this.PasswordPinDataSource = ds.Connection.RecordSet<ICollectionView>(sql).Get().Result;
                    this.RowCount = this.PasswordPinDataSource?.Cast<object>().Count() ?? 0;
                }

                if (App.EventAgg.IsSubscription<StatusEvent>() == true)
                {
                    await App.EventAgg.PublishAsync(new StatusEvent($"Bereit: {this.RowCount} Einträge"));
                }

            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        private void RefreschDataSource(string arg1, string arg2)
        {
            if (arg2 == nameof(this.SearchText))
            {
                if (string.IsNullOrEmpty(this.SearchText) == false)
                {
                    this.PasswordPinDataSource?.Refresh();
                }
            }
        }

    }
}
