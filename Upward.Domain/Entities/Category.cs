using Upwork.Domain.Common;

namespace Upwork.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
