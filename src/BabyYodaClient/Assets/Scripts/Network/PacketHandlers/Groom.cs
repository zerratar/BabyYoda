public class Groom : PacketHandler<TwitchUser>
{
    public Groom(
       GameManager game,
       GameServer server,
       ViewerManager playerManager)
       : base(game, server, playerManager)
    {
    }

    public override void Handle(TwitchUser data, GameClient client)
    {
        var viewer = ViewerManager.GetViewer(data);

        Game.Creature.Groom(viewer);

        client.SendMessage(data.Username, $"You groomed Baby Yoda!");
    }
}