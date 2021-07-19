using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [SerializeField] private ChunkConfiguration _chunkConfiguration;
    [SerializeField] private Chunk[] generateChunks;
    [SerializeField] private Chunk mainChunk;
    [SerializeField] private int CountChunk;

    private int _maxX;
    private int _maxY;
    private Chunk[,] _spawnedChunkPositions;
    private List<Chunk> _spawnedChunk;

    private IEnumerator Start()
    {
        _spawnedChunkPositions = new Chunk[CountChunk, CountChunk];
        _spawnedChunkPositions[0, 0] = mainChunk;

        _maxX = _spawnedChunkPositions.GetLength(0) - 1;
        _maxY = _spawnedChunkPositions.GetLength(1) - 1;

        for (int i = 0; i < CountChunk + 1; i++)
        {
            yield return new WaitForSeconds(0.1f);
           PlaceSpawnRoom();
        }
    }

    private void GenerateTrap()
    {
        var unlockedChunks = new List<Chunk>();
        foreach (var chunk in _spawnedChunk.Where(p => p.TrapUnlocked))
        {
            unlockedChunks.Add(chunk);
        }
        foreach (var trap in _chunkConfiguration._Traps)
        {
            while (trap.CheckSpawn())
            {
                
            }            
        }
    }
    private void PlaceSpawnRoom()
    {
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();
        for (int x = 0; x < _spawnedChunkPositions.GetLength(0); x++)
        {
            for (int y = 0; y < _spawnedChunkPositions.GetLength(1); y++)
            {
                if (_spawnedChunkPositions[x, y] == null) continue;
                if (x > 0 && _spawnedChunkPositions[x - 1, y] == null) vacantPlaces.Add(new Vector2Int(x - 1, y));
                if (y > 0 && _spawnedChunkPositions[x, y - 1] == null) vacantPlaces.Add(new Vector2Int(x, y - 1));
                if (x < _maxX && _spawnedChunkPositions[x + 1, y] == null) vacantPlaces.Add(new Vector2Int(x + 1, y));
                if (y < _maxY && _spawnedChunkPositions[x, y + 1] == null) vacantPlaces.Add(new Vector2Int(x, y + 1));
            }
        }

        Chunk newChunk = Instantiate(generateChunks[Random.Range(0, generateChunks.Length)]);
        
        var limit = 500;
        while (limit-- > 0)
        {
            var position = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));
            newChunk.GenerateDecor();
            newChunk.RotateRandomly();
            if (!ConnectToDoor(newChunk, position)) continue;
            newChunk.transform.position =
                new Vector3(position.x-5, 0, position.y-5) * 10;
            _spawnedChunkPositions[position.x, position.y] = newChunk;
            _spawnedChunk.Add(newChunk);
            break;
        }
    }

    private bool CheckDoor(GameObject door1, GameObject door2)
    {
        return door1 != null && door2 != null && door1.GetComponent<EmptyDoor>() == null &&
               door2.GetComponent<EmptyDoor>() == null;
    }

    private bool ConnectToDoor(Chunk chunk, Vector2Int pos)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();

        if (pos.y < _maxY)
            if (_spawnedChunkPositions[pos.x, pos.y + 1] != null)
                if (pos.y < _maxY)
                    if (CheckDoor(chunk.Doors.DoorUp, _spawnedChunkPositions[pos.x, pos.y + 1].Doors.DoorDown))
                        neighbours.Add(Vector2Int.up);

        if (pos.y > 0)
            if (_spawnedChunkPositions[pos.x, pos.y - 1] != null)
                if (CheckDoor(chunk.Doors.DoorDown, _spawnedChunkPositions[pos.x, pos.y - 1].Doors.DoorUp))
                    neighbours.Add(Vector2Int.down);

        if (pos.x < _maxX)
            if (_spawnedChunkPositions[pos.x + 1, pos.y] != null)
                if (CheckDoor(chunk.Doors.DoorRight, _spawnedChunkPositions[pos.x + 1, pos.y].Doors.DoorLeft))
                    neighbours.Add(Vector2Int.right);

        if (pos.x > 0)
            if (_spawnedChunkPositions[pos.x - 1, pos.y] != null)
                if (CheckDoor(chunk.Doors.DoorLeft, _spawnedChunkPositions[pos.x - 1, pos.y].Doors.DoorRight))
                    neighbours.Add(Vector2Int.left);


        if (neighbours.Count == 0) return false;

        Vector2Int selectedDirection = neighbours[Random.Range(0, neighbours.Count)];
        Chunk selectedRoom = _spawnedChunkPositions[pos.x + selectedDirection.x, pos.y + selectedDirection.y];

        if (selectedDirection == Vector2Int.up)
        {
            chunk.Doors.DoorUp.SetActive(false);
            selectedRoom.Doors.DoorDown.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.down)
        {
            chunk.Doors.DoorDown.SetActive(false);
            selectedRoom.Doors.DoorUp.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.right)
        {
            chunk.Doors.DoorRight.SetActive(false);
            selectedRoom.Doors.DoorLeft.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.left)
        {
            chunk.Doors.DoorLeft.SetActive(false);
            selectedRoom.Doors.DoorRight.SetActive(false);
        }


        return true;
    }
}