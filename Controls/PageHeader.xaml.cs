using Windows.Foundation;
using Windows.UI.Xaml;

namespace TogglTimer.Controls
{
    public sealed partial class PageHeader
    {
        public PageHeader()
        {
            InitializeComponent();

            Loaded += (s, a) =>
            {
                AppShell.Current.TogglePaneButtonRectChanged += Current_TogglePaneButtonSizeChanged;
                TitleBar.Margin = new Thickness(AppShell.Current.TogglePaneButtonRect.Right, 0, 0, 0);
            };
        }

        private void Current_TogglePaneButtonSizeChanged(AppShell sender, Rect e)
        {
            TitleBar.Margin = new Thickness(e.Right, 0, 0, 0);
        }

        public UIElement HeaderContent
        {
            get => (UIElement) GetValue(HeaderContentProperty);
            set => SetValue(HeaderContentProperty, value);
        }

        // Using a DependencyProperty as the backing store for HeaderContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderContentProperty =
            DependencyProperty.Register("HeaderContent", typeof(UIElement), typeof(PageHeader),
                new PropertyMetadata(DependencyProperty.UnsetValue));
    }
}