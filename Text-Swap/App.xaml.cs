using Microsoft.Win32;
using System.Configuration;
using System.Data;
using System.Security.Principal;
using System.Windows;
using Text_Swap.Services;
using Text_Swap.View;

namespace Text_Swap;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public bool NavigatingToRegister { get; set; } = false;
    protected void ApplicationStart(object sender, StartupEventArgs e)
    {

        var loginView = new LoginView();
        loginView.Show();
        loginView.IsVisibleChanged += (s, ev) =>
        {
            if (loginView.IsVisible == false && loginView.IsLoaded && !NavigatingToRegister)
            {
                var mainView = new MainView();
                mainView.Show();
                //loginView.Close();
            }
        };
    }

    
}
