using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Text_Swap.Repositories;
using Text_Swap.View;
using Text_Swap.ViewModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Text_Swap.Commands
{
    public class RegisterCommand : AsyncCommandBase
    {
        private readonly RegisterViewModel _registerViewModel;
        private readonly IAuthentication _authentication;
        private readonly INavigationService _loginNavigationService;

        public RegisterCommand(RegisterViewModel registerViewModel, IAuthentication authentication, INavigationService loginNavigationService)
        {
            _registerViewModel = registerViewModel;
            _authentication = authentication;
            _loginNavigationService = loginNavigationService;
        }
        protected override async Task ExecuteAsync(object parameter)
        {
            var username = _registerViewModel.Username;
            var password = _registerViewModel.Password;
            var email = _registerViewModel.Email;
            var confirmPassword = _registerViewModel.ConfirmPassword;
            if (password != confirmPassword)
            {
                return;
            }
            try
            {

                var userResponse = await _authentication.Register(username, email, password, confirmPassword);
                if (userResponse != null && !userResponse.Error)
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
                    _registerViewModel.ErrorMessage = "Inscription réussie ! (Simulation)";
                }
                else
                {
                    _registerViewModel.ErrorMessage = "* Invalid usernae or password";
                }

                _loginNavigationService.Navigate();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
