using Refit;

namespace MessageLoop.Web.Common.Services.Api
{
    public interface IMessageApi
    {
        [Patch("/api/v{ver}/message/{loopKey}/{message}")]
        Task<IApiResponse> Unicast(
            [AliasAs("loopKey")]string loopKey,
            [AliasAs("message")]string message,
            [AliasAs("ver")] int version = 1);

        [Put("/api/v{ver}/message/{message}")]
        Task<IApiResponse> Broadcast(
            [AliasAs("message")]string message, 
            [AliasAs("ver")] int version = 1);
    }
}
