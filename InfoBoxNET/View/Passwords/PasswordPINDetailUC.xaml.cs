//-----------------------------------------------------------------------
// <copyright file="PasswordPINDetailUC.cs" company="Lifeprojects.de">
//     Class: PasswordPINDetailUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>26.10.2026</date>
//
// <summary>
// Erstellen und Ändern von PIN-Codes.
// Die Klasse PasswordPINDetailUC ist eine Benutzeroberfläche (UserControl) in einer WPF-Anwendung,
// die es dem Benutzer ermöglicht, Details zu PIN-Codes anzuzeigen und zu bearbeiten.
// Sie enthält Logik für das Laden der Ansicht, das Zurücknavigieren und die Verwaltung von Befehlen.
// </summary>
//-----------------------------------------------------------------------

namespace InfoBoxNET.View
{
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;

    using InfoBoxNET.Model;

    /// <summary>
    /// Interaktionslogik für PasswordWEBDetailUC.xaml
    /// </summary>
    public partial class PasswordPINDetailUC : UserControlBase
    {
        public PasswordPINDetailUC(PasswordPin rowView, DataRowAction rowAction = DataRowAction.Change) : base(typeof(PasswordPINDetailUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentRow = rowView;
            this.RowAction = rowAction;
            this.Background = System.Windows.Media.Brushes.LightBlue;

            this.DataContext = this;
        }

        #region Properties
        public PasswordPin CurrentRow
        {
            get => base.GetValue<PasswordPin>();
            set => base.SetValue(value);
        }

        private PasswordPin OriginalRow { get; set; }
        private PasswordPin CopyRow { get; set; }
        private DataRowAction RowAction { get; set; }
        private MessageBase Message { get; } = new MessageBase();

        #endregion Properties

        #region Windows Events

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
        }
        #endregion Windows Events

        #region Command Events
        #endregion Command Events

    }
}
