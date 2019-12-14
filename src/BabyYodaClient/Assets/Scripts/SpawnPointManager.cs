using System.Linq;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    [SerializeField] private Transform[] points;

    public Vector3 GetRandom()
    {
        if (points == null || points.Length == 0) return Vector3.zero;
        return points
            .OrderBy(x => Random.value)
            .Select(x => x.position)
            .FirstOrDefault();
    }
}
