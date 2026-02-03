namespace MessageLoop.Common.Models.LongRun
{
    public class LongRunResult
    {
        public TaskStatus Status { get; set; }
        public string Exception { get; set; }
        public object Data { get; set; }
    }
}
