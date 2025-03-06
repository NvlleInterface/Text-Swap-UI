using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Text_Swap.Commands;
using Text_Swap.Repositories;

namespace Text_Swap.ViewModel
{
    public class RegisterViewModel: ViewModelBase
    {
        private string _username;
        private string _email;
        private SecureString _password;
        private SecureString _confirmPassword;
        private string _errorMessage;
        private bool _isViewVisible;
        private string _nom;
        private string _prenom;

        public string Prenom
        {
            get { return _prenom; }
            set { _prenom = value; OnPropertyChanged(nameof(Prenom)); }
        }


        public string Nom
        {
            get { return _nom; }
            set { _nom = value; OnPropertyChanged(nameof(Nom)); }
        }


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

        public ICommand SubmitCommand { get; }
        public ICommand NavigateLoginCommand { get; }

        public RegisterViewModel(IAuthentication authentication, INavigationService loginNavigateService)
        {
            SubmitCommand = new RegisterCommand(this, authentication, loginNavigateService);
            NavigateLoginCommand = new NavigateCommand(loginNavigateService);
        }
    }
}
