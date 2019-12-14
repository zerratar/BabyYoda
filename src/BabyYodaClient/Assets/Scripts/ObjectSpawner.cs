using Assets.Scripts.Models;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] foodPrefabs;
    [SerializeField] private GameObject[] toyPrefabs;
    [SerializeField] private GameObject[] groomingPrefabs;
    [SerializeField] private SpawnPointManager spawnPointManager;
    [SerializeField] private ObjectRegistry objectRegistry;

    public bool SpawnFood(Viewer spawner)
    {
        var availableFood = objectRegistry.GetAvailableFood();
        if (availableFood.Count > 20)
        {
            return false;
        }

        var spawnPoint = spawnPointManager.GetRandom();
        var foodPrefab = foodPrefabs.Random();

        var rightSeed = Vector3.right * Random.value;
        var leftSeed = Vector3.left * Random.value;

        var obj = Instantiate(foodPrefab, spawnPoint + rightSeed + leftSeed, Quaternion.identity);
        var rb = obj.GetComponent<Rigidbody>();
        var consumable = obj.GetComponent<Consumable>();

        consumable.Spawner = spawner;
        rb.AddForce(rightSeed + leftSeed, ForceMode.Impulse);
        objectRegistry.AddFood(consumable);
        return true;
    }

    public void SpawnToy()
    {
    }

    public void SpawnGroomingItems()
    {
    }
}
