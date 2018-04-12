using System.Collections.ObjectModel;
using MetroLog;
using Prism.Mvvm;
using TogglTimer.Services;
using TogglTimer.Services.Model;

namespace TogglTimer.ViewModels
{
    public class TimeEntriesPageViewModel : BindableBase, INavigationListeningViewModel
    {
        private readonly ILogger _log = LogManagerFactory.DefaultLogManager.GetLogger<TimeEntriesPageViewModel>();

        private User _user;
        private ObservableCollection<TimeEntry> _entries;

        private readonly ITimeEntryService _timeEntryService;

        public TimeEntriesPageViewModel(ITimeEntryService timeEntryService)
        {
            _timeEntryService = timeEntryService;
        }

        public ObservableCollection<TimeEntry> TimeEntries
        {
            get => _entries;
            set => SetProperty(ref _entries, value);
        }

        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public async void OnNavigatedTo()
        {
            _log.Debug("Load current entries.");
            TimeEntries = new ObservableCollection<TimeEntry>(await _timeEntryService.ListLastEntries());
        }
    }
}