using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;

public class ParticlePool : MonoBehaviour
{
    private ObjectPool<ParticleSystem> pool;
    [SerializeField]
    private ParticleSystem prefab;

    private void Awake()
    {
        pool = new ObjectPool<ParticleSystem>(CreatePooledObject, OnTakeFromPool, OnReturnToPool, OnDestroyObject, false, 20, 20);
    }

    /// <summary>
    /// gets a particlesystem from the pool
    /// </summary>
    /// <returns></returns>
    public ParticleSystem Get()
    {
        return pool.Get();
    }

    /// <summary>
    /// returns a particlesystem to the pool
    /// </summary>
    /// <param name="obj"></param>
    public void Release(ParticleSystem obj)
    {
        pool.Release(obj);
    }    

    private ParticleSystem CreatePooledObject()
    {
        ParticleSystem instance = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        instance.gameObject.SetActive(false);
        return instance;
    }

    private void OnTakeFromPool(ParticleSystem instance)
    {
        instance.gameObject.SetActive(true);
    }

    private void OnReturnToPool(ParticleSystem instance)
    {
        instance.gameObject.SetActive(false);
    }

    private void OnDestroyObject(ParticleSystem instance)
    {
        Destroy(instance.gameObject);
    }
}