using UnityEngine;

public class TerrainTrigger : MonoBehaviour
{
    private TerrainDetector terrainDetector;

    private void Awake()
    {  
        terrainDetector = new TerrainDetector(GetComponentInParent<Terrain>());
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement pm = other.transform.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.AssignTerrainDetector(terrainDetector);
        }
        else
        {
            Animal animal = other.transform.GetComponent<Animal>();
            if (animal != null)
            {
                animal.AssignTerrainDetector(terrainDetector);
            }
        }
    }    
}