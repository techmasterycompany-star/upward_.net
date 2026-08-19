using Upward.Domain.Common;

namespace Upward.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
