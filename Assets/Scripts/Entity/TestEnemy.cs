using UnityEngine;

public class TestEnemy : Entity
{
    private Rigidbody rb;

    protected override void Start() 
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    protected override void Die(HitInfo info)
    {
        base.Die(info);
        rb.isKinematic = false;
        rb.AddForce(info.force * info.direction);
        Destroy(this.gameObject, 2f);
    }
}