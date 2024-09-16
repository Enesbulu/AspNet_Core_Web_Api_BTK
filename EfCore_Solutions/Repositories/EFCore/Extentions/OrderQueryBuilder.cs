using System.Reflection;
using System.Text;

namespace Repositories.EFCore.Extentions
{
    public static class OrderQueryBuilder
    {
        public static String CreateOrderQuery<T>(String orderByQueryString)
        {
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var objectPropery = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
                if (objectPropery is null)
                    continue;

                var direction = param.EndsWith("desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectPropery.Name.ToString()} {direction},");

            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            return orderQuery;
        }
    }
}
