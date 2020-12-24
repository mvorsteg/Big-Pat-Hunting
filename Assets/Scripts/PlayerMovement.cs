using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Vector2 move;
    public Vector2 look;

    private float speed = 12f;
    public float jumpHeight = 3f;
    private CharacterController cc;
    public CameraController cameraController;

    private Vector3 velocity;

    public Transform groundCheck;
    private bool isGrounded;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        cameraController.Rotate(look.x, look.y);
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 inputMovement = transform.right * move.x + transform.forward * move.y;    
        cc.Move(inputMovement * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }    

    public void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}