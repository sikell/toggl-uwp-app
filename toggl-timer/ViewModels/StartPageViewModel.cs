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
            Load(authService);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private async void Load(IAuthService authService)
        {
            var user = await authService.GetUser();
            Username = user.data.fullname;
        }

    }
}
