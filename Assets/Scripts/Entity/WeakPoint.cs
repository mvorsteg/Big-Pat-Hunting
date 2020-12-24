using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour
{

    public Entity parent;                   // entity this weak point is attached to 
    public float damageMultiplier = 1.0f;    // damage multiplier when damaged from this point
    
    // Start is called before the first frame update
    void Start()
    {

    }

    /*  applies damage to the entity after applying the multiplier */
    public void TakeDamage(HitInfo info)
    {
        info.damage *= damageMultiplier;
        parent.TakeDamage(info);
    }
}
