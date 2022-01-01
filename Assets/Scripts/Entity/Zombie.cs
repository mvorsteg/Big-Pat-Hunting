using UnityEngine;

public class Zombie : Animal, IDamageSource
{
    public Entity target;
    public EntitySensor sensor;
    public Transform attackPoint;
    public float attackRange;
    public LayerMask mask;

    public float attackDamage = 25f;
    public float force = 100f;

    private bool targetInRange = false;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        SetState(AIState.Attack);
    }

    protected override void Update()
    {
        if (!isAlive)
        {
            return;
        }
        GetFollowDestination(target);
        Move();
        
        if (targetInRange)
        {
            if (sensor.IsEntityInRange(target))
            {
                anim.SetTrigger("attack");
            }
            else
            {
                targetInRange = false;
            }
        }

        base.Update();
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public bool IsPlayer()
    {
        return false;
    }

    public override string GetName()
    {
        return "Zombie";
    }

    /// <summary>
    /// If an entity walks close to the zombie, attack it
    /// </summary>
    /// <param name="other">The other entity that has been sensed by this one</param>
    public override void SenseEntity(Entity other)
    {
        if (isAlive && other == target)
        {
            targetInRange = true;
        }
    }

    /// <summary>
    /// Causes the attack trigger to enable and deal damage to anything close
    /// Called by an attack animation
    /// </summary>
    public void Attack()
    {
        //attackTrigger.enabled = true;
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, mask);

        foreach (Collider col in hitEnemies)
        {
            Entity e = col.GetComponent<Entity>();
            if (e != null && e != this && type.Attacks(e.type))
            {
                HitInfo info = new HitInfo(attackDamage, 0f, force, (col.transform.position - transform.position).normalized, this);
                e.TakeDamage(info);
            }
        }
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