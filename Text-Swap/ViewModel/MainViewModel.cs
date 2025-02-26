using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Text_Swap.Model;
using Text_Swap.Repositories;

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

    public MainViewModel()
    {
        _userRepository = new UserRepository();
        CurrentUserAccount = new UserAccountModel();

        ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
        ShowCustomerViewCommand = new ViewModelCommand(ExecuteShowCustomerViewCommand);
        ShowRaccourciViewCommand = new ViewModelCommand(ExecuteShowRaccourciViewCommand);

        //Default view 
        ExecuteShowHomeViewCommand(null);
        LoadCurrentUserData();
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
        Caption = "Dashboard";
        Icon = IconChar.Home;
    }

    private void LoadCurrentUserData()
    {
        //var user = _userRepository.GerByName(Thread.CurrentPrincipal.Identity.Name);
        //if (user == null)
        //{

        //    CurrentUserAccount.UserName = user.UserName;
        //    CurrentUserAccount.DisplayName = $"{user.Name} {user.LastName}";
        //    CurrentUserAccount.ProfilePicture = null;

        //}
        //else
        //{
        //    CurrentUserAccount.DisplayName ="Invalid user, not logged in";
        //}
    }
}
