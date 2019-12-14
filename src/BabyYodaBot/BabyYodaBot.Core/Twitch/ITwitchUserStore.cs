namespace BabyYodaBot.Core.Twitch
{
    public interface ITwitchUserStore
    {
        ITwitchUser Get(string username);
    }
}