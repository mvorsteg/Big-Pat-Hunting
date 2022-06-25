using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public enum AIState
    {
        Idle,       // No movement
        Wander,     // Wandering aimlessly
        Freeze,     // Freeze in place
        Flee,       // Running away from immediate danger
        Investigate,// Investigating a noise or something
        Follow,     // Following another entity
        Attack,     // Attacking another entity
    }

    public float maxHealth = 100;
    public EntityType type;

    protected bool isAlive = true;
    protected float health;
    
    protected Animator anim;

    public bool IsAlive { get => isAlive; }
    public float Health { get => health; }

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public virtual void SenseEntity(Entity other)
    {

    }

    public virtual Vector3 GetPosition()
    {
        return transform.position;
    }

    public virtual string GetName()
    {
        return transform.name;
    }

    /// <summary>
    /// Deals damage to the entity from the source specified.
    /// If the damage causes the entity, to drop below 0 health, it will die
    /// </summary>
    /// <param name="info">The HitInfo struct that contains data for this hit.</param>
    public virtual void TakeDamage(HitInfo info)
    {
        if (isAlive)
        {
            Debug.Log(transform.name + " took " + info.damage + " damage");
            health -= info.damage;
            if (health <= 0)
            {
                Die(info);
            }
        }
        
    }

    protected virtual void Die(HitInfo info)
    {
        isAlive = false;
        if (info.source.IsPlayer())
        {
            //Player p = (Player)info.source;
            QuestManager.RegisterKill(this, info);
        }
    }
}
