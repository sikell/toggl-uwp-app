using Windows.UI.Xaml.Controls;

namespace Boilerplate.Views
{
    public sealed partial class DrillInPage : Page
    {
        public DrillInPage()
        {
            this.InitializeComponent();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(
                typeof(BasicSubPage),
                e.ClickedItem,
                new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
        }
    }
}
