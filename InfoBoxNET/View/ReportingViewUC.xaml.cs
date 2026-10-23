//-----------------------------------------------------------------------
// <copyright file="ReportingViewUC.cs" company="Lifeprojects.de">
//     Class: ReportingViewUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>18.10.2026</date>
//
// <summary>
// Template für eine neues UserControl
// </summary>
//-----------------------------------------------------------------------

namespace InfoBoxNET.View
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;

    using InfoBoxNET.Core;
    using InfoBoxNET.Data;
    using InfoBoxNET.Model;

    /// <summary>
    /// Interaktionslogik für ReportingViewUC.xaml
    /// </summary>
    public partial class ReportingViewUC : UserControlBase
    {
        public ReportingViewUC(ChangeViewEventArgs args) : base(typeof(ReportingViewUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);
            this.PrintCommand = new CommandBase(commandParam => this.OnPrint(commandParam), () => true);
            this.FitToWidthCommand = new CommandBase(commandParam => this.OnFitToWidth(commandParam), () => true);
            this.FitToMaxPagesAcrossCommand = new CommandBase(commandParam => this.OnFitToMaxPagesAcross(commandParam), () => true);
            this.ZoomCommand = new CommandBase(commandParam => this.OnZoom(commandParam), () => true);

            this.CurrentCtorArgs = args;

            this.ZoomValue = 100;

            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase PrintCommand { get; private set; }
        public CommandBase FitToWidthCommand { get; private set; }
        public CommandBase FitToMaxPagesAcrossCommand { get; private set; }
        public CommandBase ZoomCommand { get; private set; }

        public FixedDocument Document
        {
            get => base.GetValue<FixedDocument>();
            set => base.SetValue(value);
        }

        public double ZoomValue
        {
            get => base.GetValue<double>();
            set => base.SetValue(value);
        }

        private ReportSettings ReportSetting { get; set; }
        private ChangeViewEventArgs CurrentCtorArgs { get; set; }
        #endregion Properties

        #region Windows Events

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var settings = new ReportSettings
                {
                    Orientation = ReportOrientation.Landscape,
                    PrintJobName = "Password-PIN-Übersicht",
                    HeaderFactory = (page, total) => 
                    new StackPanel 
                        { Children = 
                            {
                                new TextBlock {Text = "Password-PIN-Übersicht", FontSize = 18, FontWeight = FontWeights.Bold},
                                new TextBlock {Text = $"Erstellt am: {DateTime.Now:dd.MM.yyyy}"}
                            }
                        },

                    FooterFactory = (page, total) =>
                        new TextBlock
                        {
                            Text = $"Seite {page} von {total}",
                            HorizontalAlignment = HorizontalAlignment.Right
                        }
                };

                this.Document = GenericReportBuilder.Build<PasswordPin>(this.CurrentCtorArgs.DataSource, settings);

            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

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
                    // Viewer zuerst vom Dokument lösen.
                    ReportViewer.Document = null;

                    ChangeViewEventArgs args = new();
                    args.MenuButton = CommandButtons.Passwords;
                    args.FromPage = this.CurrentCtorArgs.MenuButton;
                    if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                    {
                        await App.EventAgg.PublishAsync(args);
                    }
                }
            }
        }

        private void OnPrint(object commandParam)
        {
            try
            {
                ReportPrintService.Print(this.Document, this.ReportSetting.PrintJobName);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        private void OnFitToWidth(object commandParam)
        {
            DocumentViewer.FitToWidthCommand.Execute(null, this.ReportViewer);
        }

        private void OnFitToMaxPagesAcross(object commandParam)
        {
            DocumentViewer.FitToMaxPagesAcrossCommand.Execute(commandParam, this.ReportViewer);
        }

        private void OnZoom(object commandParam)
        {
            NavigationCommands.Zoom.Execute("100.0", this.ReportViewer);
        }
        #endregion Command Events

    }
}
