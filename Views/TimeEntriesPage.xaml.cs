using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using TogglTimer.ViewModels;

namespace TogglTimer.Views
{
    public sealed partial class TimeEntriesPage
    {
        public TimeEntriesPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var viewModel = (INavigationListeningViewModel) DataContext;
            viewModel.OnNavigatedTo();
        }
    }
}