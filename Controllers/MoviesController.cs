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
    }
}

