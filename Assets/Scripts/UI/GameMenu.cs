using System;
using DG.Tweening;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class GameMenu : MonoBehaviour,IOnEventCallback
{
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private Canvas finishCanvas;
    [SerializeField] private TMP_Text finishGameText;

    private void Start()
    {
        finishGameText.transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        if(photonEvent.Code != (int) EventType.PlayerFindAllKeys) return;
        object[] data = (object[])photonEvent.CustomData;
        gameCanvas.enabled = false;
        finishCanvas.enabled = true;
        finishGameText.transform.DOScale(1, 3);
        finishGameText.text = (string) data[0] +" WIN";
    }
}
