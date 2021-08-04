using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorBattles.Shared;

namespace BlazorBattles.Client.Services
{
    public interface IAuthService
    {
        Task<ServiceReponse<int>> Register(UserRegister request);
        Task<ServiceReponse<string>> Login(UserLogin request);

    }
}
