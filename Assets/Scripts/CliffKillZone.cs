using UnityEngine;

public class CliffKillZone : MonoBehaviour
{

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter(Collider other)
    {
        Entity entity;
        if (other.TryGetComponent<Entity>(out entity))
        {
            entity.TakeDamage(FallDamage.CalculateHit(entity, -1000));
        }
    }    
}