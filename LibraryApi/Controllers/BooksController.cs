using LibraryApi.Context;
using LibraryApi.Models;
using LibraryApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
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


        public BooksController(IUnitOfWork uof,
             ILogger<GenresController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        [HttpGet("books/{id}")]
        public ActionResult<IEnumerable<Book>> GetBooksGenre(int id)
        {
            var books = _uof.BookRepository.GetBooksByGenre(id);

            if (books is null)
                return NotFound();

            return Ok(books);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Book>> Get()
        {

            var books = _uof.BookRepository.GetAll().ToList();

            if (books is null) { 
            _logger.LogWarning("Books not found...");
            return NotFound("Books not found...");
        }
                return books;
            }
     
        

        [HttpGet("{id:int}", Name = "GetBook")]
        public ActionResult<Book> Get(int id)
        {
            var book = _uof.BookRepository.Get(b => b.BookId == id);

            if (book is null)
            {
                _logger.LogWarning("Book not found...");
                return NotFound("Book not found...");
            }

                return Ok(book);
 
        }


        [HttpPost]
        public ActionResult Post(Book book)
        {

            if (book is null)
            {
                _logger.LogWarning("Invalid Data");
                return BadRequest("Invalid Data");
            }

            var bookCreated = _uof.BookRepository.Create(book);
            _uof.Commit();

                return new CreatedAtRouteResult("GetBook", new { id = bookCreated.BookId }, bookCreated);
        }


        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Book book)
        {

            if (id != book.BookId)
            {
                _logger.LogWarning("Invalid Data");
                return BadRequest("Invalid Data");
            }

            var updated = _uof.BookRepository.Update(book);
            _uof.Commit();

                return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {

            var book = _uof.BookRepository.Get(b => b.BookId == id);

            if (book is null)
            {
                _logger.LogWarning("Book not found...");
                return NotFound("Book not found...");
            }

             var deleted = _uof.BookRepository.Delete(book);
            _uof.Commit();

              return Ok(deleted);

        }

    }
}
