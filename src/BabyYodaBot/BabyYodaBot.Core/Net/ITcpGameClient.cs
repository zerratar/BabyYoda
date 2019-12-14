using System;
using System.Threading.Tasks;

namespace BabyYodaBot.Core.Net
{
    public interface ITcpGameClient
    {
        Task ProcessAsync();
        IGameClientSubcription Subscribe(string cmdIdentifier, Action<IGameCommand> onCommand);
    }
}