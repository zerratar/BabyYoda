using BabyYodaBot.Core;

namespace BabyYodaBot
{
    public class DefaultChannelProvider : IChannelProvider
    {
        private readonly IAppSettings settings;

        public DefaultChannelProvider(IAppSettings appSettings)
        {
            this.settings = appSettings;
        }
        public string Get()
        {
            return settings.TwitchChannel;
        }
    }
}