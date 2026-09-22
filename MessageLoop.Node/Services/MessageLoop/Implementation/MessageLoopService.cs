using System.Collections.Concurrent;

using MessageLoop.Common.Models.LongRun;
using MessageLoop.Node.Models;
using MessageLoop.Service.Services.Message;
using MessageLoop.Web.Common.Code.Enums;

using Microsoft.Extensions.Options;

namespace MessageLoop.Node.Services.MessageLoop.Implementation
{
    public class MessageLoopService : IMessageLoopService
    {
        private readonly IMessageService<Messages> _service;
        private readonly IOptions<MessageLoopOptions> _options;
        private readonly LongRunContext _context;

        public MessageLoopService(
            IMessageService<Messages> service,
            IOptions<MessageLoopOptions> options,
            LongRunContext context)
        {
            _service = service;
            _context = context;
            _options = options;
        }

        public async Task RunMessageLoop(string key)
        {
            var queue = new BlockingCollection<Messages>();
            _service.Add(key, queue);

            var opt = _options.Value;
            try
            {
                var source = _context.Cancellation.Value;
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
                        while (queue.IsCompleted)
                        {
                            var msg = queue.Take(token);

                            switch (msg)
                            {
                                case var code when (code & Messages.Ok) != 0:
                                    queue.CompleteAdding();
                                    break;
                                case var code when (code & Messages.Fail) != 0:
                                    ++failsCount;
                                    break;
                                case var code when (code & Messages.Cancel) != 0:
                                    source.Cancel();
                                    break;

                                case var code when (code & Messages.Message_1) != 0:
                                    break;
                                case var code when (code & Messages.Message_2) != 0:
                                    break;
                                case var code when (code & Messages.Message_1 | Messages.Message_2) != 0:
                                    break;
                                case var code when (code & Messages.Message_2 | Messages.Message_3) != 0:
                                    break;
                            }

                            if (maxFails  > 0 && failsCount >= maxFails)
                            {
                                throw new Exception($"Too many fails inside the loop.");
                            }

                            await Task.Delay(opt.LoopFrequencyMs);
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
