using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Text_Swap.ViewModel;

namespace Text_Swap.View
{
    /// <summary>
    /// Logique d'interaction pour HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        private readonly HomeViewModel _viewModel;
        public HomeView()
        {
            InitializeComponent();
            _viewModel = new HomeViewModel();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            _viewModel.HandleKeyPress(e.Key);
        }
    }
}
