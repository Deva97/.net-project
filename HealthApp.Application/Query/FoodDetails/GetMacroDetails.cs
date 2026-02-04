using MediatR;
using System;
using System.Collections.Generic;
using HealthApp.Domain.Entities;
using System.Text;

using HealthApp.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Application.Query.FoodDetails
{
    public class GetMacroDetails
    {

        public class Request : IRequest<Result>
        {
            public Guid Id { get; set; }
        }

        public class Result
        {
            public Food food { get; set; }
        }

        public class Handler : IRequestHandler<Request, Result>
        {
            private readonly IApplicationDbContext? _context;

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var item = await _context.Foods.Where(x => x.Id.Equals(request.Id)).FirstAsync();

                return new Result()
                {
                    food = item
                };
            }
        }
    }
}
