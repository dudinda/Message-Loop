using Refit;

namespace MessageLoop.Web.Common.Services.Api
{
    public interface IMessageApi
    {
        [Patch("/api/v{ver}/{enumName}/{loopKey}/{message}")]
        Task<IApiResponse> Unicast(
            [AliasAs("enumName")] string enumName,
            [AliasAs("loopKey")]string loopKey,
            [AliasAs("message")]string message,
            [AliasAs("ver")] int version = 1);

        [Put("/api/v{ver}/{enumName}/{message}")]
        Task<IApiResponse> Broadcast(
            [AliasAs("enumName")] string enumName,
            [AliasAs("message")]string message, 
            [AliasAs("ver")] int version = 1);
    }
}
