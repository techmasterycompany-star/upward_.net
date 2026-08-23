using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upward.Application.DTOs.Common;

namespace Upward.Application.Interfaces.IRepo
{
    public interface IJobRepository
    {
        Task<PagedResultDto<JobSearchResultDto>> SearchAsync(JobSearchRequestDto request);
    }
}
