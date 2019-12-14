public class Pet : PacketHandler<TwitchUser>
{
    public Pet(
       GameManager game,
       GameServer server,
       ViewerManager playerManager)
       : base(game, server, playerManager)
    {
    }

    public override void Handle(TwitchUser data, GameClient client)
    {
        var viewer = ViewerManager.GetViewer(data);

        Game.Creature.Pet(viewer);

        client.SendMessage(data.Username, $"You pet Baby Yoda!");
    }
}