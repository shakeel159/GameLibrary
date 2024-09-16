namespace GameLibrary.Models
{
    public class PaginatedGamesViewModel
    {
        public List<CardModel> Cards { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalGames { get; set; }

        // For pagination controls
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int PreviousPageGroup { get; set; }
        public int NextPageGroup { get; set; }
    }
}
