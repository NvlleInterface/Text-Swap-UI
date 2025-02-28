using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Text_Swap.Repositories;

public interface IUserRepository
{
    Response LoginAsync(NetworkCredential networkCredential);
    Response Register(string username, string email, SecureString password, SecureString confirmPassword);
}
