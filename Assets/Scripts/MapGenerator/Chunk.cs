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
    
    [SerializeField] private List<LeaverActivator> listActivators;
    [SerializeField] private List<Transform> pointsLever;
    [SerializeField] private GameObject lever;
    
    [SerializeField] private Doors doors;
    [SerializeField] private List<GameObject> decorations;
    [SerializeField] private bool trapUnlocked = true;
    
    public Doors Doors => doors;
    public List<TrapPositions> TrapPositions => trapPositions;
    public List<Transform> PositionKeys => positionKeys;
    public List<LeaverActivator> ListActivators => listActivators;
    public List<Transform> PointsLever => pointsLever;
    public bool spawnedLever;

    public GameObject Lever => lever;
    public bool TrapUnlocked => trapUnlocked;

    public void GenerateDecor(int newSeed)
    {
        Random.InitState(newSeed);
        decorations[Random.Range(0,decorations.Count)].SetActive(true);
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