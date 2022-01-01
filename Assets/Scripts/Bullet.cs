using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float rotationsPerSecond = 3000;
    public float speed = 50;

    public float threshold = 0.2f;

    private bool isHit;
    private Vector3 target;
    private BulletTime bulletTime;

    public void Initialize(BulletTime bulletTime, Vector3 target)
    {
        this.bulletTime = bulletTime;
        this.target = target;
    }

    private void Update()
    {
        transform.localEulerAngles += new Vector3(0, 0, rotationsPerSecond * Time.deltaTime / Time.timeScale);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (!isHit && Vector3.Distance(transform.position, target) < threshold)
        {
            //Debug.Break();

            isHit = true;
            bulletTime.BulletImpact();
            Destroy(this.gameObject);
        }
    }
}