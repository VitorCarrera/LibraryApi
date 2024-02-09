using LibraryApi.Context;
using LibraryApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GenresController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("books")]
        public ActionResult<IEnumerable<Genre>> GetAllBooks()
        {

            try { 
                var genres = _context.Genres.Include(b => b.Books).ToList();

                if (genres is null)
                 return NotFound("Genres and Books not found...");

                return genres;

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "There was a problem handling your request.");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Genre>> Get()
        {
    
            try
            {
                var genres = _context.Genres.AsNoTracking().Take(10).ToList();

                if (genres is null)
                    return NotFound("Genres not found...");

                return genres;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "There was a problem handling your request.");
            }
        }

        [HttpGet("{id:int}", Name = "GetGenre")]   
        public ActionResult<Genre> Get(int id)
        {

            try
            {
                var genre = _context.Genres.FirstOrDefault(g => g.GenreId == id);

                if (genre is null)
                    return NotFound("Genre not found...");

                return Ok(genre);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "There was a problem handling your request.");
            }


        }


        [HttpPost]
        public ActionResult Post(Genre genre)
        {

            try
            {
                if (genre is null)
                    return BadRequest();

                _context.Genres.Add(genre);
                _context.SaveChanges();

                return new CreatedAtRouteResult("GetGenre", new { id = genre.GenreId }, genre);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "There was a problem handling your request.");
            }
        }


        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Genre genre)
        {

            try
            {
                if (id != genre.GenreId)
                    return BadRequest();

                _context.Entry(genre).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(genre);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "There was a problem handling your request.");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {

            try
            {
                var genre = _context.Genres.FirstOrDefault(g => g.GenreId == id);

                if (genre is null)
                    return NotFound("Genre not found...");

                _context.Genres.Remove(genre);
                _context.SaveChanges();

                return Ok(genre);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "There was a problem handling your request.");
            }
        }

    }
}
