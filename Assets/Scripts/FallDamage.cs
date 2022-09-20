using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour, IDamageSource
{
    public static FallDamage instance;
    public static float minVelocity = -18f;

    public float safeHeight = 3f;
    public float killHeight = 10f;

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

        // safeVelocity = instance.safeHeight * -9.81f;
        // killVelocity = instance.killHeight * -9.81f; 
    }

    public static HitInfo CalculateHit(Entity entity, float velocity)
    {
        instance.hitPositon = entity.transform;
        
        float velocityAdjusted = velocity + 2f;
        float height = Mathf.Abs((velocityAdjusted * velocityAdjusted) / (2f * -9.81f));
        // float damage = -4f * (velocity - minVelocity);
        // damage = Mathf.Clamp(damage, minDamage, maxDamage);
        float damage = 0;
        if (height > instance.safeHeight)
        {
            float heightNormalized = height / instance.killHeight;
            damage = Mathf.Lerp(minDamage, maxDamage, heightNormalized);
        }
        HitInfo info = new HitInfo(damage, height, 0f, Vector3.down, instance);
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
