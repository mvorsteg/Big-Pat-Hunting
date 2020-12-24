using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float mouseSensitivity = 100f;
    private float scopedSensitivity = 1f;

    public Transform playerBody;

    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetSensitivity(float val)
    {
        scopedSensitivity = val;
    }

    // Update is called once per frame
    public void Rotate(float x, float y)
    {
        x *= mouseSensitivity * scopedSensitivity * Time.deltaTime;
        y *= mouseSensitivity * scopedSensitivity * Time.deltaTime;
        
        xRotation -= y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * x);
    }
}
