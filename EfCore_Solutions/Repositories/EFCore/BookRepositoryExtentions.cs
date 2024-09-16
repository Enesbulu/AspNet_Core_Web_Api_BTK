using Entities.Models;

namespace Repositories.EFCore
{
    public static class BookRepositoryExtentions
    {
        public static IQueryable<Book> FilterBooks(this IQueryable<Book> books, uint minPrice, uint maxPrice)
            => books.Where(book => (book.Price >= minPrice) && (book.Price <= maxPrice));


    }
}
