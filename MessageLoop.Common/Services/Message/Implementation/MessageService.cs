using System.Threading.Channels;

namespace MessageLoop.Service.Services.Message.Implementation
{
    public class MessageService<TMessage> : IMessageService<TMessage> 
    {
        private readonly object _lock = new object();
        private readonly Dictionary<string, List<Channel<TMessage>>> _msgLoops = new();

        public IEnumerable<string> LoopKeys
        {
            get
            {
                lock(_lock)
                {
                    return _msgLoops.Keys.ToArray();
                }
            }
        }

        /// <inheritdoc />
        public void SendMessage(string key, TMessage message)
        {
            Channel<TMessage>[] channels;
            lock(_lock)
            {
                if (!_msgLoops.TryGetValue(key, out var msgLoop))
                {
                    throw new InvalidOperationException($"Message loops with the {key} could not be found.");
                }
                channels = msgLoop.ToArray();
            }

            foreach (var channel in channels)
            {
                channel.Writer.TryWrite(message);
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
                if(!_msgLoops.TryGetValue(key, out var msgLoop))
                {
                    msgLoop = new List<Channel<TMessage>>();
                    _msgLoops.Add(key, msgLoop);
                }
                msgLoop.Add(value);
            }
        }

        /// <inheritdoc />
        public bool TryRemove(string key, Channel<TMessage> value)
        {
            lock (_msgLoops)
            {
                if(!_msgLoops.TryGetValue(key, out var msgLoop))
                {
                    return false;
                }

                if(!msgLoop.Remove(value))
                {
                    return false;
                }

                if (msgLoop.Count == 0)
                {
                    return _msgLoops.Remove(key, out msgLoop);
                }
            }

            value.Writer.TryComplete();
            return true;
        }

        public void Dispose()
        {
            Channel<TMessage>[] channels;
            lock (_msgLoops)
            {
                channels = _msgLoops.Values.SelectMany(_ => _).ToArray();
                _msgLoops.Clear();
            }
            
            foreach(var channel in channels)
            {
                channel.Writer.TryComplete();
            }
        }
    }
}
