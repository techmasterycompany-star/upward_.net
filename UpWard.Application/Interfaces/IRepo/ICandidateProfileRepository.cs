using System;
using System.Collections.Generic;
using System.Linq;
using Upward.Domain.Entities;

namespace Upward.Application.Interfaces.IRepo
{
    public interface ICandidateProfileRepository
    {
        Task<CandidateProfile?> GetByUserIdAsync(long userId);

        Task<bool> ExistsAsync(long userId);

        Task AddAsync(CandidateProfile profile);

        void Update(CandidateProfile profile);

        Task SaveChangesAsync();
    }
}
