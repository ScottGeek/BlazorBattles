using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using BlazorBattles.Client.Services;

namespace BlazorBattles.Client
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        private readonly IBananaService _bananaService;

        public CustomAuthStateProvider(ILocalStorageService localStorage, 
                HttpClient httpClient, 
                IBananaService bananaService)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
            _bananaService = bananaService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            string authToken = await _localStorage.GetItemAsStringAsync("authToken"); //maynot exist
            var identity = new ClaimsIdentity();  //empty identity

            _httpClient.DefaultRequestHeaders.Authorization = null; //reset the headers

            if (!string.IsNullOrEmpty(authToken))  //we are logged in lets create the identity and place headers
            {
                try
                {
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");  //token comes from the auth jwt
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.Replace("\"",""));
                    await _bananaService.GetBananas();
                }
                catch (Exception)
                {
                    await _localStorage.RemoveItemAsync("authToken");
                    identity = new ClaimsIdentity();  //empty identity
                }
            }

            //these are either a login or empty

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;

        }

        private byte[] ParseBae64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {

                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);

        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBae64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            var claims = keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ));
            return claims;

        }

    }
}
