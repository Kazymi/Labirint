using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnpoints;

    private void Awake()
    {
        ServiceLocator.Subscribe<SpawnManager>(this);
    }

    public Transform GetSpawnPoint()
    {
        return spawnpoints[Random.Range(0, spawnpoints.Count)].transform;
    }
}