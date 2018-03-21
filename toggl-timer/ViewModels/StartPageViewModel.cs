using System.ComponentModel.DataAnnotations;
using MetroLog;
using Prism.Commands;
using Prism.Windows.Validation;
using toggl_timer.Services.Api;

namespace toggl_timer.ViewModels
{
    class StartPageViewModel : ValidatableBindableBase
    {
        private readonly ILogger _log = LogManagerFactory.DefaultLogManager.GetLogger<StartPageViewModel>();
        private string _username;
        private string _token;

        public StartPageViewModel(IApiClient apiClient)
        {
            GetUserCommand = new DelegateCommand(async () =>
            {
                if (!ValidateProperties())
                {
                    _log.Warn("Not all props are valid.");
                    return;
                }

                var user = await apiClient.GetUser(_token);
                Username = user.data.fullname;
            });
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        [Required(ErrorMessage = "Token is required.")]
        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }

        public DelegateCommand GetUserCommand { get; private set; }
    }
}
