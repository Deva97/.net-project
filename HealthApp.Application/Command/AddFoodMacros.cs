using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using AutoMapper;
using HealthApp.Application.Common.Interfaces;
using HealthApp.Domain.Entities;
using System.Net;

namespace HealthApp.Application.Command
{
    public class AddFoodMacros
    {

        public class Request : IRequest<Result>
        {
            public string Name { get; set; } = null!;

            public int Calories { get; set; }

            public decimal Protein { get; set; }

            public decimal Carbs { get; set; }

            public decimal Fat { get; set; }

            public int Cost { get; set; }

            [Range(0, 100)]
            public int BioAvailibility { get; set; }
        }

        public class Result
        {
            public HttpStatusCode Code { get; set; }
        }

        public class Handler : IRequestHandler<Request, Result>
        {
            private readonly IMapper _mapper;
            private readonly IFoodRepository _context;

            public Handler(IMapper mapper, IFoodRepository context)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                try
                {
                    var data =  _mapper.Map<Food>(request);
                    data.Id = Guid.NewGuid();
                    await _context.SaveAsync(data,cancellationToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine("error occured" + e);
                    throw;
                }

                return new Result()
                {
                    Code = HttpStatusCode.Created
                };
            }
        }
    }
}
