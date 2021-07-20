using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryPhoton
{
    private int _countElement;

    private PoolPhoton pool { get; set; }

    public FactoryPhoton(string spawnElementName, int countElement, Transform parent)
    {
        _countElement = countElement;
        pool = new PoolPhoton(spawnElementName, _countElement, parent);
    }

    public GameObject Create()
    {
        return pool.Pull();
    }

    public void Mix()
    {
        pool.Mix();
    }

    public void Prepolulate(string nameGameObject, int count)
    {
        pool.Prepolulate(count, nameGameObject);
    }

    public void Destroy(GameObject gameObject)
    {
        pool.Push(gameObject);
    }
}