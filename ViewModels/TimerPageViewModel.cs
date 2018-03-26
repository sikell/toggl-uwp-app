using MetroLog;
using Prism.Commands;
using Prism.Mvvm;
using TogglTimer.Services;
using TogglTimer.Services.Model;

namespace TogglTimer.ViewModels
{
    public class TimerPageViewModel : BindableBase
    {
        private readonly ILogger _log = LogManagerFactory.DefaultLogManager.GetLogger<TimerPageViewModel>();

        private User _user;
        private TimeEntry _current;

        private readonly IAuthService _authService;
        private readonly ITimeEntryService _timeEntryService;

        public TimerPageViewModel(ITimeEntryService timeEntryService, IAuthService authService)
        {
            _timeEntryService = timeEntryService;
            _authService = authService;

            OnNavigatedTo();

            StartTimerCommand = new DelegateCommand(async () => Current = await _timeEntryService.StartCurrentTimer(_current));

            StopTimerCommand = new DelegateCommand(async () =>
            {
                await _timeEntryService.StopCurrentTimer();
                Current = await _timeEntryService.GetCurrent();
            });
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

        public DelegateCommand StartTimerCommand;
        public DelegateCommand StopTimerCommand;

        public async void OnNavigatedTo()
        {
            _log.Debug("Load current user info.");
            User = await _authService.GetUser();
            Current = await _timeEntryService.GetCurrent();
        }
    }
}