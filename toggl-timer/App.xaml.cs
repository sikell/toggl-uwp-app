using System;
using System.Collections.Generic;
using System.IO;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Prism.Unity.Windows;
using System.Threading.Tasks;
using toggl_timer.Services.Api;

namespace toggl_timer
{
    /// <summary>
    /// Stellt das anwendungsspezifische Verhalten bereit, um die Standardanwendungsklasse zu ergänzen.
    /// </summary>
    sealed partial class App : PrismUnityApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate("Start", null);
            return Task.FromResult(true);
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            RegisterTypeIfMissing(typeof(IApiClient), typeof(ApiClient), true);
        }
    }
}
