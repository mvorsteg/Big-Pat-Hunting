using UnityEngine;

public interface IWeapon
{
    bool CanShoot();
    bool IsScoped();
    bool CanReload();
    void Shoot();
    void Reload();
    void Aim(bool state);
}