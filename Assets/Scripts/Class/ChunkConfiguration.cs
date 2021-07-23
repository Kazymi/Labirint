using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChunkConfiguration
{
    [SerializeField] private List<Trap> _traps = new List<Trap>();

    public List<Trap> _Traps => _traps;
}