using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Text_Swap.Repositories;
using Text_Swap.Services;
using Text_Swap.ViewModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Text_Swap.Commands
{
    public class LoginCommand : AsyncCommandBase
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly IAuthentication _authentication;
        private readonly INavigationService _contentNavigationService;

        public LoginCommand(LoginViewModel loginViewModel, IAuthentication authentication, INavigationService contentNavigationService)
        {
            _loginViewModel = loginViewModel;
            _authentication = authentication;
            _contentNavigationService = contentNavigationService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            var password = _loginViewModel.Password;
            var email = _loginViewModel.Email;

            var userResponse = await _authentication.LoginAsync(new System.Net.NetworkCredential(email, password));
            if (userResponse != null && !userResponse.Error)
            {
                var token = userResponse.Data?.Token;
                var refreshToken = userResponse.Data?.RefreshToken;
                var role = userResponse.Data?.Role;
                var (username, roles) = JwtHelper.DecodeJwtToken(token);

                if (!string.IsNullOrEmpty(username))
                {
                    var identity = new GenericIdentity(username);
                    var principal = new GenericPrincipal(identity, roles);
                    Thread.CurrentPrincipal = principal;
                    if (Thread.CurrentPrincipal == null && Thread.CurrentPrincipal.Identity.IsAuthenticated == false)
                    {
                        // Si aucun principal n'est défini ou l'utilisateur n'est pas authentifié, définissons le nouveau principal
                        AppDomain.CurrentDomain.SetThreadPrincipal(principal);
                    }
                    else
                    {
                        // Le principal est déjà défini
                        Console.WriteLine("Un principal a déjà été défini sur ce thread.");
                    }// Garde les infos dans tous les threads

                    //SaveUserToSecureStorage(username, roles, token); // Sauvegarde pour une reconnexion future
                    _contentNavigationService.Navigate();
                }
            }
            else if (userResponse != null && userResponse.Error)
            {
                _loginViewModel.ErrorMessage = string.Join("", userResponse.Message);
            }
            else
            {
                _loginViewModel.ErrorMessage = "Le serveur n'a pas pu etre joint. verifeir votre connexion reseau";
            }
        }
    }
}
