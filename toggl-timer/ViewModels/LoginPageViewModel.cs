using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MetroLog;
using Prism.Commands;
using Prism.Windows.Navigation;
using Prism.Windows.Validation;
using toggl_timer.Services;

namespace toggl_timer.ViewModels
{
    public class LoginPageViewModel : ValidatableBindableBase, INavigationAware
    {
        private readonly ILogger _log = LogManagerFactory.DefaultLogManager.GetLogger<LoginPageViewModel>();

        private string _username;
        private string _password;

        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;

        public LoginPageViewModel(INavigationService navigationService, IAuthService authService)
        {
            _navigationService = navigationService;
            _authService = authService;

            LoginCommand = new DelegateCommand(async () =>
            {
                _log.Warn("Login user.");
                if (!ValidateProperties())
                {
                    _log.Warn("Not all props are valid.");
                    return;
                }

                if (await authService.AuthUser(_username, _password))
                {
                    navigationService.Navigate("Start", null);
                }
            });
        }
        
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

        public void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            _log.Info("Navigated to login page");
            if (_authService.IsAuthenticated())
            {
                _log.Info("User is authenticated navigate to start.");
                _navigationService.Navigate("Start", null);
            }
        }

        public void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState,
            bool suspending)
        {
            _log.Info("Leave login page");
        }
    }
}