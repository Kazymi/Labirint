using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class PoolPhoton
{
    private List<GameObject> _pooledObjects = new List<GameObject>();
    private Transform _parent;
    private readonly string _nameGameObject;

    public PoolPhoton(string nameGameObject, int count, Transform parent)
    {
        _parent = parent;
        _nameGameObject = nameGameObject;
        Prepolulate(count, nameGameObject);
    }

    public GameObject Pull()
    {
        GameObject returnValue;
        if (_pooledObjects.Count == 0) returnValue = CreateNewElement(_nameGameObject);
        else
        {
            returnValue = _pooledObjects[0];
            _pooledObjects.RemoveAt(0);
        }

        returnValue.SetActive(true);
        return returnValue;
    }

    public void Prepolulate(int count, string nameGameObject)
    {
        for (int i = 0; i != count; i++)
        {
            _pooledObjects.Add(CreateNewElement(nameGameObject));
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

    private GameObject CreateNewElement(string nameGameObject)
    {
        var newElement = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs",nameGameObject), Vector3.zero, Quaternion.identity);
        newElement.SetActive(false);
        return newElement;
    }
}