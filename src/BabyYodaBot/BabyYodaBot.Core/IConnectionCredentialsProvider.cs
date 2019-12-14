using TwitchLib.Client.Models;

namespace BabyYodaBot.Core
{
    public interface IConnectionCredentialsProvider
    {
        ConnectionCredentials Get();
    }
}