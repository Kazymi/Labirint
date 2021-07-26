using System.Collections;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PhotonView))]
public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField] private int needToFindKeys = 3;

    // TODO: unused
    private PhotonView _photonView;
    // TODO: unused
    private bool _gameFineshed;
    
    public int NeedToFindKeys => needToFindKeys;

    public override void OnEnable()
    {
        ServiceLocator.Subscribe<GameManager>(this);
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        ServiceLocator.Unsubscribe<GameManager>();
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }

    public void OnEvent(EventData photonEvent)
    {
        var eventCode = photonEvent.Code;
        if (eventCode != (int)EventType.PlayerFindAllKeys)
        {
            return;
        }
        PlayerWin();
    }

    private void PlayerWin()
    {
        _gameFineshed = true;
        StartCoroutine(Disconnect());
    }

    private IEnumerator Disconnect()
    {
        yield return new WaitForSeconds(7f);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
}