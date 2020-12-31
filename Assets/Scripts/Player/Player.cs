using UnityEngine;
using UnityEngine.AI;

public class Player : Entity
{

    public IWeapon weapon;
    public NoiseGenerator noiseGenerator;

    public Transform groundCheck;
    public Vector3 navPosition;
    
    protected override void Awake()
    {

    }

    protected override void Start()
    {
        weapon = GetComponentInChildren<IWeapon>();
    }

    protected override void Update()
    {
        //SetAgentPosition();
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
        DamageSystem.CreateIndicator(info.source.GetTransform());
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
}