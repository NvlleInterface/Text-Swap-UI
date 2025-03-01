using FontAwesome.Sharp;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using Text_Swap.Commands;
using Text_Swap.Repositories;

namespace Text_Swap.ViewModel
{
    public class ContentViewModel : ViewModelBase
    {
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;

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
        public ICommand NavigateRaccourciCommand { get; }
        public ICommand NavigateLoginCommand { get; }
        public ICommand NavigateHomeViewCommand { get; }
        public ContentViewModel(
            INavigationService loginNavigateService,
        INavigationService raccourcisNavigateService)
        {
            NavigateLoginCommand = new NavigateCommand(loginNavigateService);
            NavigateRaccourciCommand = new RaccourciCommand(this);
            NavigateHomeViewCommand = new HomeCommand(this);

            ExecuteDefaultViewCommand();
        }

        private void ExecuteDefaultViewCommand()
        {
            CurrentChildView = new HomeViewModel();
            Caption = "Accueil";
            Icon = IconChar.Home;
        }
    }
}
