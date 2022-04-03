using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInput : MonoBehaviour
{
    private PlayerControls controls;
    private PlayerMovement playerMovement;
    private Player player;
    private PauseMenu pauseMenu;

    public bool toggleAim = true;
    public bool paused = false;
    public bool aiming = false;

    private UnityAction onLevelStart;
    private UnityAction onLevelEnd;
    private UnityAction onBulletTimeStart;
    private UnityAction onPlayerDeath;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        player = GetComponent<Player>();
        pauseMenu = GetComponentInChildren<PauseMenu>();

        controls = new PlayerControls();

        #region movement controls

        controls.Movement.Move.performed += ctx => playerMovement.move = ctx.ReadValue<Vector2>();
        controls.Movement.Move.canceled += ctx => playerMovement.move = Vector2.zero;

        controls.Movement.Sprint.performed += ctx => playerMovement.Sprint(true);
        controls.Movement.Sprint.canceled += ctx => playerMovement.Sprint(false);

        controls.Movement.Crouch.performed += ctx => playerMovement.Crouch();

        controls.Movement.Jump.performed += ctx => playerMovement.Jump();

        #endregion

        #region FPS controls

        controls.FPS.Look.performed += ctx => playerMovement.look = ctx.ReadValue<Vector2>();
        controls.FPS.Look.canceled += ctx => playerMovement.look = Vector2.zero;

        controls.FPS.Shoot.performed += ctx => player.Shoot();

        controls.FPS.Reload.performed += ctx => player.Reload();

        controls.FPS.Use.performed += ctx => Interaction.Go();

        #endregion

        controls.AnyKey.AnyKey.performed += ctx => AnyKeyPressed();

        controls.Debug.KillYourself.performed += ctx => player.TakeDamage(FallDamage.CalculateHit(player, -1000));

        controls.PauseMenu.Pause.performed += ctx => TogglePause();
       
        SetAimControls(toggleAim);

        // initialize listeners
        onLevelStart = new UnityAction(OnLevelStart);
        onLevelEnd = new UnityAction(OnLevelEnd);
        onBulletTimeStart = new UnityAction(OnBulletTimeStart);
        onPlayerDeath = new UnityAction(OnPlayerDeath);
    }

    private void OnEnable()
    {
        controls.FPS.Enable();
        controls.Movement.Enable();
        controls.PauseMenu.Enable();
        controls.AnyKey.Enable();

        Messenger.Subscribe(MessageIDs.LevelStart, onLevelStart);
        Messenger.Subscribe(MessageIDs.LevelEnd, onLevelEnd);
        Messenger.Subscribe(MessageIDs.BulletTimeStart, onBulletTimeStart);
        Messenger.Subscribe(MessageIDs.PlayerDeath, onPlayerDeath);
    }

    private void OnDisable()
    {
        controls.FPS.Disable();
        controls.Movement.Disable();
        controls.PauseMenu.Disable();
        controls.AnyKey.Disable();

        Messenger.Unsubscribe(MessageIDs.LevelStart, onLevelStart);
        Messenger.Unsubscribe(MessageIDs.LevelEnd, onLevelEnd);
        Messenger.Unsubscribe(MessageIDs.BulletTimeStart, onBulletTimeStart);
        Messenger.Unsubscribe(MessageIDs.PlayerDeath, onPlayerDeath);
    }

    public void SetAimControls(bool toggle)
    {
        if (toggle)
        {
            controls.FPS.Aim.performed += ctx => aiming = !aiming;
            controls.FPS.Aim.performed += ctx => player.Aim(aiming);
        }
        else
        {
            controls.FPS.Aim.performed += ctx => player.Aim(true);
            controls.FPS.Aim.canceled += ctx => player.Aim(false);
        }
    }

    public void SetUserControl(bool toggle)
    {
        if (toggle)
        {
            controls.FPS.Enable();
            controls.Movement.Enable();
        }
        else
        {
            controls.FPS.Disable();
            controls.Movement.Disable();
        }
    }

    public void TogglePause()
    {
        paused = !paused;
        pauseMenu.EnablePauseMenu(paused);
        if (paused)
        {
            Time.timeScale = 0f;
            SetUserControl(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            SetUserControl(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void AnyKeyPressed()
    {
        Messenger.SendMessage(MessageIDs.AnyKeyPressed);
    }

    private void OnLevelStart()
    {
        SetUserControl(true);
    }

    private void OnLevelEnd()
    {
        SetUserControl(false);
    }
    
    private void OnBulletTimeStart()
    {
        SetUserControl(false);
    }

    private void OnPlayerDeath()
    {
        SetUserControl(false);
    }


}