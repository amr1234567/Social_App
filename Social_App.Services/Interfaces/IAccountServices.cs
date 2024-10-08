using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_App.Services.Interfaces
{
    public interface IAccountServices
    {
        string HashPasswordWithSalt(string salt,string password);
        string CreateSalt();
    }
}
