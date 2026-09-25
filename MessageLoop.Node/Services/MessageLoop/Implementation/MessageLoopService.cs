using System.Collections.Concurrent;

using MessageLoop.Node.Models.Options;
using MessageLoop.Service.Services.Message;
using MessageLoop.Web.Common.Code.Enums;

using Microsoft.Extensions.Options;

namespace MessageLoop.Node.Services.MessageLoop.Implementation
{
    public class MessageLoopService : IMessageLoopService
    {
        private readonly IMessageService<Messages> _service;
        private readonly IOptions<MessageLoopOptions> _options;
        private readonly ILogger<MessageLoopService> _logger;

        public MessageLoopService(
            IMessageService<Messages> service,
            IOptions<MessageLoopOptions> options,
            ILogger<MessageLoopService> logger)
        {
            _service = service;
            _options = options;
            _logger = logger;
        }

        public async Task RunMessageLoop(string key, CancellationTokenSource source)
        {
            var queue = new BlockingCollection<Messages>();
            _service.Add(key, queue);

            var opt = _options.Value;
            try
            {
                var parentToken = source.Token;
                parentToken.ThrowIfCancellationRequested();
                using (var cancel = CancellationTokenSource.CreateLinkedTokenSource(parentToken))
                {
                    cancel.CancelAfter(opt.LoopTimeoutMs);
                    var token = cancel.Token;

                    int failsCount = 0;
                    var maxFails = opt.MaxFails;
                    try
                    {
                        while (!queue.IsCompleted)
                        {
                            var msg = queue.Take(token);
                            _logger.LogInformation($"Processing message: {msg}");
                            switch (msg)
                            {
                                case Messages.Fail:
                                    ++failsCount;
                                    break;

                                case Messages.Message_1:
                                    break;
                                case Messages.Message_2:
                                    break;
                                case Messages.Message_3:
                                    break;
                                case Messages.Message_1 | Messages.Message_2:
                                    break;
                                case Messages.Message_1 | Messages.Message_3:
                                    break;
                                case Messages.Message_2 | Messages.Message_3:
                                    break;
                                case Messages.Message_1 | Messages.Message_2 | Messages.Message_3:
                                    break;


                                case Messages.Cancel:
                                    source.Cancel();
                                    break;
                                case Messages.Ok:
                                    queue.CompleteAdding();
                                    break;
                            }

                            if (maxFails  > 0 && failsCount >= maxFails)
                            {
                                throw new Exception($"Too many fails inside the loop.");
                            }

                            await Task.Delay(opt.LoopFrequencyMs, token);
                        }
                    }
                    catch (OperationCanceledException e)
                    {
                        if (source.IsCancellationRequested)
                        {
                            throw;
                        }

                        throw new TimeoutException($"Timeout of the loop {key} expired.", e);
                    }
                }
            }
            finally
            {
                _service.TryRemove(key, queue);
            }
        }
    }
}
