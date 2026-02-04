using System.Net.Mime;

using Asp.Versioning;

using MessageLoop.Common.Models.LongRun;
using MessageLoop.Common.Services.LongRun;

using Microsoft.AspNetCore.Mvc;

namespace MessageLoop.Master.Controllers.v1
{
    [ApiController, ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/longrun")]
    [Produces(MediaTypeNames.Application.Json)]
    public class LongRunController : ControllerBase
    {
        private readonly ILongRunService<LongRunItem> _storage;
		private readonly ILogger<LongRunController> _logger;

		public LongRunController(
              ILongRunService<LongRunItem> storage,
			  ILogger<LongRunController> logger)
        {
            _storage = storage;
			_logger = logger;
        }

        [HttpGet("{tokenId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetLongRunStatus(Guid tokenId)
		{
            if (_storage.TryGetResult(tokenId, out var token))
            {
                return Ok(token);
            }
            return NotFound();
		}

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return Ok(_storage.Items);
        }

        [HttpPatch("abort/{tokenId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Abort(Guid tokenId)
        {
            _storage.Abort(new LongRunToken(tokenId));
            return Ok();
        }    
    }
}
