using Newtonsoft.Json;

public abstract class PacketHandler
{
    protected PacketHandler(GameManager game, GameServer server, ViewerManager viewerManager)
    {
        Game = game;
        Server = server;
        ViewerManager = viewerManager;
    }

    protected GameManager Game { get; }
    protected GameServer Server { get; }
    protected ViewerManager ViewerManager { get; }

    public abstract void Handle(Packet packet);
}

public abstract class PacketHandler<TPacketType> : PacketHandler
{
    protected PacketHandler(GameManager game, GameServer server, ViewerManager viewerManager)
        : base(game, server, viewerManager)
    {
    }

    public override void Handle(Packet packet)
    {
        var request = JsonConvert.DeserializeObject<TPacketType>(packet.JsonData);
        Handle(request, packet.Client);
    }

    public abstract void Handle(TPacketType data, GameClient client);
}