using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Text_Swap.Repositories;
using Text_Swap.Services;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Windows;

namespace Text_Swap.ViewModel;

public class LoginViewModel : ViewModelBase
{
    private string _username;
    private SecureString _password;
    private string _errorMessage;
    private bool _isViewVisible = true;

    private readonly IUserRepository _userRepository;

    public string Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(nameof(Username)); }
    }
    public SecureString Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(nameof(Password)); }
    }
    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
    }
    public bool IsViewVisible
    {
        get => _isViewVisible;
        set { _isViewVisible = value; OnPropertyChanged(nameof(IsViewVisible)); }
    }

    public ICommand LoginCommand { get; }
    public ICommand RecoverPasswwordCommand { get; }
    public ICommand ShowPasswordCommand { get; }
    public ICommand RememberPasswordCommand { get; }

    public LoginViewModel()
    {
        _userRepository = new UserRepository();
        LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        RecoverPasswwordCommand = new ViewModelCommand(p => ExecuteRecoverPassCommand("", ""));
    }

    private void ExecuteRecoverPassCommand(string username, string email)
    {
        throw new NotImplementedException();
    }

    private bool CanExecuteLoginCommand(object obj)
    {
        bool validDate;
        if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3
            || Password == null || Password.Length < 3)
        {
            validDate = false;
        }
        else
        {
            validDate = true;
        }
        return validDate;
    }

    private void ExecuteLoginCommand(object obj)
    {
        var userResponse = _userRepository.LoginAsync(new System.Net.NetworkCredential(Username, Password));
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
            }

            //Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);

            //Application.Current.Properties["UserName"] = userName;
            IsViewVisible = false;
        }
        else if (userResponse != null && userResponse.Error)
        {
            ErrorMessage = string.Join("", userResponse.Message);
        }
        else
        {
            ErrorMessage = "Le serveur n'a pas pu etre joint. verifeir votre connexion reseau";
        }
    }
}
