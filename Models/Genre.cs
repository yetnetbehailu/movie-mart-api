using System;
namespace movie_mart_api.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }

        // Navigation property one-to-many with Movie
        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}

