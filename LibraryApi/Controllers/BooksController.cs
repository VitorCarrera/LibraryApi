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

        public BooksController(AppDbContext context)
        {
            _context = context; 
        }


        [HttpGet]
        public ActionResult<IEnumerable<Book>> Get()
        {

            try
            {
                var books = _context.Books.AsNoTracking().Take(10).ToList();

                if (books is null)
                    return NotFound("Books not found...");

                return books;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "There was a problem handling your request.");
            }
        }

        [HttpGet("{id:int}", Name = "GetBook")]
        public ActionResult<Book> Get(int id)
        {

            try
            {
                var book = _context.Books.FirstOrDefault(b => b.BookId == id);

                if (book is null)
                    return NotFound("Book not found...");

                return Ok(book);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "There was a problem handling your request.");
            }


        }


        [HttpPost]
        public ActionResult Post(Book book)
        {

            try
            {
                if (book is null)
                    return BadRequest();

                _context.Books.Add(book);
                _context.SaveChanges();

                return new CreatedAtRouteResult("GetBook", new { id = book.BookId }, book);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "There was a problem handling your request.");
            }
        }


        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Book book)
        {

            try
            {
                if (id != book.BookId)
                    return BadRequest();

                _context.Entry(book).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(book);
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
                var book = _context.Books.FirstOrDefault(b => b.BookId == id);

                if (book is null)
                    return NotFound("Book not found...");

                _context.Books.Remove(book);
                _context.SaveChanges();

                return Ok(book);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "There was a problem handling your request.");
            }
        }

    }
}
