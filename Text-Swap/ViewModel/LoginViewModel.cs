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
using MVVMEssentials.ViewModels;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using Text_Swap.Commands;

namespace Text_Swap.ViewModel;

public class LoginViewModel : ViewModelBase
{
    private string _email;
    private SecureString _password;
    private string _errorMessage;
    private bool _isViewVisible = true;


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

    public ICommand SubmitCommand { get; }
    public ICommand NavigateForgotPasswordCommand { get; }
    public ICommand ShowPasswordCommand { get; }
    public ICommand RememberPasswordCommand { get; }
    public ICommand NavigateRegisterCommand { get; }

    public LoginViewModel(IAuthentication authentication,
        INavigationService registerNavigateService,
        INavigationService contentNavigateService)
    {
        SubmitCommand = new LoginCommand(this, authentication, contentNavigateService);
        NavigateRegisterCommand = new NavigateCommand(registerNavigateService);
    }

}
