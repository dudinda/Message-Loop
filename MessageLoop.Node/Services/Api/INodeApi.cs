using MessageLoop.Common.Models.LongRun;

using Refit;

namespace MessageLoop.Node.Services.Api
{
    public interface INodeApi
    {
        [Post("/api/v{ver}/node/onSelf")]
        Task<IApiResponse<LongRunToken>> RunOnSelf([AliasAs("ver")] int version = 1);

        [Post("/api/v{ver}/node/onNode")]
        Task<IApiResponse<LongRunToken>> RunOnNode([AliasAs("ver")] int version = 1);
    }
}
