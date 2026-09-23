namespace MessageLoop.Node.Services.MessageLoop
{
    public interface IMessageLoopService
    {
        Task RunMessageLoop(string key, CancellationTokenSource source);
    }
}
