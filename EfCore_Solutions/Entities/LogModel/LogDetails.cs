using System.Text.Json;
using System.Text.Json.Nodes;

namespace Entities.LogModel
{
    public class LogDetails
    {
        public Object? ModelModel { get; set; }
        public Object? Controller { get; set; }
        public Object? Action { get; set; }
        public Object? Id{ get; set; }
        public Object? CreateAt{ get; set; }

        public LogDetails()
        {
            CreateAt= DateTime.Now;
        }
        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
