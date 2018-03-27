using System;

namespace TogglTimer.ViewModels
{
    public interface INavigationViewModel
    {
        event EventHandler<Type> NavigateToPage;
    }
}