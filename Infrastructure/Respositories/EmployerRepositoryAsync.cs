using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Respositories
{
    public class EmployerRepositoryAsync : GenericRepositoryAsync<Employer>, IEmployerRepositoryAsync
    {
        private readonly DbSet<Employer> _employer;

        public EmployerRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _employer = dbContext.Set<Employer>();
        }
    }
}
