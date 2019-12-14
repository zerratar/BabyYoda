using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BabyYodaBot.Core.Net;
using BabyYodaBot.Core.BabyYoda;
using BabyYodaBot.Core;
using BabyYodaBot.Core.Handlers;
using BabyYodaBot.Core.BabyYoda.Commands;

namespace BabyYodaBot.Commands.Games.BabyYoda
{
    public class FeedCommandProcessor : CommandProcessor
    {
        private readonly IBabyYodaClient game;
        private readonly IPlayerProvider playerProvider;

        public FeedCommandProcessor(IBabyYodaClient game, IPlayerProvider playerProvider)
        {
            this.game = game;
            this.playerProvider = playerProvider;
        }

        public override async Task ProcessAsync(IMessageBroadcaster broadcaster, ICommand cmd)
        {
            if (!await this.game.ProcessAsync(BabyYoda.Settings.UNITY_SERVER_PORT))
            {
                //broadcaster.Broadcast(
                broadcaster.Send(cmd.Sender.Username,
                    BabyYoda.Localization.GAME_NOT_STARTED);
                return;
            }

            var player = playerProvider.Get(cmd.Sender);
            await game.FeedAsync(player);
        }
    }
}
