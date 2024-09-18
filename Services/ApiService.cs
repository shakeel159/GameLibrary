namespace GameLibrary.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using GameLibrary.Configurations;
    using GameLibrary.Models;
    using Humanizer.Localisation;
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
        public async Task<string> GetGamesAsync(string search = null, string startDate = null, string endDate = null, int page = 1, int pageSize = 20)
        {
            // Construct the URL with API key and any additional query parameters if needed
            //var url = $"https://api.rawg.io/api/games?key={_apiKey}";
            var url = $"https://api.rawg.io/api/games?key={_apiKey}&page={page}&page_size={pageSize}";
            // Add search query if provided
            if (!string.IsNullOrEmpty(search))
            {
                url += $"&search={Uri.EscapeDataString(search)}";
            }

            // Add date range to the URL if provided
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                url += $"&dates={startDate},{endDate}";
            }

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
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

        //get names of categories
        public async Task<List<GenreModel>> GetCategoriesAsync()
        {

            var url = $"https://api.rawg.io/api/genres?key={_apiKey}";
            var response = await _httpClient.GetStringAsync(url);
            var responseObject = JObject.Parse(response);
            var genres = responseObject["results"].ToObject<List<GenreModel>>(); // Deserialize the 'results' array
            return genres;
        }
        public async Task<List<int>> GetYearsAsync(string startDate, string endDate)
        {
            List<int> years = new List<int>();

            try
            {
                // Construct the API URL
                var url = $"https://api.rawg.io/api/games?key={_apiKey}&dates={startDate},{endDate}";

                // Make the API call
                var response = await _httpClient.GetStringAsync(url);
                var responseObject = JObject.Parse(response);

                // Extract the 'results' array
                var games = responseObject["results"].ToObject<List<JObject>>();

                // Extract years from the 'released' field of each game
                foreach (var game in games)
                {
                    string releasedDate = game["released"]?.ToString();
                    if (!string.IsNullOrEmpty(releasedDate))
                    {
                        // Parse the year from the released date (assuming YYYY-MM-DD format)
                        int year = DateTime.Parse(releasedDate).Year;

                        // Add the year to the list if it's not already present
                        if (!years.Contains(year))
                        {
                            years.Add(year);
                        }
                    }
                }
                // Ensure how many years 
                int currentYear = DateTime.Now.Year;
                for (int i = currentYear; i >= currentYear - 20; i--)
                {
                    if (!years.Contains(i))
                    {
                        years.Add(i);
                    }
                }

                // Sort years in descending order
                years = years.OrderByDescending(y => y).ToList();
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"Request error: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return years;
        }

    }
}
