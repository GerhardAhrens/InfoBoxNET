//-----------------------------------------------------------------------
// <copyright file="ImpExportPasswordsUC.cs" company="Lifeprojects.de">
//     Class: ImpExportPasswordsUC
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
    using System.Data.SQLite;
    using System.IO;
    using System.Text.Json;
    using System.Windows;
    using System.Windows.Controls;

    using InfoBoxNET.Core;

    /// <summary>
    /// Interaktionslogik für ImpExportPasswordsUC.xaml
    /// </summary>
    public partial class ImpExportPasswordsUC : UserControlBase
    {
        public ImpExportPasswordsUC(ChangeViewEventArgs args) : base(typeof(ImpExportPasswordsUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentCtorArgs = args;

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);
            this.ImportCommand = new CommandBase(commandParam => this.OnImport(commandParam), () => true);
            this.ExportCommand = new CommandBase(commandParam => this.OnExport(commandParam), () => true);

            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase ImportCommand { get; private set; }
        public CommandBase ExportCommand { get; private set; }

        public bool IsProgressOverlay
        {
            get => base.GetValue<bool>();
            set => base.SetValue(value);
        }

        public int ProgressValue
        {
            get => base.GetValue<int>();
            set => base.SetValue(value);
        }

        public string ProgressText
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string ExportFolder
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string ImportFolder
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        private ChangeViewEventArgs CurrentCtorArgs { get; set; }
        private MessageBase Message { get; } = new MessageBase();
        #endregion Properties

        #region Windows Events

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (App.EventAgg.IsSubscription<StatusEvent>() == true)
            {
                await App.EventAgg.PublishAsync(new StatusEvent("Bereit"));
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
                    args.MenuButton = this.CurrentCtorArgs.FromPage;
                    args.FromPage = this.CurrentCtorArgs.MenuButton;
                    if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                    {
                        await App.EventAgg.PublishAsync(args);
                    }
                }
            }
        }

        private void OnExport(object commandParam)
        {
            try
            {
            }
            catch (Exception ex)
            {
                App.ErrorMessage(ex, $"Dialog: {this.Name}");
                App.ApplicationExit();
            }
        }

        private void OnImport(object commandParam)
        {
            if (string.IsNullOrEmpty(this.ImportFolder) == true)
            {
                this.Message.Warning("Verzeichnis", "Es wurde kein Verzeichnis ausgewählt");
                return;
            }

            try
            {
                this.IsProgressOverlay = true;

                this.ImportAllRows(this.ImportFolder);

                this.IsProgressOverlay = false;
            }
            catch (Exception ex)
            {
                App.ErrorMessage(ex, $"Dialog: {this.Name}");
                App.ApplicationExit();
            }
        }
        #endregion Command Events

        private void ImportAllRows(string folder)
        {
            string importSyncFile = string.Empty;

            try
            {
                importSyncFile = $"{this.ImportFolder}\\PasswortSync.Tag";
                if (File.Exists(importSyncFile) == false)
                {
                    return;
                }

                string jsonText = File.ReadAllText(importSyncFile);
                List<Region> importRegion = jsonText.JsonToList<Region>();
                if (importRegion != null || importRegion.Count > 0)
                {
                    this.ProgressText = "Import Passwortdaten";
                    this.ProgressValue = 0;
                    App.DoEvents();

                    using (DatabaseService ds = new DatabaseService(App.DatabasePath))
                    {
                        ds.OpenConnection();
                        if (ds.Connection == null)
                        {
                            return;
                        }

                        ds.Connection.RecordSet<int>("DELETE FROM TAB_Region").Execute();

                        double stepValue = 0;
                        double maxValue = importRegion.Count;
                        foreach (Region region in importRegion)
                        {
                            stepValue++;
                            this.ProgressValue = Convert.ToInt32(Math.Abs((stepValue / maxValue) * 100));
                            Thread.Sleep(1000);
                            App.DoEvents();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("ImportAllRows:", ex);
            }
        }

    }
}
