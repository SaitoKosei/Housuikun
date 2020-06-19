using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> _poolObjects;
    private GameObject _poolObj;

    //オブジェクトプールを作成
    public void CreatePool(GameObject obj, int maxCount)
    {
        _poolObj = obj;
        _poolObjects = new List<GameObject>();
        for(int i = 0; i < maxCount; i++)
        {
            var newObj = CreateNewObject();
            newObj.SetActive(false);
            _poolObjects.Add(newObj);
        }
    }

    public GameObject GetObject()
    {
        //使用中でないものを探して禁止
        foreach (var obj in _poolObjects)
        {
            if (obj.activeSelf == false)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        //全て使用中だったら新しく通って返す
        var newObj = CreateNewObject();
        newObj.SetActive(false);
        _poolObjects.Add(newObj);
        return newObj;
    }

    private GameObject CreateNewObject()
    {
        var newObj = Instantiate(_poolObj);
        newObj.name = _poolObj.name + (_poolObjects.Count + 1);
        return newObj;
    }
}
