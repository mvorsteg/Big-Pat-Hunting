using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wolf : Animal, INoiseListener, IDamageSource
{
    public NoiseGenerator noiseGenerator;

    public Transform attackPoint;
    public float attackRange;
    public LayerMask mask;

    public float attackDamage = 25f;
    public float force = 100f;

    protected Entity target;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        if (leader != null)
        {
            //SetState(AIState.Follow);
            leader.AddFollower(this);
            agent.stoppingDistance = Random.Range(1f, 2f);
        }
        SetState(AIState.Idle);
    }


    protected override void Update()
    {
        if (!isAlive)
        {
            return;
        }
        // if animal is following
        if (state == AIState.Follow && leader.State != AIState.Idle)
        {
            GetFollowDestination(leader);
        }
        if (state == AIState.Attack)
        {
            if (target != null && target.IsAlive)
            {
                GetFollowDestination(target);
                if (Vector3.Distance(attackPoint.position, target.transform.position) <= attackRange)
                {
                    anim.SetTrigger("attack");
                }
            }
        }
        // if animal needs to move
        if (state != AIState.Idle)
        {
            Move();
        }
        // animal is idle
        else
        {

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

    /// <summary>
    /// Hears a noise and runs away
    /// </summary>
    /// <param name="generator">The noise generator that created the noise</param>
    public void HearNoise(NoiseGenerator generator)
    {
        if (isAlive)
        {
            Investigate(generator.GetPosition());
        }
    }

    /// <summary>
    /// If this entity is dangerous, run away
    /// </summary>
    /// <param name="other">The other entity that has been sensed by this one</param>
    public override void SenseEntity(Entity other)
    {
        if (isAlive && type.RunsFrom(other.type))
        {
            Flee(other.transform.position);
        }
        else if (isAlive && type.Attacks(other.type))
        {
            // SetTarget(other);
            target = other;
            SetState(AIState.Attack);
        }
    }

    public void SetTarget(Entity entity)
    {
        GetFollowDestination(entity);
    }

    /// <summary>
    /// Causes the attack trigger to enable and deal damage to anything close
    /// Called by an attack animation
    /// </summary>
    public void Attack()
    {
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
    
    /// <summary>
    /// Switches the animal between different idle states
    /// </summary>
    /// <param name="maxStates">The max number of idle states that will be visited</param>
    protected override IEnumerator IdleStateSwitch(int maxStates)
    {
        int state = Random.Range(0, 3);
        int count = 0;
        float duration;
        while (this.state == AIState.Idle && count < maxStates)
        {
            anim.SetInteger("idleState", state);
            switch (state)
            {   
                case 0 :
                    duration = Random.Range(3.9f, 12.2f);
                    break;
                case 1 :
                    duration = Random.Range(5.9f, 14.4f);
                    break;
                case 2 :
                    duration = Random.Range(2.9f, 5.9f);
                    break;
                default :
                    // shouldnt be reached tbh
                    duration = 2f;
                    break;
            }
            //Debug.Log("state : " + state + " duration : " + duration);
            yield return new WaitForSeconds(duration);
            if (isAlive)
            {
                state = (state + Random.Range(1, 3)) % 3;   // ensures a new state
                if (leader == null)
                    count++;
            }
        }
        // if still idle, do some wandering
        if (this.state == AIState.Idle)
        {
            SetState(AIState.Wander);
        }
    }        
}
