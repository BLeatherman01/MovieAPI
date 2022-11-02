using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext _context;

        public MoviesController(MovieContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet("GetAllMovies")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await _context.Movies.ToListAsync();
        }
        
        //Get movies by category
        [HttpGet("SearchByGenre/{genre}")]
        public async Task<ActionResult<IEnumerable<Movie>>> SearchByGenre(string genre)
        {
            return await _context.Movies.Where(m => m.Genre.Contains(genre)).ToListAsync();
        }

        //Get random movie by category
        [HttpGet("RandomByGenre/{genre}")]
        public async Task<ActionResult<String>> RandomByGenre(string genre)
        {
            IEnumerable<Movie> selected = await _context.Movies.Where(m => m.Genre.Contains(genre)).ToListAsync();
            List<string> genreList = new List<string>();
            Random random = new Random();

            foreach (Movie m in selected)
            {
                genreList.Add(m.Title);
            }
            int index = random.Next(genreList.Count);
            return genreList[index];
        }

        //Get random movie 
        [HttpGet("RandomMovie")]
        public async Task<ActionResult<String>> GetRandom()
        {
            return GetRandomMovie();
        }
        private string GetRandomMovie()
        {
            Random random = new Random();
            List<string> title = new List<string>();
            List<Movie> movie = _context.Movies.ToList();

            foreach (Movie m in movie)
            {
                title.Add(m.Title);
            }
            int index = random.Next(title.Count);
            return title[index];
        }

        [HttpGet("GetRandomByUserQuanity")]
        public async Task<ActionResult<IEnumerable<String>>> GetRandomByUserQuanity(int userAmount)
        {
            Random random = new Random();
            List<string> randomMovies = new List<string>();
            List<Movie> movies = _context.Movies.ToList();

            for (int i = 0; i < userAmount; i++)
            {
                int index = random.Next(movies.Count);
                randomMovies.Add(movies[index].Title);
            }

            return randomMovies;
        }

        //GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }


        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}

