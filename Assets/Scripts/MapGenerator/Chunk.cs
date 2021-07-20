using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chunk : MonoBehaviour
{
    [SerializeField] private List<TrapPositions> trapPositions;
    [SerializeField] private Doors _doors;
    [SerializeField] private List<GameObject> _decorations;
    [SerializeField] private bool trapUnlocked = true;
    public Doors Doors => _doors;
    public List<TrapPositions> TrapPositions => trapPositions;

    private void Start()
    {
        Random.InitState(ServiceLocator.GetService<SeedGenerator>().Seed);
    }

    public bool TrapUnlocked
    {
        get => trapUnlocked;
        set => trapUnlocked = value;
    }

    public void GenerateDecor()
    {
        //_decorations[Random.Range(0,_decorations.Count)].SetActive(true);
    }

    public bool CheckSpawnTrapByType(TrapType trapType)
    {
        foreach (var trap in trapPositions)
        {
            if (trap.TrapType == trapType) return true;
        }

        return false;
    }
    
    public void RotateRandomly()
    {
        transform.Rotate(0, 90, 0);
            var tmp = _doors.DoorLeft;
            var tmpTrap = _doors.TrapDoorPointLeft;
            
            _doors.DoorLeft = _doors.DoorDown;
            _doors.TrapDoorPointLeft = _doors.TrapDoorPointDown;
            
            _doors.DoorDown = _doors.DoorRight;
            _doors.TrapDoorPointDown = _doors.TrapDoorPointRight;
            
            _doors.DoorRight = _doors.DoorUp;
            _doors.TrapDoorPointRight = _doors.TrapDoorPointUp;
            
            _doors.DoorUp = tmp;
            _doors.TrapDoorPointUp = tmpTrap;
    }
}