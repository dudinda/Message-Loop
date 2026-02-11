using System.Net.Mime;

using Asp.Versioning;

using Microsoft.AspNetCore.Mvc;

namespace MessageLoop.Web.Common.Controllers.v1
{
    [ApiController, ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/message")]
    [Produces(MediaTypeNames.Application.Json)]
    internal class MessageController
    {
        
    }
}
