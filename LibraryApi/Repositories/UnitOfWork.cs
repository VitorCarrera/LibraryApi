using LibraryApi.Context;
using LibraryApi.Repositories.Interfaces;

namespace LibraryApi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IBookRepository? _bookRepo;

        private IGenreRepository? _genreRepo;

        public AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        //obter uma instancia -> lazy loading
        //Adiar a obtenção dos objetos até que eles sejam realmente necessários
        public IBookRepository BookRepository
        {
            get
            {
                return _bookRepo = _bookRepo ?? new BookRepository(_context);
            }
        }

        public IGenreRepository GenreRepository
        {
            get
            {
                return _genreRepo = _genreRepo ?? new GenreRepository(_context);
            }
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();    
        }


    }
}
