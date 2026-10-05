using MessageLoop.Common.Models.LongRun;

namespace MessageLoop.Node.Services.Schedule
{
    public interface IScheduleService
    {
        /// <summary>
        /// Runs the long-running task on child nodes and returns a list of tokens representing the tasks.
        /// </summary>
        Task<List<LongRunToken>> RunOnChildNodes();

        /// <summary>
        /// Polls the child nodes for the status of the long-running tasks represented by the provided tokens.
        /// </summary>
        Task<List<LongRunResult>> PollChildNodes(
            IEnumerable<LongRunToken> tokens, CancellationToken cncl);
    }
}
