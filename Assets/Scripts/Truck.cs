using System.Collections.Generic;
using UnityEngine;
using EasyRoads3Dv3;

[RequireComponent(typeof(Rigidbody))]
public class Truck : MonoBehaviour, IDamageSource
{
    public List<Vector3> waypoints;
    private Queue<Vector3> waypointQueue;

    public float movementSpeed = 20f;
    public float rotationSpeed = 10f;
    public float stoppingDistance = 3f;

    private Vector3 nextDest;

    private Rigidbody rb;
    public Transform frontGroundCheck, backGroundCheck;
    private LayerMask mask;
    private float angle;

    private bool isDriving = true;

    public AudioClip engineClip, hornClip, hitClip;
    private AudioSource source;

    public GameObject[] wheels;
    public float wheelRotationSpeed = 50f;
    

    private void Awake()
    {
        waypointQueue = new Queue<Vector3>(waypoints.Count); // we take those optimizations where we can get em yes sir
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
        mask = LayerMask.GetMask("Ground") | LayerMask.GetMask("Default");
    }

    private void Start()
    {
        foreach (Vector3 v in waypoints)
        {
            waypointQueue.Enqueue(v);
        }
        if (waypointQueue.Count > 0)
        {
            nextDest = waypointQueue.Dequeue();
        }
        source.clip = engineClip;
        source.loop = true;
        source.Play();
    }

    private void Update()
    {
        if (isDriving)
        {
            float dist = Vector3.Distance(transform.position, nextDest);
            if (dist < stoppingDistance)
            {
                if (waypointQueue.Count > 0)
                {
                    nextDest = waypointQueue.Dequeue();
                }
            }

            // rotate
            Vector3 dir = nextDest - transform.position;
            //dir.y = -normal;
            Quaternion newRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * rotationSpeed);

            // get angle of surface
            RaycastHit frontHit, backHit;
            if (Physics.Raycast(frontGroundCheck.position, Vector3.down, out frontHit, 10, mask) && Physics.Raycast(backGroundCheck.position, Vector3.down, out backHit, 10, mask))
            {
                Debug.DrawLine(frontGroundCheck.position, frontHit.point);
                Debug.DrawLine(backGroundCheck.position, backHit.point);
                float l = Vector3.Distance(frontHit.point, backHit.point);
                float h = frontHit.point.y - backHit.point.y;

                angle = Mathf.Lerp(angle, Mathf.Asin(-h / l) * 180 / Mathf.PI, Time.deltaTime);
                transform.localEulerAngles = new Vector3(angle, transform.localEulerAngles.y, transform.localEulerAngles.z);
            }

            // move foward according to current speed
            Vector3 newVelocity = movementSpeed * transform.forward;
            rb.velocity = new Vector3(newVelocity.x, rb.velocity.y, newVelocity.z);
            
            Debug.DrawLine(transform.position, nextDest, Color.blue);

            // rotate wheels
            foreach (GameObject wheel in wheels)
            {
                wheel.transform.Rotate(new Vector3(-wheelRotationSpeed * 6 * Time.deltaTime, 0, 0), Space.Self);
            }
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        Entity entity = other.transform.GetComponent<Entity>();
        if (entity != null)
        {
            source.PlayOneShot(hornClip); // beep beep!
            Messenger.SendMessage(MessageIDs.NoiseGenerated, new NoiseInfo(50f, transform.position, NoiseType.CarHorn, this.gameObject));
            entity.TakeDamage(new HitInfo(100f, 0, 2000, transform.forward, this));
            if (entity is Player)
            {
                // stop everything
                isDriving = false;
            }
            else
            {
                // already playing player damage sound
                source.PlayOneShot(hitClip);
            }
            
        }
    }

    public bool IsPlayer()
    {
        return false;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public string GetName()
    {
        return "Truck";
    }

}