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
        private readonly ILogger<GenresController> _logger;

        public GenresController(AppDbContext context, ILogger<GenresController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet("books")]
        public ActionResult<IEnumerable<Genre>> GetAllBooks()
        {
            var genres = _context.Genres.Include(b => b.Books).ToList();

            if (genres is null)
            {
                _logger.LogWarning("Genres and Books not found...");
                return NotFound("Genres and Books not found...");
            }

            return genres;

        }

        [HttpGet]
        public ActionResult<IEnumerable<Genre>> Get()
        {

            var genres = _context.Genres.AsNoTracking().Take(10).ToList();

            if (genres is null)
            {
                _logger.LogWarning("Genres not found...");
                return NotFound("Genres not found...");
            }

            return genres;
        }

        [HttpGet("{id:int}", Name = "GetGenre")]
        public ActionResult<Genre> Get(int id)
        {
            var genre = _context.Genres.FirstOrDefault(g => g.GenreId == id);

            if (genre is null)
            {

                _logger.LogWarning($"Genre not found...");
                return NotFound("Genre not found...");

            }

            return Ok(genre);

        }


        [HttpPost]
        public ActionResult Post(Genre genre)
        {

            if (genre is null)
            {
                _logger.LogWarning("Invalid Data...");
                return BadRequest("Invalid Data...");
            }

            _context.Genres.Add(genre);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetGenre", new { id = genre.GenreId }, genre);

        }


        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Genre genre)
        {


            if (id != genre.GenreId) {
                _logger.LogWarning("Invalid Data");
                return BadRequest("Invalid Data");
            }
                _context.Entry(genre).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(genre);
            
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
                var genre = _context.Genres.FirstOrDefault(g => g.GenreId == id);

            if (genre is null)
            {
                _logger.LogWarning("Genres not found...");
                return NotFound("Genre not found...");
            }

                _context.Genres.Remove(genre);
                _context.SaveChanges();

                return Ok(genre);
     
        }

    }
}