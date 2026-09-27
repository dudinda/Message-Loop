namespace MessageLoop.Node.Services.MessageLoop
{
    public interface IMessageLoopService
    {
        Task RunMessageLoop(string key, CancellationToken source);
    }
}
