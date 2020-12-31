using UnityEngine;

public class Zombie : Animal
{
    public Entity target;
    public Collider attackTrigger;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        state = AIState.Attack;
        maxSpeed = walkSpeed;
        agent.speed = walkSpeed;
    }

    protected override void Update()
    {
        Debug.Log("updage");
        if (!isAlive)
        {
            return;
        }
        GetFollowDestination(target);
        Move();
        
        base.Update();
    }

    /// <summary>
    /// If an entity walks close to the zombie, attack it
    /// </summary>
    /// <param name="other">The other entity that has been sensed by this one</param>
    public override void SenseEntity(Entity other)
    {
        if (isAlive && type.Attacks(other.type))
        {
            anim.SetTrigger("attack");
        }
    }

    /// <summary>
    /// Causes the attack trigger to enable and deal damage to anything close
    /// Called by an attack animation
    /// </summary>
    public void Attack()
    {
        attackTrigger.enabled = true;
    }

    /// <summary>
    /// Deals damage to the entity from the source specified.
    /// If the damage causes the entity, to drop below 0 health, it will die
    /// </summary>
    /// <param name="info">The HitInfo struct that contains data for this hit.</param>
    public override void TakeDamage(HitInfo info)
    {
        if (isAlive)
        {
            base.TakeDamage(info);
        }
    }    
}