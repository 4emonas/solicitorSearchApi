using Microsoft.AspNetCore.Mvc;
using MediatR;
using SolicitorSearch.Common.Models;
using SolicitorSearch.Domain.Implementation.Requests;

namespace SolicitorSearch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolicitorSearchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SolicitorSearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetSolicitors")]
        public async Task<IActionResult> GetSolicitors([FromQuery] string location)
        {
            var query = new GetSolicitorsRequest(location);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost("GetSolicitor")]
        public async Task<IActionResult> GetSolicitor([FromBody] Solicitor solicitor, [FromQuery] string location)
        {
            var query = new GetSolicitorRequest(solicitor)
            {
                Location = location
            };
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost("UpdateSolicitorNotes")]
        public async Task<IActionResult> UpdateSolicitorNotes([FromBody] Solicitor solicitor)
        {
            var command = new UpdateSolicitorNotesRequest(solicitor);
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
