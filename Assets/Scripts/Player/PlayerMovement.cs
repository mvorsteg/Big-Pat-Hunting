using UnityEngine;
using UnityEngine.Events;

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

    private Vector3 velocity;
    private Vector3 hitNormal;
    private Vector3 slidingDirection;
    private float prevVelocityY;

    public Transform groundCheck;
    private bool isGrounded;
    private bool isSliding = false;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private UnityAction onPlayerDeath;

    public float MaxSpeed { get => isSprinting ? sprintSpeed : walkSpeed; }
    public bool IsSprinting { get => isSprinting; }
    public bool IsGrounded { get => isGrounded; }
    public Vector3 Velocity { get => cc.velocity; }

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        // initialize listeners
        onPlayerDeath = new UnityAction(OnPlayerDeath);
    }

    private void OnEnable()
    {
        EventManager.StartListening("PlayerDeath", onPlayerDeath);
    }

    private void OnDisable()
    {
        EventManager.StopListening("PlayerDeath", onPlayerDeath);
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
            cc.Move(((slidingDirection * MaxSpeed) + velocity) * Time.deltaTime);
        }
        else
        {
            cc.Move(((inputMovement * MaxSpeed) + velocity) * Time.deltaTime);
        }

        // do animation
        weaponAnimationSpeed = cc.velocity.magnitude / MaxSpeed;
        weaponAnimator.SetFloat("weaponSpeed", weaponAnimationSpeed);
        weaponAnimator.SetBool("isGrounded", isGrounded);
    }    

    public void MoveToPosition(Vector3 position)
    {
        cc.enabled = false;
        transform.position = position;
        cc.enabled = true;
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

    private void OnPlayerDeath()
    {

    }

    void OnControllerColliderHit (ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }
}