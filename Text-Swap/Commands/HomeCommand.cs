using FontAwesome.Sharp;
using MVVMEssentials.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_Swap.ViewModel;

namespace Text_Swap.Commands
{
    public class HomeCommand : CommandBase
    {
        private readonly ContentViewModel _currentViewModel;

        public HomeCommand(ContentViewModel currentViewModel)
        {
            _currentViewModel = currentViewModel;
        }

        public override void Execute(object parameter)
        {
            _currentViewModel.CurrentChildView = new HomeViewModel();
            _currentViewModel.Caption = "Acceuil";
            _currentViewModel.Icon = IconChar.Home;
        }
    }
}
