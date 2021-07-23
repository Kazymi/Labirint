using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
	// TODO: Singleton
	public static Launcher Instance;

	[SerializeField] TMP_InputField roomNameInputField;
	[SerializeField] TMP_Text errorText;
	[SerializeField] TMP_Text roomNameText;
	[SerializeField] Transform roomListContent;
	[SerializeField] GameObject roomListItemPrefab;
	[SerializeField] Transform playerListContent;
	[SerializeField] GameObject PlayerListItemPrefab;
	[SerializeField] GameObject startGameButton;

	[SerializeField] private Button createRoomButton;
	[SerializeField] private Button leaveRoomButton;
	[SerializeField] private Button startGameRoomButton;


	[SerializeField] private MenuManager menuManager;
	[SerializeField] private Menu titleMenu;
	[SerializeField] private Menu loadMenu;
	[SerializeField] private Menu errorMenu;
	[SerializeField] private Menu roomMenu;
	
	private void Awake()
	{
		Instance = this;
	}

	public override void OnEnable()
	{
		base.OnEnable();
		createRoomButton.onClick.AddListener(CreateRoom);
		leaveRoomButton.onClick.AddListener(LeaveRoom);
		startGameRoomButton.onClick.AddListener(StartGame);
	}
	

	public override void OnDisable()
	{
		base.OnDisable();
		createRoomButton.onClick.RemoveListener(CreateRoom);
		leaveRoomButton.onClick.RemoveListener(LeaveRoom);
		startGameRoomButton.onClick.RemoveListener(StartGame);
	}

	private void Start()
	{
		PhotonNetwork.ConnectUsingSettings();
		ServiceLocator.Initialize();
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to Master");
		PhotonNetwork.JoinLobby();
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	public override void OnJoinedLobby()
	{
		menuManager.OpenMenu(titleMenu);
	}

	public void CreateRoom()
	{
		if(string.IsNullOrEmpty(roomNameInputField.text))
		{
			return;
		}
		PhotonNetwork.CreateRoom(roomNameInputField.text);
		menuManager.OpenMenu(loadMenu);
	}

	public override void OnJoinedRoom()
	{
		menuManager.OpenMenu(roomMenu);
		roomNameText.text = PhotonNetwork.CurrentRoom.Name;

		Player[] players = PhotonNetwork.PlayerList;

		foreach(Transform child in playerListContent)
		{
			Destroy(child.gameObject);
		}

		for(int i = 0; i < players.Count(); i++)
		{
			Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
		}

		startGameButton.SetActive(PhotonNetwork.IsMasterClient);
	}

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		startGameButton.SetActive(PhotonNetwork.IsMasterClient);
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		errorText.text = "Room Creation Failed: " + message;
		Debug.LogError("Room Creation Failed: " + message);
		menuManager.OpenMenu(errorMenu);
	}

	public void StartGame()
	{
		PhotonNetwork.LoadLevel(1);
	}

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
		menuManager.OpenMenu(loadMenu);
	}

	public void JoinRoom(RoomInfo info)
	{
		PhotonNetwork.JoinRoom(info.Name);
		menuManager.OpenMenu(loadMenu);
	}

	public override void OnLeftRoom()
	{
		menuManager.OpenMenu(titleMenu);
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		foreach(Transform trans in roomListContent)
		{
			Destroy(trans.gameObject);
		}

		for(int i = 0; i < roomList.Count; i++)
		{
			if(roomList[i].RemovedFromList)
				continue;
			Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
	}
}