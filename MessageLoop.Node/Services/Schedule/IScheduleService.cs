using MessageLoop.Common.Models.LongRun;

namespace MessageLoop.Node.Services.Schedule
{
    public interface IScheduleService
    {
        Task<List<LongRunToken>> RunOnChildNodes();
        Task<List<LongRunResult>> PollChildNodes(
            IEnumerable<LongRunToken> tokens, CancellationToken cncl);
    }
}
