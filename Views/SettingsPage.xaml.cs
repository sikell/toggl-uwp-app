using TogglTimer.ViewModels;

namespace TogglTimer.Views
{
    public sealed partial class SettingsPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            var viewModel = (SettingsPageViewModel) DataContext;
            viewModel.NavigateToPage += (sender, e) => { Frame.Navigate(e); };
        }
    }
}