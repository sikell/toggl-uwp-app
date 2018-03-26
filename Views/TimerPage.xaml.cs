using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TogglTimer.Views
{
    public sealed partial class TimerPage : Page
    {
        public TimerPage()
        {
            this.InitializeComponent();
            this.Loaded += CommandBarPage_Loaded;
        }

        private void CommandBarPage_Loaded(object sender, RoutedEventArgs e)
        {
            double? diagonal = DisplayInformation.GetForCurrentView().DiagonalSizeInInches;

            //move commandbar to page bottom on small screens
            if (diagonal < 7)
            {
                Topbar.Visibility = Visibility.Collapsed;
                PageTitleContainer.Visibility = Visibility.Visible;
                Bottombar.Visibility = Visibility.Visible;
            }
            else
            {
                Topbar.Visibility = Visibility.Visible;
                PageTitleContainer.Visibility = Visibility.Collapsed;
                Bottombar.Visibility = Visibility.Collapsed;
            }
        }
    }
}
