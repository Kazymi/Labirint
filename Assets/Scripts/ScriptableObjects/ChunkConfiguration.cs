using System.Collections.Generic;
using UnityEngine;

public class ChunkConfiguration : ScriptableObject
{
    [SerializeField] private List<Trap> _traps = new List<Trap>();

    public List<Trap> _Traps => _traps;
}