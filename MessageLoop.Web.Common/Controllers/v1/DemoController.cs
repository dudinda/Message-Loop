using System.Net.Mime;

using Asp.Versioning;

using MessageLoop.Common.Models.LongRun;
using MessageLoop.Common.Services.LongRun;
using MessageLoop.Web.Common.Code.Filters;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessageLoop.Web.Common.Controllers.v1
{
    [ApiController, ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/demo")]
    [Produces(MediaTypeNames.Application.Json)]
    public class DemoController : ControllerBase
    {
        private readonly ILongRunService<LongRunItem> _service;

        public DemoController(ILongRunService<LongRunItem> service)
        {
            _service = service;
        }

        [HttpPost("onSelf")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [LongRun(Description = "Long run operation on the self")]
        public IActionResult GetLongRunStatus(Guid tokenId)
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
    }
}


