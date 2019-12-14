using System.Collections.Generic;

namespace BabyYodaBot.Core.Net.WebSocket
{
    public interface IConnectionProvider
    {
        IReadOnlyList<IConnection> GetAll();
        IConnection Get(System.Net.WebSockets.WebSocket socket);
    }
}