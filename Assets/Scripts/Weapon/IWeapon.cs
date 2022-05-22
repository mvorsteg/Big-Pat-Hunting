using UnityEngine;

public interface IWeapon
{
    bool IsReady();
    bool IsScoped();
    bool CanReload();
    void Shoot();
    void Reload();
    void Aim(bool state);
    void PlayMiscAudio(AudioClip clip);
}