using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GameLibrary.Services;
using GameLibrary.Models;
using Newtonsoft.Json.Linq; // Add this for JObject
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApiService _apiService;

        public HomeController(ILogger<HomeController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(string category = null, string year = null, string search = null, int page = 1, int pageSize = 20)
        {
            
            var cards = new List<CardModel>();
            int totalGames = 0;  // To store the total number of games
            int totalPages = 0;  // To store the total number of pages

            try
            {

                // Fetch games from the API 
                var apiResponse = await _apiService.GetGamesAsync(search, page, pageSize);
                var responseJson = JObject.Parse(apiResponse);
                var games = responseJson["results"].ToObject<List<GameModel>>();

                //// Fetch total count of games from API response, if available
                totalGames = JObject.Parse(apiResponse)["count"].ToObject<int>();
                totalPages = (int)Math.Ceiling((double)totalGames / pageSize);

                // Filter games by category if the category is provided
                if (!string.IsNullOrEmpty(category))
                {
                    games = games.Where(g => g.Genres.Any(genre => genre.Name.Equals(category, StringComparison.OrdinalIgnoreCase))).ToList();
                }
                // Filter games by year if the year is provided
                if (!string.IsNullOrEmpty(year))
                {
                    // Implement year filtering logic if needed
                }
                // Filter games by search query if the search term is provided
                if (!string.IsNullOrEmpty(search))
                {
                    games = games.Where(g => g.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                // Limit the number of games to 10
                //games = games.Take(pageSize).ToList(); // Take only the first 10 games

                // Prepare the card data for the current page
                // For each game in the filtered subset, fetch its screenshots and prepare the card data
                foreach (var game in games)
                {
                    // Fetch screenshots for each game
                    var screenshots = await _apiService.GetScreenshotsAsync(game.Id);
                    var imageUrl = screenshots.FirstOrDefault() ?? "/images/default.jpg"; // Use the first screenshot or a default image
                    cards.Add(new CardModel
                    {
                        Text = game.Name, // Use the game name or any other property
                        ImageUrl = imageUrl
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching games.");
                // Handle the error (e.g., show an error message to the user)
            }

            // Calculate pagination
            int pageGroupSize = 10; // Number of pages to display at a time
            int totalGroups = (int)Math.Ceiling((double)totalPages / pageGroupSize);

            int currentGroup = (int)Math.Ceiling((double)page / pageGroupSize);
            int startPage = (currentGroup - 1) * pageGroupSize + 1;
            int endPage = Math.Min(startPage + pageGroupSize - 1, totalPages);

            int previousPageGroup = (currentGroup > 1) ? ((currentGroup - 2) * pageGroupSize) + 1 : 0;
            int nextPageGroup = (currentGroup < totalGroups) ? (currentGroup * pageGroupSize) + 1 : 0;

            var viewModel = new PaginatedGamesViewModel
            {
                Cards = cards,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalGames = totalGames,
                StartPage = startPage,
                EndPage = endPage,
                PreviousPageGroup = previousPageGroup,
                NextPageGroup = nextPageGroup
            };
            //return View(cards);
            return View(viewModel);
        }
      
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
