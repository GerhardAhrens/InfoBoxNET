namespace InfoBoxNET.Core
{
    using System.ComponentModel;

    public enum CommandButtons
    {
        [Description("Keine Auswahl")]
        None = 0,
        [Description("Anwendung beenden")]
        AppQuit = 1,
        [Description("Startseite")]
        Home = 2,
        [Description("Hilfe")]
        Help = 3,
        [Description("Zurück zur vorherigen Seite")]
        GoBack = 4,
        [Description("Passwort Verwaltung")]
        Passwords = 10,
        [Description("Import / Export Passwörter")]
        ImportExportPasswords = 11,
        [Description("Informationen")]
        InformationPopup = 20,
        [Description("Einstellungen")]
        SettingsPopup = 21,
        [Description("Anmeldung")]
        Login = 22,
    }
}
