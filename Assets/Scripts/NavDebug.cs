using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavDebug : MonoBehaviour
{

    public Transform dest;
    private NavMeshAgent agent;
    private Rigidbody rb; 

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        agent.updatePosition = false;
        agent.SetDestination(dest.position);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("desired " + agent.desiredVelocity + " actual " + agent.velocity);
        rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * agent.velocity.magnitude, Time.deltaTime * 8f);

        Quaternion newRot = Quaternion.LookRotation(agent.velocity.normalized);
        rb.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * 2f);
        agent.nextPosition = transform.position;
    }
}
