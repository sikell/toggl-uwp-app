using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using toggl_timer.ViewModels;

namespace toggl_timer.Views
{
    public sealed partial class MenuView : INotifyPropertyChanged
    {
        public MenuView()
        {
            InitializeComponent();
            DataContextChanged += MenuControl_DataContextChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MenuViewViewModel ConcreteDataContext => DataContext as MenuViewViewModel;

        private void MenuControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var propertyChanged = PropertyChanged;
            propertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConcreteDataContext)));
        }
    }
}
