namespace System.Windows.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Media3D;

    /// <summary>
    /// Interaktionslogik für WindowAutoCompleteTextBox.xaml
    /// </summary>
    public partial class WindowAutoCompleteTextBox : UserControl
    {
        private Window _ownerWindow;
        private bool _closingPopup;

        public WindowAutoCompleteTextBox()
        {
            this.InitializeComponent();
        }

        #region Dependency Properties

        // Text
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(WindowAutoCompleteTextBox),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnTextChanged));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private void PART_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.UpdatePlaceholder();
            this.UpdateSuggestions();
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WindowAutoCompleteTextBox control)
            {
                control.UpdatePlaceholder();
                control.UpdateSuggestions();
            }
        }

        // Icon
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(
                nameof(Icon),
                typeof(DrawingImage),
                typeof(WindowAutoCompleteTextBox));

        public DrawingImage Icon
        {
            get => (DrawingImage)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        // IconBackground
        public static readonly DependencyProperty IconBackgroundProperty =
            DependencyProperty.Register(
                nameof(IconBackground),
                typeof(Brush),
                typeof(WindowAutoCompleteTextBox),
                new PropertyMetadata(Brushes.White));

        public Brush IconBackground
        {
            get => (Brush)GetValue(IconBackgroundProperty);
            set => SetValue(IconBackgroundProperty, value);
        }

        // TextBackground
        public static readonly DependencyProperty TextBackgroundProperty =
            DependencyProperty.Register(
                nameof(TextBackground),
                typeof(Brush),
                typeof(WindowAutoCompleteTextBox),
                new PropertyMetadata(Brushes.White));

        public Brush TextBackground
        {
            get => (Brush)GetValue(TextBackgroundProperty);
            set => SetValue(TextBackgroundProperty, value);
        }

        // TextForeground
        public static readonly DependencyProperty TextForegroundProperty =
            DependencyProperty.Register(
                nameof(TextForeground),
                typeof(Brush),
                typeof(WindowAutoCompleteTextBox),
                new PropertyMetadata(Brushes.Black));

        public Brush TextForeground
        {
            get => (Brush)GetValue(TextForegroundProperty);
            set => SetValue(TextForegroundProperty, value);
        }

        // Placeholder
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(
                nameof(Placeholder),
                typeof(string),
                typeof(WindowAutoCompleteTextBox),
                new PropertyMetadata(string.Empty, OnPlaceholderChanged));

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        private static void OnPlaceholderChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is WindowAutoCompleteTextBox control)
                control.UpdatePlaceholder();
        }

        // PlaceholderForeground
        public static readonly DependencyProperty PlaceholderForegroundProperty =
            DependencyProperty.Register(
                nameof(PlaceholderForeground),
                typeof(Brush),
                typeof(WindowAutoCompleteTextBox),
                new PropertyMetadata(Brushes.Gray));

        public Brush PlaceholderForeground
        {
            get => (Brush)GetValue(PlaceholderForegroundProperty);
            set => SetValue(PlaceholderForegroundProperty, value);
        }

        // IsReadOnly
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(
                nameof(IsReadOnly),
                typeof(bool),
                typeof(WindowAutoCompleteTextBox),
                new PropertyMetadata(false));

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        // MaxLength
        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register(
                nameof(MaxLength),
                typeof(int),
                typeof(WindowAutoCompleteTextBox),
                new PropertyMetadata(0));

        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        // CornerRadius
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(WindowAutoCompleteTextBox),
                new PropertyMetadata(new CornerRadius(7)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        // SuggestionMaxHeight
        public static readonly DependencyProperty SuggestionMaxHeightProperty =
            DependencyProperty.Register(
                nameof(SuggestionMaxHeight),
                typeof(double),
                typeof(WindowAutoCompleteTextBox),
                new PropertyMetadata(220.0, OnSuggestionMaxHeightChanged));

        public double SuggestionMaxHeight
        {
            get => (double)GetValue(SuggestionMaxHeightProperty);
            set => SetValue(SuggestionMaxHeightProperty, value);
        }

        private static void OnSuggestionMaxHeightChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is WindowAutoCompleteTextBox control &&
                control.SuggestionList != null)
            {
                control.SuggestionList.MaxHeight =
                    (double)e.NewValue;
            }
        }

        // HighlightBrush
        public static readonly DependencyProperty HighlightBrushProperty =
            DependencyProperty.Register(
                nameof(HighlightBrush),
                typeof(Brush),
                typeof(WindowAutoCompleteTextBox),
                new PropertyMetadata(
                    new SolidColorBrush(Color.FromRgb(220, 235, 255))));

        public Brush HighlightBrush
        {
            get => (Brush)GetValue(HighlightBrushProperty);
            set => SetValue(HighlightBrushProperty, value);
        }

        // Suggestions: einfache List<string>
        public static readonly DependencyProperty SuggestionsProperty =
            DependencyProperty.Register(
                nameof(Suggestions),
                typeof(List<string>),
                typeof(WindowAutoCompleteTextBox),
                new PropertyMetadata(null, OnSuggestionsChanged));

        public List<string> Suggestions
        {
            get => (List<string>)GetValue(SuggestionsProperty);
            set => SetValue(SuggestionsProperty, value);
        }

        private static void OnSuggestionsChanged(DependencyObject d,DependencyPropertyChangedEventArgs e)
        {
            if (d is WindowAutoCompleteTextBox control)
            {
                control.UpdateSuggestions();
            }
        }

        // ObservableSuggestions
        public static readonly DependencyProperty ObservableSuggestionsProperty =
            DependencyProperty.Register(
                nameof(ObservableSuggestions),
                typeof(ObservableCollection<string>),
                typeof(WindowAutoCompleteTextBox),
                new PropertyMetadata(null, OnObservableSuggestionsChanged));

        public ObservableCollection<string> ObservableSuggestions
        {
            get => (ObservableCollection<string>)GetValue(ObservableSuggestionsProperty);
            set => SetValue(ObservableSuggestionsProperty, value);
        }

        private static void OnObservableSuggestionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not WindowAutoCompleteTextBox control)
            {
                return;
            }

            if (e.OldValue is ObservableCollection<string> oldCollection)
            {
                oldCollection.CollectionChanged -= control.ObservableSuggestions_CollectionChanged;
            }

            if (e.NewValue is ObservableCollection<string> newCollection)
            {
                newCollection.CollectionChanged += control.ObservableSuggestions_CollectionChanged;
            }

            control.UpdateSuggestions();
        }

        private void ObservableSuggestions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new Action(UpdateSuggestions));
                return;
            }

            UpdateSuggestions();
        }

        #endregion

        #region ICommands
        public static readonly DependencyProperty F2CommandProperty =
        DependencyProperty.Register(
            nameof(F2Command),
            typeof(ICommand),
            typeof(WindowAutoCompleteTextBox),
            new PropertyMetadata(null));

        public ICommand F2Command
        {
            get => (ICommand)GetValue(F2CommandProperty);
            set => SetValue(F2CommandProperty, value);
        }

        public static readonly DependencyProperty F2CommandParameterProperty =
        DependencyProperty.Register(
            nameof(F2CommandParameter),
            typeof(object),
            typeof(WindowAutoCompleteTextBox),
            new PropertyMetadata(null));

        public object F2CommandParameter
        {
            get => GetValue(F2CommandParameterProperty);
            set => SetValue(F2CommandParameterProperty, value);
        }

        public static readonly DependencyProperty ENTERCommandProperty =
        DependencyProperty.Register(
            nameof(ENTERCommand),
            typeof(ICommand),
            typeof(WindowAutoCompleteTextBox),
            new PropertyMetadata(null));

        public ICommand ENTERCommand
        {
            get => (ICommand)GetValue(ENTERCommandProperty);
            set => SetValue(ENTERCommandProperty, value);
        }

        #endregion ICommands

        #region Placeholder

        private void UpdatePlaceholder()
        {
            if (PlaceholderText == null || PART_TextBox == null)
            {
                return;
            }

            bool visible = string.IsNullOrEmpty(Text) && !PART_TextBox.IsKeyboardFocused && !string.IsNullOrEmpty(Placeholder);

            PlaceholderText.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

        #region Suggestions

        private class SuggestionItem
        {
            public string Value { get; set; }
            public string Prefix { get; set; }
            public string Match { get; set; }
            public string Suffix { get; set; }

            public Brush HighlightBrush { get; set; }
            public Brush HighlightForeground { get; set; }
        }

        private void SubscribeSuggestions()
        {
            if (this.ObservableSuggestions != null)
            {
                this.ObservableSuggestions.CollectionChanged -= this.ObservableSuggestions_CollectionChanged;

                this.ObservableSuggestions.CollectionChanged += this.ObservableSuggestions_CollectionChanged;
            }
        }

        private void UpdateSuggestions()
        {
            if (!IsLoaded || SuggestionList == null || PART_TextBox == null)
            {
                return;
            }

            IEnumerable<string> source = Enumerable.Empty<string>();

            if (this.Suggestions != null)
            {
                source = source.Concat(this.Suggestions);
            }

            if (this.ObservableSuggestions != null)
            {
                source = source.Concat(this.ObservableSuggestions);
            }

            string search = this.PART_TextBox.Text ?? string.Empty;

            var filtered = source
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Where(x =>
                    string.IsNullOrWhiteSpace(search) ||
                    x.IndexOf(search,
                        StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(x => CreateSuggestionItem(x, search))
                .ToList();

            SuggestionList.ItemsSource = filtered;

            SuggestionPopup.Width = ActualWidth;

            if (PART_TextBox.IsKeyboardFocused && !IsReadOnly && filtered.Count > 0)
            {
                this.OpenSuggestionPopup();
            }
            else
            {
                this.CloseSuggestionPopup();
            }
        }

        private SuggestionItem CreateSuggestionItem(string value, string search)
        {
            int index = string.IsNullOrEmpty(search) ? -1 : value.IndexOf(search, StringComparison.OrdinalIgnoreCase);

            if (index < 0)
            {
                return new SuggestionItem
                {
                    Value = value,
                    Prefix = value,
                    Match = string.Empty,
                    Suffix = string.Empty,
                    HighlightBrush = HighlightBrush,
                    HighlightForeground = TextForeground
                };
            }

            return new SuggestionItem
            {
                Value = value,
                Prefix = value.Substring(0, index),
                Match = value.Substring(index, search.Length),
                Suffix = value.Substring(index + search.Length),
                HighlightBrush = HighlightBrush,
                HighlightForeground = TextForeground
            };
        }

        // Für Änderungen an einer normalen List<string>
        // nach der Änderung explizit aufrufen.
        public void RefreshSuggestions()
        {
            UpdateSuggestions();
        }

        private void SuggestionList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (SuggestionList.SelectedItem is SuggestionItem item)
            {
                ApplySuggestion(item.Value);
            }
        }

        private void ApplySuggestion(string value)
        {
            Text = value;

            PART_TextBox.CaretIndex = value.Length;

            CloseSuggestionPopup();

            PART_TextBox.Focus();
        }

        #endregion

        #region Popup Animation

        private void OpenSuggestionPopup()
        {
            if (!SuggestionPopup.IsOpen)
            {
                _closingPopup = false;

                SuggestionPopup.Width = ActualWidth;
                SuggestionPopup.IsOpen = true;
                SuggestionPopup.Opacity = 0;

                var animation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromMilliseconds(150),
                    EasingFunction = new QuadraticEase
                    {
                        EasingMode = EasingMode.EaseOut
                    }
                };

                SuggestionPopup.BeginAnimation(UIElement.OpacityProperty, animation);
            }
        }

        private void CloseSuggestionPopup()
        {
            if (!this.SuggestionPopup.IsOpen || this._closingPopup)
            {
                return;
            }

            this._closingPopup = true;

            var animation = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromMilliseconds(120),
                EasingFunction = new QuadraticEase
                {
                    EasingMode = EasingMode.EaseIn
                }
            };

            animation.Completed += (s, e) =>
            {
                if (_closingPopup)
                {
                    this.SuggestionPopup.IsOpen = false;
                    this.SuggestionPopup.BeginAnimation(UIElement.OpacityProperty, null);

                    this.SuggestionPopup.Opacity = 1;
                    _closingPopup = false;
                }
            };

            this.SuggestionPopup.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        #endregion

        #region Keyboard

        private void PART_TextBox_GotFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.PART_TextBox.Text) == true)
            {
                return;
            }

            MainBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 120, 212));

            this.UpdatePlaceholder();
            this.UpdateSuggestions();
        }

        private void PART_TextBox_LostFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            MainBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(209, 209, 209));

            this.UpdatePlaceholder();
        }

        private void PART_TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                var parameter = new WindowTextBoxCommandParameter(
                    PART_TextBox.Text,
                    PART_TextBox.CaretIndex,
                    PART_TextBox.SelectionStart,
                    PART_TextBox.SelectionLength,
                    PART_TextBox.SelectedText,
                    PART_TextBox.SelectionStart
                );

                if (F2Command?.CanExecute(parameter) == true)
                {
                    F2Command.Execute(parameter);
                    e.Handled = true;
                }
            }

            if (e.Key == Key.Enter)
            {
                if (ENTERCommand?.CanExecute(null) == true)
                {
                    ENTERCommand.Execute(null);
                    e.Handled = true;
                }
            }

            if (!this.SuggestionPopup.IsOpen)
            {
                return;
            }

            int count = this.SuggestionList.Items.Count;

            if (count == 0)
            {
                return;
            }

            if (e.Key == Key.Down)
            {
                this.SuggestionList.SelectedIndex = this.SuggestionList.SelectedIndex < count - 1 ? this.SuggestionList.SelectedIndex + 1 : 0;

                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                this.SuggestionList.SelectedIndex = this.SuggestionList.SelectedIndex <= 0 ? count - 1 : this.SuggestionList.SelectedIndex - 1;

                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                if (this.SuggestionList.SelectedItem is SuggestionItem item)
                {
                    this.ApplySuggestion(item.Value);
                    e.Handled = true;
                }
                else
                {
                    this.CloseSuggestionPopup();
                }
            }
            else if (e.Key == Key.Escape)
            {
                this.CloseSuggestionPopup();
                e.Handled = true;
            }
        }

        #endregion

        #region Clear

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.PART_TextBox.Clear();
            this.CloseSuggestionPopup();
            this.PART_TextBox.Focus();
        }

        #endregion

        #region Window Events

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            this._ownerWindow = Window.GetWindow(this);

            if (this._ownerWindow != null)
            {
                this._ownerWindow.PreviewMouseDown += OwnerWindow_PreviewMouseDown;

                this._ownerWindow.Deactivated += OwnerWindow_Deactivated;
            }

            this.SubscribeSuggestions();

            this.UpdatePlaceholder();

            // Erst nach dem Laden und der Binding-Initialisierung
            Dispatcher.BeginInvoke(new Action(UpdateSuggestions), System.Windows.Threading.DispatcherPriority.DataBind);
        }

        private void Control_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this._ownerWindow != null)
            {
                this._ownerWindow.PreviewMouseDown -= this.OwnerWindow_PreviewMouseDown;
                this._ownerWindow.Deactivated -= this.OwnerWindow_Deactivated;

                this._ownerWindow = null;
            }

            this.SuggestionPopup.IsOpen = false;
        }

        private void Control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.SuggestionPopup.Width = this.ActualWidth;
        }

        private void OwnerWindow_Deactivated(object sender, EventArgs e)
        {
            this.CloseSuggestionPopup();
        }

        private void OwnerWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!SuggestionPopup.IsOpen)
            {
                return;
            }

            DependencyObject source = e.OriginalSource as DependencyObject;

            if (this.IsInside(source, this) || this.IsInside(source, PopupBorder))
            {
                return;
            }

            this.CloseSuggestionPopup();
        }

        private void SuggestionList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(SuggestionList, e.OriginalSource as DependencyObject) as ListBoxItem;

            if (item?.DataContext is SuggestionItem suggestion)
            {
                this.ApplySuggestion(suggestion.Value);
                e.Handled = true;
            }
        }

        private bool IsInside(DependencyObject child, DependencyObject parent)
        {
            while (child != null)
            {
                if (child == parent)
                    return true;

                if (child is Visual || child is Visual3D)
                {
                    child = VisualTreeHelper.GetParent(child);
                }
                else
                {
                    child = LogicalTreeHelper.GetParent(child);
                }
            }

            return false;
        }
        #endregion
    }

    public record WindowTextBoxCommandParameter(
        string Text,
        int CaretIndex,
        int SelectionStart,
        int SelectionLength,
        string SelectedText,
        int SelectedIndex
    );
}
