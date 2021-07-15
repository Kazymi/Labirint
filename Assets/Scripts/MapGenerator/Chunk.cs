using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chunk : MonoBehaviour
{
    [SerializeField] private Doors _doors;
    public Doors Doors => _doors;
}