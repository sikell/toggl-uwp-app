using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Events;
using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

namespace toggl_timer.ViewModels
{
    public class MenuViewViewModel : ViewModelBase
    {
        private const string CurrentPageTokenKey = "CurrentPageToken";
        private readonly Dictionary<PageTokens, bool> _canNavigateLookup;
        private PageTokens _currentPageToken;
        private readonly INavigationService _navigationService;
        private readonly ISessionStateService _sessionStateService;

        public MenuViewViewModel(IEventAggregator eventAggregator, INavigationService navigationService,
            IResourceLoader resourceLoader, ISessionStateService sessionStateService)
        {
            eventAggregator.GetEvent<NavigationStateChangedEvent>().Subscribe(OnNavigationStateChanged);
            _navigationService = navigationService;
            _sessionStateService = sessionStateService;

            Commands = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel
                {
                    DisplayName = resourceLoader.GetString("MainPageMenuItemDisplayName"),
                    FontIcon = "\ue15f",
                    Command = new DelegateCommand(() => NavigateToPage(PageTokens.Login),
                        () => CanNavigateToPage(PageTokens.Login))
                },
                new MenuItemViewModel
                {
                    DisplayName = resourceLoader.GetString("SecondPageMenuItemDisplayName"),
                    FontIcon = "\ue19f",
                    Command = new DelegateCommand(() => NavigateToPage(PageTokens.Start),
                        () => CanNavigateToPage(PageTokens.Start))
                }
            };

            _canNavigateLookup = new Dictionary<PageTokens, bool>();

            foreach (PageTokens pageToken in Enum.GetValues(typeof(PageTokens)))
            {
                _canNavigateLookup.Add(pageToken, true);
            }

            if (_sessionStateService.SessionState.ContainsKey(CurrentPageTokenKey))
            {
                // Resuming, so update the menu to reflect the current page correctly
                if (Enum.TryParse(_sessionStateService.SessionState[CurrentPageTokenKey].ToString(),
                    out PageTokens currentPageToken))
                {
                    UpdateCanNavigateLookup(currentPageToken);
                    RaiseCanExecuteChanged();
                }
            }
        }

        public ObservableCollection<MenuItemViewModel> Commands { get; set; }

        private void OnNavigationStateChanged(NavigationStateChangedEventArgs args)
        {
            if (Enum.TryParse(args.Sender.Content.GetType().Name.Replace("Page", string.Empty),
                out PageTokens currentPageToken))
            {
                _sessionStateService.SessionState[CurrentPageTokenKey] = currentPageToken.ToString();
                UpdateCanNavigateLookup(currentPageToken);
                RaiseCanExecuteChanged();
            }
        }

        private void NavigateToPage(PageTokens pageToken)
        {
            if (CanNavigateToPage(pageToken))
            {
                if (_navigationService.Navigate(pageToken.ToString(), null))
                {
                    UpdateCanNavigateLookup(pageToken);
                    RaiseCanExecuteChanged();
                }
            }
        }

        private bool CanNavigateToPage(PageTokens pageToken)
        {
            return _canNavigateLookup[pageToken];
        }

        private void UpdateCanNavigateLookup(PageTokens navigatedTo)
        {
            _canNavigateLookup[_currentPageToken] = true;
            _canNavigateLookup[navigatedTo] = false;
            _currentPageToken = navigatedTo;
        }

        private void RaiseCanExecuteChanged()
        {
            foreach (var item in Commands)
            {
                (item.Command as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }
    }
}