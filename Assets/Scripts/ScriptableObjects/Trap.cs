using System;
using UnityEngine;

[Serializable]
public class Trap
{
    [SerializeField] private GameObject trapGameObject;
    [SerializeField] private int countTraps;
    [SerializeField] private TrapType trapType;

    public GameObject TrapGameObject => trapGameObject;
    public int CountTraps => countTraps;
    public TrapType TrapType => trapType;

    private int _countSpawnedTrap;

    public bool CheckSpawn()
    {
        if (_countSpawnedTrap >= countTraps)
        {
            return false;
        }

        _countSpawnedTrap++;
        return true;
    }
}