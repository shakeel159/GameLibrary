namespace GameLibrary.Models
{
    public class CardModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
    }

    public class GameModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BackgroundImage { get; set; }


        //  list of GenreModel objects
        public List<GenreModel> Genres { get; set; }
        public int Year { get; set; }
    }

    public class GenreModel
    {
        public int Id { get; set; }
        public string Name { get; set; } // Genre name
        public int Years { get; set; }
    }
}
