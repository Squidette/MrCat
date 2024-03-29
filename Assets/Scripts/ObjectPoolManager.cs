using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Unity.VisualScripting;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

    static GameObject _objectPoolEmptyHolder;

    void Awake()
    {
        _objectPoolEmptyHolder = new GameObject("PooledObjects"); // 풀링된 오브젝트들을 담을 폴더 역할의 오브젝트
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        /// 이름 기준으로 해당하는 풀을 찾자
        PooledObjectInfo pool = null;
        foreach (PooledObjectInfo p in ObjectPools)
        {
            if (p.LookupString == objectToSpawn.name)
            {
                pool = p;
                break;
            }
        }
        // PooledObjectInfo pool2 = ObjectPools.Find(p => p.LookupString == objectToSpawn.name); // 람다식 버전

        /// 풀이 존재하지 않는다면, 만들자
        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        /// 쓸수있는 inactive 오브젝트가 풀에 있는지 확인하자
        GameObject spawnableObj = null;
        foreach (GameObject obj in pool.InactiveObjects)
        {
            if (obj != null)
            {
                spawnableObj = obj;
                break;
            }
        }
        //GameObject spawnableObj2 = pool.InactiveObjects.FirstOrDefault(); // 링큐 버전

        if (spawnableObj == null) /// 아무것도 없다는 뜻이므로 진짜 Instantiate을 해줘야함
        {
            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            spawnableObj.transform.SetParent(_objectPoolEmptyHolder.transform);
        }
        else /// 대기중인 inactive 오브젝트가 있다면 transform 정보만 바꿔서 반환해 주자
        {
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        IPoolable poolable = spawnableObj.GetComponent<IPoolable>();
        if (poolable != null) poolable.OnSpawn();

        return spawnableObj;
    }

    public static void ReturnObjectToPool(GameObject obj)
    {
        // 복제된 오브젝트에 붙어있는 "(Clone)"이라는 이름을 빼준다
        string goName = obj.name.Substring(0, obj.name.Length - 7);

        // 이름 기준으로 해당하는 풀을 찾자
        PooledObjectInfo pool = null;
        foreach (PooledObjectInfo p in ObjectPools)
        {
            if (p.LookupString == goName)
            {
                pool = p;
                break;
            }
        }
        // PooledObjectInfo pool2 = ObjectPools.Find(p => p.LookupString == goName); // 람다식 버전

        // 풀에 존재하지 않는다면, 만들자
        if (pool == null)
        {
            Debug.LogWarning("풀링되지 않은 오브젝트를 해제하려고 합니다: " + obj.name);
        }
        else
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }
}

public class PooledObjectInfo
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}

public interface IPoolable
{
    void OnSpawn();
}