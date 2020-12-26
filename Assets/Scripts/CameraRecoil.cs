using UnityEngine;

public class CameraRecoil : MonoBehaviour 
{
    private Vector2 recoil = Vector2.zero;
    public float rotationSpeed = 6f;
    public float returnSpeed = 25f;

    public Vector3 recoilRotation = new Vector3(2f, 2f, 2f);
    public Vector3 recoilRotationAiming = new Vector3(0.5f, 0.5f, 1.5f);

    private Vector3 currentRotation;
    private Vector3 rot;

    public bool aiming;

    private void Update() 
    {
        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        rot = Vector3.Slerp(rot, currentRotation, rotationSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(rot);
    }

    public void AddRecoil()
    {
        if (aiming)
            currentRotation += new Vector3(-recoilRotationAiming.x, Random.Range(-recoilRotationAiming.y, recoilRotationAiming.y), Random.Range(-recoilRotationAiming.z, recoilRotationAiming.z));
        else
            currentRotation += new Vector3(-recoilRotation.x, Random.Range(-recoilRotation.y, recoilRotation.y), Random.Range(-recoilRotation.z, recoilRotation.z));
    }
}