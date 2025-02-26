using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace Text_Swap.View;

/// <summary>
/// Logique d'interaction pour MainView.xaml
/// </summary>
public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
        //if (Application.Current.Properties.Contains("UserName"))
        //{
        //    WelcomeText.Text = $"Bienvenue, {Application.Current.Properties["UserName"]}";
        //}
        var identity = Thread.CurrentPrincipal.Identity;
        if (identity.IsAuthenticated)
        {
            WelcomeText.Text = $"Bienvenue, {identity.Name}";
        }

        var principal = Thread.CurrentPrincipal as GenericPrincipal;
        if (principal.IsInRole("Admin"))
        {
           // AdminPanel.Visibility = Visibility.Visible; // Afficher un menu Admin si nécessaire
        }
    }

    [DllImport("user32.dll")]
    public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
    private void pnlControleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        WindowInteropHelper helper = new WindowInteropHelper(this);
        SendMessage(helper.Handle, 161, 2, 0);
    }

    private void pnlControleBar_MouseEnter(object sender, MouseEventArgs e)
    {
        this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void btnMinimize_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Minimized;
    }

    private void btnMaximize_Click(object sender, RoutedEventArgs e)
    {
        if (this.WindowState == WindowState.Normal)
        {
            this.WindowState = WindowState.Maximized;
        }
        else
        {
            this.WindowState = WindowState.Normal;
        }
    }

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        // 1️⃣ Effacer l'utilisateur actuel
        Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(string.Empty), null);
        var config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Définit le répertoire de base
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Charge le fichier JSON
                    .Build();
        string path = config["LocalStorage"];
        // 2️⃣ Supprimer les infos stockées (registre Windows)
        var key = Registry.CurrentUser.OpenSubKey(path, true);
        if (key != null)
        {
            key.DeleteValue("Username", false);
            key.DeleteValue("Roles", false);
            key.DeleteValue("Token", false);
            key.Close();
        }

        // 3️⃣ Rediriger vers `LoginView`
        var loginView = new LoginView();
        loginView.Show();

        // 4️⃣ Fermer `MainView`
        this.Close();
    }

    private void UserMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
        {
            e.Handled = true; // Bloque l'affichage du menu
        }
    }
}
