using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using MetroLog;
using Prism.Commands;
using Prism.Mvvm;
using TogglTimer.Services;
using TogglTimer.Services.Api.Model;
using TogglTimer.Services.Model;

namespace TogglTimer.ViewModels
{
    public class TimerPageViewModel : BindableBase
    {
        private readonly ILogger _log = LogManagerFactory.DefaultLogManager.GetLogger<TimerPageViewModel>();

        private User _user;
        private TimeEntry _current;
        private TimeEntry _newEntry;
        private Workspace _workspace;
        private ObservableCollection<Workspace> _workspaces;
        private ObservableCollection<Project> _projects;

        private readonly IAuthService _authService;
        private readonly ITimeEntryService _timeEntryService;
        private readonly IProjectService _projectService;
        private readonly IWorkspaceService _workspaceService;

        public TimerPageViewModel(ITimeEntryService timeEntryService, IAuthService authService,
            IProjectService projectService, IWorkspaceService workspaceService)
        {
            _timeEntryService = timeEntryService;
            _authService = authService;
            _projectService = projectService;
            _workspaceService = workspaceService;

            OnNavigatedTo();

            StartTimerCommand =
                new DelegateCommand(async () =>
                {
                    NewEntry.Start = DateTime.Now;
                    Current = await _timeEntryService.StartCurrentTimer(_newEntry);
                });

            StopTimerCommand = new DelegateCommand(async () =>
            {
                await _timeEntryService.StopCurrentTimer();
                Current = await _timeEntryService.GetCurrent();
            });

            RefreshCommand = new DelegateCommand(OnNavigatedTo);
            
            PropertyChanged += async (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case nameof(Workspace):
                        var projects = await _projectService.ListWorkspaceProjects(_workspace);
                        Projects = new ObservableCollection<Project>(projects);
                        break;
                }
            };
        }

        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public TimeEntry NewEntry
        {
            get => _newEntry;
            set => SetProperty(ref _newEntry, value);
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

        public ObservableCollection<Project> Projects
        {
            get => _projects;
            set => SetProperty(ref _projects, value);
        }

        public Workspace Workspace
        {
            get => _workspace;
            set => SetProperty(ref _workspace, value);
        }

        public ObservableCollection<Workspace> Workspaces
        {
            get => _workspaces;
            set => SetProperty(ref _workspaces, value);
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
            var workspaces = await _workspaceService.ListUserWorkspaces();
            Workspaces = new ObservableCollection<Workspace>(workspaces);
            Workspace = workspaces.Last();
            User = await _authService.GetUser();
            Current = await _timeEntryService.GetCurrent();
            var projects = await _projectService.ListWorkspaceProjects(_workspace);
            Projects = new ObservableCollection<Project>(projects);
            NewEntry = new TimeEntry()
            {
                Description = ""
            };
        }

        private static Visibility BooleanToVisibility(bool value) => value ? Visibility.Visible : Visibility.Collapsed;
    }
}