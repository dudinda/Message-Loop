using MessageLoop.Common.Models.LongRun;
using MessageLoop.Web.Common.Services.Api;

using Refit;

namespace MessageLoop.Node.Services.Api
{
    public interface INodeApi : ILongRunApi<LongRunItem>
    {
        /// <summary>
        /// Start a long-running operation on the current node.
        /// The operation runs a message loop.
        ///  <para>See <see cref="Controllers.v1.NodeController.RunOnSelf)"/></para>
        /// </summary>
        [Post("/api/v{ver}/node/onSelf")]
        Task<IApiResponse<LongRunToken>> RunOnSelf([AliasAs("ver")] int version = 1);

        /// <summary>
        /// Start a long-running operation on the current node and all child nodes.
        /// Upon completion, the result shows the exact hierarchy of all external background operations.
        /// <para>See <see cref="Controllers.v1.NodeController.RunOnNode)"/></para>
        /// </summary>
        [Post("/api/v{ver}/node/onNode")]
        Task<IApiResponse<LongRunToken>> RunOnNode([AliasAs("ver")] int version = 1);
    }
}
