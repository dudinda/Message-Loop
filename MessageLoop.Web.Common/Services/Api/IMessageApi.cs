using Refit;

namespace MessageLoop.Web.Common.Services.Api
{
    public interface IMessageApi
    {
        /// <summary>
        /// Send a <paramref name="message"/> to the corresponding event loop.
        /// If several loops are running under the same constant <paramref name="loopKey"/>,
        /// the action acts as a multicast and sends the message  to all of them.
        /// <para>See <see cref="Controllers.v1.MessageController{Enum}.Unicast(string, string)"/></para>
        /// </summary>
        [Patch("/api/v{ver}/{enumName}/{loopKey}/{message}")]
        Task<IApiResponse> Unicast(
            [AliasAs("enumName")] string enumName,
            [AliasAs("loopKey")]string loopKey,
            [AliasAs("message")]string message,
            [AliasAs("ver")] int version = 1);

        /// <summary>
        /// Broadcast a <paramref name="message"/> to all event loops.
        /// <para>See <see cref="Controllers.v1.MessageController{Enum}.Broadcast(string)" /></para>
        /// </summary>
        [Put("/api/v{ver}/{enumName}/{message}")]
        Task<IApiResponse> Broadcast(
            [AliasAs("enumName")] string enumName,
            [AliasAs("message")]string message, 
            [AliasAs("ver")] int version = 1);
    }
}
