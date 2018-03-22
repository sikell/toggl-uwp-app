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
        private User _user;
        private TimeEntry _current;

        public StartPageViewModel(IAuthService authService, ITimeEntryService timeEntryService)
        {
            Load(authService, timeEntryService);
        }

        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public TimeEntry Current
        {
            get => _current;
            set => SetProperty(ref _current, value);
        }

        private async void Load(IAuthService authService, ITimeEntryService timeEntryService)
        {
            User = await authService.GetUser();
            Current = await timeEntryService.GetCurrent();
        }

    }
}
