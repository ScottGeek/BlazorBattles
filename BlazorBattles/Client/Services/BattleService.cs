using BlazorBattles.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorBattles.Client.Services
{
    public class BattleService : IBattleService
    {
        private readonly HttpClient _httpClient;
        public BattleResult LastBattle { get; set; } = new BattleResult();
        public IList<BattleHistoryEntry> History { get; set; } = new List<BattleHistoryEntry>();

        public BattleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BattleResult> StartBattle(int opponentId)
        {
            var result = await _httpClient.PostAsJsonAsync("api/battle", opponentId);
            LastBattle = await result.Content.ReadFromJsonAsync<BattleResult>();
            return LastBattle;
        }

        public async Task GetHistory()
        {
            History = await _httpClient.GetFromJsonAsync<BattleHistoryEntry[]>("api/user/history");
        }
    }
}
