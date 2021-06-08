using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject[] weapons;       // array of all possible weapon game objects (these MUST have an IWeapon component)
    private GameObject currWeapon;      // the currently selected weapon

    private void Awake()
    {
        // disable all weapons, only one will be enabled by the player (called in Player)
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
    }

    /// <summary>
    /// switches the current weapon to the one referenced with the 0-indexed id number passed in
    /// </summary>
    /// <param name="weaponId"></param>
    /// <returns>The IWeapon component of the newly equipped weapon</returns>
    public IWeapon SetWeapon(int weaponId)
    {
        if (currWeapon != null)
            currWeapon.SetActive(false);
        currWeapon = weapons[weaponId];
        currWeapon.SetActive(true);
        return currWeapon.GetComponent<IWeapon>();
    }

    /// <summary>
    /// returns the IWeapon component of the equipped weapon
    /// </summary>
    /// <returns>IWeapon component of the current weapon</returns>
    public IWeapon GetCurrWeapon()
    {
        return currWeapon.GetComponent<IWeapon>();
    }
}