using System.Collections.Generic;
using MetroLog;
using Prism.Mvvm;
using Prism.Windows.Navigation;
using TogglTimer.Services;
using TogglTimer.Services.Model;

namespace TogglTimer.ViewModels
{
    public class CommandBarPageViewModel : BindableBase
    {
        private readonly ILogger _log = LogManagerFactory.DefaultLogManager.GetLogger<CommandBarPageViewModel>();

        private User _user;
        private TimeEntry _current;

        private readonly IAuthService _authService;
        private readonly ITimeEntryService _timeEntryService;

        public CommandBarPageViewModel(ITimeEntryService timeEntryService, IAuthService authService)
        {
            _timeEntryService = timeEntryService;
            _authService = authService;
            OnNavigatedTo();
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

        public async void OnNavigatedTo()
        {
            User = await _authService.GetUser();
            Current = await _timeEntryService.GetCurrent();
        }
    }
}