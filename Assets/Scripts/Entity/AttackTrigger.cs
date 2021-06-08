using UnityEngine;

public class AttackTrigger : MonoBehaviour, IDamageSource
{
    public Entity owner;
    public float damage;
    public float force;

    private Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
        col.enabled = false;    
    }

    public Transform GetTransform()
    {
        return owner.transform;
    }

    public bool IsPlayer()
    {
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("sensor");
        Entity entity = other.GetComponent<Entity>();
        if (entity != null)
        {
            HitInfo info = new HitInfo(damage, 0f, force, (other.transform.position - transform.position).normalized, this);
            entity.TakeDamage(info);
        }
        col.enabled = false;
    }
}