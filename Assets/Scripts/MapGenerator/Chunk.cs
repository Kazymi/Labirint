using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chunk : MonoBehaviour
{
    [SerializeField] private Doors _doors;
    [SerializeField] private List<GameObject> _decorations;
    public Doors Doors => _doors;

    public void GenerateDecor()
    {
        _decorations[Random.Range(0,_decorations.Count)].SetActive(true);
    }

    public void RotateRandomly()
    {
        int count = Random.Range(0, 4);
        
        for (int i = 0; i < count; i++)
        {
            transform.Rotate(0, 90, 0);
            var tmp = _doors.DoorLeft;
            _doors.DoorLeft = _doors.DoorDown;
            _doors.DoorDown = _doors.DoorRight;
            _doors.DoorRight = _doors.DoorUp;
            _doors.DoorUp = tmp;
        }
    }
}