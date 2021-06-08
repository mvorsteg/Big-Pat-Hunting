using UnityEngine;

public class EntitySensor : MonoBehaviour
{
    private Entity owner;

    private void Awake() 
    {
        owner = GetComponentInParent<Entity>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity e = other.GetComponent<Entity>();
        if (e != null)
        {
            owner.SenseEntity(e);
        }

    }
}