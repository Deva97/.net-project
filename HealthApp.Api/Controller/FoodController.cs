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


        [HttpGet("getMacro/{id}")]
        public async Task<IActionResult> getMacrosDetail(Guid Id)
        {
            var result = await _mediator.Send(new GetMacroDetails.Request()
            {
                Id = Id
            });
            return Ok(result);
        }

    }
}
