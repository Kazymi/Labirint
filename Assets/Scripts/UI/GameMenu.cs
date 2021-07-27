using DG.Tweening;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour,IOnEventCallback
{
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private Canvas finishCanvas;
    [SerializeField] private Canvas pausedCanvas;
    [SerializeField] private TMP_Text finishGameText;
    [SerializeField] private Button disconnectButton;

    private bool _finishedGame;
    private bool _paused;
    private InputHandler _inputHandler;
    private void Start()
    {
        finishGameText.transform.localScale = Vector3.zero;
        _inputHandler = ServiceLocator.GetService<InputHandler>();
        _inputHandler.PausedAction += Paused;
    }

    private void OnEnable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.PausedAction += Paused;
        }
        PhotonNetwork.AddCallbackTarget(this);
        disconnectButton.onClick.AddListener(Disconnect);
        ServiceLocator.Subscribe<GameMenu>(this);
    }

    private void OnDisable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.PausedAction -= Paused;
        }
        PhotonNetwork.RemoveCallbackTarget(this);
        disconnectButton.onClick.RemoveListener(Disconnect);
        ServiceLocator.Unsubscribe<GameMenu>();
    }

    public void OnEvent(EventData photonEvent)
    {
        if(photonEvent.Code != (int) EventType.PlayerFindAllKeys) return;
        object[] data = (object[])photonEvent.CustomData;
        _finishedGame = true;
        gameCanvas.enabled = false;
        pausedCanvas.enabled = false;
        finishCanvas.enabled = true;
        finishGameText.transform.DOScale(1, 3);
        finishGameText.text = (string) data[0] +" WIN";
    }

    public void Paused()
    {
        if(_finishedGame) return;
        _paused = !_paused;
        gameCanvas.enabled = !_paused;
        pausedCanvas.enabled = _paused;
    }

    private void Disconnect()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }
}
