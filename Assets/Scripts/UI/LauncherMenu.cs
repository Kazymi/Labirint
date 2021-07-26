using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LauncherMenu : MonoBehaviour
{
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;

    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button leaveRoomButton;
    [SerializeField] private Button startGameRoomButton;

    [SerializeField] private MenuManager menuManager;
    [SerializeField] private Canvas titleMenu;
    [SerializeField] private Canvas loadMenu;
    [SerializeField] private Canvas errorMenu;
    [SerializeField] private Canvas roomMenu;
    [SerializeField] private Launcher launcher;
    private void Awake()
    {
        launcher.Initialize(this);
    }

    private void OnEnable()
    {
        createRoomButton.onClick.AddListener(CreateRoom);
        leaveRoomButton.onClick.AddListener(launcher.LeaveRoom);
        startGameRoomButton.onClick.AddListener(launcher.StartGame);
    }
	

    private void OnDisable()
    {
        createRoomButton.onClick.RemoveListener(CreateRoom);
        leaveRoomButton.onClick.RemoveListener(launcher.LeaveRoom);
        startGameRoomButton.onClick.RemoveListener(launcher.StartGame);
    }

    private void CreateRoom()
    {
        if(string.IsNullOrEmpty(roomNameInputField.text)) return;
        launcher.CreateRoom(roomNameInputField.text);
        Load();
    }

    public void OnJoinedLobby()
    {
        menuManager.OpenMenu(titleMenu);
    }

    public void OnJoinedRoom(string nameRoom)
    {
        menuManager.OpenMenu(roomMenu);
        roomNameText.text = nameRoom;
    }

    public void OnJoinedRoomError(string nameError)
    {
        errorText.text = "Room Creation Failed: " + nameError;
        Debug.LogError("Room Creation Failed: " + nameError);
        menuManager.OpenMenu(errorMenu);
    }

    public void Load()
    {
        menuManager.OpenMenu(loadMenu);
    }

    public void OnRoomLeft()
    {
        menuManager.OpenMenu(titleMenu);
    }
}