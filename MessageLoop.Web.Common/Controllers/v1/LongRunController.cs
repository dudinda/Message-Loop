using System.Net.Mime;

using Asp.Versioning;

using MessageLoop.Common.Models.LongRun;
using MessageLoop.Common.Services.LongRun;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessageLoop.Web.Common.Controllers.v1
{
    [ApiController, ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/longrun")]
    [Produces(MediaTypeNames.Application.Json)]
    public class LongRunController : ControllerBase
    {
        private readonly ILongRunService<LongRunItem> _storage;

		public LongRunController(
              ILongRunService<LongRunItem> storage)
        {
            _storage = storage;
        }

        [HttpGet("{tokenId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetLongRunStatus(Guid tokenId)
		{
            if (!_storage.TryGetResult(tokenId, out var token))
            {
                return NotFound();
            }
            return Ok(token);
		}

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return Ok(_storage.Items);
        }

        [HttpPatch("abort/{tokenId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Abort(Guid tokenId)
        {
            _storage.Abort(new LongRunToken(tokenId));
            return NoContent(); 
        }    
    }
}
