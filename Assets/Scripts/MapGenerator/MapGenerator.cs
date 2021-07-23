using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private SeedGenerator seedGenerator;
    [SerializeField] private ChunkGenerator chunkGenerator;
    [SerializeField] private KeyManager keyManager;

    private void Start()
    {
        StartCoroutine(GetSeed());
    }

    private IEnumerator GetSeed()
    {
        var limit = 10;
        while (seedGenerator.Seed == 0 && limit-- > 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        if (seedGenerator.Seed == 0)
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(0);
        }
        chunkGenerator.StartCoroutine(chunkGenerator.StartGenerate());
        keyManager.StartKeyManager();
    }
}