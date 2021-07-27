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
        yield return new WaitForSeconds(2f);
        chunkGenerator.StartCoroutine(chunkGenerator.StartGenerate());
        keyManager.StartKeyManager();
    }
}