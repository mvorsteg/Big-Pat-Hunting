using UnityEngine;
using System.Collections;

public class Deer : Animal, INoiseListener
{
    [SerializeField]
    protected int footstepsToDetection = 3; 
    [SerializeField]
    protected float detectionTimeoutSeconds = 5f, detectionResetSeconds = 3f, detectedPlayerSeconds;
    protected float detectionLevel = 0f, detectionRate, detectionTimer;

    public Vector2 peripheralCutoff;
    public LayerMask sightMask;

    [SerializeField]
    protected Transform head;
    protected Transform lookTarget;

    protected override void Awake()
    {
        base.Awake();
        detectionRate = 1f / footstepsToDetection;
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
        
        // move head if frozen
        if (state == AIState.Freeze && statusIndicator.Status == EntityStatus.Question && lookTarget != null)
        {
            // stare at noise source
            Vector2 lookDir = CalculateLookDir(lookTarget.position);
            // Debug.Log(lookDir);
            // check if can see player
            if (Mathf.Abs(lookDir.x) <= peripheralCutoff.x && Mathf.Abs(lookDir.y) <= peripheralCutoff.y)
            {

                RaycastHit hit;
                if (Physics.Linecast(head.position, lookTarget.position, out hit, sightMask))
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        // can see player
                        detectionTimer = detectionTimeoutSeconds;
                        detectionLevel += (1 / detectedPlayerSeconds) * Time.deltaTime;
                        detectionLevel = Mathf.Clamp01(detectionLevel);
                        statusIndicator.StatusLevel = detectionLevel;
                    }
                }
                anim.SetFloat("lookX", lookDir.x);
                anim.SetFloat("lookY", lookDir.y);
            }
            // cannot see player
            else
            {
                detectionTimer -= Time.deltaTime;
                detectionTimer = Mathf.Clamp(detectionTimer, 0, detectionTimeoutSeconds);
                if (detectionTimer <= 0)
                {
                    detectionLevel -= (Time.deltaTime / detectionResetSeconds);
                    detectionLevel = Mathf.Clamp01(detectionLevel);
                    statusIndicator.StatusLevel = detectionLevel;
                }
                if (detectionLevel <= 0)
                {
                    SetState(AIState.Idle);
                    statusIndicator.Status = EntityStatus.Normal;
                }
            }
            // if we're at max detection level, gtfo
            if (Mathf.Approximately(detectionLevel, 1f))
            {
                statusIndicator.Animate();
                Flee(lookTarget.position);
                lookTarget = null;
            }
        }

        


        base.Update();
    }

    /// <summary>
    /// Hears a noise and runs away
    /// </summary>
    /// <param name="generator">The noise generator that created the noise</param>
    public void HearNoise(NoiseInfo info)
    {
        if (isAlive && state != AIState.Flee)
        {
            // Flee(info.position);
            switch (info.noiseType)
            {
                case (NoiseType.CarHorn) :
                case (NoiseType.Footstep) :
                    // freeze in place
                    if (state != AIState.Freeze && state != AIState.Flee)
                    {
                        SetState(AIState.Freeze);
                        lookTarget = info.source.transform;
                    }
                    // set new status
                    if (statusIndicator.Status == EntityStatus.Normal)
                    {
                        statusIndicator.Status = EntityStatus.Question;
                    }
                    detectionLevel += detectionRate;
                    detectionLevel = Mathf.Clamp01(detectionLevel);
                    statusIndicator.StatusLevel = detectionLevel;
                    detectionTimer = detectionTimeoutSeconds;
                    break;
                case (NoiseType.Gunshot) :
                    detectionLevel = 1f;
                    break;
                    // Flee(info.position); // TODO do I really want deer to run away from car? 
            }
            statusIndicator.Animate();
            if (Mathf.Approximately(detectionLevel, 1f))
            {
                statusIndicator.Animate();
                Flee(info.position);
            }
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

    private Vector2 CalculateLookDir(Vector3 lookPos)
    {
        Vector3 dir = lookTarget.position - head.position;
        //dir = transform.InverseTransformDirection(dir).normalized;
        Vector3 horizontalDir = new Vector3(dir.x, 0, dir.z);
        Vector3 verticalDir = dir - horizontalDir;
        float horizontalAngle = Vector3.SignedAngle(horizontalDir, transform.forward, transform.up);
        float verticalAngle = Vector3.SignedAngle(horizontalDir, dir, transform.right);

        // Debug.DrawRay(head.position, transform.forward * 5, Color.blue);
        // Debug.DrawRay(head.position, horizontalDir, Color.magenta);
        // Debug.DrawRay(head.position, dir - horizontalDir, Color.green);
        // Debug.DrawRay(head.position, dir, Color.yellow);

        return new Vector2(-horizontalAngle, -verticalAngle);
    }

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