using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class Player : Entity
{

    public IWeapon weapon;
    [SerializeField]
    private WeaponSwitcher weaponSwitcher;

    private bool isSprintingOrFalling;

    public Transform groundCheck;
    public Vector3 navPosition;

    public float healthRegenRate = 5f;

    private RaycastHit hit;
    private Interaction interaction;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip fallDamageSound;
    [SerializeField]
    private CameraController cameraController;
    public PlayerUI playerUI;

    public int baseWeapon = 0;

    private UnityAction onScopeIn;
    private UnityAction onUnscope;
    private UnityAction onReloadStart;
    private UnityAction onReloadFinish;
    private UnityAction onReloadCancel;
    private UnityAction onSprintStart;
    private UnityAction onSprintEnd;
    private UnityAction onBulletTimeEnd;
    private UnityAction onLevelEnd;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();

        onScopeIn = new UnityAction(OnScopeIn);
        onUnscope = new UnityAction(OnUnscope);
        onReloadStart = new UnityAction(OnReloadStart);
        onReloadFinish = new UnityAction(OnReloadFinish);
        onReloadCancel = new UnityAction(OnReloadCancel);
        onSprintStart = new UnityAction(OnSprintStart);
        onSprintEnd = new UnityAction(OnSprintEnd);
        onBulletTimeEnd = new UnityAction(OnBulletTimeEnd);
        onLevelEnd = new UnityAction(OnLevelEnd);
    }

    protected override void Start()
    {
        base.Start();
        weapon = weaponSwitcher.SetWeapon(baseWeapon);
        cameraController.EnableCameraGravity(false);     
        FallDamage.Initialize(maxHealth);   
    }

    private void OnEnable()
    {
        Messenger.Subscribe(MessageIDs.ScopeIn, onScopeIn);
        Messenger.Subscribe(MessageIDs.Unscope, onUnscope);
        Messenger.Subscribe(MessageIDs.ReloadStart, onReloadStart);
        Messenger.Subscribe(MessageIDs.ReloadFinish, onReloadFinish);
        Messenger.Subscribe(MessageIDs.ReloadCancel, onReloadCancel);
        Messenger.Subscribe(MessageIDs.SprintStart, onSprintStart);
        Messenger.Subscribe(MessageIDs.SprintEnd, onSprintEnd);
        Messenger.Subscribe(MessageIDs.BulletTimeEnd, onBulletTimeEnd);
        Messenger.Subscribe(MessageIDs.LevelEnd, onLevelEnd);
    }

    private void OnDisable()
    {
        Messenger.Unsubscribe(MessageIDs.ScopeIn, onScopeIn);
        Messenger.Unsubscribe(MessageIDs.Unscope, onUnscope);
        Messenger.Unsubscribe(MessageIDs.ReloadStart, onReloadStart);
        Messenger.Unsubscribe(MessageIDs.ReloadFinish, onReloadFinish);
        Messenger.Unsubscribe(MessageIDs.ReloadCancel, onReloadCancel);
        Messenger.Unsubscribe(MessageIDs.SprintStart, onSprintStart);
        Messenger.Unsubscribe(MessageIDs.SprintEnd, onSprintEnd);
        Messenger.Unsubscribe(MessageIDs.BulletTimeEnd, onBulletTimeEnd);
        Messenger.Unsubscribe(MessageIDs.LevelEnd, onLevelEnd);
    }

    public void ResetPlayer()
    {
        health = maxHealth;
        DamageSystem.SetVignette(1 - health / maxHealth);
        StopCoroutine("HealthRegen");
        GetComponent<PlayerInput>().SetUserControl(true);

        playerUI.SetDeathScreen(false, "");
        cameraController.ResetCamera();
        weapon.Aim(false);
        ((Gun)weapon).ReloadCallback();
    }

    protected override void Update()
    {
        //SetAgentPosition();
        // check if anything interactible is right in front of the player
        //Debug.DrawRay(transform.position, 2 * transform.forward, Color.green, 2f);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1.5f, ~0))
        {
            interaction = hit.transform.GetComponent<Interaction>();
            if (interaction != null)
            {
                Interaction.LoadInteraction(interaction);
            }
            else
            {
                Interaction.Reset();
            }
        }
        else
        {
            Interaction.Reset();
        }
    }

    /// <summary>
    /// Updates AgentPosition to keep it up with where the transform is on the navMesh
    /// </summary>
    protected void SetAgentPosition()
    {
        NavMeshHit hit;
        if(NavMesh.SamplePosition(groundCheck.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            navPosition = hit.position;
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

        audioSource.PlayOneShot(fallDamageSound);
        if (!(info.source is FallDamage))
        {
            DamageSystem.CreateIndicator(info.source.GetTransform());

        }
        DamageSystem.SetVignette(1 - health / maxHealth);
        StopCoroutine("HealthRegen");
        if (isAlive)
        {
            // using string name because we need that to interrupt
            StartCoroutine("HealthRegen");
        }
    }

    protected override void Die(HitInfo info)
    {
        base.Die(info);
        Messenger.SendMessage(MessageIDs.PlayerDeath);
        //GetComponent<PlayerInput>().SetUserControl(false);
        cameraController.EnableCameraGravity(true);
        playerUI.SetDeathScreen(true, info.source.GetName());
        
    }

    /// <summary>
    /// Calls the weapon's Shoot() method
    /// </summary>
    public void Shoot()
    {
        if (weapon.IsReady())
        {
            weapon.Shoot();
        }
    }

    /// <summary>
    /// Calls the weapon's Aim() method, 
    /// </summary>
    /// <param name="state">Whether or not to aim</param>
    public void Aim(bool state)
    {
        if (state && !weapon.IsReady())
        {
            return;
        }
        weapon.Aim(state);
    }

    /// <summary>
    /// Calls the weapon's Reload() method if available
    /// </summary>
    public void Reload()
    {
        if (weapon.CanReload() && !isSprintingOrFalling)
        {
            weapon.Reload();
        }
    }

    private IEnumerator HealthRegen()
    {
        yield return new WaitForSeconds(5f);
        while (health < maxHealth)
        {
            health += healthRegenRate * Time.deltaTime;
            DamageSystem.SetVignette(1 - health / maxHealth);
            yield return null;
        }
        health = maxHealth;
    }

    private void OnScopeIn()
    {

    }

    private void OnUnscope()
    {

    }

    private void OnReloadStart()
    {
        
    }

    private void OnReloadFinish()
    {
        
    }

    private void OnReloadCancel()
    {
        
    }

    private void OnSprintStart()
    {
        isSprintingOrFalling = false;
    }

    private void OnSprintEnd()
    {
        isSprintingOrFalling = true;
    }

    private void OnBulletTimeEnd()
    {
        Aim(false);
    }

    private void OnLevelEnd()
    {
        Aim(false);
    }
}