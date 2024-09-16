using Entities.Models;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace Repositories.EFCore.Extentions
{
    public static class BookRepositoryExtentions
    {
        public static IQueryable<Book> FilterBooks(this IQueryable<Book> books, uint minPrice, uint maxPrice)
            => books.Where(book => book.Price >= minPrice && book.Price <= maxPrice);

        public static IQueryable<Book> Search(this IQueryable<Book> books, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return books;

            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return books.Where(book => book.Title
                .ToLower()
                .Contains(lowerCaseTerm));
        }
        public static IQueryable<Book> Sort(this IQueryable<Book> books, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return books.OrderBy(b => b.Id);

           /* //var orderParams = orderByQueryString.Trim().Split(',');
            //var propertyInfos = typeof(Book).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //var orderQueryBuilder = new StringBuilder
            //title ascending, price descending, id ascending,
            //foreach (var param in orderParams)
            //{
            //    if (string.IsNullOrWhiteSpace(param))
            //        continue;
            //    var propertyFromQueryName = param.Split(" ")[0];
            //    var objectPropery = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
            //    if (objectPropery is null)
            //        continue;
            //    var direction = param.EndsWith("desc") ? "descending" : "ascending";
            //    orderQueryBuilder.Append($"{objectPropery.Name.ToString()} {direction},");
            //}
            //var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
           */

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Book>(orderByQueryString);
            if (orderQuery is null)
                return books.OrderBy(b => b.Id);
            return books.OrderBy(orderQuery);

        }
    }
}
