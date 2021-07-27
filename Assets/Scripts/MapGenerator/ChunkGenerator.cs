using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChunkGenerator : MonoBehaviour,IPreloadingAComponent
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
    private PhotonView _photonView;
    public bool PreloadingCompleted { get; set; }
    public List<Chunk> SpawnedChunk => _spawnedChunk;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        ServiceLocator.Subscribe<ChunkGenerator>(this);
        Preloading.Subscribe(this);
    }

    private void OnDisable()
    {
        ServiceLocator.Unsubscribe<ChunkGenerator>();
        Preloading.Unsubscribe(this);
    }

    [PunRPC]
    public void PreloadingFinish()
    {
        PreloadingCompleted = true;
        Preloading.CheckPreloading();
    }
    
    public IEnumerator Start()
    {
        if (PhotonNetwork.IsMasterClient == false) yield break;
        _trapManager = ServiceLocator.GetService<TrapManager>();
        _spawnedChunkPositions = new Chunk[countChunk, countChunk];
        _spawnedChunkPositions[5, 5] = mainChunk;

        _maxX = _spawnedChunkPositions.GetLength(0) - 1;
        _maxY = _spawnedChunkPositions.GetLength(1) - 1;

        for (int i = 0; i < countChunk + 1; i++)
        {
            yield return new WaitForSeconds(0.1f);
            PlaceSpawnRoom();
        }
        GenerateTrap();
        GenerateLeverTrap();
        ServiceLocator.GetService<KeyManager>().Initialize(this);
        GenerateDecor();
        _photonView.RPC(RPCEventType.PreloadingFinish,RpcTarget.All);
    }

    private int GetChunkWithoutLever()
    {
        var chunks = new List<Chunk>();
        foreach (var chunk in _spawnedChunk.Where(t => t.spawnedLever == false))
        {
            chunks.Add(chunk);
        }
        var returnChunk = Random.Range(0, chunks.Count);
        _spawnedChunk[returnChunk].spawnedLever = true;
        return returnChunk;
    }

    private void GenerateDecor()
    {
        foreach (var chunk in _spawnedChunk)
        {
            var idDecor = Random.Range(0,chunk.CountDecor);
            chunk.GetComponent<PhotonView>().RPC(RPCEventType.GenerateDecor,RpcTarget.All,idDecor);
        }
    }
    
    private void GenerateLeverTrap()
    {
        for (int i = 0; i != countLeavers; i++)
        {
            var idChunk = GetChunkWithoutLever();
            var chunk = _spawnedChunk[idChunk];
            idChunk = chunk.GetComponent<PhotonView>().ViewID;
            var positionLeaver = chunk.PointsLever[Random.Range(0, chunk.PointsLever.Count)];
            var leaver = PhotonNetwork.Instantiate(chunk.Lever.name, positionLeaver.position, positionLeaver.rotation);
            leaver.GetComponent<PhotonView>().RPC(RPCEventType.LeverInitialize,RpcTarget.All,idChunk);
        }
    }
    
    private void GenerateTrap()
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
                        var posDoor = chunk.Doors.GetDoorTrapPosition();
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
                        i.PhotonView.RPC(RPCEventType.SetPosition, RpcTarget.All,
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

        var newChunkName = generateChunks[Random.Range(0, generateChunks.Length)].name;
        var newChunk = PhotonNetwork.Instantiate(newChunkName, Vector3.zero, Quaternion.identity).GetComponent<Chunk>();
        var limit = 500;
        while (limit-- > 0)
        {
            var position = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));
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
            chunk.GetComponent<PhotonView>().RPC(RPCEventType.OpenDoor,RpcTarget.All,chunk.Doors.DoorUp.name);
            selectedRoom.GetComponent<PhotonView>().RPC(RPCEventType.OpenDoor,RpcTarget.All,selectedRoom.Doors.DoorDown.name);
        }
        else if (selectedDirection == Vector2Int.down)
        {
            chunk.GetComponent<PhotonView>().RPC(RPCEventType.OpenDoor,RpcTarget.All,chunk.Doors.DoorDown.name);
            selectedRoom.GetComponent<PhotonView>().RPC(RPCEventType.OpenDoor,RpcTarget.All,selectedRoom.Doors.DoorUp.name);
        }
        else if (selectedDirection == Vector2Int.right)
        {
            chunk.GetComponent<PhotonView>().RPC(RPCEventType.OpenDoor,RpcTarget.All,chunk.Doors.DoorRight.name);
            selectedRoom.GetComponent<PhotonView>().RPC(RPCEventType.OpenDoor,RpcTarget.All,selectedRoom.Doors.DoorLeft.name);
        }
        else if (selectedDirection == Vector2Int.left)
        {
            chunk.GetComponent<PhotonView>().RPC(RPCEventType.OpenDoor,RpcTarget.All,chunk.Doors.DoorLeft.name);
            selectedRoom.GetComponent<PhotonView>().RPC(RPCEventType.OpenDoor,RpcTarget.All,selectedRoom.Doors.DoorRight.name);
        }
        return true;
    }
}