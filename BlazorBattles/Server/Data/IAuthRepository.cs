using BlazorBattles.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBattles.Server.Data
{
    public interface IAuthRepository
    {

        Task<ServiceReponse<int>> Register(User user, string password);
        Task<ServiceReponse<string>> Login(string email, string password);
        Task<bool> UserExists(string email);

    }
}
