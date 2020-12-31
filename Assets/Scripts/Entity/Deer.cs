using UnityEngine;

public class Deer : Animal, INoiseListener
{
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
    /// <summary>
    /// Hears a noise and runs away
    /// </summary>
    /// <param name="generator">The noise generator that created the noise</param>
    public void HearNoise(NoiseGenerator generator)
    {
        if (isAlive)
        {
            Flee(generator.GetPosition());
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
            if (isAlive)
            {
                Flee(info.source.GetTransform().position);
            }
        }
    }    
}