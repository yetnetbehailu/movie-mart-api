using System;
using Microsoft.EntityFrameworkCore;
using movie_mart_api.Models;


namespace movie_mart_api
{
	public class MovieMartContext: DbContext
	{
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Director> Directors { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public DbSet<Actor> Actors { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        //Constructor
        public MovieMartContext (DbContextOptions options) : base(options){

		}
	}
}

