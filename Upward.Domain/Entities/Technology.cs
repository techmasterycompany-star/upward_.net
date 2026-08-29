using Upwork.Domain.Common;

namespace Upwork.Domain.Entities
{
    public class Technology : BaseEntity
    {
        public string Name { get; set; } = null!;

        public ICollection<JobTechnology> JobTechnologies { get; set; } = new List<JobTechnology>();
    }
}
