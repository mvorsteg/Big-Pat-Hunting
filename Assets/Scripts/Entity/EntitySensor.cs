using UnityEngine;

public class EntitySensor : MonoBehaviour
{
    private Entity owner;

    private void Start() 
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