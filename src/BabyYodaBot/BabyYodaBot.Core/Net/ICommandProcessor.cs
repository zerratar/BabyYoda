using System;
using System.Threading.Tasks;
using BabyYodaBot.Core.Handlers;

namespace BabyYodaBot.Core.Net
{
    public abstract class CommandProcessor : ICommandProcessor
    {
        public virtual void Dispose()
        {
        }

        public abstract Task ProcessAsync(IMessageBroadcaster broadcaster, ICommand cmd);
    }

    public interface ICommandProcessor : IDisposable
    {
        Task ProcessAsync(IMessageBroadcaster broadcaster, ICommand cmd);
    }
}