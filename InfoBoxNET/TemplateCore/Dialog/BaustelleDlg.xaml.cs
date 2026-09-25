namespace System.Windows.Controls
{
    using System.Runtime.InteropServices;
    using System.Windows.Interop;

    /// <summary>
    /// Interaktionslogik für BaustelleDlg.xaml
    /// </summary>
    public partial class BaustelleDlg : Window
    {
        /* API Importe (GetWindowLong, SetWindowLong) */
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        public BaustelleDlg(string title, string message)
        {
            this.InitializeComponent();

            this.SourceInitialized += (s, e) =>
            {
                IntPtr hwnd = new WindowInteropHelper(this).Handle;
                // API Aufruf zum Entfernen des Systemmenüs
                _ = SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            };

            this.Dispatcher.Invoke(() =>
            {
                this.TB_Titel.Text = title;
                this.TB_Message.Text = message;
            });

            this.Result = new DialogValueResult
            {
                Accepted = false
            };
        }

        public DialogValueResult Result { get; private set; }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Result.Accepted = true;
            this.DialogResult = true;
            this.Close();
        }

        #region Aufruf WIN 32 API
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        #endregion Aufruf WIN 32 API
    }
}
