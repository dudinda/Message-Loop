using System.Threading.Channels;

namespace MessageLoop.Service.Services.Message
{
    /// <summary>
	/// Provides a service that allows to send messages to a waiting thread via a channel.
	/// </summary>
    public interface IMessageService<TMessage> : IDisposable
    {
        IEnumerable<string> LoopKeys { get; }

        /// <summary>
        /// Try to remove a channel <paramref name="value"/> 
        /// defined by the given <paramref name="key"/>.
        /// </summary>
        bool TryRemove(string key, Channel<TMessage> value);

        /// <summary>
        /// Try to add a new channel <paramref name="value"/>
        /// defined by the given <paramref name="key"/>.
        /// </summary>
        void Add(string key, Channel<TMessage> value);

        /// <summary>
        /// Determine whether a waiting channel by the given <paramref name="key"/> exists.
        /// </summary>
        bool Contains(string key);

        /// <summary>
        /// Send a <see cref="TEnum"/> message to a waiting channel defined by the <paramref name="key"/>.
        /// </summary>
        void SendMessage(string key, TMessage message);
    }
}
