using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnData
    {
        public GameObject prefab;
        public float weight;
        public float scaleRange;
    }

    public SpawnData[] possibleSpawns;

    private void Awake()
    {
        // normalizes the weights for the spawnpoint
        float totalWeight = 0;
        for (int i = 0; i < possibleSpawns.Length; i++)
        {
            totalWeight += possibleSpawns[i].weight;
            possibleSpawns[i].weight = totalWeight;
        }
        float scale = 1 / totalWeight;
        for (int i = 0; i < possibleSpawns.Length; i++)
        {
            possibleSpawns[i].weight *= scale;
        }
    }

    /// <summary>
    /// Spawns an animal from the list of this Spawner's possible spawns, with weights
    /// </summary>
    /// <returns>The type of the animal spawned</returns>
    public GameObject SpawnAnimalNatural()
    {
        float rand = Random.Range(0f, 1f);
        for (int i = 0; i < possibleSpawns.Length; i++)
        {
            if (rand <= possibleSpawns[i].weight)
            {
                return SpawnAnimal(possibleSpawns[i].prefab, transform, possibleSpawns[i].scaleRange);
            }
        }
        return null;
    }

    /// <summary>
    /// Instantiates an AI animal at the selected spawn point
    /// </summary>
    /// <param name="prefab">The prefab game object of the animal being spawned in</param>
    /// <param name="spawnPoint">The position, in world space, that the animal will be spawned at</param>
    /// <param name="scaleRange">How much variability there can be in the animal's scale, so that (1 - scaleRange) < 1 < (1 + scaleRange)</param>
    /// <returns>The type of the animal spawned</returns>
    public GameObject SpawnAnimal(GameObject prefab, Transform spawnPoint, float scaleRange)
    {
        GameObject animal = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        float scale = Random.Range(1 - scaleRange, 1 + scaleRange);
        animal.transform.localScale = new Vector3(scale, scale, scale);
        return animal;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, 1f);
    }
}
