using UnityEngine;

public class CommandServer : MonoBehaviour
{
    private GameServer server;

    public GameServer Server => server;

    public void StartServer(GameManager gameManager)
    {
        server = new GameServer(gameManager);

        server.Register<Feed>("feed");
        server.Register<Play>("play");
        server.Register<Groom>("groom");
        server.Register<Pet>("pet");

        server.Start();
    }
}