using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TrapPositions
{
    [SerializeField] private TrapType trapType;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    public TrapType TrapType => trapType;
    public List<Transform> SpawnPoint => spawnPoints;

}