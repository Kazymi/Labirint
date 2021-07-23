using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


public class PlayerStatistics : MonoBehaviour
{
    private float _foundedKeys;
    private GameStatisticMenu _gameStatisticMenu;
    private float _needToFindKeys;
    private PhotonView _photonView;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        _gameStatisticMenu = ServiceLocator.GetService<GameStatisticMenu>();
        _gameStatisticMenu.UpdateKeySlider(0);
    }

    private void OnEnable()
    {
        if (_photonView.IsMine)
            ServiceLocator.Subscribe<PlayerStatistics>(this);
    }

    private void OnDisable()
    {
        if (_photonView.IsMine)
            ServiceLocator.Unsubscribe<PlayerStatistics>();
    }

    public void Initialize(GameManager gameManager)
    {
        _needToFindKeys = gameManager.NeedToFindKeys;
    }

    public void AddFoundKey()
    {
        _foundedKeys++;
        Debug.Log(_foundedKeys);
        Debug.Log(_needToFindKeys);
        Debug.Log(_foundedKeys / _needToFindKeys);
        _gameStatisticMenu.UpdateKeySlider(_foundedKeys / _needToFindKeys);
        if (_foundedKeys >= _needToFindKeys)
        {
            object[] content = new object[]{_photonView.Controller.NickName};
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.All};
            PhotonNetwork.RaiseEvent((int) EventType.PlayerFindAllKeys, content, raiseEventOptions,
                SendOptions.SendReliable);
        }
    }
}