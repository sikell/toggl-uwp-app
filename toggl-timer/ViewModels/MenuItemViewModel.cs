using System.Windows.Input;
using Prism.Windows.Mvvm;

namespace toggl_timer.ViewModels
{
    public class MenuItemViewModel : ViewModelBase
    {
        public string DisplayName { get; set; }

        public string FontIcon { get; set; }

        public ICommand Command { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}