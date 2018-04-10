using System;
using System.Collections.ObjectModel;
using System.Linq;
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
    public class TimerPageViewModel : BindableBase, INavigationListeningViewModel
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

            StartTimerCommand = new DelegateCommand(async () =>
            {
                NewEntry.Start = DateTime.Now;
                Current = await _timeEntryService.StartCurrentTimer(_newEntry);
            });

            StopTimerCommand = new DelegateCommand(async () =>
            {
                await _timeEntryService.StopCurrentTimer();
                LoadCurrentTimeEntry();
            });

            RefreshCommand = new DelegateCommand(OnNavigatedTo);

            PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case nameof(Workspace):
                        LoadProjects(Task.FromResult(Workspace));
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

        public void OnNavigatedTo()
        {
            _log.Debug("Load current user info.");
            LoadUser();
            NewEntry = new TimeEntry()
            {
                Description = ""
            };
            var workspace = LoadWorkspaces();
            LoadProjects(workspace);
            LoadCurrentTimeEntry();
        }

        private async void LoadUser()
        {
            User = await _authService.GetUser();
        }

        private async void LoadCurrentTimeEntry()
        {
            Current = await _timeEntryService.GetCurrent();
        }

        private async void LoadProjects(Task<Workspace> workspace)
        {
            var projects = await _projectService.ListWorkspaceProjects(await workspace);
            if (projects == null) return;
            Projects = new ObservableCollection<Project>(projects);
        }

        private async Task<Workspace> LoadWorkspaces()
        {
            var workspaces = await _workspaceService.ListUserWorkspaces();
            if (workspaces == null) return null;
            Workspaces = new ObservableCollection<Workspace>(workspaces);
            Workspace = workspaces.Last();
            return Workspace;
        }

        private static Visibility BooleanToVisibility(bool value) => value ? Visibility.Visible : Visibility.Collapsed;
    }
}