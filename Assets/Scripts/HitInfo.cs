using UnityEngine;

/// <summary>
/// A structure containing data about a hit on an entity
/// </summary>
public struct HitInfo
{
    public IDamageSource source;
    public Vector3 direction;
    public float damage;
    public float distance;
    public float force;

    public HitInfo(float damage, float distance, float force, Vector3 direction, IDamageSource source)
    {   
        this.damage = damage;
        this.distance = distance;
        this.force = force;
        this.direction = direction;
        this.source = source;
    } 
}