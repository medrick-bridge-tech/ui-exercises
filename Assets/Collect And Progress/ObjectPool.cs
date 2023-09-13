using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    private List<GameObject> _pool;
    [Range(1, 50)] public int _poolSize = 20;
    [SerializeField] GameObject _objectToPool;
    [SerializeField] bool _dynamicSize = false;

    void Awake() {
        _pool = new List<GameObject>();

        if (_objectToPool == null) Debug.LogError("Object to pool is not set.");

        for(int i = 0; i < _poolSize; i++)
            GenerateObject();
    }

    GameObject GenerateObject() {
        GameObject go = Instantiate(_objectToPool, transform);
        _pool.Add(go);
        go.SetActive(false);
        return go;
    }

    public GameObject GetObject() {
        for(int i = 0; i < _poolSize; i++) {
            if (!_pool[i].activeInHierarchy) return _pool[i];
        }
        if (!_dynamicSize) return null;
        else return GenerateObject();
    }
}