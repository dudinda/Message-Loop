using System.Net.Mime;

using Asp.Versioning;

using Microsoft.AspNetCore.Mvc;

namespace MessageLoop.Master.Controllers.v1
{
    [ApiController, ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/demo")]
    [Produces(MediaTypeNames.Application.Json)]
    public class DemoController
    {

    }
}
