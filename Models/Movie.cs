using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace movie_mart_api.Models
{
	public class Movie
	{
        public int MovieId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Director is required")]
        public int DirectorId { get; set; } // FK for Director
        public Director Director { get; set; } // Navigation property

        [Required(ErrorMessage = "Genre is required")]
        public int GenreId { get; set; } // FK
        public Genre Genre { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReleaseDate { get; set; }

        [Required(ErrorMessage = "Language is required")]
        public string Language { get; set; }

        // Duration of movie
        public TimeSpan? Duration { get; set; }

        public string ImageUrl { get; set; }

        // Many-to-many relation with Actor
        public List<Actor> Actors { get; set; } = new List<Actor>();

    }
}

