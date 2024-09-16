namespace GameLibrary.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using GameLibrary.Configurations;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json.Linq;

    public class ApiService //handle api services //CRUD calls
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ApiService(HttpClient httpClient, IOptions<AppSettings> appSettings)
        {
            _httpClient = httpClient;
            _apiKey = appSettings.Value.ApiKey;
        }
        //all games
        public async Task<string> GetGamesAsync(string search = null, int page = 1, int pageSize = 20)
        {
            // Construct the URL with API key and any additional query parameters if needed
            //var url = $"https://api.rawg.io/api/games?key={_apiKey}";
            var url = $"https://api.rawg.io/api/games?key={_apiKey}&page={page}&page_size={pageSize}";
            // Add search query if provided
            if (!string.IsNullOrEmpty(search))
            {
                url += $"&search={Uri.EscapeDataString(search)}";
            }


            try
            {
                // Send the HTTP GET request
                var response = await _httpClient.GetStringAsync(url);

                // Return the response content as a string
                return response;
            }
            catch (HttpRequestException ex)
            {
                // Handle the exception
                throw;
            }
        }
        //images
        public async Task<List<string>> GetScreenshotsAsync(int gameId)
        {
            var url = $"https://api.rawg.io/api/games/{gameId}/screenshots?key={_apiKey}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var screenshots = JObject.Parse(jsonResponse)["results"]
                                  .Select(s => s["image"].ToString())
                                  .ToList();

            return screenshots;
        }
        //games searched by category  
        public async Task<string> GetGamesGenreAsync()
        {
            // Construct the URL with API key and any additional query parameters if needed
            var url = $"https://api.rawg.io/api/genres?key={_apiKey}";

            // Send the HTTP GET request
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            // Read and return the response content as a string
            return await response.Content.ReadAsStringAsync();
        }
    }
}
