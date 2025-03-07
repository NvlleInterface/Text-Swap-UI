using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using MVVMEssentials.Services;
using MVVMEssentials.Stores;
using MVVMEssentials.ViewModels;
using System.Configuration;
using System.Data;
using System.Security.Principal;
using System.Windows;
using Text_Swap.Repositories;
using Text_Swap.Services;
using Text_Swap.ViewModel;

namespace Text_Swap;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;
    public App()
    {
        _host = Host.CreateDefaultBuilder()
             .ConfigureServices((context, service) =>
             {
                 var url = context.Configuration.GetValue<string>("UrlServerAuthentication");
                 service.AddScoped<IAuthentication, Authentication>();
                 service.AddSingleton<NavigationStore>();
                 service.AddSingleton<ModalNavigationStore>();

                 service.AddSingleton<NavigationService<RegisterViewModel>>(
                     (service) => new NavigationService<RegisterViewModel>(
                         service.GetRequiredService<NavigationStore>(),
                         () => new RegisterViewModel(
                             service.GetRequiredService<IAuthentication>(),
                             service.GetRequiredService<NavigationService<LoginViewModel>>())));

                 service.AddSingleton<NavigationService<LoginViewModel>>(
                     (service) => new NavigationService<LoginViewModel>(
                         service.GetRequiredService<NavigationStore>(),
                         () => new LoginViewModel(
                             service.GetRequiredService<IAuthentication>(),
                             service.GetRequiredService<NavigationService<RegisterViewModel>>(),
                             service.GetRequiredService<NavigationService<ContentViewModel>>())));

                 service.AddSingleton<NavigationService<ContentViewModel>>(
                     (service) => new NavigationService<ContentViewModel>(
                         service.GetRequiredService<NavigationStore>(),
                         () => new ContentViewModel(
                             service.GetRequiredService<NavigationService<LoginViewModel>>(),
                             service.GetRequiredService<NavigationService<RaccourciViewModel>>())));

                 service.AddSingleton<NavigationService<RaccourciViewModel>>(
                     (service) => new NavigationService<RaccourciViewModel>(
                         service.GetRequiredService<NavigationStore>(),
                         () => new RaccourciViewModel()));

                 service.AddSingleton<NavigationService<HomeViewModel>>(
                   (service) => new NavigationService<HomeViewModel>(
                       service.GetRequiredService<NavigationStore>(),
                       () => new HomeViewModel()));

                 service.AddSingleton<MainViewModel>();
                 service.AddSingleton<MainWindow>((service) => new MainWindow()
                 {
                     DataContext = service.GetRequiredService<MainViewModel>()
                 });
             })
            .Build();
    }
    protected override void OnStartup(StartupEventArgs e)
    {
        INavigationService navigationService = _host.Services.GetService<NavigationService<ContentViewModel>>();
        navigationService.Navigate();
        MainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow.Show();

        base.OnStartup(e);
    }
}
