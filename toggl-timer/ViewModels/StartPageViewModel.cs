using System.ComponentModel.DataAnnotations;
using MetroLog;
using Prism.Commands;
using Prism.Windows.Navigation;
using Prism.Windows.Validation;
using toggl_timer.Services;
using toggl_timer.Services.Api;

namespace toggl_timer.ViewModels
{
    class StartPageViewModel : ValidatableBindableBase
    {
        private readonly ILogger _log = LogManagerFactory.DefaultLogManager.GetLogger<StartPageViewModel>();
        private string _username;

        public StartPageViewModel(IAuthService authService)
        {
            GetUserCommand = new DelegateCommand(async () =>
            {
                if (!ValidateProperties())
                {
                    _log.Warn("Not all props are valid.");
                    return;
                }

                var user = await authService.GetUser();
                Username = user.data.fullname;
            });
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public DelegateCommand GetUserCommand { get; }
    }
}
