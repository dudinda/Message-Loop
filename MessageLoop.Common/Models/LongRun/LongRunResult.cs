using System.Text.Json.Serialization;

namespace MessageLoop.Common.Models.LongRun
{
    public class LongRunResult
    {
        [JsonInclude]
        public TaskStatus Status { get; internal set; }

        [JsonInclude]
        public string Exception { get; internal set; }

        [JsonInclude]
        public object Data { get; internal set; }

    }
}
