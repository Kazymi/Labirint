using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Trap
{
    [SerializeField] private int countTraps;
    [SerializeField] private TrapType trapType;

    private int _seed;
    private int _countSpawnedTrap;
    public TrapType TrapType => trapType;

    public bool CheckSpawn()
    {
        if (_countSpawnedTrap >= countTraps)
        {
            return false;
        }

        _countSpawnedTrap++;
        return true;
    }

    public Transform GetTrapPosition(Chunk chunk)
    {
        var trapTransforms = new List<Transform>();
        foreach (var i in chunk.TrapPositions.Where(t => t.TrapType == trapType))
        {
            trapTransforms.Add(i.SpawnPoint[Random.Range(0,i.SpawnPoint.Count)]);
        }
        return trapTransforms[Random.Range(0, trapTransforms.Count)];
    }
}