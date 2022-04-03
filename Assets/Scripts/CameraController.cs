using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float mouseSensitivity = 100f;
    private float scopedSensitivity = 1f;

    public Transform playerBody;
    public Transform recoilTransform;

    private float xRotation = 0f;

    private Vector3 currentRotation;
    private Vector3 rot;
    private float rotationSpeed = 6f;
    private float returnSpeed = 25f;

    [SerializeField]
    private float normalHeight = 0.3f, crouchHeight = -0.4f;
    [SerializeField]
    private float crouchTime = 0.2f;
    private bool isCrouchAllowed = true;

    private Rigidbody rb;
    private Collider coll;

    private Vector3 initialPos;

    [SerializeField]
    private WeaponSwitcher weaponHolder;
    [SerializeField]
    private GameObject[] dummyWeapons;
    private List<GameObject> instantiatedDummyWeapons;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        initialPos = transform.localPosition;
        instantiatedDummyWeapons = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        EnableCameraGravity(false); 
    }

    public void SetSensitivity(float val)
    {
        scopedSensitivity = val;
    }

    public void SetSpeeds(float returnSpeed, float rotationSpeed)
    {
        this.returnSpeed = returnSpeed;
        this.rotationSpeed = rotationSpeed;
    }

    public void AddRecoil(float x, float y, float z)
    {
        //Debug.Log(x + " " + y + " " + z);
        currentRotation += new Vector3(x, y, z);
    }

    public void Rotate(float x, float y)
    {
        x *= mouseSensitivity * scopedSensitivity * Time.deltaTime;
        y *= mouseSensitivity * scopedSensitivity * Time.deltaTime;
        //Debug.Log("X: " + x + " Y: " + y);
        xRotation -= y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * x);

        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        rot = Vector3.Slerp(rot, currentRotation, rotationSpeed * Time.deltaTime);
        recoilTransform.localRotation = Quaternion.Euler(rot);
    }

    public bool Crouch(bool val)
    {
        bool temp = isCrouchAllowed;
        if (isCrouchAllowed)
        {
            StartCoroutine(CrouchOverTime(val));
        }
        return temp;
    }

    private IEnumerator CrouchOverTime(bool val)
    {
        isCrouchAllowed = false;
        Vector3 start = new Vector3(0, val ? normalHeight : crouchHeight, 0);
        Vector3 end   = new Vector3(0, val ? crouchHeight : normalHeight, 0);
        float elapedTime = 0f;
        while (elapedTime < crouchTime)
        {
            elapedTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(start, end, elapedTime / crouchTime);
            yield return null;
        }
        isCrouchAllowed = true;
    }

    public void EnableCameraGravity(bool enabled)
    {
        rb.isKinematic = !enabled;
        coll.enabled = enabled;
        if (enabled)
        {
            GameObject go = Instantiate(dummyWeapons[weaponHolder.GetCurrWeaponId()], weaponHolder.transform.position, weaponHolder.transform.rotation);
            instantiatedDummyWeapons.Add(go);
            go.GetComponent<Rigidbody>().AddForce(transform.forward * 25f);
            weaponHolder.gameObject.SetActive(false);
        }
    }

    public void ResetCamera()
    {
        EnableCameraGravity(false);
        foreach (GameObject go in instantiatedDummyWeapons)
        {
            Destroy(go);
        }
        instantiatedDummyWeapons.Clear();
        weaponHolder.gameObject.SetActive(true);
        transform.localPosition = initialPos;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Default") || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            EnableCameraGravity(false);
        }
    }
   
}
