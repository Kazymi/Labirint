using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [SerializeField] private Chunk[] generateChunks;
    [SerializeField] private Chunk mainChunk;
    [SerializeField] private int CountChunk;

    private int _maxX;
    private int _maxY;
    private Chunk[,] _spawnedChunk;

    private void Start()
    {
        _spawnedChunk = new Chunk[CountChunk, CountChunk];
        _spawnedChunk[5, 5] = mainChunk;

        _maxX = _spawnedChunk.GetLength(0) - 1;
        _maxY = _spawnedChunk.GetLength(1) - 1;

        for (int i = 0; i < CountChunk + 1; i++)
        {
            PlaceSpawnRoom();
        }
    }

    private void PlaceSpawnRoom()
    {
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();
        for (int x = 0; x < _spawnedChunk.GetLength(0); x++)
        {
            for (int y = 0; y < _spawnedChunk.GetLength(1); y++)
            {
                if (_spawnedChunk[x, y] == null) continue;
                if (x > 0 && _spawnedChunk[x - 1, y] == null) vacantPlaces.Add(new Vector2Int(x - 1, y));
                if (y > 0 && _spawnedChunk[x, y - 1] == null) vacantPlaces.Add(new Vector2Int(x, y - 1));
                if (x < _maxX && _spawnedChunk[x + 1, y] == null) vacantPlaces.Add(new Vector2Int(x + 1, y));
                if (y < _maxY && _spawnedChunk[x, y + 1] == null) vacantPlaces.Add(new Vector2Int(x, y + 1));
            }
        }

        Chunk newChunk = Instantiate(generateChunks[Random.Range(0, generateChunks.Length)]);
        newChunk.transform.position = new Vector3(1000, 25, 1000);
        var limit = 2000;
        while (limit-- > 0)
        {
            var position = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));
            newChunk.GenerateDecor();
            if (ConnectToDoor(newChunk, position))
            {
                newChunk.transform.position =
                    new Vector3(position.x - 5, 0, position.y - 5) * 10;
                _spawnedChunk[position.x, position.y] = newChunk;
                break;
            } else newChunk.RotateRandomly();
        }
    }
    private bool ConnectToDoor(Chunk chunk, Vector2Int pos)
    {

        List<Vector2Int> neighbours = new List<Vector2Int>();

        if (chunk.Doors.DoorUp != null && pos.y < _maxY && _spawnedChunk[pos.x, pos.y + 1]?.Doors.DoorDown != null) neighbours.Add(Vector2Int.up);
        if (chunk.Doors.DoorDown != null && pos.y > 0 && _spawnedChunk[pos.x, pos.y - 1]?.Doors.DoorUp != null) neighbours.Add(Vector2Int.down);
        if (chunk.Doors.DoorRight != null && pos.x < _maxX && _spawnedChunk[pos.x + 1, pos.y]?.Doors.DoorLeft != null) neighbours.Add(Vector2Int.right);
        if (chunk.Doors.DoorLeft != null && pos.x > 0 && _spawnedChunk[pos.x - 1, pos.y]?.Doors.DoorRight != null) neighbours.Add(Vector2Int.left);

        if (neighbours.Count == 0) return false;

        Vector2Int selectedDirection = neighbours[Random.Range(0, neighbours.Count)];
        Chunk selectedRoom = _spawnedChunk[pos.x + selectedDirection.x, pos.y + selectedDirection.y];

        if(selectedDirection == Vector2Int.up)
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