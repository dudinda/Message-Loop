using System.Collections.Concurrent;
using System.Threading.Channels;

namespace MessageLoop.Service.Services.Message.Implementation
{
    public class MessageService<TMessage> : IMessageService<TMessage> 
    {
        private readonly ConcurrentDictionary<string, List<Channel<TMessage>>> _msgLoops = new();

        public IEnumerable<string> LoopKeys { get => _msgLoops.Keys; }

        /// <inheritdoc />
        public void SendMessage(string key, TMessage message)
        {
            if (!_msgLoops.TryGetValue(key, out var msgLoop))
            {
                throw new InvalidOperationException($"Message loops with the {key} could not be found.");
            }

            lock (_msgLoops)
            {
                foreach (var channel in msgLoop)
                {
                    channel.Writer.TryWrite(message);
                }
            }
        }

        public bool Contains(string key)
        {
            lock (_msgLoops)
            {
                return _msgLoops.ContainsKey(key);
            }
        }

        /// <inheritdoc />
        public void Add(string key, Channel<TMessage> value)
        {
            lock (_msgLoops)
            {
                _msgLoops.GetOrAdd(key, (k) => new List<Channel<TMessage>>() ).Add(value);
            }
        }

        /// <inheritdoc />
        public bool TryRemove(string key, Channel<TMessage> value)
        {
            lock (_msgLoops)
            {
                var msgLoop = _msgLoops.GetOrAdd(key, (k) =>  new List<Channel<TMessage>>() );

                msgLoop.Remove(value);

                if (msgLoop.Count == 0)
                {
                    return _msgLoops.Remove(key, out msgLoop);
                }
            }

            return false;
        }

        public void Dispose()
        {
            lock (_msgLoops)
            {
                _msgLoops.Clear();
            }
        }
    }
}
