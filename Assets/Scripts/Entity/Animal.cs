using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Animal : Entity
{
    public float walkSpeed = 1.5f;
    public float runSpeed = 7f;
    public float rotationSpeed = 50f;
    
    public float safeDistance = 15f;
    public float stoppingDistance = 0.1f;

    public Animal leader;
    protected List<Animal> followers;

    protected AIState state;
    protected Rigidbody rb;

    protected float maxSpeed;
    protected float currentSpeed;
    protected float smoothSpeed;

    protected NavMeshAgent agent;   
    protected NavMeshPath path;

    protected int pathIter = 1;
    protected Vector3 agentPosition;
    protected Vector3 destination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
    protected Vector3 endDestination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
    protected Queue<Vector3> destinationQueue = new Queue<Vector3>(2);

    public AIState State { get => state; set => state = value; }

    protected override void Awake() 
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        agent.isStopped = true;
        path = new NavMeshPath();
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.deltaTime * 8f);
        anim.SetFloat("speedX", currentSpeed);
    }

    /// <summary>
    /// Moves the animal according to what its NavMeshAgent wants, but with a wide turn radius
    /// Will exit early if the path is invalid or finished
    /// </summary>
    protected void Move()
    {
        SetAgentPosition();
        // if there was a problem making the path, get out
        if (path.corners == null || path.corners.Length == 0)
            return;

        // if we have reached the end, stop
        if (pathIter >= path.corners.Length)
        {
            ArriveAtDestination();
            return;
        }
        // else, the current destination is the next waypoint
        else
        {
            destination = path.corners[pathIter];
        }
        // now tell the agent how to get to the next destination
        // check to make sure we have valid data
        if (destination.x < float.PositiveInfinity)
        {
            // look at destination
            Vector3 direction = destination - agentPosition;
            var newDir = Vector3.RotateTowards(transform.forward, direction, rotationSpeed * Time.deltaTime, 0.0f);
            var newRot = Quaternion.LookRotation(newDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, Time.deltaTime * 2f);
            // if not close to next destination, set movement to go that direction
            float distance = Vector3.Distance(agentPosition, destination);
            if (distance > agent.radius + stoppingDistance)
            {
                Vector3 movement = currentSpeed * transform.forward * Time.deltaTime;
                agent.Move(movement);
            }
            // if close, incremenet pathIter to go to next waypoint, or finish if the path is over
            else
            {
                pathIter++;
                if (pathIter >= path.corners.Length)
                {
                    ArriveAtDestination();
                }
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
    /// changes the state of the AI
    /// </summary>
    /// <param name="state">the new AIState</param>
    protected void SetState(AIState state)
    {
        this.state = state;
        anim.SetBool("idle", false);
        switch (state)
        {
            case AIState.Idle :
                anim.SetBool("idle", true);
                maxSpeed = 0;
                StartCoroutine(IdleStateSwitch(Random.Range(2, 3)));
                StartCoroutine(GenerateRandomDestinations(Random.Range(2, 8), Random.Range(5, 10), transform.position));
                if (followers != null)
                {
                    foreach (Animal f in followers)
                    {
                        f.SetState(AIState.Idle);
                    }
                }
                break;
            case AIState.Wander :
                maxSpeed = walkSpeed;
                agent.speed = walkSpeed;
                NextDestination();
                // have followers tag along
                if (followers != null)
                {
                    foreach (Animal f in followers)
                    {
                        f.SetState(AIState.Follow);
                    }
                }
                break;
            case AIState.Flee :
                maxSpeed = runSpeed;
                agent.speed = runSpeed;
                break;
            case AIState.Follow :
                maxSpeed = walkSpeed;
                agent.speed = walkSpeed;
                break;
            case AIState.Attack :
                maxSpeed = runSpeed;
                agent.speed = runSpeed;
                break;
        }
    }

    /// <summary>
    /// Causes the animal to run away until it has reached its safe distance.
    /// Then, it takes off in a random direction
    /// </summary>
    /// <param name="source">The position of the thing the animal is fleeing from</param>
    protected void Flee(Vector3 source)
    {
        destinationQueue.Clear();
        //ArriveAtDestination();
        SetState(AIState.Flee);
        // check if the animal is running toward danger. otherwise, just run forward
        float difference = Vector3.Angle((source - transform.position).normalized, transform.forward);
        Vector3 direction;
        if (difference >= 45f)
        {
            // run forward
            direction = transform.forward;
        }
        else
        {
            // turn away from danger
            direction = transform.position - source;
            direction.y = 0;
            direction = direction.normalized;
        }
        // find position to run to
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(transform.position + safeDistance * direction, out navHit, 5f, NavMesh.AllAreas))
        {
            //agent.SetDestination(navHit.position);
            destinationQueue.Enqueue(navHit.position);
            NextDestination();
            StartCoroutine(GenerateRandomDestinations(3, safeDistance * 2, navHit.position));
        }

        
    }

    /// <summary>
    /// Deals damage to the entity from the source specified.
    /// If the damage causes the entity, to drop below 0 health, it will die
    /// </summary>
    /// <param name="info">The HitInfo struct that contains data for this hit.</param>
    public override void TakeDamage(HitInfo info)
    {
        base.TakeDamage(info);
    }

    /// <summary>
    /// Causes the animal to stop moving, fall over, and be dead
    /// </summary>
    /// <param name="info">The HitInfo struct that contains data for the thing that killed it</param>
    protected override void Die(HitInfo info)
    {
        base.Die(info);
        anim.SetTrigger("die");
        agent.isStopped = true;
        agent.enabled = false;
        rb.isKinematic = false;
    }

    /// <summary>
    /// Registers a new animal to follow this one
    /// </summary>
    /// <param name="follower">The animal that is following</param>
    public void AddFollower(Animal follower)
    {
        if (followers == null)
        {
            followers = new List<Animal>();
        }
        followers.Add(follower);
    }

    /// <summary>
    /// Updates AgentPosition to keep it up with where the transform is on the navMesh
    /// </summary>
    protected void SetAgentPosition()
    {
        NavMeshHit hit;
        if(NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            agentPosition = hit.position;
        }
    }

    public override Vector3 GetPosition()
    {
        return agentPosition;
    }

    /// <summary>
    /// Gets the next destination from the queue and starts going there
    /// </summary>
    protected void NextDestination()
    {
        Vector3 pos = destinationQueue.Dequeue();
        path = new NavMeshPath();
        endDestination = pos;
        agent.CalculatePath(endDestination, path);
        pathIter = 1;
        agent.isStopped = false;
    }
    
    /// <summary>
    /// gets the position of the leader we are following
    /// </summary>
    /// <param name="leader">The animal that we are following</param>
    protected void GetFollowDestination(Entity leader)
    {
        Vector3 pos = leader.GetPosition();
        path = new NavMeshPath();
        endDestination = pos;
        agent.CalculatePath(endDestination, path);
        pathIter = 1;
        agent.isStopped = false;
    }

    /// <summary>
    /// Arrives at the current destination
    /// If there is another to go to, go there. Else, stop moving and be idle
    /// </summary>
    protected void ArriveAtDestination()
    {
        if (destinationQueue.Count > 0)
        {
            NextDestination();
        }
        else
        {
            destination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            agent.isStopped = true;
            SetState(AIState.Idle);
        }
    }

    /// <summary>
    /// Populates the queue with random destinations on the navMesh
    /// Limit 1 per frame in case there is difficulty getting destinations
    /// </summary>
    /// <param name="num">The number of destinations to generate</param>
    /// <param name="distance">The max radius to look for a destination</param>
    /// <param name="start">The starting position for the first destination</param>
    /// <returns></returns>
    protected IEnumerator GenerateRandomDestinations(int num, float distance, Vector3 start)
    {
        Vector3 pos = start;
        // generate num of positions
        int attempts = 0;
        for (int i = 0; i < num; i++)
        {
            Vector3 nextPos = RandomNavCircle(pos, distance, NavMesh.AllAreas);
            // if we got a position
            if (nextPos != pos)
            {
                destinationQueue.Enqueue(nextPos);
                pos = nextPos;
                //Debug.Log("pos " + i);
                yield return null;
            }
            else if (attempts < 20)
            {
                i--;
                attempts++;
                //Debug.Log("bad attempt");
                yield return null;
            }
            else
            {
                i = num;
            }
        }
    }

    /// <summary>
    /// Switches the animal between different idle states
    /// </summary>
    protected IEnumerator IdleStateSwitch(int maxStates)
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
            state = (state + Random.Range(1, 4)) % 4;   // ensures a new state
            if (leader == null)
                count++;
        }
        // if still idle, do some wandering
        if (this.state == AIState.Idle)
        {
            SetState(AIState.Wander);
        }
    }

    /// <summary>
    /// Finds a point on the navmesh within a radius
    /// </summary>
    /// <param name="origin">The center of the circle to look in</param>
    /// <param name="distance">The radius of the circle</param>
    /// <param name="layermask">Area Layermask</param>
    /// <returns></returns>
    public static Vector3 RandomNavCircle (Vector3 origin, float distance, int layermask) 
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitCircle.normalized * distance;
        randomDirection += origin;
        NavMeshHit navHit;
        if (NavMesh.SamplePosition (randomDirection, out navHit, distance, layermask))
        {
            return navHit.position;
        }
        return origin;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if(path != null && path.corners != null && path.corners.Length > 0)
        {
            var prev = agentPosition;
            for(int i = pathIter; 
                i < path.corners.Length; ++i)
            {
                Gizmos.DrawLine(prev, path.corners[i]);
                prev = path.corners[i];
            }
        }
    }
}
