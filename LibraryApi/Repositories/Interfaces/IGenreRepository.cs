using LibraryApi.Models;
using LibraryApi.Pagination;
using X.PagedList;

namespace LibraryApi.Repositories.Interfaces
{
    public interface IGenreRepository : IRepository<Genre>
    {
        Task<IPagedList<Genre>> GetGenresFilterNameAsync(GenresFilterName genresParameters);
        Task<IPagedList<Genre>> GetGenresAsync(GenresParameters genresParameters);
        Task<IEnumerable<Genre>> GetAllBooksAsync();
    }
}
