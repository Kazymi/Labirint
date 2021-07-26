using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    private Launcher _launcher;
    private RoomInfo _info;

    private void Awake()
    {
        _launcher = Launcher.Instance;
    }

    public void SetUp(RoomInfo info)
    {
        _info = info;
        text.text = info.Name;
    }

    public void OnClick()
    {
        _launcher.JoinRoom(_info);
    }
}