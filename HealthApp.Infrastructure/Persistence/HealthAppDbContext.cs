using HealthApp.Application.Common.Interfaces;
using HealthApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthApp.Infrastructure.Persistence
{

    //dotnet ef migrations add UpdateIdType --project HealthApp.Infrastructure --startup-project HealthApp.Api
    //dotnet ef database update --project HealthApp.Infrastructure --startup-project HealthApp.Api
    public class HealthAppDbContext : DbContext, IApplicationDbContext
    {
        public HealthAppDbContext(DbContextOptions<HealthAppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HealthAppDbContext).Assembly);
        }

        public DbSet<Food> Foods => Set<Food>();
    }
}
