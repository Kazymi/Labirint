using System;
using UnityEngine;

[Serializable]
public class QuaternionSerializable
{
    private float x;
    private float y;
    private float z;
    private float w;

    public float X => x;
    public float Y => y;
    public float Z => z;
    public float W => w;

    public QuaternionSerializable(Quaternion quaternion)
    {
        x = quaternion.x;
        y = quaternion.y;
        z = quaternion.z;
        w = quaternion.w;
    }
}