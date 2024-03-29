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
        _objectPoolEmptyHolder = new GameObject("PooledObjects"); // Ǯ���� ������Ʈ���� ���� ���� ������ ������Ʈ
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        /// �̸� �������� �ش��ϴ� Ǯ�� ã��
        PooledObjectInfo pool = null;
        foreach (PooledObjectInfo p in ObjectPools)
        {
            if (p.LookupString == objectToSpawn.name)
            {
                pool = p;
                break;
            }
        }
        // PooledObjectInfo pool2 = ObjectPools.Find(p => p.LookupString == objectToSpawn.name); // ���ٽ� ����

        /// Ǯ�� �������� �ʴ´ٸ�, ������
        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        /// �����ִ� inactive ������Ʈ�� Ǯ�� �ִ��� Ȯ������
        GameObject spawnableObj = null;
        foreach (GameObject obj in pool.InactiveObjects)
        {
            if (obj != null)
            {
                spawnableObj = obj;
                break;
            }
        }
        //GameObject spawnableObj2 = pool.InactiveObjects.FirstOrDefault(); // ��ť ����

        if (spawnableObj == null) /// �ƹ��͵� ���ٴ� ���̹Ƿ� ��¥ Instantiate�� �������
        {
            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            spawnableObj.transform.SetParent(_objectPoolEmptyHolder.transform);
        }
        else /// ������� inactive ������Ʈ�� �ִٸ� transform ������ �ٲ㼭 ��ȯ�� ����
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
        // ������ ������Ʈ�� �پ��ִ� "(Clone)"�̶�� �̸��� ���ش�
        string goName = obj.name.Substring(0, obj.name.Length - 7);

        // �̸� �������� �ش��ϴ� Ǯ�� ã��
        PooledObjectInfo pool = null;
        foreach (PooledObjectInfo p in ObjectPools)
        {
            if (p.LookupString == goName)
            {
                pool = p;
                break;
            }
        }
        // PooledObjectInfo pool2 = ObjectPools.Find(p => p.LookupString == goName); // ���ٽ� ����

        // Ǯ�� �������� �ʴ´ٸ�, ������
        if (pool == null)
        {
            Debug.LogWarning("Ǯ������ ���� ������Ʈ�� �����Ϸ��� �մϴ�: " + obj.name);
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