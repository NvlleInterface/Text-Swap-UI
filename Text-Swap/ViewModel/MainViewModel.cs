using FontAwesome.Sharp;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Text_Swap.Model;
using Text_Swap.Repositories;
using Text_Swap.View;

namespace Text_Swap.ViewModel;

public class MainViewModel : ViewModelBase
{
    private UserAccountModel _currentUserAccount;
    private IUserRepository _userRepository;
    private ViewModelBase _currentChildView;
    private string _caption;
    private IconChar _icon;
    public UserAccountModel CurrentUserAccount
    {
        get => _currentUserAccount;
        set { _currentUserAccount = value; OnPropertyChanged(nameof(CurrentUserAccount)); }
    }

    public ViewModelBase CurrentChildView
    {
        get => _currentChildView;
        set
        {
            _currentChildView = value;
            OnPropertyChanged(nameof(CurrentChildView));
        }
    }
    public string Caption
    {
        get => _caption;
        set
        {
            _caption = value;
            OnPropertyChanged($"{nameof(Caption)}");
        }
    }
    public IconChar Icon
    {
        get => _icon;
        set
        {
            _icon = value;
            OnPropertyChanged(nameof(Icon));
        }
    }

    public ICommand ShowHomeViewCommand { get; }

    public ICommand ShowRaccourciViewCommand { get; }
    public ICommand ShowCustomerViewCommand { get; }

    public ICommand LogoutCommand { get; }

    public MainViewModel()
    {
        _userRepository = new UserRepository();
        CurrentUserAccount = new UserAccountModel();

        ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
        ShowCustomerViewCommand = new ViewModelCommand(ExecuteShowCustomerViewCommand);
        ShowRaccourciViewCommand = new ViewModelCommand(ExecuteShowRaccourciViewCommand);
        LogoutCommand = new ViewModelCommand(ExecuteLogoutCommand);

        //Default view 
        ExecuteShowHomeViewCommand(null);
    }

    private void ExecuteLogoutCommand(object obj)
    {// Supprimer l'utilisateur du Thread Principal
        var result = MessageBox.Show("Voulez-vous vraiment vous déconnecter ?",
                                 "Confirmation",
                                 MessageBoxButton.YesNo,
                                 MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(string.Empty), null);
            var config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Définit le répertoire de base
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Charge le fichier JSON
                    .Build();
            string path = config["LocalStorage"];
            // Supprimer les infos stockées dans le registre
            var key = Registry.CurrentUser.OpenSubKey(path, true);
            
            if (key != null)
            {
                key.DeleteValue("Username", false);
                key.DeleteValue("Roles", false);
                key.DeleteValue("Token", false);
                key.Close();
            }

            // Ouvrir la fenêtre LoginView
            // TOTO
            Application.Current.Dispatcher.Invoke(() =>
            {
                var name = Application.Current.MainWindow.Name; ;
                var loginView = new LoginView();
                loginView.Show();
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is not LoginView)
                    {
                        window.Close();
                        //break;
                    }
                }
            });
                // Fermer la fenêtre actuelle
                // TOTO

            }
    }

    private void ExecuteShowRaccourciViewCommand(object obj)
    {
        CurrentChildView = new RaccourciViewModel();
        Caption = "Crée vos raccourcis";
        Icon = IconChar.Clone;
    }

    private void ExecuteShowCustomerViewCommand(object obj)
    {
        CurrentChildView = new CustomerViewModel();
        Caption = "Customer";
        Icon = IconChar.UserGroup;
    }

    private void ExecuteShowHomeViewCommand(object obj)
    {
        CurrentChildView = new HomeViewModel();
        Caption = "Accueil";
        Icon = IconChar.Home;
    }
}
