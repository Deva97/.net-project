using HealthApp.Application.Query.FoodDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controller
{
    [ApiController]
    [Route("Food/api/v1")]
    public class FoodController : ControllerBase
    {
        private readonly IMediator _mediator;
        public FoodController(IMediator mediator)
        {
            _mediator = mediator;
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

        [HttpPost]
        public async Task<IActionResult> getIPKey([FromHeader(Name = "Idempotency-key")] Guid key)
        {
            return Ok(key);
        }

    }
}
