namespace LibraryApi.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IBookRepository BookRepository { get; }
        IGenreRepository GenreRepository { get; }

        void Commit(); 
    }
}
