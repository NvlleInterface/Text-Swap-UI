using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Text_Swap.Model;
using Text_Swap.ViewModel;

namespace Text_Swap.View
{
    /// <summary>
    /// Logique d'interaction pour AddShortcutView.xaml
    /// </summary>
    public partial class AddShortcutView : Window
    {
        public AddShortcutView(Action<ShortcutModel> onShorcutAdded)
        {
            InitializeComponent();
            DataContext = new AddShortcutViewModel(onShorcutAdded);
            this.Closed += RemoveBlurEffet;
        }

        private void RemoveBlurEffet(object? sender, EventArgs e)
        {
            Application.Current.MainWindow.Effect = null;
        }
    }
}
