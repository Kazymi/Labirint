using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
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
    public int CountDecor => decorations.Count;

    public GameObject Lever => lever;
    public bool TrapUnlocked => trapUnlocked;

    public bool CheckSpawnTrapByType(TrapType trapType)
    {
        foreach (var trap in trapPositions)
        {
            if (trap.TrapType == trapType) return true;
        }
        return false;
    }
    
    [PunRPC]
    public void OpenDoor(string doorName)
    {
        if(doorName == doors.DoorDown.name) doors.DoorDown.SetActive(false);
        if(doorName == doors.DoorUp.name) doors.DoorUp.SetActive(false);
        if(doorName == doors.DoorRight.name) doors.DoorRight.SetActive(false);
        if(doorName == doors.DoorLeft.name) doors.DoorLeft.SetActive(false);
    }
    
    [PunRPC]
    public void GenerateDecor(int idDecor)
    {
        decorations[idDecor].SetActive(true);
    }
}