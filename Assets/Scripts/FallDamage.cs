using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour, IDamageSource
{
    public static FallDamage instance;
    public static float minVelocity = -15f;

    private Transform hitPositon;

    private void Awake()
    {
        instance = this;    
    }

    public static HitInfo CalculateHit(Entity entity, float velocity)
    {
        instance.hitPositon = entity.transform;
        
        float distance = (velocity * velocity) / (2 * -9.81f);
        float damage = -4f * (velocity - minVelocity);
        HitInfo info = new HitInfo(damage, distance, 0f, Vector3.down, instance);
        return info;
    }

    public Transform GetTransform()
    {
        return hitPositon;
    }

    public bool IsPlayer()
    {
        return false;
    }
}
