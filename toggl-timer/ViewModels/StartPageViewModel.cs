using System.Collections.Generic;
using MetroLog;
using Prism.Windows.Navigation;
using Prism.Windows.Validation;
using toggl_timer.Services;
using toggl_timer.Services.Api.Model;
using toggl_timer.Services.Model;

namespace toggl_timer.ViewModels
{
    class StartPageViewModel : ValidatableBindableBase, INavigationAware
    {
        private readonly ILogger _log = LogManagerFactory.DefaultLogManager.GetLogger<StartPageViewModel>();

        private User _user;
        private TimeEntry _current;

        private readonly IAuthService _authService;
        private readonly ITimeEntryService _timeEntryService;

        public StartPageViewModel(ITimeEntryService timeEntryService, IAuthService authService)
        {
            _timeEntryService = timeEntryService;
            _authService = authService;
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
        
        public async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            User = await _authService.GetUser();
            Current = await _timeEntryService.GetCurrent();
        }

        public void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            _log.Info("Leave page start");
        }
    }
}
