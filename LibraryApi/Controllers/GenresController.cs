using LibraryApi.Context;
using LibraryApi.Models;
using LibraryApi.Repositories;
using LibraryApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger<GenresController> _logger;

        public GenresController(IUnitOfWork uof,
            ILogger<GenresController> logger)
        {
            _uof = uof;
            _logger = logger;
        }


        [HttpGet("books")]
        public ActionResult<IEnumerable<Genre>> GetAllBooks()
        {
            var genres = _uof.GenreRepository.GetAllBooks();

            if (genres is null)
            {
                _logger.LogWarning("Genres and Books not found...");
                return NotFound("Genres and Books not found...");
            }

            return Ok(genres);

        }

        [HttpGet]
        public ActionResult<IEnumerable<Genre>> Get()
        {

            var genres = _uof.GenreRepository.GetAll();

            return Ok(genres);
        }

        [HttpGet("{id:int}", Name = "GetGenre")]
        public ActionResult<Genre> Get(int id)
        {
            var genre = _uof.GenreRepository.Get(g => g.GenreId == id);

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

           var genreCreated = _uof.GenreRepository.Create(genre);
            _uof.Commit(); //persistir as informações

            return new CreatedAtRouteResult("GetGenre", new { id = genreCreated.GenreId }, genreCreated);

        }


        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Genre genre)
        {


            if (id != genre.GenreId) {
                _logger.LogWarning("Invalid Data");
                return BadRequest("Invalid Data");
            }

            _uof.GenreRepository.Update(genre);
            _uof.Commit();

                return Ok(genre);
            
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var genre = _uof.GenreRepository.Get(g => g.GenreId == id);

            if (genre is null)
            {
                _logger.LogWarning("Genres not found...");
                return NotFound("Genre not found...");
            }

            var genreDeleted = _uof.GenreRepository.Delete(genre);
            _uof.Commit();

            return Ok(genreDeleted);
     
        }

    }
}