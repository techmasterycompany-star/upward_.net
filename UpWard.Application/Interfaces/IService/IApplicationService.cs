using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upward.Application.DTOs.Candidate;

namespace Upward.Application.Interfaces.IService
{
    public interface IApplicationService
    {
        Task<ApplicationDto> ApplyAsync(long candidateId, long jobId, ApplyJobRequestDto request);
        Task<ApplicationDto> ApplyUsingProfileAsync(long candidateId, long jobId, ApplyUsingProfileDto request);

        Task<List<CandidateApplicationDto>> GetMyApplicationsAsync(long candidateId);

        Task CancelAsync(long candidateId, long applicationId);
    }
}
