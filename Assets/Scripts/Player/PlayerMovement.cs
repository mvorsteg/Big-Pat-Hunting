using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Vector2 move;
    public Vector2 look;

    public float weaponAnimationSpeed;

    public float walkSpeed = 12f;
    public float sprintSpeed = 16f;
    private bool isSprinting = false;
    public float jumpHeight = 3f;
    public float slopeLimit = 45;
    public float slideFriction = 0.3f;
    private CharacterController cc;
    public CameraController cameraController;
    public WeaponSwitcher weaponHolder;
    public Animator weaponAnimator;
    private AudioSource audioSource;
    private TerrainDetector terrainDetector;

    private Vector3 velocity;
    private Vector3 hitNormal;
    private Vector3 slidingDirection;
    private float prevVelocityY;
    private float timeToFootstep;

    public Transform groundCheck;
    private bool isGrounded;
    private bool isSliding = false;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask terrainMask;

    public AudioClip[] grassFootsteps;
    public AudioClip[] gravelFootsteps;
    public AudioClip[] snowFootsteps;
    public AudioClip[] stoneFootsteps;
    public AudioClip[] woodFootsteps;
    public AudioClip[] metalFootsteps;

    public float footstepDelayWalking = 0.6f;
    public float footstepDelaySprinting = 0.4f;

    public float Speed { get => isSprinting ? sprintSpeed : walkSpeed; }

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        prevVelocityY = velocity.y;
        cameraController.Rotate(look.x, look.y);
        weaponHolder.Rotate(look.x, look.y);
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);// && (Vector3.Angle (Vector3.up, hitNormal) <= slopeLimit);
        //hitColliders = Physics.OverlapSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            if (prevVelocityY < FallDamage.minVelocity)
            {
                HitInfo info = FallDamage.CalculateHit(GetComponent<Player>(), prevVelocityY);
                //HitInfo info = new HitInfo(fallDamageVelocity - prevVelocityY, FallDamage.CalculateDamage(), 0, Vector3.down, FallDamage.instance);
                GetComponent<Player>().TakeDamage(info);
                Debug.Log("Falldmaage " + prevVelocityY);
                //player
            }

        }

        // if (Vector3.Angle(Vector3.up, hitNormal) >= slopeLimit)
        // {
        //     isSliding = true;
        //     Vector3 c = Vector3.Cross(Vector3.up, hitNormal);
        //     Vector3 u = Vector3.Cross(c, hitNormal);
        //     slidingDirection = u * 4f;
        // }
        // else
        // {
        //     isSliding = false;
        //     slidingDirection = Vector3.Lerp(Vector3.zero,;
        // }

        Vector3 inputMovement = transform.right * move.x + transform.forward * move.y;    
        //cc.Move(inputMovement * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
    
        // if (!isGrounded) {
        //     velocity.x += (1f - hitNormal.y) * hitNormal.x * (speed - slideFriction);
        //     velocity.z += (1f - hitNormal.y) * hitNormal.z * (speed - slideFriction);
        // }

        if (isSliding)
        {
            cc.Move(((slidingDirection * Speed) + velocity) * Time.deltaTime);
        }
        else
        {
            cc.Move(((inputMovement * Speed) + velocity) * Time.deltaTime);
        }

        // do animation
        weaponAnimationSpeed = cc.velocity.magnitude / Speed;
        weaponAnimator.SetFloat("weaponSpeed", weaponAnimationSpeed);
        weaponAnimator.SetBool("isGrounded", isGrounded);

        timeToFootstep -= Time.deltaTime;
        if (isGrounded && cc.velocity.magnitude > 2f && timeToFootstep <= 0)
        {
            AudioClip sound = GetRandomFootstepClip();
            audioSource.clip = sound;
            audioSource.Play();
            timeToFootstep = isSprinting ? footstepDelaySprinting : footstepDelayWalking;
        }
    }    

    public void Jump()
    {
        if (isGrounded && Vector3.Angle(Vector3.up, hitNormal) <= slopeLimit)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            weaponAnimator.SetTrigger("jump");
        }
    }

    public void Sprint(bool val)
    {
        isSprinting = val;
        weaponAnimator.SetBool("isSprinting", val);
    }

    public void AssignTerrainDetector(TerrainDetector detector)
    {
        terrainDetector = detector;
    }

    private AudioClip GetRandomFootstepClip()
    {
        
        int terrainTextureIdx = -1;
        if (Physics.CheckSphere(groundCheck.position, groundDistance, terrainMask))
            terrainTextureIdx = terrainDetector.GetActiveTerrainTextureIdx(transform.position);
        
        if (terrainTextureIdx < 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(groundCheck.position, -transform.up, out hit, 1f, groundMask))
            {
                if (hit.transform.tag == "Stone")
                    return stoneFootsteps[Random.Range(0, stoneFootsteps.Length)];
                if (hit.transform.tag == "Wood")
                    return woodFootsteps[Random.Range(0, woodFootsteps.Length)];
                if (hit.transform.tag == "Metal")
                    return metalFootsteps[Random.Range(0, metalFootsteps.Length)];
                if (hit.transform.tag == "Cloth")
                    return snowFootsteps[Random.Range(0, snowFootsteps.Length)];
            }
        }
        
        if (terrainTextureIdx < 3)
            return grassFootsteps[Random.Range(0, grassFootsteps.Length)];
        if (terrainTextureIdx < 4)
            return gravelFootsteps[Random.Range(0, gravelFootsteps.Length)];
        if (terrainTextureIdx < 5)
            return snowFootsteps[Random.Range(0, snowFootsteps.Length)];
        if (terrainTextureIdx < 8)
            return stoneFootsteps[Random.Range(0, stoneFootsteps.Length)];
            // wood and metal are not terrains
            //return woodFootsteps[Random.Range(0, woodFootsteps.Length)];
            //return metalFootsteps[Random.Range(0, metalFootsteps.Length)];

        // default to grass
        return grassFootsteps[Random.Range(0, grassFootsteps.Length)];
    }

    void OnControllerColliderHit (ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }
}