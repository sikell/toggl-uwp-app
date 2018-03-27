using TogglTimer.ViewModels;

namespace TogglTimer.Views
{
    public sealed partial class LoginPage
    {
        public LoginPage()
        {
            InitializeComponent();

            var viewModel = (INavigationViewModel) DataContext;
            viewModel.NavigateToPage += (sender, e) => { Frame.Navigate(e); };
        }
    }
}