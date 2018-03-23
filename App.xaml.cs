using Boilerplate.Views;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=402347&clcid=0x409

namespace Boilerplate
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // This just gets in the way.
                //this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            // Change minimum window size
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(320, 200));

            // Darken the window title bar using a color value to match app theme
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            if (titleBar != null)
            {
                var titleBarColor = (Color)App.Current.Resources["SystemChromeMediumColor"];
                titleBar.BackgroundColor = titleBarColor;
                titleBar.ButtonBackgroundColor = titleBarColor;
            }

            var shell = Window.Current.Content as AppShell;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (shell == null)
            {
                // Create a AppShell to act as the navigation context and navigate to the first page
                shell = new AppShell();

                // Set the default language
                shell.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                // Handle navigation events
                shell.AppFrame.NavigationFailed += OnNavigationFailed;
                shell.AppFrame.Navigated += OnNavigated;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place our app shell in the current Window
                Window.Current.Content = shell;

                // Register a handler for BackRequested events
                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

                // Set visibility of the Back button
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    shell.AppFrame.CanGoBack ?
                    AppViewBackButtonVisibility.Visible :
                    AppViewBackButtonVisibility.Collapsed;
            }

            if (shell.AppFrame.Content == null)
            {
                // When the navigation stack isn't restored, navigate to the first page, suppressing the initial entrance animation.
                shell.AppFrame.Navigate(typeof(LandingPage), e.Arguments, new Windows.UI.Xaml.Media.Animation.SuppressNavigationTransitionInfo());
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when a Navigation event occurs
        /// </summary>
        /// <param name="sender">The source of the navigated request.</param>
        /// <param name="e">Details about the navigated request.</param>
        private static void OnNavigated(object sender, NavigationEventArgs e)
        {
            // Update Back button visibility
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ((Frame)sender).CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        /// <summary>
        /// Invoked when application requests to go back to the previous page
        /// </summary>
        /// <param name="sender">the source of the back request.</param>
        /// <param name="e">Details about the back request.</param>
        private static void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            // Go back if possible
            var shell = Window.Current.Content as AppShell;
            if (shell == null || !shell.AppFrame.CanGoBack) return;
            e.Handled = true;
            shell.AppFrame.GoBack();
        }
    }
}
