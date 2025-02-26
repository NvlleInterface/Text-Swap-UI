using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System.Security.Principal;

namespace Text_Swap.Services;

public class LocalStorageService
{
    public void SetUserSession(string token)
    {
        var (username, roles) = JwtHelper.DecodeJwtToken(token);

        if (!string.IsNullOrEmpty(username))
        {
            var identity = new GenericIdentity(username);
            var principal = new GenericPrincipal(identity, roles);
            Thread.CurrentPrincipal = principal;
            AppDomain.CurrentDomain.SetThreadPrincipal(principal); // Garde les infos dans tous les threads

            SaveUserToSecureStorage(username, roles, token); // Sauvegarde pour une reconnexion future
        }
    }

    private void SaveUserToSecureStorage(string username, string[] roles, string token)
    {
        Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(string.Empty), null);
        var config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Définit le répertoire de base
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Charge le fichier JSON
                    .Build();
        string path = config["LocalStorage"];

        var key = Registry.CurrentUser.CreateSubKey(path);
        key.SetValue("Username", username);
        key.SetValue("Roles", string.Join(",", roles));
        key.SetValue("Token", token);
        key.Close();
    }

    private void RestoreUserSession()
    {
        var key = Registry.CurrentUser.OpenSubKey(@"Software\MyApp");
        if (key == null) return;

        var username = key.GetValue("Username") as string;
        var roles = (key.GetValue("Roles") as string)?.Split(',') ?? Array.Empty<string>();
        var token = key.GetValue("Token") as string;

        key.Close();

        if (!string.IsNullOrEmpty(username))
        {
            var identity = new GenericIdentity(username);
            var principal = new GenericPrincipal(identity, roles);
            Thread.CurrentPrincipal = principal;
            AppDomain.CurrentDomain.SetThreadPrincipal(principal);
        }
    }
}
