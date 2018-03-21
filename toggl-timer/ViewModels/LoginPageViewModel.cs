using System.ComponentModel.DataAnnotations;
using MetroLog;
using Prism.Commands;
using Prism.Windows.Navigation;
using Prism.Windows.Validation;
using toggl_timer.Services;

namespace toggl_timer.ViewModels
{
    public class LoginPageViewModel : ValidatableBindableBase
    {
        private readonly ILogger _log = LogManagerFactory.DefaultLogManager.GetLogger<StartPageViewModel>();
        private string _username;
        private string _password;

        public LoginPageViewModel(IAuthService authService, INavigationService navigationService)
        {
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
        
    }
}