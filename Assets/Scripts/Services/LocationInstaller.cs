using UnityEngine;
using Zenject;

public class LocationInstaller : MonoInstaller
{
    [SerializeField] private SpawnManager _spawnManager;

    public override void InstallBindings()
    {
       CreateSpawnPoint();
    }

    private void CreateSpawnPoint()
    {
        Debug.Log("Spawn manager");
        Container
            .Bind<SpawnManager>()
            .FromInstance(_spawnManager)
            .AsSingle();
    }
}
