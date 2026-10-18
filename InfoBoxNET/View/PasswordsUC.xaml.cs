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
    using System.Data;
    using System.Data.SQLite;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using InfoBoxNET.Core;
    using InfoBoxNET.Data;
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
            this.AddEntryCommand = new CommandBase(commandParam => this.OnAddEntry(commandParam), () => true);
            this.CopyEntryCommand = new CommandBase(commandParam => this.OnCopyEntry(commandParam), () => true);
            this.DeleteEntryCommand = new CommandBase(commandParam => this.OnDeleteEntry(commandParam), () => true);
            this.ExportPasswordsCommand = new CommandBase(commandParam => this.OnExportPasswords(commandParam), () => true);
            this.PrintPasswordsCommand = new CommandBase(commandParam => this.OnPrintPasswords(commandParam), () => true);
            this.ImpExportPasswordsCommand = new CommandBase(commandParam => this.OnImpExport(commandParam), () => true);
            this.DoubleClickEntryCommand = new CommandBase(commandParam => this.OnDoubleClickEntry(commandParam), () => true);
            this.SelectEntryCommand = new CommandBase(commandParam => this.OnSelectEntry(commandParam), () => true);

            this.CurrentCtorArgs = args;

            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase AddEntryCommand { get; private set; }
        public CommandBase CopyEntryCommand { get; private set; }
        public CommandBase DeleteEntryCommand { get; private set; }
        public CommandBase ExportPasswordsCommand { get; private set; }
        public CommandBase PrintPasswordsCommand { get; private set; }
        public CommandBase ImpExportPasswordsCommand { get; private set; }
        public CommandBase DoubleClickEntryCommand { get; private set; }
        public CommandBase SelectEntryCommand { get; private set; }

        public ICollectionView DialogDataView
        {
            get => base.GetValue<ICollectionView>();
            set => base.SetValue(value);
        }

        public InfoBoxNET.Model.PasswordPin CurrentSelectedItem
        {
            get => base.GetValue<PasswordPin>();
            set => base.SetValue(value);
        }

        public string FilterDefaultSearch
        {
            get => base.GetValue<string>();
            set => base.SetValue(value,this.RefreschDataSource);
        }

        public ViewBase CustomView
        {
            get => base.GetValue<ViewBase>();
            set => base.SetValue(value);
        }

        public bool IsLoadingOverlay
        {
            get => base.GetValue<bool>();
            set => base.SetValue(value);
        }

        private int RowCount { get; set; }
        private int RowFilterCount { get; set; }
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

        private void OnAddEntry(object commandParam)
        {
            if (commandParam is CommandButtons button && button == CommandButtons.AddEntry)
            {
            }
        }

        private void OnCopyEntry(object commandParam)
        {
            if (commandParam is CommandButtons button && button == CommandButtons.CopyEntry)
            {
            }
        }

        private async void OnDeleteEntry(object commandParam)
        {
            if (commandParam is CommandButtons button && button == CommandButtons.DeleteEntry)
            {

            }
        }

        private async void OnPrintPasswords(object commandParam)
        {
            if (commandParam is CommandButtons button && button == CommandButtons.PrintDocument)
            {
                ChangeViewEventArgs args = new();
                args.MenuButton = CommandButtons.PrintDocument;
                args.FromPage = this.CurrentCtorArgs.MenuButton;
                if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                {
                    await App.EventAgg.PublishAsync(args);
                }

                /*
                var settings = new ReportSettings
                {
                    Title = "Password-PIN-Übersicht",
                    Orientation = ReportOrientation.Landscape,
                    PrintJobName = "Password-PIN-Übersicht"
                };

                var document = GenericReportBuilder.Build<PasswordPin>(this.DialogDataView, settings);

                ReportPrintService.Print(document, settings.PrintJobName);
                */
            }
        }

        private async void OnExportPasswords(object commandParam)
        {
            if (commandParam is CommandButtons button && button == CommandButtons.ExportPasswords)
            {
                var dialog = new SaveFileDialog
                {
                    Title = "PasswordPins exportieren",
                    Filter = "Excel-Datei (*.xlsx)|*.xlsx",
                    DefaultExt = ".xlsx",
                    AddExtension = true,
                    FileName = "PasswordPins.xlsx"
                };

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.IsLoadingOverlay = true;

                    // 2. Warten, bis der Dispatcher das UI-Layout und die Container im Hintergrund gerendert hat
                    await Dispatcher.InvokeAsync(() =>
                    {
                        ExcelExportService.Export<PasswordPin>(this.DialogDataView, dialog.FileName);

                    }, System.Windows.Threading.DispatcherPriority.Background);

                    this.IsLoadingOverlay = false;
                }
            }
        }

        private async void OnImpExport(object commandParam)
        {
            if (commandParam is CommandButtons button && button == CommandButtons.ImportExportPasswords)
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

        private void OnDoubleClickEntry(object commandParam)
        {
            if (commandParam != null && commandParam is PasswordPin pw)
            {

            }
        }

        private void OnSelectEntry(object commandParam)
        {
            if (commandParam != null && commandParam is PasswordPin pw)
            {

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
                    List<PasswordPin> dataSource = ds.Connection.RecordSet<List<PasswordPin>>(sql).Get().Result;
                    this.DialogDataView = CollectionViewSource.GetDefaultView(dataSource);
                    if (this.DialogDataView != null)
                    {
                        this.DialogDataView.Filter = rowItem => this.DataDefaultFilter(rowItem as PasswordPin);
                        this.DialogDataView.SortDescriptions.Add(new SortDescription("AccessTyp", ListSortDirection.Ascending));
                        this.DialogDataView.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
                        this.DialogDataView.MoveCurrentToFirst();
                        var item = this.DialogDataView.CurrentItem;
                    }

                    this.RowCount = this.DialogDataView?.Cast<PasswordPin>().Count() ?? 0;
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

        private bool DataDefaultFilter(PasswordPin rowItem)
        {
            bool found = false;

            if (rowItem == null)
            {
                return false;
            }

            string textFilterString = (this.FilterDefaultSearch ?? string.Empty).ToUpper();
            if (string.IsNullOrEmpty(textFilterString) == false)
            {
                string fullRow = rowItem.ToSearchFilter.ToUpper();
                if (string.IsNullOrEmpty(fullRow) == true)
                {
                    return true;
                }

                string[] words = textFilterString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words.AsParallel<string>())
                {
                    found = fullRow.Contains(word);

                    if (found == false)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return true;
            }

            return found;
        }


        private async void RefreschDataSource(string arg1, string arg2)
        {
            if (arg2 == nameof(this.FilterDefaultSearch))
            {
                if (string.IsNullOrEmpty(this.FilterDefaultSearch) == false)
                {
                    this.DialogDataView?.Refresh();
                    this.RowFilterCount = this.DialogDataView?.Cast<PasswordPin>().Count() ?? 0;

                    if (App.EventAgg.IsSubscription<StatusEvent>() == true)
                    {
                        await App.EventAgg.PublishAsync(new StatusEvent($"Bereit: {this.RowCount} Einträge / {this.RowFilterCount} gefiltert"));
                    }
                }
                else
                {
                    this.DialogDataView?.Refresh();
                    if (App.EventAgg.IsSubscription<StatusEvent>() == true)
                    {
                        await App.EventAgg.PublishAsync(new StatusEvent($"Bereit: {this.RowCount} Einträge"));
                    }
                }
            }
        }

    }
}
