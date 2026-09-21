using MessageLoop.Common.Models.LongRun;

using Refit;

namespace MessageLoop.Web.Common.Services.Api
{
    public interface ILongRunApi<T> where T : LongRunItem, new()
    {
        [Get("/api/v{ver}/longrun/{tokenId}")]
        Task<IApiResponse<LongRunResult>> GetLongRunStatus([AliasAs("tokenId")]Guid id, [AliasAs("ver")]int version = 1);

        [Get("/api/v{ver}/longrun")]
        Task<IApiResponse<List<T>>> GetAll([AliasAs("ver")] int version = 1);

        [Patch("/api/v{ver}/longrun/abort/{tokenId}")]
        Task<IApiResponse> Abort([AliasAs("tokenId")] Guid id, [AliasAs("ver")] int version = 1);
    }
}
