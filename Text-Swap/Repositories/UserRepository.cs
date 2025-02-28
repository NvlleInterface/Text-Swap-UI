using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security;
using System.Security.Claims;
using System.Text.Json;
using Text_Swap.Model;

namespace Text_Swap.Repositories;

public class UserRepository : IUserRepository
{
    private readonly HttpClient _httpClient;

    public UserRepository()
    {
        var config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Définit le répertoire de base
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Charge le fichier JSON
                    .Build();
        string path = config["UrlServerAuthentication"];
        _httpClient = new HttpClient { BaseAddress = new Uri(path) };
    }
    public Response LoginAsync(NetworkCredential networkCredential)
    {
        try
        {
            var loginCommand = new LoginCommand(networkCredential.UserName, new System.Net.NetworkCredential(string.Empty, networkCredential.SecurePassword).Password);

            HttpResponseMessage response = _httpClient.PostAsJsonAsync("login", loginCommand).Result;

                var responseBody = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.Deserialize<Response>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                    
               return result;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public Response Register(string username, string email, SecureString password, SecureString confirmPassword)
    {
        try
        {
            var registerCommand = new RegisterCommand(username, email, new System.Net.NetworkCredential(string.Empty, password).Password, new System.Net.NetworkCredential(string.Empty, confirmPassword).Password);

            HttpResponseMessage response = _httpClient.PostAsJsonAsync("register", registerCommand).Result;

            var responseBody = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Response>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return result;
        }
        catch (Exception)
        {
            return null;
        }
       
    }

    public sealed record LoginCommand(string Email, string Password);
    public sealed record RegisterCommand(string UserName, string Email, string Password, string ConfirmPassword);

    private string DecodeJwtToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return "Utilisateur inconnu";

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Récupérer le claim "name" ou "sub" (identifiant principal)
        var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "name" || c.Type == ClaimTypes.Name || c.Type == "sub");

        return nameClaim?.Value ?? "Utilisateur inconnu";
    }
}
