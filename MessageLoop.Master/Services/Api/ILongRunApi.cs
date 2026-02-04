using MessageLoop.Common.Models.LongRun;

using Refit;

namespace MessageLoop.Master.Services.Api
{
    public interface ILongRunApi
    {
        [Get("api/v{version}/{tokenId}")]
        Task<LongRunResult> GetLongRunStatus([AliasAs("tokenId")]Guid id, [AliasAs("v")]int version = 1);
    }
}
