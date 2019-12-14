using BabyYodaBot.Core.Handlers;
using BabyYodaBot.Core.BabyYoda.Models;

namespace BabyYodaBot.Core.BabyYoda.Commands
{
    public interface IPlayerProvider
    {
        Player Get(string userId, string username);
        Player Get(ICommandSender sender);
        Player Get(string username);
    }

    public class PlayerProvider : IPlayerProvider
    {
        public Player Get(ICommandSender sender)
        {
            return new Player(
                sender.UserId,
                sender.Username,
                sender.DisplayName,
                sender.ColorHex,
                sender.IsBroadcaster,
                sender.IsModerator,
                sender.IsSubscriber);
        }

        public Player Get(string username)
        {
            return new Player(null, username, username, null, false, false, false);
        }

        public Player Get(string userId, string username)
        {
            return new Player(userId, username, username, null, false, false, false);
        }
    }
}