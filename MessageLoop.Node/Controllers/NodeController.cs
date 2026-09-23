using System.Net.Mime;

using Asp.Versioning;

using MessageLoop.Common.Models.LongRun;
using MessageLoop.Common.Services.LongRun;
using MessageLoop.Node.Code.Extensions;
using MessageLoop.Node.Services.MessageLoop;
using MessageLoop.Node.Services.Schedule;
using MessageLoop.Web.Common.Code.Filters;

using Microsoft.AspNetCore.Mvc;

namespace MessageLoop.Node.Controllers.v1
{
    [ApiController, ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/node")]
    [Produces(MediaTypeNames.Application.Json)]
    public class NodeController : ControllerBase
    {
        private readonly ILongRunService<LongRunItem> _service;
        private readonly IScheduleService _schedule;
        private readonly IMessageLoopService _message;

        public NodeController(
            ILongRunService<LongRunItem> service,
            IMessageLoopService message,
            IScheduleService schedule)
        {
            _service = service;
            _schedule = schedule;
            _message = message;
        }

        [HttpPost("onSelf")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LongRun(Description = "Long run operation on the self")]
        public IActionResult RunOnSelf()
        {
            var host = Request.Host.Value;
            var token = _service.PutTask(async (cncl) =>
            {
                using var source = CancellationTokenSource.CreateLinkedTokenSource(cncl);
                await _message.RunMessageLoop(nameof(RunOnSelf), source);

                return $"Operation completed on {host}"; 
            });

            return Ok(token);
        }

        [HttpPost("onNode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LongRun(Description = $"Long run operation on a target node")]
        public IActionResult RunOnNode()
        {
            var host = Request.Host.Value;
            var token = _service.PutTask(async (cncl) =>
            {
                var childTokens = await _schedule.RunOnChildNodes();
                var results = await _schedule.PollChildNodes(childTokens, cncl);

                return $"Operation completed on {host}".BuildDataTree(results);
            });

            return Ok(token);
        }
    }
}


