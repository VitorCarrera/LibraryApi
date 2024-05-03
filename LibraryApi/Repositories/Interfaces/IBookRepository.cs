using LibraryApi.Models;

namespace LibraryApi.Repositories.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        IEnumerable<Book> GetBooksByGenre(int id);
    }
}
