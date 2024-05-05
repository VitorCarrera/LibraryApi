using LibraryApi.Context;
using LibraryApi.Models;
using LibraryApi.Pagination;
using LibraryApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Repositories
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {


        public GenreRepository(AppDbContext context) : base(context){
        }

        public IEnumerable<Genre> GetAllBooks()
        {
            var genres = _context.Genres.Include(g => g.Books).ToList();    

            return genres;
        }

        public PagedList<Genre> GetGenres(GenresParameters genresParameters)
        {
            var genres = GetAll().OrderBy(g => g.GenreId).AsQueryable();

            var genresOrdered = PagedList<Genre>.ToPagedList(genres,
                genresParameters.PageNumber,
                genresParameters.PageSize);

            return genresOrdered;
        }

        public PagedList<Genre> GetGenresFilterName(GenresFilterName genresParameters)
        {

            var genres = GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(genresParameters.Name))
                genres = genres.Where(g => g.Name.Contains(genresParameters.Name));


            var genresFiltered = PagedList<Genre>.ToPagedList(genres,
                genresParameters.PageNumber,
                genresParameters.PageSize);

            return genresFiltered;
        
        }
    }
}
