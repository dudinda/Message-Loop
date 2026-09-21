using MessageLoop.Common.Models.LongRun;

using Refit;

namespace MessageLoop.Node.Services.Api
{
    public interface INodeApi
    {
        [Post("api/v{ver}/demo")]
        Task<IApiResponse<LongRunToken>> StartLongRunOnSelf([AliasAs("ver")] int version = 1);

        [Post("api/v{ver}/demo")]
        Task<IApiResponse<LongRunToken>> StartLongRunOnNode([AliasAs("ver")] int version = 1);
    }
}
