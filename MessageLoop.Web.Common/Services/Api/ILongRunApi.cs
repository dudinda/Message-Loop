using MessageLoop.Common.Models.LongRun;

using Refit;

namespace MessageLoop.Master.Services.Api
{
    public interface ILongRunApi<T> where T : LongRunItem, new()
    {
        [Get("api/v{ver}/{tokenId}")]
        Task<LongRunResult> GetLongRunStatus([AliasAs("tokenId")]Guid id, [AliasAs("ver")]int version = 1);

        [Get("api/v{ver}")]
        Task<List<T>> GetAll([AliasAs("ver")] int version = 1);

        [Patch("api/v{ver}/abort/{tokenId}")]
        Task Abort([AliasAs("tokenId")] Guid id, [AliasAs("ver")] int version = 1);
    }
}
