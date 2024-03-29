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
    }
}

