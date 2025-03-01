using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Text_Swap.Repositories;

public interface IAuthentication
{
    Task<Response> LoginAsync(NetworkCredential networkCredential);
    Task<Response> Register(string username, string email, SecureString password, SecureString confirmPassword);
}
