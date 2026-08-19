using Upward.Domain.Common;

namespace Upward.Domain.Entities
{
    public class JobTechnology : BaseEntity
    {
        public long JobId { get; set; }

        public long TechnologyId { get; set; }

        public Job Job { get; set; } = null!;

        public Technology Technology { get; set; } = null!;
    }
}
