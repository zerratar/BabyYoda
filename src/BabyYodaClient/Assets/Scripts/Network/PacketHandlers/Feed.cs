using System;

public class Feed : PacketHandler<TwitchUser>
{
    public Feed(
       GameManager game,
       GameServer server,
       ViewerManager viewerManager)
       : base(game, server, viewerManager)
    {
    }

    public override void Handle(TwitchUser data, GameClient client)
    {
        try
        {
            var viewer = ViewerManager.GetViewer(data);
            if (Game.Creature.Feed(viewer))
            {
                client.SendMessage(data.Username, $"You gave Baby Yoda some food!");
            }
        }
        catch (Exception exc)
        {
            client.SendMessage(data.Username, exc.ToString());
        }
    }
}