using HealthApp.Application.Common.Interfaces;
using HealthApp.Domain.Entities;
using HealthApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HealthApp.Infrastructure.Services
{
    public class FoodRepository : IFoodRepository
    {
        private readonly HealthAppDbContext _context;

        public FoodRepository(HealthAppDbContext context)
        {
            _context = context;
        }
        public async Task SaveAsync(Food food, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken); // opens a transcation channel with DB

            try
            {

                _context.Foods.Add(food);
                await _context.SaveChangesAsync(cancellationToken); //ef core wrap it inside a transaction
                //this savechange should be applied to each table for atomicity

                await transaction.CommitAsync(cancellationToken); //permanently make a change to DB
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                await transaction.RollbackAsync(cancellationToken); // roll back
                throw;

            }
        }
    }
}
