using MetroLog;
using Prism.Windows.Validation;
using toggl_timer.Services;
using toggl_timer.Services.Api.Model;
using toggl_timer.Services.Model;

namespace toggl_timer.ViewModels
{
    class StartPageViewModel : ValidatableBindableBase
    {
        private readonly ILogger _log = LogManagerFactory.DefaultLogManager.GetLogger<StartPageViewModel>();
        private string _username;
        private TimeEntry _current;

        public StartPageViewModel(IAuthService authService, ITimeEntryService timeEntryService)
        {
            Load(authService, timeEntryService);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public TimeEntry Current
        {
            get => _current;
            set => SetProperty(ref _current, value);
        }

        private async void Load(IAuthService authService, ITimeEntryService timeEntryService)
        {
            var user = await authService.GetUser();
            Username = user.data.fullname;
            var currentTimeEntry = await timeEntryService.GetCurrent();
            Current = currentTimeEntry;
        }

    }
}
