using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;

    // TODO: singleton
    private LauncherMenu _launcherMenu;

    public static Launcher Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        ServiceLocator.Initialize();
    }

    public void Initialize(LauncherMenu launcherMenu)
    {
        _launcherMenu = launcherMenu;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        _launcherMenu.OnJoinedLobby();
    }

    public void CreateRoom(string nameRoom)
    {
        PhotonNetwork.CreateRoom(nameRoom);
    }

    public override void OnJoinedRoom()
    {
        var players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var player in players)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(player);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        _launcherMenu.OnJoinedRoom(PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _launcherMenu.OnJoinedRoomError(message);
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        _launcherMenu.Load();
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        _launcherMenu.Load();
    }

    public override void OnLeftRoom()
    {
        _launcherMenu.OnRoomLeft();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        foreach (var roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
            {
                continue;
            }

            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomInfo);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
}