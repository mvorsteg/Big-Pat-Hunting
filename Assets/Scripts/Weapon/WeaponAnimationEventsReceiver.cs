using UnityEngine;

public class WeaponAnimationEventsReceiver : MonoBehaviour
{
    private IWeapon parentWeapon;
    
    private void Awake()
    {
        parentWeapon = GetComponentInParent<IWeapon>();
    }

    public void PlayMiscAudio(AudioClip clip)
    {
        parentWeapon.PlayMiscAudio(clip);
    }

    public void ReloadCallback()
    {
        ((Gun)parentWeapon).ReloadCallback();
    }
}