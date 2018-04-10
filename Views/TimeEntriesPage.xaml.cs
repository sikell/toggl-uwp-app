using Windows.UI.Xaml.Controls;

namespace TogglTimer.Views
{
    public sealed partial class TimeEntriesPage : Page
    {
        public TimeEntriesPage()
        {
            this.InitializeComponent();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(
                typeof(TogglTimer.Views.BasicSubPage),
                e.ClickedItem,
                new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
        }
    }
}
