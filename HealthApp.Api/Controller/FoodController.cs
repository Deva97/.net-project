using HealthApp.Application.Command;
using HealthApp.Application.Query.FoodDetails;
using HealthApp.Domain.Entities;
using HealthApp.Domain.Enums;
using HealthApp.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HealthApp.Api.Controller
{
    [ApiController]
    [Route("Food/api/v1")]
    public class FoodController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly DBConfigurationClass dB;
        public FoodController(IMediator mediator, IOptions<DBConfigurationClass> db)
        {
            _mediator = mediator;
            dB = db.Value;
        }


        [HttpGet("getMacro/{id:int}")]
        public async Task<IActionResult> getMacrosDetail([FromRoute] Guid Id)
        {
            var result = await _mediator.Send(new GetMacroDetails.Request()
            {
                Id = Id
            });
            return Ok(result);
        }

        [HttpGet("getMacroList")]
        public async Task<IActionResult> getMacrosDetailedList([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = await _mediator.Send(new GetMacroDetailedList.Request()
            {
                Page = page,
                PageSize = pageSize
            });
            return Ok(result);
        }

        [HttpPost("test/Idempotent")]
        public async Task<IActionResult> getIPKey([FromHeader(Name = "Idempotency-key")] Guid key)
        {
            return Ok(key);
        }

        [HttpGet("")]
        public async Task<ActionResult<string?>> getConnectionString(FoodType foodType)
        {
            return dB.DefaultConnection;
        }

        [HttpPost("admin")]

        public async Task<IActionResult> AddFoodMacros([FromBody] AddFoodMacros reuqest)
        {
            return Ok(await _mediator.Send(reuqest));
        }


        

    }
}
