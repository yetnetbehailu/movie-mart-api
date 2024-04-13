using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using movie_mart_api.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace movie_mart_api.Controllers
{
    // [ApiController] indicates that this class is a Web API controller.
    // It enables automatic HTTP 400 responses for invalid input, attribute-based routing,
    // parameter binding source inferring, response formatting and model validation.
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : Controller
    {
        private readonly MovieMartContext _movieMartContext;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(MovieMartContext movieMartContext, ILogger<MoviesController> logger)
        {
            _movieMartContext = movieMartContext;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _movieMartContext.Movies.Include(m=>m.Genre).Include(m=>m.Director).ToListAsync();
            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] Movie movieRequest)
        {
            try
            {
                // Check if movie with the same title already exists
                var existingMovie = await _movieMartContext.Movies.FirstOrDefaultAsync(m => m.Title == movieRequest.Title);

                if (existingMovie != null)
                {
                    // Conflict response 409
                    return Conflict("A movie with the same title already exists");
                }


                // Check if director exists
                var existingDirector = await _movieMartContext.Directors.FirstOrDefaultAsync(d => d.Name == movieRequest.Director.Name);

                if (existingDirector == null)
                {
                    // Director does not exist, create new director
                    Director newDirector = new Director { Name = movieRequest.Director.Name };
                    await _movieMartContext.Directors.AddAsync(newDirector);
                    await _movieMartContext.SaveChangesAsync();

                    movieRequest.Director = newDirector; // Set movie's director to the newly created director
                }
                else
                {
                    // Director exists, assign it to the movie
                    movieRequest.Director = existingDirector;
                }


                // Check if genre exists
                var existingGenre = await _movieMartContext.Genres.FirstOrDefaultAsync(g => g.Name == movieRequest.Genre.Name);

                if (existingGenre == null)
                {
                    // Genre does not exist, create new genre
                    Genre newGenre = new Genre { Name = movieRequest.Genre.Name };
                    await _movieMartContext.Genres.AddAsync(newGenre);
                    await _movieMartContext.SaveChangesAsync();

                    movieRequest.Genre = newGenre; // Set movie's genre to the newly created genre
                }
                else
                {
                    // Genre exists, assign it to the movie
                    movieRequest.Genre = existingGenre;
                }


                // Handle actors
                List<Actor> movieActors = new List<Actor>();

                foreach (var actor in movieRequest.Actors)
                {
                    // Check if actor exists
                    var existingActor = await _movieMartContext.Actors.FirstOrDefaultAsync(a => a.Name == actor.Name);

                    if (existingActor == null)
                    {
                        // Actor does not exist, create new actor
                        Actor newActor = new Actor { Name = actor.Name };
                        await _movieMartContext.Actors.AddAsync(newActor);
                        movieActors.Add(newActor); // Add the new actor to the list of actors associated with the movie
                    }
                    else
                    {
                        // Actor exists, add it to the list of actors associated with the movie
                        movieActors.Add(existingActor);
                    }
                }

                // Assign the list of actors to the movie
                movieRequest.Actors = movieActors;

                // Add movie to database
                await _movieMartContext.Movies.AddAsync(movieRequest);
                await _movieMartContext.SaveChangesAsync();
                return Ok(movieRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the movie.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] Movie movieRequest)
        {
            try
            {
                // Check if the movie with the given id exists in the database
                var existingMovie = await _movieMartContext.Movies
                    .Include(m => m.Genre)
                    .Include(m => m.Director)
                    .Include(m => m.Actors)
                    .FirstOrDefaultAsync(m => m.MovieId == id);

                if (existingMovie == null)
                {
                    // 404 Not Found
                    return NotFound("Movie not found");
                }

                // Update movie properties with values from movieRequest
                existingMovie.Title = movieRequest.Title;
                existingMovie.Description = movieRequest.Description;
                existingMovie.ReleaseDate = movieRequest.ReleaseDate;
                existingMovie.Language = movieRequest.Language;
                existingMovie.Duration = movieRequest.Duration;
                existingMovie.ImageUrl = movieRequest.ImageUrl;

                // Update director
                var existingDirector = await _movieMartContext.Directors
                    .FirstOrDefaultAsync(d => d.Name == movieRequest.Director.Name);

                if (existingDirector == null)
                {
                    // Director does not exist, create new director
                    Director newDirector = new Director { Name = movieRequest.Director.Name };
                    await _movieMartContext.Directors.AddAsync(newDirector);
                    existingMovie.Director = newDirector; // Set movie's director to the newly created director
                }
                else
                {
                    // Director exists, assign it to the movie
                    existingMovie.Director = existingDirector;
                }

                // Update genre
                var existingGenre = await _movieMartContext.Genres
                    .FirstOrDefaultAsync(g => g.Name == movieRequest.Genre.Name);

                if (existingGenre == null)
                {
                    // Genre does not exist, create new genre
                    Genre newGenre = new Genre { Name = movieRequest.Genre.Name };
                    await _movieMartContext.Genres.AddAsync(newGenre);
                    existingMovie.Genre = newGenre; // Set movie's genre to the newly created genre
                }
                else
                {
                    // Genre exists, assign it to the movie
                    existingMovie.Genre = existingGenre;
                }

                // Update actors
                existingMovie.Actors.Clear(); // Remove all existing actors associated with the movie

                foreach (var actor in movieRequest.Actors)
                {
                    // Check if actor exists
                    var existingActor = await _movieMartContext.Actors
                        .FirstOrDefaultAsync(a => a.Name == actor.Name);

                    if (existingActor == null)
                    {
                        // Actor does not exist, create new actor
                        Actor newActor = new Actor { Name = actor.Name };
                        await _movieMartContext.Actors.AddAsync(newActor);
                        existingMovie.Actors.Add(newActor); // Add the new actor to the list of actors associated with the movie
                    }
                    else
                    {
                        // Actor exists, add it to the list of actors associated with the movie
                        existingMovie.Actors.Add(existingActor);
                    }
                }

                await _movieMartContext.SaveChangesAsync();

                // After saving changes
                // Remove directors not associated with any movie
                await RemoveUnassociatedDirectors();
                // Remove actors not associated with any movie
                await RemoveUnassociatedActors();

                return Ok(existingMovie); // Return the updated movie
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the movie");
                return StatusCode(500, "An error occurred while processing the request");
            }
        }

        private async Task RemoveUnassociatedDirectors()
        {
            // Find directors not associated with any movie
            var unassociatedDirectorIds = await _movieMartContext.Movies
                .Where(movie => movie.DirectorId != null)
                .Select(movie => movie.DirectorId)
                .Distinct()
                .ToListAsync();

            var directorsToRemove = await _movieMartContext.Directors
                .Where(director => !unassociatedDirectorIds.Contains(director.DirectorId))
                .ToListAsync();

            // Remove unassociated directors
            _movieMartContext.Directors.RemoveRange(directorsToRemove);

            // Save changes to remove unassociated directors from database
            await _movieMartContext.SaveChangesAsync();
        }

        private async Task RemoveUnassociatedActors()
        {
            // Find actors not associated with any movie
            var unassociatedActorIds = await _movieMartContext.Movies
                .SelectMany(movie => movie.Actors.Select(actor => actor.ActorId))
                .Distinct()
                .ToListAsync();

            var actorsToRemove = await _movieMartContext.Actors
                .Where(actor => !unassociatedActorIds.Contains(actor.ActorId))
                .ToListAsync();

            // Remove unassociated actors from the actor table
            _movieMartContext.Actors.RemoveRange(actorsToRemove);

            // Save changes to remove unassociated actors from database
            await _movieMartContext.SaveChangesAsync();
        }
    }
}