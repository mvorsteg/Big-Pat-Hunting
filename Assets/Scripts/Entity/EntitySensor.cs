using UnityEngine;

public class EntitySensor : MonoBehaviour
{
    private Entity owner;
    private Collider trigger;

    private void Awake() 
    {
        owner = GetComponentInParent<Entity>();
        trigger = GetComponent<Collider>();
    }

    /// <summary>
    /// Checks if a given entity is within the range of the sensor
    /// </summary>
    /// <param name="entity">The entity have its position checked</param>
    /// <returns>True if entity is within the bounds of the sensor, else false</returns>
    public bool IsEntityInRange(Entity entity)
    {
        return trigger.bounds.Contains(entity.transform.position);
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