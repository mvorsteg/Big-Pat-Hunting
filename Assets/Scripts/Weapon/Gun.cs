using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour, IWeapon, IDamageSource
{
    public float damage = 10f;      // The damage per shot fired from the gun
    public float force = 1f;        // The amount of force applied when a shot kills an entity
    public float fireRate = 1f;     // How many shots the gun can fire per second
    public float range = 100f;      // How far (in meters) the gun can shoot before bullets deal no damage
    public bool isScoped = true;    // If true, the gun will provide a scope overlay when aimed
    public float recoil = 1f;       // How much displacement is applied after firing
    public int maxAmmo = 3;         // The maximum ammo this gun can hold before reloading
    public float reloadSpeed = 5f;  // How long (in seconds) it takes to reload the gun

    public Transform origin;        // Where the bullets originate from
    public CameraController cameraController;
    public CameraRecoil cameraRecoil;
    public WeaponRecoil weaponRecoil;
    public GameObject scopeOverlay;      // The crosshair image that appears when scoped
    public Camera mainCamera;
    public GameObject rifleCamera;
    public float scopedFOV = 15f;
    public float scopedSensitivity = 0.5f;
    private float normalFOV;

    private bool isReadyToShoot = true; // The gun can only shoot when this is true
    private int currAmmo;               // How much ammo currently in the gun

    private AudioSource audioSource;
    private Animator anim;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInParent<Animator>();
    }

    private void Start()
    {
        currAmmo = maxAmmo;
        normalFOV = mainCamera.fieldOfView;
    }

    /// <summary>
    /// Determines if the gun is cooled down, loaded, and otherwise able to shoot
    /// </summary>
    /// <returns>True if the gun can shoot, otherwise false</returns>
    public bool CanShoot()
    {
        return isReadyToShoot && currAmmo > 0;
    }

    /// <summary>
    /// Determines whether the gun uses a scope overlay when aimed
    /// </summary>
    /// <returns>True if the gun has a scope, otherwise false</returns>
    public bool IsScoped()
    {
        return isScoped;
    }

    /// <summary>
    /// Causes the gun to fire a raycast from its origin outward
    /// If it hits any entity that can take damage, it applies damage to it
    /// </summary>
    public void Shoot()
    {
        audioSource.Play();
        StartCoroutine(FireCooldown());
        cameraRecoil.AddRecoil();
        weaponRecoil.Fire();
        RaycastHit hit;
        if (Physics.Raycast(origin.position, origin.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            // check if we hit a weak point
            WeakPoint weakPoint = hit.transform.GetComponent<WeakPoint>();
            if (weakPoint != null)
            {
                weakPoint.TakeDamage(new HitInfo(damage, hit.distance, force, (hit.transform.position - origin.position).normalized, this));
            }

            // otherwise, check for an entity and apply damage to it
            Entity entity = hit.transform.GetComponent<Entity>();
            if (entity != null)
            {
                entity.TakeDamage(new HitInfo(damage, hit.distance, force, (hit.transform.position - origin.position).normalized, this));
            }
        }
        // add recoil
        
    }

    /// <summary>
    /// Causes the gun to move to the center and aim from the center
    /// </summary>
    public void Aim(bool state)
    {
        weaponRecoil.aiming = state;
        cameraRecoil.aiming = state;
        anim.SetBool("isScoped", state);
        cameraController.SetSensitivity(state ? scopedSensitivity : 1f);
        if (isScoped)
        {
            if (state)
            {
                StartCoroutine(ScopeIn());
            }
            else
            {
                //HUD.SetActive(true);
                cameraController.SetSensitivity(1.0f);
                rifleCamera.SetActive(true);
                scopeOverlay.SetActive(false);
                mainCamera.fieldOfView = normalFOV;
            }
        }
    }

    /// <summary>
    /// Cau
    /// </summary>
    private IEnumerator ScopeIn()
    {
        //playerMov.AdjustSensitivity(0.5f);
        yield return new WaitForSeconds(0.15f);
        scopeOverlay.SetActive(true);
        rifleCamera.SetActive(false);
        mainCamera.fieldOfView = scopedFOV;
        

        //HUD.SetActive(false);
    }

    /// <summary>
    /// A delay that occurs after every shot is fired before the next shot can be fired.
    /// </summary>
    private IEnumerator FireCooldown()
    {
        isReadyToShoot = false;
        yield return new WaitForSeconds(1f / fireRate);
        isReadyToShoot = true;
    }

    private IEnumerator Reload()
    {
        isReadyToShoot = false;
        yield return new WaitForSeconds(reloadSpeed);

    }
}