using LibraryApi.Context;
using LibraryApi.Models;
using LibraryApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {


        public BookRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Book> GetBooksByGenre(int id)
        {
            var books = GetAll().Where(b => b.GenreId == id).ToList();

            return books;
        }
    }
}
