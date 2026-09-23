using System.Text.Json.Serialization;

namespace MessageLoop.Common.Models.LongRun
{
    public record LongRunItem
    {
        public Guid Id { get; internal set; } = Guid.NewGuid();

        public string Description { get; internal set; }

        [JsonIgnore]
        public CancellationTokenSource Cancellation { get; private set; } = new CancellationTokenSource();

        [JsonIgnore]
        public Task<object> Execution { get; internal set; }
    }
}
