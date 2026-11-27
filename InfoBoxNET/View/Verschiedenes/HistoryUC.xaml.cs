namespace InfoBoxNET.View
{
    using System.ComponentModel;
    using System.Data;
    using System.Data.SQLite;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using InfoBoxNET.Core;
    using InfoBoxNET.Core.Enums;
    using InfoBoxNET.Model;

    /// <summary>
    /// Interaktionslogik für HistoryUC.xaml
    /// </summary>
    public partial class HistoryUC : UserControlBase
    {
        public HistoryUC(ID id, AccessTyp accessTyp)
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.PasswortTyp = accessTyp;
            this.ObjectId = id;
            this.DataContext = this;
        }

        #region Properties
        public ICollectionView DialogDataView
        {
            get => base.GetValue<ICollectionView>();
            set => base.SetValue(value);
        }

        public History CurrentSelectedItem
        {
            get => base.GetValue<History>();
            set => base.SetValue(value);
        }

        private AccessTyp PasswortTyp { get; set; }
        private ID ObjectId { get; set; }
        #endregion Properties

        #region Windows Events
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TimeSpan t = Performance.Measure(async () =>
                {
                    using (DatabaseService ds = new DatabaseService(App.DatabasePath))
                    {
                        ds.OpenConnection();
                        if (ds.Connection == null)
                        {
                            return;
                        }

                        string sql = ds.SelectRows<History>(whereClause: $"ObjectId = '{this.ObjectId}'", orderBy: "Version DESC");
                        DataTable dataSource = ds.Connection.RecordSet<DataTable>(sql).Get().Result;
                        this.DialogDataView = CollectionViewSource.GetDefaultView(dataSource);
                    }
                });

                await Dispatcher.InvokeAsync(() =>
                {
                    this.lvwMain.SelectedIndex = 0;

                    var firstItem = lvwMain.ItemContainerGenerator.ContainerFromIndex(0) as ListViewItem;
                    if (firstItem != null)
                    {
                        firstItem.Focus();
                        Keyboard.Focus(firstItem);
                    }
                }, System.Windows.Threading.DispatcherPriority.Background);

                if (App.EventAgg.IsSubscription<StatusEvent>() == true)
                {
                    await App.EventAgg.PublishAsync(new StatusEvent($"Bereit; {t.TotalMilliseconds:N1} ms"));
                }

            }
            catch (Exception ex)
            {
                string errorText = $"Error in {nameof(this.OnLoaded)}: {ex.Message}";
                throw;
            }
        }
        #endregion Windows Events
    }
}
