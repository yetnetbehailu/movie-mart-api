using System;
namespace movie_mart_api.Models
{
	public class Actor
	{
        public int ActorId { get; set; }

        public string Name { get; set; }

        // Many-to-many relation with Movie
        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}

