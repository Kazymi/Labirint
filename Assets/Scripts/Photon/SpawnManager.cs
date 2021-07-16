using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	[SerializeField] private List<Transform> _spawnpoints;


	public Transform GetSpawnPoint()
	{
		return _spawnpoints[Random.Range(0, _spawnpoints.Count)].transform;
	}
}
