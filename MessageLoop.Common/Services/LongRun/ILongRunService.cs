using MessageLoop.Common.Models.LongRun;

namespace MessageLoop.Common.Services.LongRun
{
    public interface ILongRunService<T> where T : LongRunItem, new()
    {
        LongRunToken PutTask(Func<CancellationToken, Task<object>> task);
        bool TryGetResult(Guid id, out LongRunResult result);
        void Abort(LongRunToken id);
        List<T> Items { get; }
    }
}
