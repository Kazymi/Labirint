using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Pool
{
    private List<GameObject> _pooledObjects = new List<GameObject>();
    private Transform _parent;
    private readonly GameObject _elementSpawn;

    public Pool(GameObject element, int count, Transform parent)
    {
        _parent = parent;
        _elementSpawn = element;
        Prepolulate(count);
    }

    public GameObject Pull()
    {
        GameObject returnValue;
        if (_pooledObjects.Count == 0) returnValue = CreateNewElement();
        else
        {
            returnValue = _pooledObjects[0];
            _pooledObjects.RemoveAt(0);
        }

        returnValue.SetActive(true);
        return returnValue;
    }

    public void Prepolulate(int count, GameObject spawnGameObject)
    {
        for (int i = 0; i != count; i++)
        {
            _pooledObjects.Add(CreateNewElement(spawnGameObject));
        }
    }

    public void Mix()
    {
        for (int i = _pooledObjects.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = _pooledObjects[j];
            _pooledObjects[j] = _pooledObjects[i];
            _pooledObjects[i] = temp;
        }
    }

    public void Push(GameObject element)
    {
        element.SetActive(false);
        element.transform.parent = _parent;
        _pooledObjects.Add(element);
    }

    private GameObject CreateNewElement()
    {
        var newElement = GameObject.Instantiate(_elementSpawn, _parent, true);
        newElement.SetActive(false);
        return newElement;
    }

    private GameObject CreateNewElement(GameObject spawnObject)
    {
        var newElement = GameObject.Instantiate(spawnObject, _parent, true);
        newElement.SetActive(false);
        return newElement;
    }
    
    private void Prepolulate(int count)
    {
        for (int i = 0; i != count; i++)
        {
            _pooledObjects.Add(CreateNewElement());
        }
    }
}