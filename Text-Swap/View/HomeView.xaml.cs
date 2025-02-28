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
        public HomeView()
        {
            InitializeComponent();
            DataContext = new HomeViewModel();
        }

        private void ShortcutTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                var vm = DataContext as HomeViewModel;
                //vm?.UpdateExpression();
            }
        }

        private void RecentShortcuts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var vm = DataContext as HomeViewModel;
                //vm?.SelectRecentShortcut(e.AddedItems[0].ToString());
            }
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnClearSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
