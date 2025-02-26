using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Text_Swap.Services;

public static class JwtHelper
{
    public static (string Username, string[] Roles) DecodeJwtToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return (string.Empty, Array.Empty<string>());

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var username = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type == "sub")?.Value ?? string.Empty;
        var roles = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                                   .Select(c => c.Value)
                                   .ToArray();

        return (username, roles);
    }
}