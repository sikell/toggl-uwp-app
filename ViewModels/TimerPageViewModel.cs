using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
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

            StartTimerCommand =
                new DelegateCommand(async () => Current = await _timeEntryService.StartCurrentTimer(_current));

            StopTimerCommand = new DelegateCommand(async () =>
            {
                await _timeEntryService.StopCurrentTimer();
                Current = await _timeEntryService.GetCurrent();
            });

            RefreshCommand = new DelegateCommand(OnNavigatedTo);
        }

        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        
        public TimeEntry Current
        {
            get => _current;
            set => SetProperty(ref _current, value, () =>
            {
                RaisePropertyChanged(nameof(WhenRunningVisible));
                RaisePropertyChanged(nameof(WhenRunningNotVisible));
                RaisePropertyChanged(nameof(RunningTimeDuration));
            });
        }

        public Visibility WhenRunningNotVisible => BooleanToVisibility(_current == null);
        public Visibility WhenRunningVisible => BooleanToVisibility(_current != null);

        public string RunningTimeDuration =>
            _current == null ? null : DateTime.Now.Subtract(_current.Start).ToString("hh\\:mm\\:ss");

        public DelegateCommand StartTimerCommand { get; }
        public DelegateCommand StopTimerCommand { get; }
        public DelegateCommand RefreshCommand { get; }

        public async void OnNavigatedTo()
        {
            _log.Debug("Load current user info.");
            User = await _authService.GetUser();
            Current = await _timeEntryService.GetCurrent();
        }

        private static Visibility BooleanToVisibility(bool value) => value ? Visibility.Visible : Visibility.Collapsed;
        
    }
}