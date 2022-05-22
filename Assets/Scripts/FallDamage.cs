using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour, IDamageSource
{
    public static FallDamage instance;
    public static float minVelocity = -18f;

    private static float minDamage;
    private static float maxDamage;

    private Transform hitPositon;

    private void Awake()
    {
        instance = this;    
    }

    public static void Initialize(float inMaxHealth)
    {
        maxDamage = inMaxHealth;
        minDamage = maxDamage / 3;    
    }

    public static HitInfo CalculateHit(Entity entity, float velocity)
    {
        instance.hitPositon = entity.transform;
        
        float distance = (velocity * velocity) / (2 * -9.81f);
        float damage = -4f * (velocity - minVelocity);
        damage = Mathf.Clamp(damage, minDamage, maxDamage);
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

    public string GetName()
    {
        return "Gravity";
    }
}
