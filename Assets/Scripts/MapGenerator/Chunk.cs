using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chunk : MonoBehaviour
{
    [SerializeField] private Doors _doors;
    [SerializeField] private List<GameObject> _decorations;
    [SerializeField] private bool trapUnlocked = true;
    public Doors Doors => _doors;

    public bool TrapUnlocked
    {
        get => trapUnlocked;
        set => trapUnlocked = value;
    }

    public void GenerateDecor()
    {
        _decorations[Random.Range(0,_decorations.Count)].SetActive(true);
    }

    public void RotateRandomly()
    {
        transform.Rotate(0, 90, 0);
            var tmp = _doors.DoorLeft;
            _doors.DoorLeft = _doors.DoorDown;
            _doors.DoorDown = _doors.DoorRight;
            _doors.DoorRight = _doors.DoorUp;
            _doors.DoorUp = tmp;
    }
}