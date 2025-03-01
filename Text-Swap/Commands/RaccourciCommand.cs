using FontAwesome.Sharp;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_Swap.enums;
using Text_Swap.Repositories;
using Text_Swap.ViewModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Text_Swap.Commands
{
    public class RaccourciCommand : CommandBase
    {
        private readonly ContentViewModel _currentViewModel;

        public RaccourciCommand(ContentViewModel currentViewModel)
        {
            _currentViewModel = currentViewModel;
        }

        public override void Execute(object parameter)
        {
            _currentViewModel.CurrentChildView = new RaccourciViewModel();
            _currentViewModel.Caption = "Crée vos raccourcis";
            _currentViewModel.Icon = IconChar.Clone;
        }
    }
}
