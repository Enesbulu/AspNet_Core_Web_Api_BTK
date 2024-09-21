using Services.Contracts;
using System.Dynamic;
using System.Reflection;

namespace Services.Manager
{
    public class DataShaper<T> : IDataShaper<T> where T : class
    {
        public PropertyInfo[] Properties { get; set; }

        //Ctor
        public DataShaper()
        {
            Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);  //prop ların hepsi alındı
        }
        public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fieldString)
        {
            var requiredFields = GetRequiredProperties(fieldString);
            return FetchData(entities, requiredFields);
        }

        public ExpandoObject ShapeData(T entity, string fieldString)
        {
            var requiredProperties = GetRequiredProperties(fieldString);
            return FetchDataForEntity(entity, requiredProperties);
        }

        /// <summary>
        /// Gerekli olabilecek <see langword="property"/> listesi döner.
        /// </summary>
        /// <param name="fieldString"></param>
        /// <returns></returns>
        private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldString)
        {
            var requiredFields = new List<PropertyInfo>();
            if (!string.IsNullOrWhiteSpace(fieldString))
            {
                var fields = fieldString.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var field in fields)
                {
                    var property = Properties.FirstOrDefault(pi => pi.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));
                    if (property is null)
                        continue;

                    requiredFields.Add(property);
                }

            }
            else
            {
                requiredFields = Properties.ToList();
            }
            return requiredFields;
        }

        private ExpandoObject FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedObject = new ExpandoObject();
            foreach (var property in requiredProperties)
            {
                var objectProperyValue = property.GetValue(entity);
                shapedObject.TryAdd(property.Name, objectProperyValue);
            }
            return shapedObject;
        }

        private IEnumerable<ExpandoObject> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedData = new List<ExpandoObject>();
            foreach (var entity in entities)
            {
                var shapedObject = FetchDataForEntity(entity, requiredProperties);
                shapedData.Add(shapedObject);
            }
            return shapedData;
        }


    }
}
