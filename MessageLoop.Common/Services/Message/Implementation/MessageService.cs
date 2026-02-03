using System.Collections.Concurrent;

namespace MessageLoop.Service.Services.Message.Implementation
{
    public class MessageService<TEnum> : IMessageService<TEnum> where TEnum : Enum
    {
        private readonly ConcurrentDictionary<string, List<BlockingCollection<TEnum>>> _msgLoops = new();

        /// <inheritdoc />
        public void SendMessage(string key, TEnum message)
        {
            if (!_msgLoops.TryGetValue(key, out var msgLoop))
            {
                throw new InvalidOperationException($"Message loop by the {key} is not found.");
            }

            lock (_msgLoops)
            {
                for (var i = 0; i < msgLoop.Count; ++i)
                {
                    if (!msgLoop[i].IsAddingCompleted)
                    {
                        msgLoop[i].Add(message);
                    }
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
        public void Add(string key, BlockingCollection<TEnum> value)
        {
            lock (_msgLoops)
            {
                _msgLoops.GetOrAdd(key, (k) => new List<BlockingCollection<TEnum>>() ).Add(value);
            }
        }

        /// <inheritdoc />
        public bool TryRemove(string key, BlockingCollection<TEnum> value)
        {
            lock (_msgLoops)
            {
                var msgLoop = _msgLoops.GetOrAdd(key, (k) =>  new List<BlockingCollection<TEnum>>() );

                msgLoop.Remove(value);

                if (msgLoop.Count == 0)
                {
                    return _msgLoops.Remove(key, out msgLoop);
                }
            }

            return false;
        }

        /// <summary>
        /// Release all the waiting threads.
        /// Used by a DI-container in a singleton scope.
        /// </summary>
        public void Dispose()
        {
            lock (_msgLoops)
            {
                _msgLoops.Clear();
            }
        }
    }
}
