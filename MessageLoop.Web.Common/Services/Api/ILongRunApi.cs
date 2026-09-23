using MessageLoop.Common.Models.LongRun;

using Refit;

namespace MessageLoop.Web.Common.Services.Api
{
    public interface ILongRunApi<T> where T : LongRunItem, new()
    {
        /// <summary>
        /// Try to get the status of a long run operation by its token ID.
        /// <para>See <see cref="Controllers.v1.LongRunController.GetLongRunStatus(Guid)"/></para>
        /// </summary>
        [Get("/api/v{ver}/longrun/{tokenId}")]
        Task<IApiResponse<LongRunResult>> GetLongRunStatus([AliasAs("tokenId")]Guid id, [AliasAs("ver")]int version = 1);

        /// <summary>
        /// List all long run operations that are currently in progress or have completed.
        /// <para>See <see cref="Controllers.v1.LongRunController.GetAll"/></para>
        /// </summary>
        [Get("/api/v{ver}/longrun")]
        Task<IApiResponse<List<T>>> GetAll([AliasAs("ver")] int version = 1);

        /// <summary>
        /// Abort a long run operation by its token ID.
        /// <para>See <see cref="Controllers.v1.LongRunController.Abort(Guid)"/></para>
        /// </summary>
        [Patch("/api/v{ver}/longrun/abort/{tokenId}")]
        Task<IApiResponse> Abort([AliasAs("tokenId")] Guid id, [AliasAs("ver")] int version = 1);
    }
}
