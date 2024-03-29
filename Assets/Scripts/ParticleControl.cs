using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControl : MonoBehaviour, IPoolable
{
    void Start()
    {
        //Invoke("ReturnThisToPool", 1.0F);
    }

    public void OnSpawn()
    {
        Invoke("ReturnThisToPool", 0.2F);
    }

    void ReturnThisToPool()
    {
        //Destroy(gameObject);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}