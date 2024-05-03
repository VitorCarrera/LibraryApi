using LibraryApi.Models;

namespace LibraryApi.Repositories.Interfaces
{
    public interface IGenreRepository : IRepository<Genre>
    {
 
        IEnumerable<Genre> GetAllBooks();
    }
}
