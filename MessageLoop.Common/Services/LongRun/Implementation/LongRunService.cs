using MessageLoop.Common.Models.LongRun;

using Microsoft.Extensions.Logging;

namespace MessageLoop.Common.Services.LongRun.Implementation
{
    public class LongRunService<T> : ILongRunService<T> where T : LongRunItem, new()
    {
        private readonly Dictionary<Guid, T> _running = new();
        private readonly Dictionary<Guid, T> _end = new();
        private readonly LongRunContext _context;
        private readonly ILogger<LongRunService<T>> _logger;

        public LongRunService(
            ILogger<LongRunService<T>> logger,
            LongRunContext context)
        {
            _context = context;
            _logger = logger;
        }

        public List<T> Items => _running.Values.ToList();

        public bool TryGetResult(Guid id, out LongRunResult result)
        {
            result = new LongRunResult();

            if (!_end.ContainsKey(id))
            {
                return false;
            }
            
            lock (_end)
            {
                var t = _end[id];

                result.Status = t.Execution.Status;
                result.Exception = t.Execution.Exception?.ToString();

                if (result.Status == TaskStatus.RanToCompletion)
                {
                    result.Data = t.Execution.Result;
                }

                if (result.Status == TaskStatus.RanToCompletion ||
                    result.Status == TaskStatus.Faulted ||
                    result.Status == TaskStatus.Canceled)
                {
                    return _end.Remove(id);
                }
            }

            return true;
        }

        public LongRunToken PutTask(Func<CancellationToken, Task<object>> work)
        {
            if (!Guid.TryParse(_context.Id?.Value, out var id))
            {
                id = Guid.NewGuid();
            }

            var result = new LongRunToken(id);
            
            lock (_running)
            {
                var item = new T() { Id = result.Id };
                item.Description ??= _context.Description?.Value;

                var workTask = Task.Run(async () =>
                {
                    try
                    {
                        _context.Cancellation.Value = item.Cancellation;

                        using (item.Cancellation)
                        {
                            return await work(_context.Cancellation.Value.Token).ConfigureAwait(false);
                        }
                    }
                    catch(OperationCanceledException ex)
                    {
                        _logger.LogInformation( $"Operation {id} - {item.Description} was cancelled.");
                        throw;
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                        throw;
                    }
                    finally
                    {
                        lock (_running)
                        {
                            _running.Remove(result.Id);
                        }
                    }
                });

                item.Execution = workTask;

                _running.Add(result.Id, item);

                lock (_end)
                {
                    _end.Add(result.Id, item);
                }
            }

            return result;
        }

        public void Abort(LongRunToken token)
        {
            if (_running.ContainsKey(token.Id) &&
               !_running[token.Id].Cancellation.IsCancellationRequested)
            {
                _running[token.Id].Cancellation.Cancel();
            }
        }
    }
}
