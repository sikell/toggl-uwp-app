using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TogglTimer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BasicSubPage : Page
    {
        public BasicSubPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string)
            {
                this.Message = String.Format("Clicked on {0}", e.Parameter);
            }
            else
            {
                this.Message = "Hi!";
            }

            base.OnNavigatedTo(e);
        }

        public string Message { get; set; }
    }
}
