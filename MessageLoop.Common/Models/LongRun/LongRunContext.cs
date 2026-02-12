namespace MessageLoop.Common.Models.LongRun
{
    public record LongRunContext
    {
        public AsyncLocal<string> Id = new AsyncLocal<string>();
        public AsyncLocal<string> Description = new AsyncLocal<string>();
        public AsyncLocal<CancellationTokenSource> Cancellation = new AsyncLocal<CancellationTokenSource>();
    }
}
