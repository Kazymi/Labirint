using System;
using UnityEngine;

[Serializable]
public class SerializableVector3
{
    private float x;
    private float y;
    private float z;

    public float X => x;
    public float Y => y;
    public float Z => z;

    public SerializableVector3(Vector3 vector3)
    {
        x = vector3.x;
        y = vector3.y;
        z = vector3.z;
    }
}