using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upwork.Application.DTOs.Candidate;

namespace Upwork.Application.Interfaces.IService
{
    public interface IApplicationService
    {
        Task<ApplicationDto> ApplyAsync(long userId, long jobId, ApplyJobRequestDto request);
        Task<ApplicationDto> ApplyUsingProfileAsync(long userId, long jobId, ApplyUsingProfileDto request);

        Task<List<CandidateApplicationDto>> GetMyApplicationsAsync(long userId);

        Task CancelAsync(long userId, long applicationId);
    }
}
