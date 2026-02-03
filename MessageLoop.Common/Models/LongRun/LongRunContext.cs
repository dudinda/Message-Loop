namespace MessageLoop.Common.Models.LongRun
{
    public record LongRunContext
    {
        public AsyncLocal<CancellationTokenSource> Cancellation = new AsyncLocal<CancellationTokenSource>();
        public AsyncLocal<string> OperationId = new AsyncLocal<string>();
        public AsyncLocal<string> OperationDescription = new AsyncLocal<string>();
    }
}
