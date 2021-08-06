using BlazorBattles.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;


namespace BlazorBattles.Client.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly HttpClient _httpClient;

        public IList<UserStatistic> Leaderboard { get; set; }

        public LeaderboardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task GetLeaderboard()
        {
            Leaderboard = await _httpClient.GetFromJsonAsync<IList<UserStatistic>>("api/user/leaderboard");
        }
    }
}
