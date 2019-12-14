internal interface IGameManager
{
    GameServer Server { get; }
    ViewerManager Viewers { get; }
    SpawnPointManager SpawnPoints { get; }
    CreatureController Creature { get; }
    ObjectSpawner Spawner { get; }
    ObjectRegistry Objects { get; }
}