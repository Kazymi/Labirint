using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private ChunkGenerator chunkGenerator;
    [SerializeField] private KeyManager keyManager;

    private void Start()
    {
        StartCoroutine(GetSeed());
    }

    private IEnumerator GetSeed()
    {
        yield return new WaitForSeconds(5f);
        if (Preloading.Preloaded == false)
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(0);
            yield break;
        }
    }
}