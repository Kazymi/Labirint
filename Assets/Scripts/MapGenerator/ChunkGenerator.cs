using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChunkGenerator : MonoBehaviour
{
    [SerializeField] private ChunkConfiguration chunkConfiguration;
    [SerializeField] private Chunk[] generateChunks;
    [SerializeField] private Chunk mainChunk;
    [SerializeField] private int countChunk;
    [SerializeField] private int countLeavers;

    private int _maxX;
    private int _maxY;
    private TrapManager _trapManager;
    private Chunk[,] _spawnedChunkPositions;
    private List<Chunk> _spawnedChunk = new List<Chunk>();

    public List<Chunk> SpawnedChunk => _spawnedChunk;

    private void OnEnable()
    {
        ServiceLocator.Subscribe<ChunkGenerator>(this);
    }

    private void OnDisable()
    {
        ServiceLocator.Unsubscribe<ChunkGenerator>();
    }

    public IEnumerator StartGenerate()
    {
        var seed = ServiceLocator.GetService<SeedGenerator>().Seed;
        Random.InitState(seed);
        Debug.LogError(seed);
        _trapManager = ServiceLocator.GetService<TrapManager>();
        _spawnedChunkPositions = new Chunk[countChunk, countChunk];
        _spawnedChunkPositions[0, 0] = mainChunk;

        _maxX = _spawnedChunkPositions.GetLength(0) - 1;
        _maxY = _spawnedChunkPositions.GetLength(1) - 1;

        for (int i = 0; i < countChunk + 1; i++)
        {
            yield return new WaitForSeconds(0.1f);
            PlaceSpawnRoom();
        }

        if (PhotonNetwork.IsMasterClient)
        {
            GenerateTrap(seed);
            GenerateLeverTrap(seed);
            ServiceLocator.GetService<KeyManager>().Initialize(this);
        }
        GenerateDecor();
    }
    public void GenerateLeverTrap(int seed)
    {
        for (int i = 0; i != countLeavers; i++)
        {
            var idChunk = GetChunkWithoutLever();
            var chunk = _spawnedChunk[idChunk];
            var positionLeaver = chunk.PointsLever[Random.Range(0, chunk.PointsLever.Count)];
            var leaver = PhotonNetwork.Instantiate(chunk.Lever.name, positionLeaver.position, positionLeaver.rotation);
            leaver.GetComponent<PhotonView>().RPC("Initialize",RpcTarget.All,idChunk,seed);
        }
    }

    private int GetChunkWithoutLever()
    {
        var chunks = new List<Chunk>();
        foreach (var chunk in _spawnedChunk.Where(t => t.SpawnedLeaver == false))
        {
            chunks.Add(chunk);
        }
        var returnChunk = Random.Range(0, chunks.Count);
        _spawnedChunk[returnChunk].SpawnedLeaver = true;
        return returnChunk;
    }

    private void GenerateDecor()
    {
        foreach (var chunk in _spawnedChunk)
        {
            var newSeed = Random.Range(10000, 99999);
            Random.InitState(newSeed);
            chunk.GenerateDecor(newSeed);
        }
    }
    private void GenerateTrap(int seed)
    {
        var unlockedChunks = new List<Chunk>();
        foreach (var chunk in _spawnedChunk.Where(p => p.TrapUnlocked))
        {
            unlockedChunks.Add(chunk);
        }

        foreach (var trap in chunkConfiguration._Traps)
        {
            while (trap.CheckSpawn())
            {
                var limit = 500;
                while (limit-- > 0)
                {
                    var chunk = unlockedChunks[Random.Range(0, unlockedChunks.Count)];
                    if (chunk.CheckSpawnTrapByType(trap.TrapType) == false)
                    {
                        continue;
                    }

                    Transform spawnTransform;
                    if (trap.TrapType == TrapType.DoorTrap)
                    {
                        var posDoor = chunk.Doors.GetDoorTrapPosition(seed);
                        if (posDoor == null) continue;
                        spawnTransform = posDoor;
                    }
                    else spawnTransform = trap.GetTrapPosition(chunk);

                    if (spawnTransform.childCount != 0) continue;
                    var newTrap = _trapManager.GetTrapByType(trap.TrapType);
                    newTrap.transform.parent = spawnTransform;
                    var i = newTrap.GetComponent<ITrapSetting>();
                    if (i != null)
                    {
                        i.PhotonView = newTrap.GetComponent<PhotonView>();
                        i.PhotonView.RPC("SetPosition", RpcTarget.All,
                            spawnTransform.position,
                            spawnTransform.rotation);
                    }
                    break;
                }
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
            newChunk.RotateRandomly();
            if (!ConnectToDoor(newChunk, position)) continue;
            newChunk.transform.position =
                new Vector3(position.x - 5, 0, position.y - 5) * 10;
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