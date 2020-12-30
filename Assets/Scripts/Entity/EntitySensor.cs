using UnityEngine;

public class EntitySensor : MonoBehaviour
{
    private Entity entity;

    private void Start() 
    {
        entity = GetComponentInParent<Entity>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity e = other.GetComponent<Entity>();
        if (e != null)
        {
            entity.SenseEntity(e);
        }

    }    
}