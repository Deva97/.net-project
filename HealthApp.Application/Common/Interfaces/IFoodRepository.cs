using HealthApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthApp.Application.Common.Interfaces
{
    public interface IFoodRepository
    {
        Task SaveAsync(Food food, CancellationToken cancellation);
    }
}
