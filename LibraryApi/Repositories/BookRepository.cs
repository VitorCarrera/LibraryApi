using LibraryApi.Context;
using LibraryApi.Models;
using LibraryApi.Pagination;
using LibraryApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LibraryApi.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {


        public BookRepository(AppDbContext context) : base(context)
        {
        }

        public PagedList<Book> GetBooks(BooksFilterPrice booksParameters)
        {
            var books =  GetAll()
                .OrderBy(b => b.BookId)
                .AsQueryable();

            var booksOrdered = PagedList<Book>.ToPagedList(books,
                 booksParameters.PageNumber,
                 booksParameters.PageSize);

            return booksOrdered;
        }

        public IEnumerable<Book> GetBooksByGenre(int id)
        {
            var books = GetAll().Where(b => b.GenreId == id).ToList();

            return books;
        }

        public PagedList<Book> GetBooksFilterPrice(BooksFilterPrice booksFilterParameters)
        {
            var books = GetAll().AsQueryable();

            if (booksFilterParameters.Price.HasValue && !string.IsNullOrEmpty(booksFilterParameters.PriceCriteria))
            {
                if (booksFilterParameters.PriceCriteria.Equals("maior", StringComparison.OrdinalIgnoreCase))
                {
                    books = books.Where(b => b.Price > booksFilterParameters.Price.Value).OrderBy(b => b.Price);
                }else if(booksFilterParameters.PriceCriteria.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    books = books.Where(b => b.Price < booksFilterParameters.Price.Value).OrderBy(b => b.Price);
                }
                else if (booksFilterParameters.PriceCriteria.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    books = books.Where(b => b.Price == booksFilterParameters.Price.Value).OrderBy(b => b.Price);
                }
            }

            var booksFiltered = PagedList<Book>.ToPagedList(books, 
                                                    booksFilterParameters.PageNumber,
                                                    booksFilterParameters.PageSize);

            return booksFiltered;
        }
    }
}
