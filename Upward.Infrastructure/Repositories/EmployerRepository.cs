using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upward.Application.Interfaces;
using Upward.Domain.Entities;
using Upward.Infrastructure.Data;

namespace Upward.Infrastructure.Repositories
{
    public class EmployerRepository : IEmployerRepository
    {
        private readonly AppDBContext _context;
        public EmployerRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task<EmployerProfile> GetById(long id)
        {
            var result = await _context.EmployerProfiles.FirstOrDefaultAsync(p => p.Id == id);
            return result;
        }
    }
}
