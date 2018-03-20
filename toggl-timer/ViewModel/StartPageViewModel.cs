using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Windows.Validation;

namespace toggl_timer.ViewModel
{
    class StartPageViewModel : ValidatableBindableBase
    {
        private string _username;

        [Required(ErrorMessage = "Username is required.")]
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

    }
}
