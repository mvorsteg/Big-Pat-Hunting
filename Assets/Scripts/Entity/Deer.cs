using UnityEngine;
using System.Collections;

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

    /// <summary>
    /// Calls the IdleStateSwitch Coroutine
    /// </summary>
    // protected override void IdleStateSwitch()
    // {
    //     StartCoroutine(IdleStateSwitch())
    // }

    /// <summary>
    /// Switches the animal between different idle states
    /// </summary>
    /// <param name="maxStates">The max number of idle states that will be visited</param>
    protected override IEnumerator IdleStateSwitch(int maxStates)
    {
        int state = Random.Range(0, 4);
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
                case 3 : 
                    duration = Random.Range(10f, 20f);
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
                state = (state + Random.Range(1, 4)) % 4;   // ensures a new state
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