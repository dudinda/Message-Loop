namespace MessageLoop.Node.Services.MessageLoop
{
    public interface IMessageLoopService
    {
        /// <summary>
        /// Run a message loop with the given key and cancellation token. 
        /// The loop will continue until the cancellation token is triggered or the loop times out.
        /// </summary>
        Task RunMessageLoop(string key, CancellationToken source);
    }
}
