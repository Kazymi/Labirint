using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameInput;

    private const string _saveName = "Nickname";

    private void OnEnable()
    {
        usernameInput.onValueChanged.AddListener(UpdateNickName);
    }

    private void OnDisable()
    {
        usernameInput.onValueChanged.RemoveListener(UpdateNickName);
    }

    private void Start()
    {
        var nick = PlayerPrefs.GetString(_saveName);
        if (string.IsNullOrEmpty(nick))
        {
            nick = "Player" + Random.Range(0, 1000);
        }
        UpdateNickName(nick);
    }

    private void UpdateNickName(string newText)
    {
        usernameInput.text = newText;
        PhotonNetwork.NickName = newText;
        PlayerPrefs.SetString(_saveName, newText);
    }
}