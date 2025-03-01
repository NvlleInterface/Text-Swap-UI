using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Text_Swap.View
{
    /// <summary>
    /// Logique d'interaction pour ContentView.xaml
    /// </summary>
    public partial class ContentView : UserControl
    {
        public ContentView()
        {
            InitializeComponent();

            var identity = Thread.CurrentPrincipal?.Identity;
            if (identity != null && identity.IsAuthenticated)
            {
                WelcomeText.Text = $"Bienvenue, {identity.Name}";
            }

            var principal = Thread.CurrentPrincipal as GenericPrincipal;
            if (principal != null && principal.IsInRole("Admin"))
            {
                // AdminPanel.Visibility = Visibility.Visible; // Afficher un menu Admin si nécessaire
            }
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void pnlControleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            WindowInteropHelper helper = new WindowInteropHelper(parentWindow);
            SendMessage(helper.Handle, 161, 2, 0);
        }

        private void pnlControleBar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                if (parentWindow.WindowState == WindowState.Normal)
                {
                    parentWindow.WindowState = WindowState.Maximized;
                }
                else
                {
                    parentWindow.WindowState = WindowState.Normal;
                }
            }

        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.WindowState = WindowState.Minimized;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            try
            {
                if (parentWindow != null)
                {
                    parentWindow.DragMove();
                }
            }
            catch (Exception)
            {

            }
           
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Left = (SystemParameters.PrimaryScreenWidth - parentWindow.Width) / 2;
                parentWindow.Top = (SystemParameters.PrimaryScreenHeight - parentWindow.Height) / 2;
            }
        }
    }
}
