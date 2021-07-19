using System.Collections.Generic;
using UnityEngine;

public class Factory
{
    private int _countElement;

    private Pool pool { get; set; }

    public Factory(GameObject spawnElement,int countElement, Transform parent)
    {
        _countElement = countElement;
        pool = new Pool(spawnElement, _countElement, parent);
    }

    public GameObject Create()
    {
        return pool.Pull();
    }

    public List<GameObject> CreateMany(int count)
    {
        var returnValues = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
           returnValues.Add( pool.Pull());
        }

        return returnValues;
    }

    public void Mix()
    {
        pool.Mix();
    }
    public void Prepolulate(GameObject gameObject, int count)
    {
        pool.Prepolulate(count,gameObject);
    }

    public void Destroy(GameObject gameObject)
    {
        pool.Push(gameObject);
    }
}