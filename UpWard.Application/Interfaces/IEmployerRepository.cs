using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upward.Domain.Entities;

namespace Upward.Application.Interfaces
{
    public interface IEmployerRepository
    {
        Task<EmployerProfile> GetById(long id);
    }
}
