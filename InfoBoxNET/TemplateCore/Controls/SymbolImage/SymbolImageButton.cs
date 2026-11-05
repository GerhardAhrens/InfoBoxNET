namespace System.Windows.Controls
{
    using System.Windows;
    using System.Windows.Input;

    public class SymbolImageButton : SymbolImage
    {
        public static readonly DependencyProperty DoubleClickCommandProperty =
            DependencyProperty.Register(
                nameof(DoubleClickCommand),
                typeof(ICommand),
                typeof(SymbolImageButton),
                new PropertyMetadata(null));

        public ICommand DoubleClickCommand
        {
            get => (ICommand)GetValue(DoubleClickCommandProperty);
            set => SetValue(DoubleClickCommandProperty, value);
        }

        public static readonly DependencyProperty DoubleClickCommandParameterProperty =
            DependencyProperty.Register(
                nameof(DoubleClickCommandParameter),
                typeof(object),
                typeof(SymbolImageButton),
                new PropertyMetadata(null));

        public object DoubleClickCommandParameter
        {
            get => GetValue(DoubleClickCommandParameterProperty);
            set => SetValue(DoubleClickCommandParameterProperty, value);
        }

        public static readonly DependencyProperty MouseOverOpacityProperty =
            DependencyProperty.Register(
                nameof(MouseOverOpacity),
                typeof(double),
                typeof(SymbolImageButton),
                new PropertyMetadata(0.65d));

        public double MouseOverOpacity
        {
            get => (double)GetValue(MouseOverOpacityProperty);
            set => SetValue(MouseOverOpacityProperty, value);
        }

        public SymbolImageButton()
        {
            MouseLeftButtonDown += OnMouseLeftButtonDown;
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            Opacity = MouseOverOpacity;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            Opacity = 1.0;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 1)
                return;

            if (DoubleClickCommand is ICommand command && command.CanExecute(DoubleClickCommandParameter))
            {
                command.Execute(DoubleClickCommandParameter);
            }

            e.Handled = true;
        }
    }
}
