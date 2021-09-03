using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInput : MonoBehaviour
{
    private PlayerControls controls;
    private PlayerMovement playerMovement;
    private Player player;

    public bool toggleAim = true;
    public bool aiming = false;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        player = GetComponent<Player>();

        controls = new PlayerControls();

        controls.Gameplay.Move.performed += ctx => playerMovement.move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => playerMovement.move = Vector2.zero;

        controls.Gameplay.Look.performed += ctx => playerMovement.look = ctx.ReadValue<Vector2>();
        controls.Gameplay.Look.canceled += ctx => playerMovement.look = Vector2.zero;

        controls.Gameplay.Jump.performed += ctx => playerMovement.Jump();

        controls.Gameplay.Sprint.performed += ctx => playerMovement.Sprint(true);
        controls.Gameplay.Sprint.canceled += ctx => playerMovement.Sprint(false);

        controls.Gameplay.Shoot.performed += ctx => player.Shoot();

        controls.Gameplay.Reload.performed += ctx => player.Reload();

        controls.Gameplay.Use.performed += ctx => Interaction.Go();

        SetAimControls(toggleAim);

        controls.Gameplay.Enable();
    }

    public void SetAimControls(bool toggle)
    {
        if (toggle)
        {
            controls.Gameplay.Aim.performed += ctx => aiming = !aiming;
            controls.Gameplay.Aim.performed += ctx => player.Aim(aiming);
        }
        else
        {
            controls.Gameplay.Aim.performed += ctx => player.Aim(true);
            controls.Gameplay.Aim.canceled += ctx => player.Aim(false);
        }
    }

    public void SetUserControl(bool toggle)
    {
        if (toggle)
        {
            controls.Gameplay.Enable();
        }
        else
        {
            controls.Gameplay.Disable();
        }
    }


}