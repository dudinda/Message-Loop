using System.Net.Mime;

using Asp.Versioning;

using MessageLoop.Service.Services.Message;
using MessageLoop.Web.Common.Code.Constants;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessageLoop.Web.Common.Controllers.v1
{
    [ApiController, ApiVersion("1.0")]
    [Route("api/v{{version:apiVersion}}/{0}")]
    [Produces(MediaTypeNames.Application.Json)]
    public class MessageController<TEnum> : ControllerBase where TEnum : Enum
    {
        private readonly IMessageService<TEnum> _message;

        public MessageController(IMessageService<TEnum> message)
        {
            _message = message;
        }

        [HttpPatch("{loopKey}/{message}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Unicast(string loopKey, string message)
        {
            if (!_message.Contains(loopKey))
            {
                return Problem(string.Format(NotFoundDetails.MessageLoopsNotFound, loopKey),
                    statusCode: StatusCodes.Status404NotFound);
            }

            if (!Enum.TryParse(typeof(TEnum), message, true, out var @enum))
            {
                return Problem(string.Format(NotFoundDetails.MessageNotFound, message),
                    statusCode: StatusCodes.Status404NotFound);
            }
           
            _message.SendMessage(loopKey, (TEnum)@enum);
            return NoContent();
        }

        [HttpPut("{message}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Broadcast(string message)
        {
            if (!Enum.TryParse(typeof(TEnum), message, true, out var @enum))
            {
                return Problem(string.Format(NotFoundDetails.MessageNotFound, message),
                    statusCode: StatusCodes.Status404NotFound);
            }

            foreach (var loopKey in _message.LoopKeys)
            {
                _message.SendMessage(loopKey, (TEnum)@enum);
            }
            return NoContent();
        }
    }
}
