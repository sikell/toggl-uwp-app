using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Prism.Unity.Windows;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using toggl_timer.Services;
using toggl_timer.Services.Api;

namespace toggl_timer
{

    sealed partial class App : PrismUnityApplication
    {

        public App()
        {
            InitializeComponent();
        }

        protected override UIElement CreateShell(Frame rootFrame)
        {
            var shell = Container.TryResolve<AppShell>();
            shell.SetContentFrame(rootFrame);
            return shell;
        }
        
        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate(PageTokens.Login.ToString(), null);
            return Task.FromResult(true);
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            RegisterTypeIfMissing(typeof(IApiClient), typeof(ApiClient), true);
            RegisterTypeIfMissing(typeof(IAuthService), typeof(AuthService), true);
            RegisterTypeIfMissing(typeof(ITimeEntryService), typeof(TimeEntryService), true);
        }
    }
}
