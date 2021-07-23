using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Chunk : MonoBehaviour
{
    [SerializeField] private List<TrapPositions> trapPositions;
    [SerializeField] private List<Transform> positionKeys;
    [SerializeField] private Doors doors;
    [SerializeField] private List<GameObject> decorations;
    [SerializeField] private bool trapUnlocked = true;
    public Doors Doors => doors;
    public List<TrapPositions> TrapPositions => trapPositions;
    public List<Transform> PositionKeys => positionKeys;

    private void Start()
    {
        Random.InitState(ServiceLocator.GetService<SeedGenerator>().Seed);
    }

    public bool TrapUnlocked => trapUnlocked;

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
            var tmp = doors.DoorLeft;
            var tmpTrap = doors.TrapDoorPointLeft;
            
            doors.DoorLeft = doors.DoorDown;
            doors.TrapDoorPointLeft = doors.TrapDoorPointDown;
            
            doors.DoorDown = doors.DoorRight;
            doors.TrapDoorPointDown = doors.TrapDoorPointRight;
            
            doors.DoorRight = doors.DoorUp;
            doors.TrapDoorPointRight = doors.TrapDoorPointUp;
            
            doors.DoorUp = tmp;
            doors.TrapDoorPointUp = tmpTrap;
    }
}