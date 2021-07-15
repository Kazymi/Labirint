using System;
using UnityEngine;

[Serializable]
public class Doors
{
    [SerializeField] private GameObject doorUp;
    [SerializeField] private GameObject doorDown;
    [SerializeField] private GameObject doorLeft;
    [SerializeField] private GameObject doorRight;

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
}