using LibraryApi.Context;
using LibraryApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly ILogger<GenresController> _logger;


        public BooksController(AppDbContext context, ILogger<GenresController> logger)
        {
            _context = context; 
            _logger = logger;
        }


        [HttpGet]
        public ActionResult<IEnumerable<Book>> Get()
        {

            var books = _context.Books.AsNoTracking().Take(10).ToList();

            if (books is null) { 
            _logger.LogWarning("Books not found...");
            return NotFound("Books not found...");
        }
                return books;
            }
     
        

        [HttpGet("{id:int}", Name = "GetBook")]
        public ActionResult<Book> Get(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookId == id);

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

                _context.Books.Add(book);
                _context.SaveChanges();

                return new CreatedAtRouteResult("GetBook", new { id = book.BookId }, book);
        }


        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Book book)
        {

            if (id != book.BookId)
            {
                _logger.LogWarning("Invlaid Data");
                return BadRequest("Invalid Data");
            }

                _context.Entry(book).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(book);

        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {

            var book = _context.Books.FirstOrDefault(b => b.BookId == id);

            if (book is null)
            {
                _logger.LogWarning("Book not found...");
                return NotFound("Book not found...");
            }

                _context.Books.Remove(book);
                _context.SaveChanges();

                return Ok(book);

        }

    }
}
