using UnityEngine;

public class Player : Entity
{

    public IWeapon weapon;

    protected override void Awake()
    {

    }

    protected override void Start()
    {
        weapon = GetComponentInChildren<IWeapon>();
    }

    protected override void Update()
    {

    }

    /// <summary>
    /// Calls the weapon's Shoot() method
    /// </summary>
    public void Shoot()
    {
        if (weapon.CanShoot())
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
        weapon.Aim(state);
    }
}