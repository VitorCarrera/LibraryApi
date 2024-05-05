using LibraryApi.Models;
using LibraryApi.Pagination;

namespace LibraryApi.Repositories.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        PagedList<Book> GetBooks(BooksFilterPrice booksParameters);
        PagedList<Book> GetBooksFilterPrice(BooksFilterPrice booksFilterParameters);
        IEnumerable<Book> GetBooksByGenre(int id);
    }
}
