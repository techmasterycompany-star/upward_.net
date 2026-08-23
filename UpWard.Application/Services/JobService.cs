using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Upward.Application.DTOs.Common;
using Upward.Application.Interfaces.IRepo;
using Upward.Application.Interfaces.IService;

namespace Upward.Application.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }
        public async Task<PagedResultDto<JobSearchResultDto>> SearchAsync(JobSearchRequestDto request)
        {
            ValidateRequest(request);

            return await _jobRepository.SearchAsync(request);            
        }

        private static void ValidateRequest(JobSearchRequestDto request)
        {
            if (request.MinSalary.HasValue &&request.MaxSalary.HasValue && request.MinSalary > request.MaxSalary)
            {
                throw new ArgumentException( "Minimum salary cannot be greater than maximum salary.");
            }

            if (request.Page < 1) request.Page = 1;           
            if (request.PageSize < 1) request.PageSize = 10;         
            if (request.PageSize > 100) request.PageSize = 100;
            
        }
    }
}
