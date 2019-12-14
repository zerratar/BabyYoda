using System.Threading.Tasks;
using BabyYodaBot.Core.Net;

namespace BabyYodaBot.Core.Handlers
{
    public interface ICommandHandler
    {
        Task HandleAsync(IMessageBroadcaster listener, ICommand cmd);
        void Register<TCommandProcessor>(string cmd, params string[] aliases) 
            where TCommandProcessor : ICommandProcessor;        
    }
}