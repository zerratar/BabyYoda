using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager
{
    [SerializeField] private CommandServer commandServer;
    [SerializeField] private GameSettings settings;
    [SerializeField] private ViewerManager viewerManager;
    [SerializeField] private SpawnPointManager spawnPointManager;
    [SerializeField] private ObjectSpawner objectSpawner;
    [SerializeField] private ObjectRegistry objectRegistry;
    [SerializeField] private CreatureController creatureController;
    [SerializeField] private IoCContainer ioc;
    [SerializeField] private Message message;
    [SerializeField] private ProgressBar hungerProgressBar;

    public GameServer Server => commandServer?.Server;
    public ViewerManager Viewers => viewerManager;
    public SpawnPointManager SpawnPoints => spawnPointManager;
    public CreatureController Creature => creatureController;
    public ObjectSpawner Spawner => objectSpawner;
    public ObjectRegistry Objects => objectRegistry;
    public Message Message => message;
    // Start is called before the first frame update   
    void Start()
    {
        if (!commandServer) commandServer = GetComponent<CommandServer>();
        if (!ioc) ioc = FindObjectOfType<IoCContainer>();
        ioc.Container.RegisterCustomShared<IGameManager>(() => this);
        commandServer.StartServer(this);
    }


    public void Announce(string message)
    {
        if (Server.Client != null && Server.Client.Connected)
        {
            Server.Client.SendMessage("", message);
        }

        Debug.Log(message);
    }


    // Update is called once per frame
    void Update()
    {
        hungerProgressBar.SetValue(1f - Creature.Hunger);

        HandleKeyDown();

        UpdateChatBotCommunication();
    }

    public void Log(string message)
    {
        Debug.Log(message);
    }

    public void LogError(string message)
    {
        Debug.LogError(message);
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        var client = Server.Client;
        if (client == null) return;
        Server.Stop();
    }

    private void UpdateChatBotCommunication()
    {
        if (Server == null || !Server.IsBound)
        {
            return;
        }

        Server.HandleNextPacket(this, Server, viewerManager);
    }

    //private IslandController lastIslandToggle = null;
    private void HandleKeyDown()
    {
        var isShiftDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        var isControlDown = isShiftDown || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        if (Input.GetKeyUp(KeyCode.Space))
        {
            objectSpawner.SpawnFood(null);
        }
    }


    private void SaveGameStat<T>(string name, T value)
    {
        try
        {
            if (!System.IO.Directory.Exists(settings.StreamLabelsFolder))
                System.IO.Directory.CreateDirectory(settings.StreamLabelsFolder);

            System.IO.File.WriteAllText(
                System.IO.Path.Combine(settings.StreamLabelsFolder, name + ".txt"),
                value.ToString());
        }
        catch
        {
            // Ignore: since we do not want this to interrupt any execution of the script.
        }
    }
}