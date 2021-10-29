using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using Cinemachine;

public class BulletTime : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;

    [SerializeField]
    private CinemachineSmoothPath path;
    [SerializeField]
    private CinemachineDollyCart cart;
    [SerializeField]
    private GameObject actualCam;
    [SerializeField]
    private CinemachineVirtualCamera vCam;

    public float timeScale = 0.1f;
    public float bulletSpeed = 50f;

    public GameObject[] playerCameras;
    public GameObject hud;
    public GameObject scope;

    public GameObject[] balls;

    private Player player;
    private WeakPoint weakPoint;
    private HitInfo hitInfo;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    public void StartShot(GameObject bulletPrefab, Vector3 startPos, Vector3 endPos, Quaternion rot, WeakPoint weakPoint, HitInfo info)
    {
        Time.timeScale = timeScale;
        mixer.SetFloat("masterPitch", 0.5f);
        Debug.Log("Cinematic Shot!");

        this.weakPoint = weakPoint;
        this.hitInfo = info;

        path.m_Waypoints = new CinemachineSmoothPath.Waypoint[2];
        path.m_Waypoints[0].position = startPos - rot * Vector3.forward;
        path.m_Waypoints[1].position = endPos + rot * Vector3.forward;
        path.m_Resolution = 10;

        // balls[0].transform.position = startPos;
        // balls[1].transform.position = endPos;

        Bullet bullet = Instantiate(bulletPrefab, startPos, rot).GetComponent<Bullet>();
        bullet.Initialize(this, endPos);

        vCam.gameObject.SetActive(true);
        vCam.LookAt = bullet.transform;

        // calculate speed required for dolly 
        cart.m_Path = path;
        cart.m_Position = 0;
        float distanceToTarget = Vector3.Distance(bullet.transform.position, endPos);
        float travelTime = distanceToTarget / bulletSpeed;

        // cart.m_Speed = distanceToTarget / travelTime;
        // bullet.speed = bulletSpeed;
        

        actualCam.SetActive(true);
        
        foreach (GameObject g in playerCameras)
        {
            g.SetActive(false);
        }

        scope.SetActive(false);
        hud.SetActive(false);

        //Debug.Break();

        StartCoroutine(ApplySpeed(distanceToTarget / travelTime / timeScale, bulletSpeed / timeScale, bullet));
    }

    public void EndShot()
    {
        vCam.LookAt = weakPoint.transform;
        //vCam.Follow = weakPoint.transform;
        cart.m_Speed = 0;
        weakPoint.TakeDamage(hitInfo);
        Time.timeScale = 0.5f;
    }

    private IEnumerator ApplySpeed(float camSpeed, float bulletSpeed, Bullet bullet)
    {
        cart.m_Speed = 0;
        bullet.speed = 0;
        yield return new WaitForSecondsRealtime(0.2f);
        cart.m_Speed = camSpeed;
        bullet.speed = bulletSpeed;
    }
}