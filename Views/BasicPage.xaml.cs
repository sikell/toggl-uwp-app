using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Unity;
using TogglTimer.Services;
using TogglTimer.ViewModels;

namespace TogglTimer.Views
{
    public sealed partial class BasicPage : Page
    {
        public BasicPage()
        {
            this.InitializeComponent();
            var unityContainer = (UnityContainer) Application.Current.Resources["IoC"];
            var viewModel = (BasicPageViewModel) DataContext;
            viewModel.NavigateToPage += ViewModel_NavigateToPage;
        }

        private void ViewModel_NavigateToPage(object sender, Type e)
        {
            Frame.Navigate(e);
        }
    }
}