using LibraryApi.Models;
using LibraryApi.Pagination;
using X.PagedList;

namespace LibraryApi.Repositories.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IPagedList<Book>> GetBooksAsync(BooksFilterPrice booksParameters);
        Task<IPagedList<Book>> GetBooksFilterPriceAsync(BooksFilterPrice booksFilterParameters);
        Task<IEnumerable<Book>> GetBooksByGenreAsync(int id);
    }
}
