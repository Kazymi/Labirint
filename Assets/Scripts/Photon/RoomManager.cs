using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks,IOnEventCallback
{

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	public override void OnEnable()
	{
		base.OnEnable();
		PhotonNetwork.AddCallbackTarget(this);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public override void OnDisable()
	{
		base.OnDisable();
		PhotonNetwork.RemoveCallbackTarget(this);
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
	
	private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		if(scene.buildIndex == 1)
		{
			PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
		}
	}
	public void OnEvent(EventData photonEvent)
	{
		byte eventCode = photonEvent.Code;
		if (eventCode != (int) EventType.PlayerFindAllKeys) return;
		Destroy(gameObject);
	}
}