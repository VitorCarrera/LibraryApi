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
using X.PagedList;
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
        public async Task<ActionResult<IEnumerable<GenreDTO>>> GetAsync([FromQuery] GenresParameters genresParameters)
        {
            var genres = await _uof.GenreRepository.GetGenresAsync(genresParameters);

            return GetGenres(genres);
        }

        

        [HttpGet("filter/nome/pagination")]
        public async Task<ActionResult<IEnumerable<GenreDTO>>> GetGenresFilteredAsync([FromQuery] GenresFilterName genresFilter)
        {
            var genresFiltered = await _uof.GenreRepository.GetGenresFilterNameAsync(genresFilter);

            return GetGenres(genresFiltered); 
        }

        [HttpGet("books")]
        public async Task<ActionResult<IEnumerable<GenreDTO>>> GetAllBooksAsync()
        {
            var genres = await _uof.GenreRepository.GetAllBooksAsync();

            if (genres is null)
            {
                _logger.LogWarning("Genres and Books not found...");
                return NotFound("Genres and Books not found...");
            }

            return Ok(genres);

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDTO>>> GetAsync()
        {

            var genres = await _uof.GenreRepository.GetAllAsync();

            if (genres is null)
                return NotFound("Genres don't exist...");


            var genresDTO = genres.ToGenreDTOList();

            return Ok(genresDTO);
        }


        [HttpGet("{id:int}", Name = "GetGenre")]
        public async Task<ActionResult<GenreDTO>> GetAsync(int id)
        {
            var genre = await _uof.GenreRepository.GetAsync(g => g.GenreId == id);

            if (genre is null)
            {

                _logger.LogWarning($"Genre not found...");
                return NotFound("Genre not found...");

            }

            var genreDTO = genre.ToGenreDTO();

            return Ok(genreDTO);

        }


        [HttpPost]
        public async Task<ActionResult<GenreDTO>> PostAsync(GenreDTO genreDTO)
        {

            if (genreDTO is null)
            {
                _logger.LogWarning("Invalid Data...");
                return BadRequest("Invalid Data...");
            }

            var genre = genreDTO.ToGenre();

            var genreCreated = _uof.GenreRepository.Create(genre);
            await _uof.CommitAsync(); //persistir as informações


            var newGenreDTO = genreCreated.ToGenreDTO();

            return new CreatedAtRouteResult("GetGenre", new { id = newGenreDTO.GenreId }, newGenreDTO);

        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<GenreDTO>> PutAsync(int id, GenreDTO genreDTO)
        {


            if (id != genreDTO.GenreId) {
                _logger.LogWarning("Invalid Data");
                return BadRequest("Invalid Data");
            }

            var genre = genreDTO.ToGenre();

            var genreUpdated = _uof.GenreRepository.Update(genre);
            await _uof.CommitAsync();

            var newGenreDTO = genreUpdated.ToGenreDTO();

            return Ok(newGenreDTO);
            
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<GenreDTO>> DeleteAsync(int id)
        {
            var genre = await _uof.GenreRepository.GetAsync(g => g.GenreId == id);

            if (genre is null)
            {
                _logger.LogWarning("Genres not found...");
                return NotFound("Genre not found...");
            }

            var genreDeleted = _uof.GenreRepository.Delete(genre);
            await _uof.CommitAsync();

            var genreDeletedDTO = genreDeleted.ToGenreDTO();

            return Ok(genreDeletedDTO);
     
        }

        private ActionResult<IEnumerable<GenreDTO>> GetGenres(IPagedList<Genre> genres)
        {
            var metadata = new
            {
                genres.Count,
                genres.PageSize,
                genres.PageCount,
                genres.TotalItemCount,
                genres.HasNextPage,
                genres.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var genresDTO = genres.ToGenreDTOList();

            return Ok(genresDTO);
        }


    }
}