using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
	[SerializeField] private List<Transform> _spawnpoints;

	private void Awake()
	{
		ServiceLocator.Subscribe<SpawnManager>(this);
	}

	public Transform GetSpawnPoint()
	{
		return _spawnpoints[Random.Range(0, _spawnpoints.Count)].transform;
	}
}
