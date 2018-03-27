using System;
using Prism.Commands;
using Prism.Mvvm;
using TogglTimer.Services;
using TogglTimer.Views;

namespace TogglTimer.ViewModels
{
    public class SettingsPageViewModel : BindableBase
    {
        public SettingsPageViewModel(IAuthService authService)
        {
            LogoutCommand = new DelegateCommand(() =>
            {
                authService.LogoutUser();
                NavigateToPage?.Invoke(this, typeof(LoginPage));
            });
        }

        public DelegateCommand LogoutCommand { get; }

        public event EventHandler<Type> NavigateToPage;
    }
}