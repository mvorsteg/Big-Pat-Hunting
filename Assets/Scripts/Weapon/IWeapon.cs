using UnityEngine;

public interface IWeapon
{
    bool CanShoot();
    bool IsScoped();
    void Shoot();
    void Aim(bool state);
}