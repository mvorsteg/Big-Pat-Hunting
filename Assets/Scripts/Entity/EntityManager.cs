using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EntityManager : MonoBehaviour
{
    public int activeSpawnPoints = 3;
    public KillQuest quest;

    private Dictionary<EntityType, int> entityCount;
    private AnimalSpawner[] spawnPoints;
    [SerializeField]
    private Utility.DictEntry<EntityType, GameObject>[] entityTypePrefabsSerializable;
    private Dictionary<EntityType, GameObject> entityTypePrefabs;

    private void Awake()
    {
        entityCount = new Dictionary<EntityType, int>();
        for (int i = 0; i < entityTypePrefabsSerializable.Length; i++)
        {
            entityCount[entityTypePrefabsSerializable[i].key] = 0;
        }
        entityTypePrefabs = Utility.GetDict(entityTypePrefabsSerializable);
    }

    private void Start()
    {
        SpawnBaseAnimals(activeSpawnPoints, quest.GetRequiredEntities());
    }

    /// <summary>
    /// Records that a certain type of entity has spawned
    /// </summary>
    /// <param name="type"></param>
    public void LogEntitySpawn(EntityType type)
    {
        if (type != null)
        {
            // add to entityCount
            if (!entityCount.ContainsKey(type))
            {
                entityCount[type] = 1;
            }
            else
            {
                entityCount[type]++;
            }
        }
    }    

    /// <summary>
    /// Records that a certain type of entity has despawn
    /// </summary>
    /// <param name="type"></param>
    public void LogEntityDespawn(EntityType type)
    {
        entityCount[type]--;
    }    

    /// <summary>
    /// Spawns and instantiates animals at a random subset of spawn points in the level
    /// </summary>
    /// <param name="numSpawnPoints">The number of spawn points that will spawn animals</param>
    /// <param name="requiredSpawns">An EntityType-int dictionary, defining the types (and amounts) of animals that must be instantiated
    private void SpawnBaseAnimals(int numSpawnPoints, Dictionary<EntityType, int> requiredSpawns)
    {
        // put spawnpoints in random order
        spawnPoints = GetComponentsInChildren<AnimalSpawner>();
        List<AnimalSpawner> spawnLst = spawnPoints.ToList();
        Utility.Shuffle(spawnLst);

        // spawn natural animals at n spawn points
        int i = 0;  // need this later
        for (i = 0; i < numSpawnPoints; i++)
        {
            LogEntitySpawn(spawnLst[i].SpawnAnimalNatural());   
        }
        
        // ensure we have our required entities spawned in
        foreach (EntityType key in requiredSpawns.Keys) 
        {
            while (entityCount[key] < requiredSpawns[key])
            {
                // must spawn more!
                LogEntitySpawn(spawnLst[i].SpawnAnimal(entityTypePrefabs[key], spawnLst[i].transform, 1));
                Debug.Log("spawning " + key + " manually"); 
            }
        }
    }
}