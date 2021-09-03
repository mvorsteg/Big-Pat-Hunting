using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject[] weapons;       // array of all possible weapon game objects (these MUST have an IWeapon component)
    private GameObject currWeapon;      // the currently selected weapon
    private int currWeaponId;

    public float swayAmount = 1;
    public float swaySmoothing;
    public float swayResetSmoothing;
    public float swayClampX;
    public float swayClampY;
    public bool swayXInverted;
    public bool swayYInverted;

    private Vector3 newWeaponRotation;
    private Vector3 newWeaponRotationVelocity;

    private Vector3 targetWeaponRotation;
    private Vector3 targetWeaponRotationVelocity;

    private void Awake()
    {
        // disable all weapons, only one will be enabled by the player (called in Player)
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
    }

    private void Start()
    {
        
    }

    public void Rotate(float inputX, float inputY)
    {
        targetWeaponRotation.x += swayAmount * (swayYInverted ? -inputY : inputY) * Time.deltaTime;
        targetWeaponRotation.y += swayAmount * (swayXInverted ? -inputX : inputX) * Time.deltaTime;
        
        targetWeaponRotation.x = Mathf.Clamp(targetWeaponRotation.x, -swayClampX, swayClampX);
        targetWeaponRotation.y = Mathf.Clamp(targetWeaponRotation.y, -swayClampY, swayClampY);

        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity, swayResetSmoothing);
        newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, targetWeaponRotation, ref newWeaponRotationVelocity, swaySmoothing);

        currWeapon.transform.localRotation = Quaternion.Euler(newWeaponRotation);
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
        newWeaponRotation = currWeapon.transform.localEulerAngles;   
        currWeaponId = weaponId;
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

    public int GetCurrWeaponId()
    {
        return currWeaponId;
    }
}