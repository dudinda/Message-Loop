using MessageLoop.Node.Models;

using Microsoft.Extensions.Options;

namespace MessageLoop.Node.Services.Schedule.Implementation
{
    public class ScheduleService : IScheduleService
    {
        private readonly IOptions<LongRunOptions > _options;
        private readonly ILogger<ScheduleService> _logger;

        public ScheduleService(
            ILogger<ScheduleService> logger,
            IOptions<LongRunOptions> options)
        {
            _options = options;
            _logger = logger;
        }


        public void ScheduleToNodes()
        {
           
        }
    }
}
