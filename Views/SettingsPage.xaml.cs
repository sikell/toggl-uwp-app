using TogglTimer.ViewModels;

namespace TogglTimer.Views
{
    public sealed partial class SettingsPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            var viewModel = (INavigationViewModel) DataContext;
            viewModel.NavigateToPage += (sender, e) => { Frame.Navigate(e); };
        }
    }
}