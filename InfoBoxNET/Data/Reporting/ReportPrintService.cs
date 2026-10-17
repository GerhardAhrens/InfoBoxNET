namespace InfoBoxNET.Data
{
    using System.Windows.Controls;
    using System.Windows.Documents;


    public static class ReportPrintService
    {
        public static void Print(FixedDocument document)
        {
            var dialog = new PrintDialog();

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            dialog.PrintDocument(document.DocumentPaginator, "Report");
        }

        public static void Print(FixedDocument document, string jobName)
        {
            var dialog = new PrintDialog();

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            dialog.PrintDocument(document.DocumentPaginator, jobName);
        }
    }
}
