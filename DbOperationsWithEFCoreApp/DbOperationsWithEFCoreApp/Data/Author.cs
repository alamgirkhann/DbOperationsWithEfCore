using System.Text.Json.Serialization;

namespace DbOperationsWithEFCoreApp.Data
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
