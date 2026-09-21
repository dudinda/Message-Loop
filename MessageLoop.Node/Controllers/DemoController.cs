using System.Net.Mime;

using Asp.Versioning;

using MessageLoop.Common.Models.LongRun;
using MessageLoop.Common.Services.LongRun;
using MessageLoop.Node.Services.Schedule;
using MessageLoop.Web.Common.Code.Filters;

using Microsoft.AspNetCore.Mvc;

namespace MessageLoop.Node.Controllers.v1
{
    [ApiController, ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/demo")]
    [Produces(MediaTypeNames.Application.Json)]
    public class DemoController : ControllerBase
    {
        private readonly ILongRunService<LongRunItem> _service;
        private readonly IScheduleService _schedule;

        public DemoController(
            ILongRunService<LongRunItem> service,
            IScheduleService schedule)
        {
            _service = service;
            _schedule = schedule;
        }

        [HttpPost("onSelf")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LongRun(Description = "Long run operation on the self")]
        public IActionResult StartLongRunOnSelf()
        {
            var token = _service.PutTask(async (cncl) =>
            {
                using (var child = CancellationTokenSource.CreateLinkedTokenSource(cncl))
                {
                    child.CancelAfter(TimeSpan.FromSeconds(300));
                    var token = child.Token;
                    while (!token.IsCancellationRequested)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5), token);
                    }
                    token.ThrowIfCancellationRequested();
                }
                return 0;
            });

            return Ok(token);
        }

        [HttpPost("onNode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [LongRun(Description = $"Long run operation on a target node")]
        public IActionResult StartLongRunOnNode()
        {
            var token = _service.PutTask(async (cncl) =>
            {
                return 0;
            });

            return Ok(token);
        }
    }
}


