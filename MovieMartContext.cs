using System;
using System.Reflection.Emit;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using movie_mart_api.Models;


namespace movie_mart_api
{
	public class MovieMartContext: IdentityDbContext<User, Role, int>
    {
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Director> Directors { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public DbSet<Actor> Actors { get; set; }

        //Constructor
        public MovieMartContext (DbContextOptions options) : base(options){

		}

        // Seed data 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Genre>().HasData(
                new Genre { GenreId = 1, Name = "Action"},
                new Genre { GenreId = 2, Name = "Drama"},
                new Genre { GenreId = 3, Name = "Comedy" }
            );


            builder.Entity<Director>().HasData(
            new Director { DirectorId = 1, Name = "Director1" },
            new Director { DirectorId = 2, Name = "Director2" },
            new Director { DirectorId = 3, Name = "Director3" }

            );


            builder.Entity<Actor>().HasData(
                new Actor { ActorId = 1, Name = "Actor1" },
                new Actor { ActorId = 2, Name = "Actor2" },
                new Actor { ActorId = 3, Name = "Actor3" }
            );


            builder.Entity<Movie>().HasData(
                new Movie {
                    MovieId = 1,
                    Title = "IMAGE",
                    DirectorId = 1,
                    GenreId = 1,
                    Description = "Description for Movie1",
                    ReleaseDate = DateTime.Now,
                    Language = "English",
                    Duration = TimeSpan.FromMinutes(120),
                    ImageUrl = "https://m.media-amazon.com/images/M/MV5BNzQxNzQxNjk5NV5BMl5BanBnXkFtZTgwNTI4MTU0MzE@._V1_.jpg",
                },
                new Movie
                {
                    MovieId = 2,
                    Title = "TILL",
                    DirectorId = 2,
                    GenreId = 2,
                    Description = "Description for Movie2",
                    ReleaseDate = DateTime.Now,
                    Language = "English",
                    Duration = TimeSpan.FromMinutes(110),
                    ImageUrl = "https://resizing.flixster.com/aqOkZpMlbwsRHEU9tpnBr8845YM=/fit-in/180x240/v2/https://resizing.flixster.com/O6GxoETackXlsx2N9MJPIbp4-54=/ems.cHJkLWVtcy1hc3NldHMvbW92aWVzLzQwNmE0YjRkLTFiM2ItNDk4Yi1hZTFhLWJiMDI2OTM1NDAyMy5qcGc=",
                },
                new Movie
                {
                    MovieId = 3,
                    Title = "White Chicks",
                    DirectorId = 3,
                    GenreId = 3,
                    Description = "Description for Movie 3",
                    ReleaseDate = DateTime.Now,
                    Language = "English",
                    Duration = TimeSpan.FromMinutes(130),
                    ImageUrl = "https://m.media-amazon.com/images/I/81wR1ScI7nL._AC_UF1000,1000_QL80_.jpg",
                }
            );

        }
    }
}

