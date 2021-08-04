using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorBattles.Shared;

namespace BlazorBattles.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceReponse<string>> Login(UserLogin request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            return await response.Content.ReadFromJsonAsync<ServiceReponse<string>>();
        }

        public async Task<ServiceReponse<int>> Register(UserRegister request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register",request);
            return await response.Content.ReadFromJsonAsync<ServiceReponse<int>>();

        }
    }
}
