namespace InfoBoxNET.Data
{
    using System.Printing;
    using System.Windows.Controls;
    using System.Windows.Documents;

    public static class ReportPrintService
    {
        public static bool Print(DocumentPaginator paginator, ReportSettings settings, string jobName = null)
        {
            ArgumentNullException.ThrowIfNull(paginator);
            ArgumentNullException.ThrowIfNull(settings);

            var dialog = new PrintDialog();

            // Druckeinstellungen vorbereiten.
            dialog.PrintTicket ??= new PrintTicket();

            dialog.PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4);

            dialog.PrintTicket.PageOrientation = settings.Orientation == ReportOrientation.Landscape ? PageOrientation.Landscape : PageOrientation.Portrait;

            // Druckdialog anzeigen.
            if (dialog.ShowDialog() != true)
            {
                return false;
            }

            // Benutzeränderungen im Druckdialog bleiben erhalten.
            dialog.PrintDocument(paginator, jobName ?? settings.PrintJobName);

            return true;
        }
    }
}
