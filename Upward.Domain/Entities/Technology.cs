using Upward.Domain.Common;

namespace Upward.Domain.Entities
{
    public class Technology : BaseEntity
    {
        public string Name { get; set; } = null!;

        public ICollection<JobTechnology> JobTechnologies { get; set; } = new List<JobTechnology>();
    }
}
