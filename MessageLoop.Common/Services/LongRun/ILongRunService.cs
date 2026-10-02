using MessageLoop.Common.Models.LongRun;

namespace MessageLoop.Common.Services.LongRun
{
    public interface ILongRunService<T> where T : LongRunItem, new()
    {
        /// <summary>
        /// Start a long-running scenario. The task will be executed in the background,
        /// and the caller can use the returned token to check its status or retrieve the result later.
        /// </summary>
        LongRunToken PutTask(Func<CancellationToken, Task<object>> task);

        /// <summary>
        ///  Try to get the result of a long-running operation by its ID.
        /// </summary>
        bool TryGetResult(Guid id, out LongRunResult result);

        /// <summary>
        /// Abort a long-running operation.
        /// </summary>
        void Abort(LongRunToken id);

        /// <summary>
        /// List all long-running operations that are currently in progress.
        /// </summary>

        List<T> Items { get; }
    }
}
