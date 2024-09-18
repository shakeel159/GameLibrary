namespace GameLibrary.Models
{
    public class PaginatedGamesViewModel
    {

        public List<CardModel> Cards { get; set; }
        public List<GenreModel> Categories { get; set; }
        public List<int> Years { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalGames { get; set; }
        public bool HasMore { get; set; } // This indicates if there are more games to load
        public string SelectedCategory { get; set; }
        public string SelectedYear { get; set; }
        public string SelectedSearch { get; set; }

        // Pagination controls
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int PreviousPageGroup { get; set; }
        public int NextPageGroup { get; set; }
    }
}
