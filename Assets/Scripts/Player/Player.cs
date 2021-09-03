using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Player : Entity
{

    public IWeapon weapon;
    [SerializeField]
    private WeaponSwitcher weaponSwitcher;
    public NoiseGenerator noiseGenerator;

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

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void Start()
    {
        base.Start();
        weapon = weaponSwitcher.SetWeapon(0);
        cameraController.EnableCameraGravity(false);        
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
        GetComponent<PlayerInput>().SetUserControl(false);
        cameraController.EnableCameraGravity(true);
        playerUI.SetDeathScreen(true, info.source.GetName());
        
    }

    /// <summary>
    /// Calls the weapon's Shoot() method
    /// </summary>
    public void Shoot()
    {
        if (weapon.CanShoot())
        {
            weapon.Shoot();
            noiseGenerator.GenerateNoise();
        }
    }

    /// <summary>
    /// Calls the weapon's Aim() method, 
    /// </summary>
    /// <param name="state">Whether or not to aim</param>
    public void Aim(bool state)
    {
        weapon.Aim(state);
    }

    /// <summary>
    /// Calls the weapon's Reload() method if available
    /// </summary>
    public void Reload()
    {
        if (weapon.CanReload())
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
}