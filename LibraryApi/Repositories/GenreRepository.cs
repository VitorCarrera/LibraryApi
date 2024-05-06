using LibraryApi.Context;
using LibraryApi.Models;
using LibraryApi.Pagination;
using LibraryApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace LibraryApi.Repositories
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {


        public GenreRepository(AppDbContext context) : base(context){
        }

        public async Task<IEnumerable<Genre>> GetAllBooksAsync()
        {
            var genres = await _context.Genres.Include(g => g.Books).ToListAsync();    

            return genres;
        }

        public async Task<IPagedList<Genre>> GetGenresAsync(GenresParameters genresParameters)
        {
            var genres = await GetAllAsync();
                
            var genresOrder = genres.OrderBy(g => g.GenreId).AsQueryable();

            var result = await genresOrder.ToPagedListAsync(genresParameters.PageNumber,
                genresParameters.PageSize);

            return result;
        }

        public async Task<IPagedList<Genre>> GetGenresFilterNameAsync(GenresFilterName genresParameters)
        {

            var genres = await GetAllAsync();
            

            if (!string.IsNullOrEmpty(genresParameters.Name))
                genres = genres.Where(g => g.Name.Contains(genresParameters.Name));


            var genresFiltered = await genres.ToPagedListAsync(genresParameters.PageNumber,
                genresParameters.PageSize);

            return genresFiltered;
        
        }
    }
}
