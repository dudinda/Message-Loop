using System.Collections.Concurrent;

namespace MessageLoop.Service.Services.Message
{
    /// <summary>
	/// Provides a service that allows to send messages to a waiting thread via a waiting queue.
	/// </summary>
    public interface IMessageService<TEnum> where TEnum : Enum
    {
        /// <summary>
        /// Try to remove a watiing <paramref name="value"/> queue
        /// defined by the given <paramref name="key"/>.
        /// </summary>
        bool TryRemove(string key, BlockingCollection<TEnum> value);

        /// <summary>
        /// Try to add a new waiting queue <paramref name="value"/>
        /// defined by the given <paramref name="key"/>.
        /// </summary>
        void Add(string key, BlockingCollection<TEnum> value);

        /// <summary>
        /// Determines whether a waiting queue defined by the given <paramref name="key"/>.
        /// </summary>
        bool Contains(string key);

        /// <summary>
        /// Send a <see cref="TEnum"/> message to a waiting queue defined by the <paramref name="key"/>.
        /// </summary>
        void SendMessage(string key, TEnum message);
    }
}
