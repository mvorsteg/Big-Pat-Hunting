using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    public Transform recoilPosition;
    public Transform rotationPoint;

    public float positionRecoilSpeed = 8f;
    public float rotationRecoilSpeed = 8f;

    public float positionReturnSpeed = 18f;
    public float rotationReturnSpeed = 38f;

    public Vector3 recoilRotation = new Vector3(10f, 5f, 7f);
    public Vector3 recoilKickback = new Vector3(0.015f, 0f, -0.2f);

    public Vector3 recoilRotationAim = new Vector3(10f, 4f, 6f);
    public Vector3 recoilKickbackAim = new Vector3(0.015f, 0f, -0.2f);

    private Vector3 rotationRecoil;
    private Vector3 positionRecoil;
    private Vector3 rot;
    private Vector3 startPos;
    public bool aiming;

    private void Start() {
        startPos = recoilPosition.localPosition;
    }

    private void Update()
    {
        rotationRecoil = Vector3.Lerp(rotationRecoil, Vector3.zero, rotationReturnSpeed * Time.deltaTime);
        positionRecoil = Vector3.Lerp(positionRecoil, startPos, positionReturnSpeed * Time.deltaTime);

        recoilPosition.localPosition = Vector3.Slerp(recoilPosition.localPosition, positionRecoil, positionRecoilSpeed * Time.deltaTime);
        rot = Vector3.Slerp(rot, rotationRecoil, rotationRecoilSpeed * Time.deltaTime);
        rotationPoint.localRotation = Quaternion.Euler(rot);    
    }

    public void Fire()
    {
        if (aiming)
        {
            rotationRecoil += new Vector3(-recoilRotationAim.x, Random.Range(-recoilRotationAim.y, recoilRotationAim.y), Random.Range(-recoilRotationAim.z, recoilRotationAim.z));
            positionRecoil += new Vector3(Random.Range(-recoilKickbackAim.x, recoilKickbackAim.x), Random.Range(-recoilKickbackAim.y, recoilKickbackAim.y), recoilKickbackAim.z);
        }
        else
        {
            rotationRecoil += new Vector3(-recoilRotation.x, Random.Range(-recoilRotation.y, recoilRotation.y), Random.Range(-recoilRotation.z, recoilRotation.z));
            positionRecoil += new Vector3(Random.Range(-recoilKickback.x, recoilKickback.x), Random.Range(-recoilKickback.y, recoilKickback.y), recoilKickback.z);
        }
    }
}