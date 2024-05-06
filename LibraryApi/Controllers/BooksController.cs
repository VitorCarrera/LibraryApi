using AutoMapper;
using LibraryApi.Context;
using LibraryApi.DTOs;
using LibraryApi.Models;
using LibraryApi.Pagination;
using LibraryApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        
        private readonly IUnitOfWork _uof;
        private readonly ILogger<GenresController> _logger;
        private readonly IMapper _mapper;


        public BooksController(IUnitOfWork uof,
             ILogger<GenresController> logger,
             IMapper mapper)
        {
            _uof = uof;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAsync([FromQuery] BooksFilterPrice booksParameters)
        {
            var books = await _uof.BookRepository.GetBooksAsync(booksParameters);
            return GetBooks(books);
        }

        

        [HttpGet("filter/prico/pagination")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooksFilterPriceAsync([FromQuery] BooksFilterPrice booksFilterParameters)
        {
            var books = await _uof.BookRepository.GetBooksFilterPriceAsync(booksFilterParameters);

            return GetBooks(books);

        }

        [HttpGet("books/{id}")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooksGenreAsync(int id)
        {
            var books = await _uof.BookRepository.GetBooksByGenreAsync(id);

            if (books is null)
                return NotFound();

            //var destino = _mapper.Map<Destino>(origem);
            var booksDTO = _mapper.Map<IEnumerable<BookDTO>>(books);

            return Ok(booksDTO);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAsync()
        {

            var booksRep = await _uof.BookRepository.GetAllAsync();

            var books = booksRep.ToList();

            if (books is null) { 
            _logger.LogWarning("Books not found...");
            return NotFound("Books not found...");
        }

            var booksDTO = _mapper.Map<IEnumerable<BookDTO>>(books);
                return Ok(booksDTO);
            }
     
        

        [HttpGet("{id:int}", Name = "GetBook")]
        public async Task<ActionResult<BookDTO>> GetAsync(int id)
        {
            var book = await _uof.BookRepository.GetAsync(b => b.BookId == id);

            if (book is null)
            {
                _logger.LogWarning("Book not found...");
                return NotFound("Book not found...");
            }

            var bookDTO = _mapper.Map<BookDTO>(book);
                return Ok(book);
 
        }


        [HttpPost]
        public async Task<ActionResult<BookDTO>> PostAsync(BookDTO bookDTO)
        {

            if (bookDTO is null)
            {
                _logger.LogWarning("Invalid Data");
                return BadRequest("Invalid Data");
            }

            var book = _mapper.Map<Book>(bookDTO);

            var bookCreated = _uof.BookRepository.Create(book);
            await _uof.CommitAsync();

            var newBookDTO = _mapper.Map<BookDTO>(bookCreated);

                return new CreatedAtRouteResult("GetBook", new { id = newBookDTO.BookId }, newBookDTO);
        }

        
        [HttpPatch("{id}/UpdatePartial")]
        public async Task<ActionResult<BookDTOUpdateResponse>> PatchAsync(int id,
            JsonPatchDocument<BookDTOUpdateRequest> patchBookDTO)
        {
            if (patchBookDTO is null || id <= 0)
                return BadRequest();

            var book = await _uof.BookRepository.GetAsync(c => c.BookId == id);

            if (book is null)
                return NotFound();

            var bookUpdateRequest = _mapper.Map<BookDTOUpdateRequest>(book);

            patchBookDTO.ApplyTo(bookUpdateRequest, ModelState);

            if (!ModelState.IsValid || TryValidateModel(bookUpdateRequest))
                return BadRequest();

            _mapper.Map(bookUpdateRequest, book);

            _uof.BookRepository.Update(book);
            await _uof.CommitAsync();

            return Ok(_mapper.Map<BookDTOUpdateResponse>(book));
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<BookDTO>> PutAsync(int id, Book bookDTO)
        {

            if (id != bookDTO.BookId)
            {
                _logger.LogWarning("Invalid Data");
                return BadRequest("Invalid Data");
            }

            var book = _mapper.Map<Book>(bookDTO);

            var updated = _uof.BookRepository.Update(book);
            await _uof.CommitAsync();

            var newBookDTO = _mapper.Map<BookDTO>(updated);


            return Ok(newBookDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<BookDTO>> DeleteAsync(int id)
        {

            var book = await _uof.BookRepository.GetAsync(b => b.BookId == id);

            if (book is null)
            {
                _logger.LogWarning("Book not found...");
                return NotFound("Book not found...");
            }

             var deleted = _uof.BookRepository.Delete(book);
            await _uof.CommitAsync();

            var bookDTO = _mapper.Map<BookDTO>(deleted);

            return Ok(bookDTO);

        }

        private ActionResult<IEnumerable<BookDTO>> GetBooks(IPagedList<Book> books)
        {
            var metadata = new
            {
                books.Count,
                books.PageSize,
                books.PageCount,
                books.TotalItemCount,
                books.HasNextPage,
                books.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var booksDTO = _mapper.Map<IEnumerable<BookDTO>>(books);

            return Ok(booksDTO);
        }

    }
}
