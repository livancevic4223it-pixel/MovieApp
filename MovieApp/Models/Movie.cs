using SQLite;

namespace MovieApp.Models
{
    public class Movie
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Cast { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsFavorite { get; set; } = false;
        public double AverageRating { get; set; } = 0;
    }
}