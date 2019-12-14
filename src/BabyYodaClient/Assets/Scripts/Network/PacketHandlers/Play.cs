public class Play : PacketHandler<TwitchUser>
{
    public Play(
       GameManager game,
       GameServer server,
       ViewerManager playerManager)
       : base(game, server, playerManager)
    {
    }

    public override void Handle(TwitchUser data, GameClient client)
    {
        var viewer = ViewerManager.GetViewer(data);

        Game.Creature.Play(viewer);

        client.SendMessage(data.Username, $"You played with Baby Yoda!");
    }
}
