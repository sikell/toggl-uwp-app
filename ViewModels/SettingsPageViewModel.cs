using Prism.Commands;
using Prism.Mvvm;
using TogglTimer.Services;

namespace TogglTimer.ViewModels
{
    public class SettingsPageViewModel : BindableBase
    {
        public SettingsPageViewModel(IAuthService authService)
        {
            LogoutCommand = new DelegateCommand(() => authService.LogoutUser());
        }

        public DelegateCommand LogoutCommand { get; }
    }
}