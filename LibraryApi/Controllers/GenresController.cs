using LibraryApi.Context;
using LibraryApi.DTOs;
using LibraryApi.DTOs.Mappings;
using LibraryApi.Models;
using LibraryApi.Pagination;
using LibraryApi.Repositories;
using LibraryApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static System.Reflection.Metadata.BlobBuilder;

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

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<GenreDTO>> Get([FromQuery] GenresParameters genresParameters)
        {
            var genres = _uof.GenreRepository.GetGenres(genresParameters);

            return GetGenres(genres);
        }

        

        [HttpGet("filter/nome/pagination")]
        public ActionResult<IEnumerable<GenreDTO>> GetGenresFiltered([FromQuery] GenresFilterName genresFilter)
        {
            var genresFiltered = _uof.GenreRepository.GetGenresFilterName(genresFilter);

            return GetGenres(genresFiltered); 
        }

        [HttpGet("books")]
        public ActionResult<IEnumerable<GenreDTO>> GetAllBooks()
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
        public ActionResult<IEnumerable<GenreDTO>> Get()
        {

            var genres = _uof.GenreRepository.GetAll();

            if (genres is null)
                return NotFound("Genres don't exist...");


            var genresDTO = genres.ToGenreDTOList();

            return Ok(genresDTO);
        }


        [HttpGet("{id:int}", Name = "GetGenre")]
        public ActionResult<GenreDTO> Get(int id)
        {
            var genre = _uof.GenreRepository.Get(g => g.GenreId == id);

            if (genre is null)
            {

                _logger.LogWarning($"Genre not found...");
                return NotFound("Genre not found...");

            }

            var genreDTO = genre.ToGenreDTO();

            return Ok(genreDTO);

        }


        [HttpPost]
        public ActionResult<GenreDTO> Post(GenreDTO genreDTO)
        {

            if (genreDTO is null)
            {
                _logger.LogWarning("Invalid Data...");
                return BadRequest("Invalid Data...");
            }

            var genre = genreDTO.ToGenre();

            var genreCreated = _uof.GenreRepository.Create(genre);
            _uof.Commit(); //persistir as informações


            var newGenreDTO = genreCreated.ToGenreDTO();

            return new CreatedAtRouteResult("GetGenre", new { id = newGenreDTO.GenreId }, newGenreDTO);

        }


        [HttpPut("{id:int}")]
        public ActionResult<GenreDTO> Put(int id, GenreDTO genreDTO)
        {


            if (id != genreDTO.GenreId) {
                _logger.LogWarning("Invalid Data");
                return BadRequest("Invalid Data");
            }

            var genre = genreDTO.ToGenre();

            var genreUpdated = _uof.GenreRepository.Update(genre);
            _uof.Commit();

            var newGenreDTO = genreUpdated.ToGenreDTO();

            return Ok(newGenreDTO);
            
        }

        [HttpDelete("{id:int}")]
        public ActionResult<GenreDTO> Delete(int id)
        {
            var genre = _uof.GenreRepository.Get(g => g.GenreId == id);

            if (genre is null)
            {
                _logger.LogWarning("Genres not found...");
                return NotFound("Genre not found...");
            }

            var genreDeleted = _uof.GenreRepository.Delete(genre);
            _uof.Commit();

            var genreDeletedDTO = genreDeleted.ToGenreDTO();

            return Ok(genreDeletedDTO);
     
        }

        private ActionResult<IEnumerable<GenreDTO>> GetGenres(PagedList<Genre> genres)
        {
            var metadata = new
            {
                genres.TotalCount,
                genres.PageSize,
                genres.CurrentPage,
                genres.TotalPages,
                genres.HasNext,
                genres.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var genresDTO = genres.ToGenreDTOList();

            return Ok(genresDTO);
        }


    }
}