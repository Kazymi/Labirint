using UnityEngine;

public class FactoryPhoton
{
    private int _countElement;

    private PoolPhoton _pool;

    public FactoryPhoton(string spawnElementName, int countElement, Transform parent)
    {
        _countElement = countElement;
        _pool = new PoolPhoton(spawnElementName, _countElement, parent);
    }
    
    public GameObject Create()
    {
        return _pool.Pull();
    }

    public void Mix()
    {
        _pool.Mix();
    }

    public void Prepolulate(string nameGameObject, int count)
    {
        _pool.Prepolulate(count, nameGameObject);
    }

    public void Destroy(GameObject gameObject)
    {
        _pool.Push(gameObject);
    }
}