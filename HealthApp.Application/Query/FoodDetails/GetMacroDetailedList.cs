using HealthApp.Application.Common.Interfaces;
using HealthApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static HealthApp.Application.Query.FoodDetails.GetMacroDetailedList;

namespace HealthApp.Application.Query.FoodDetails
{
    public  class GetMacroDetailedList
    {
        public class Request : IRequest<PagedResult<Food>>
        {
            public int PageSize { get; set; }

            public int Page { get; set; }


        }

        public class PagedResult<T>
        {
            public int Page { get; set; }

            public int PageSize { get; set; }

            public int TotalCount { get; set; }

            public IReadOnlyList<T> Foods { get; set; }
        }

        public class Handler : IRequestHandler<GetMacroDetailedList.Request, PagedResult<Food>>
        {

            private readonly IApplicationDbContext _context;
            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<PagedResult<Food>> Handle(Request request, CancellationToken cancellation)
            {
                var food = _context.Foods.AsNoTracking();

                var count = await food.CountAsync(cancellation);
            
                var result =  await food.
                    OrderBy(x => x.Id).
                    Skip((request.PageSize - 1) * request.PageSize).
                    Take(request.PageSize).
                    Select(x => x).ToListAsync(cancellation);



                return new PagedResult<Food>()
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalCount = count,
                    Foods = result
                };
            }
        }
    }
}
