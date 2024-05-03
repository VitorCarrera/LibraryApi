using LibraryApi.Context;
using LibraryApi.Models;
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


        
    }
}
