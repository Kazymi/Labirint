using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
	[SerializeField] TMP_Text text;

	private Launcher _launcher;

	private void Awake()
	{
		_launcher = Launcher.Instance;
	}

	public RoomInfo info;

	public void SetUp(RoomInfo _info)
	{
		info = _info;
		text.text = _info.Name;
	}

	public void OnClick()
	{
		_launcher.JoinRoom(info);
	}
}