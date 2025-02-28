using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Text_Swap.Repositories;
using Text_Swap.View;

namespace Text_Swap.ViewModel;

public class RegisterViewModel : ViewModelBase
{
    private string _username;
    private string _email;
    private SecureString _password;
    private SecureString _confirmPassword;
    private string _errorMessage;
    private bool _isViewVisible;
    private readonly IUserRepository _userRepository;

    public string Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(nameof(Username)); }
    }

    public string Email
    {
        get => _email;
        set { _email = value; OnPropertyChanged(nameof(Email)); }
    }

    public SecureString Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(nameof(Password)); }
    }

    public SecureString ConfirmPassword
    {
        get => _confirmPassword;
        set { _confirmPassword = value; OnPropertyChanged(nameof(ConfirmPassword)); }
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

    public ICommand RegisterCommand { get; }

    public RegisterViewModel()
    {
        _userRepository = new UserRepository();
        RegisterCommand = new ViewModelCommand(ExecuteRegisterCommand, CanExecuteRegisterCommand);
    }

    private bool CanExecuteRegisterCommand(object obj)
    {
        return !string.IsNullOrWhiteSpace(Username) &&
               !string.IsNullOrWhiteSpace(Email) &&
               Password != null && Password.Length >= 6 &&
               ConfirmPassword != null && ConfirmPassword.Length >= 6;
    }

    private void ExecuteRegisterCommand(object obj)
    {
        if (new System.Net.NetworkCredential(string.Empty, Password).Password !=
            new System.Net.NetworkCredential(string.Empty, ConfirmPassword).Password)
        {
            ErrorMessage = "Les mots de passe ne correspondent pas.";
            return;
        }

        var userResponse = _userRepository.Register(Username, Email, Password, ConfirmPassword);
        if (userResponse != null && !userResponse.Error)
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
            IsViewVisible = false;
            //Application.Current.Dispatcher.Invoke(() =>
            //{
                var loginView = new LoginView();
                loginView.Show();

                // Fermer la fenêtre actuelle
                Application.Current.MainWindow.Close();
            //});
            ErrorMessage = "Inscription réussie ! (Simulation)";
        }
        else
        {
            ErrorMessage = "* Invalid usernae or password";
        }
        
    }
}
