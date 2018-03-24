using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Windows.UI.Xaml.Controls;
using MetroLog;
using Prism.Commands;
using Prism.Windows.Navigation;
using Prism.Windows.Validation;
using TogglTimer.Services;
using TogglTimer.Views;

namespace TogglTimer.ViewModels
{
    public class LoginPageViewModel : ValidatableBindableBase
    {
        private readonly ILogger _log = LogManagerFactory.DefaultLogManager.GetLogger<LoginPageViewModel>();

        private string _username;
        private string _password;

        private readonly IAuthService _authService;
        
        public LoginPageViewModel(IAuthService authService)
        {
            _authService = authService;

            LoginCommand = new DelegateCommand(async () =>
            {
                if (!ValidateProperties())
                {
                    _log.Warn("Not all props are valid.");
                    return;
                }

                if (await authService.AuthUser(_username, _password))
                {
                    NavigateToPage?.Invoke(this, typeof(CommandBarPage));
                }
            });
        }

        public event EventHandler<Type> NavigateToPage;

        [Required(ErrorMessage = "Username is required.")]
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        [Required(ErrorMessage = "Password is required.")]
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public DelegateCommand LoginCommand { get; }

    }
}