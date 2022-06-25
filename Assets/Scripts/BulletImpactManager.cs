using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;

public class BulletImpactManager : MonoBehaviour
{
    [System.Serializable]
    public struct BulletImpact
    {
        public MaterialType materialType;
        public ParticleSystem particleSystem;
    }

    [SerializeField]
    private List<BulletImpact> impactList;
    private Dictionary<MaterialType, ObjectPool<ParticleSystem>> impactDict;

    private UnityAction<object> onBulletImpact;
    private UnityAction<object> onBulletImpactBlood;

    private void Awake()
    {
        impactDict = new Dictionary<MaterialType, ObjectPool<ParticleSystem>>();
        // create object pool
        foreach (BulletImpact bi in impactList)
        {
            ObjectPool<ParticleSystem> pool = new ObjectPool<ParticleSystem>(
                () =>                                   // on create pooled object
                {
                    ParticleSystem x = Instantiate(bi.particleSystem, Vector3.zero, Quaternion.identity);
                    x.gameObject.SetActive(false);
                    return x;
                },
                x => x.gameObject.SetActive(true),      // on take from pool
                x => x.gameObject.SetActive(false),     // on return to pool
                x => Destroy(x.gameObject),             // on destroy pooled object
                false,
                20,
                20
            );
            impactDict.Add(bi.materialType, pool);
        }

        onBulletImpact = new UnityAction<object>(OnBulletImpact);
        onBulletImpactBlood = new UnityAction<object>(OnBulletImpactBlood);
    }

    private void OnEnable()
    {
        Messenger.Subscribe(MessageIDs.BulletImpact, onBulletImpact);
    }


    /// <summary>
    /// Creates a bullet impact particle effect at the hit position that matches the material
    /// of the object that was hit<br/>
    /// The ParticleSystem will be drawn from an object pool
    /// </summary>
    /// <param name="hit">The RayCastHit that defines what was hit and where</param>
    /// <param name="isBlood">True if the material is already known to be blood</param>
    /// <returns>The ParticleSystem that was created for this impact</returns>
    public ParticleSystem AddBulletImpact(RaycastHit hit, bool isBlood = false)
    {
        ParticleSystem particleSystem;
        MaterialType hitMaterial;
        // if use blood material
        if (isBlood)
        {
            hitMaterial = MaterialType.Blood;
        }
        // if object is tagged, use that material
        else if (hit.transform.CompareTag("Wood"))
        {
            hitMaterial = MaterialType.Wood;
        }
        else if (hit.transform.CompareTag("Stone"))
        {
            hitMaterial = MaterialType.Stone;
        }
        else if (hit.transform.CompareTag("Metal"))
        {
            hitMaterial = MaterialType.Metal;
        }
        else if (hit.transform.CompareTag("Cloth"))
        {
            hitMaterial = MaterialType.Cloth;
        }
        else if (hit.transform.CompareTag("Road"))
        {
            hitMaterial = MaterialType.Gravel;
        }
        // attempt to get material from terrain
        else
        {
            // check if hit tree
            if (hit.point.y > TerrainDetector.SampleHeight(hit.point) + 0.01f)
            {
                    hitMaterial = MaterialType.Wood;
            }
            else
            {
                // otherwise it hit the ground
                try
                {
                    hitMaterial = TerrainDetector.GetMaterialFromTextureIdx(TerrainDetector.GetActiveTerrainTextureIdx(hit.point));
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.StackTrace);
                    hitMaterial = MaterialType.Default;
                }
            }
        }
        // check if hit ground
        if (impactDict.ContainsKey(hitMaterial))
        {
            particleSystem = impactDict[hitMaterial].Get();
        }
        else
        {
            // take first material as default value
            particleSystem = impactDict[MaterialType.Default].Get(); 
        }
        particleSystem.transform.forward = hit.normal;
        particleSystem.transform.position = hit.point + particleSystem.transform.forward * 0.01f;
        return particleSystem;
    }

    private void OnBulletImpact(object data)
    {
        RaycastHit hit = (RaycastHit)data;
        AddBulletImpact(hit);
    }

    private void OnBulletImpactBlood(object data)
    {
        RaycastHit hit = (RaycastHit)data;
        AddBulletImpact(hit, true);
    }
}