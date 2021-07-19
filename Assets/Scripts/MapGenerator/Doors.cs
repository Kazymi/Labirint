using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Doors
{
    [SerializeField] private GameObject doorUp;
    [SerializeField] private GameObject doorDown;
    [SerializeField] private GameObject doorLeft;
    [SerializeField] private GameObject doorRight;

    [SerializeField] private Transform trapDoorPointUp;
    [SerializeField] private Transform trapDoorPointDown;
    [SerializeField] private Transform trapDoorPointLeft;
    [SerializeField] private Transform trapDoorPointRight;

    public Transform TrapDoorPointUp
    {
        get => trapDoorPointUp;
        set => trapDoorPointUp = value;
    }

    public Transform TrapDoorPointDown
    {
        get => trapDoorPointDown;
        set => trapDoorPointDown = value;
    }

    public Transform TrapDoorPointLeft
    {
        get => trapDoorPointLeft;
        set => trapDoorPointLeft = value;
    }

    public Transform TrapDoorPointRight
    {
        get => trapDoorPointRight;
        set => trapDoorPointRight = value;
    }
    
    public GameObject DoorUp
    {
        get => doorUp;
        set => doorUp = value;
    }

    public GameObject DoorDown
    {
        get => doorDown;
        set => doorDown = value;
    }

    public GameObject DoorLeft
    {
        get => doorLeft;
        set => doorLeft = value;
    }

    public GameObject DoorRight
    {
        get => doorRight;
        set => doorRight = value;
    }

    public Transform GetDoorTrapPosition()
    {
        var positions = new List<Transform>();
        if (doorDown.activeSelf == false) positions.Add(trapDoorPointDown);
        if (doorLeft.activeSelf == false) positions.Add(trapDoorPointLeft);
        if (doorRight.activeSelf == false) positions.Add(trapDoorPointRight);
        if (doorUp.activeSelf == false) positions.Add(trapDoorPointUp);
        if (positions.Count == 0) return null;
        Debug.Log(positions.Count);
        return positions[Random.Range(0, positions.Count)];
    }
}