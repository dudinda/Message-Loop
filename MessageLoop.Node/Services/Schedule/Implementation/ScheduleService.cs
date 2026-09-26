using MessageLoop.Common.Models.LongRun;
using MessageLoop.Node.Models;
using MessageLoop.Node.Services.Api;
using MessageLoop.Web.Common.Code.Extensions;
using MessageLoop.Web.Common.Services.Api;

using Microsoft.Extensions.Options;

using Refit;

namespace MessageLoop.Node.Services.Schedule.Implementation
{
    public class ScheduleService : IScheduleService
    {
        private readonly ILogger<ScheduleService> _logger;
        private readonly IOptions<NodeOptions> _options;
        private readonly Dictionary<Guid, ILongRunApi<LongRunItem>> _map = new();

        public ScheduleService(
            ILogger<ScheduleService> logger,
            IOptions<NodeOptions> options)
        {
            _logger = logger;
            _options = options;
        }


        public async Task<List<LongRunToken>> RunOnChildNodes()
        {
            var apis = _options.Value.ChildNodes.ToDictionary(
                key => key, RestService.For<INodeApi>);
            var result = new List<LongRunToken>();
            foreach (var kv in apis)
            {
                var api = kv.Value;
                var url = kv.Key;
                var response = await api.RunOnNode();
                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException(response.Error.Message);
                }
                result.Add(response.Content);
                _map.Add(response.Content.Id, RestService.For<ILongRunApi<LongRunItem>>(url));

            }
            return result;
        }

        public async Task<List<LongRunResult>> PollChildNodes(
            IEnumerable<LongRunToken> tokens, CancellationToken cncl)
        {
            var tasks = new List<Task<LongRunResult>>();
            foreach (var token in tokens)
            {
                var api = _map[token.Id];
                tasks.Add(api.Poll(token, cncl));
            }
            await Task.WhenAll(tasks);
            return tasks.Select(t => t.Result).ToList();
        }
    }
}
