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
    using System.Windows;
    using System.Windows.Controls;

    using InfoBoxNET.Core;

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

        private ChangeViewEventArgs CurrentCtorArgs { get; set; }
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

    }
}
