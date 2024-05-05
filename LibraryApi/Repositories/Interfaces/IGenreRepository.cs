using LibraryApi.Models;
using LibraryApi.Pagination;

namespace LibraryApi.Repositories.Interfaces
{
    public interface IGenreRepository : IRepository<Genre>
    {
        PagedList<Genre> GetGenresFilterName(GenresFilterName genresParameters);
        PagedList<Genre> GetGenres(GenresParameters genresParameters);
        IEnumerable<Genre> GetAllBooks();
    }
}
