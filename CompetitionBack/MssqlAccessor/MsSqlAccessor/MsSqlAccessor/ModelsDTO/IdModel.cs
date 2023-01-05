using System.Text.Json.Serialization;

namespace MsSqlAccessor.Models
{
    public interface IdModel
    {
        public abstract int Id { get; set; }
        public abstract int StatusId { get; set; }
    }
}
