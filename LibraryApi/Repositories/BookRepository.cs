using LibraryApi.Context;
using LibraryApi.Models;
using LibraryApi.Pagination;
using LibraryApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using X.PagedList;

namespace LibraryApi.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {


        public BookRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IPagedList<Book>> GetBooksAsync(BooksFilterPrice booksParameters)
        {
            var books = await GetAllAsync();

            var booksOrder = books.OrderBy(b => b.BookId)
                .AsQueryable();

            var booksOrdered = await booksOrder.ToPagedListAsync(booksParameters.PageNumber,
                 booksParameters.PageSize);

            return booksOrdered;
        }

        public async Task<IEnumerable<Book>> GetBooksByGenreAsync(int id)
        {
            var books = await GetAllAsync();
            
            var result = books.Where(b => b.GenreId == id).ToList();

            return result;
        }

        public async Task<IPagedList<Book>> GetBooksFilterPriceAsync(BooksFilterPrice booksFilterParameters)
        {
            var booksBD = await GetAllAsync();

            var books = booksBD.AsQueryable();

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

            var booksFiltered = await books.ToPagedListAsync(booksFilterParameters.PageNumber,
                                                    booksFilterParameters.PageSize);

            return booksFiltered;
        }
    }
}
