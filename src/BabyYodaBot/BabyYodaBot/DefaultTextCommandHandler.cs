using BabyYodaBot.Core;
using BabyYodaBot.Core.Handlers;
using BabyYodaBot.Core.BabyYoda.Commands;
using BabyYodaBot.Commands.Games.BabyYoda;

namespace BabyYodaBot
{
    public class DefaultTextCommandHandler : TextCommandHandler
    {
        public DefaultTextCommandHandler(IoC ioc)
            : base(ioc)
        {
            Register<FeedCommandProcessor>("feed");
        }
    }
}
