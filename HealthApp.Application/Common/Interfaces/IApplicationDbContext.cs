using HealthApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Food> Foods { get; }
    }
}
