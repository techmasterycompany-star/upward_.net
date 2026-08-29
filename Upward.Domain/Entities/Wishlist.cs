using Upwork.Domain.Common;

namespace Upwork.Domain.Entities
{
    public class Wishlist : BaseEntity
    {
        public long CandidateId { get; set; }

        public long JobId { get; set; }

        public CandidateProfile Candidate { get; set; } = null!;

        public Job Job { get; set; } = null!;
    }
}
