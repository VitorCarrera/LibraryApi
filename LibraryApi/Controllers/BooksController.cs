using AutoMapper;
using LibraryApi.Context;
using LibraryApi.DTOs;
using LibraryApi.Models;
using LibraryApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("books/{id}")]
        public ActionResult<IEnumerable<BookDTO>> GetBooksGenre(int id)
        {
            var books = _uof.BookRepository.GetBooksByGenre(id);

            if (books is null)
                return NotFound();

            //var destino = _mapper.Map<Destino>(origem);
            var booksDTO = _mapper.Map<IEnumerable<BookDTO>>(books);

            return Ok(booksDTO);
        }

        [HttpGet]
        public ActionResult<IEnumerable<BookDTO>> Get()
        {

            var books = _uof.BookRepository.GetAll().ToList();

            if (books is null) { 
            _logger.LogWarning("Books not found...");
            return NotFound("Books not found...");
        }

            var booksDTO = _mapper.Map<IEnumerable<BookDTO>>(books);
                return Ok(booksDTO);
            }
     
        

        [HttpGet("{id:int}", Name = "GetBook")]
        public ActionResult<BookDTO> Get(int id)
        {
            var book = _uof.BookRepository.Get(b => b.BookId == id);

            if (book is null)
            {
                _logger.LogWarning("Book not found...");
                return NotFound("Book not found...");
            }

            var bookDTO = _mapper.Map<BookDTO>(book);
                return Ok(book);
 
        }


        [HttpPost]
        public ActionResult<BookDTO> Post(BookDTO bookDTO)
        {

            if (bookDTO is null)
            {
                _logger.LogWarning("Invalid Data");
                return BadRequest("Invalid Data");
            }

            var book = _mapper.Map<Book>(bookDTO);

            var bookCreated = _uof.BookRepository.Create(book);
            _uof.Commit();

            var newBookDTO = _mapper.Map<BookDTO>(bookCreated);

                return new CreatedAtRouteResult("GetBook", new { id = newBookDTO.BookId }, newBookDTO);
        }

        
        [HttpPatch("{id}/UpdatePartial")]
        public ActionResult<BookDTOUpdateResponse> Patch(int id,
            JsonPatchDocument<BookDTOUpdateRequest> patchBookDTO)
        {
            if (patchBookDTO is null || id <= 0)
                return BadRequest();

            var book = _uof.BookRepository.Get(c => c.BookId == id);

            if (book is null)
                return NotFound();

            var bookUpdateRequest = _mapper.Map<BookDTOUpdateRequest>(book);

            patchBookDTO.ApplyTo(bookUpdateRequest, ModelState);

            if (!ModelState.IsValid || TryValidateModel(bookUpdateRequest))
                return BadRequest();

            _mapper.Map(bookUpdateRequest, book);

            _uof.BookRepository.Update(book);
            _uof.Commit();

            return Ok(_mapper.Map<BookDTOUpdateResponse>(book));
        }


        [HttpPut("{id:int}")]
        public ActionResult<BookDTO> Put(int id, Book bookDTO)
        {

            if (id != bookDTO.BookId)
            {
                _logger.LogWarning("Invalid Data");
                return BadRequest("Invalid Data");
            }

            var book = _mapper.Map<Book>(bookDTO);

            var updated = _uof.BookRepository.Update(book);
            _uof.Commit();

            var newBookDTO = _mapper.Map<BookDTO>(updated);


            return Ok(newBookDTO);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<BookDTO> Delete(int id)
        {

            var book = _uof.BookRepository.Get(b => b.BookId == id);

            if (book is null)
            {
                _logger.LogWarning("Book not found...");
                return NotFound("Book not found...");
            }

             var deleted = _uof.BookRepository.Delete(book);
            _uof.Commit();

            var bookDTO = _mapper.Map<BookDTO>(deleted);

            return Ok(bookDTO);

        }

    }
}
