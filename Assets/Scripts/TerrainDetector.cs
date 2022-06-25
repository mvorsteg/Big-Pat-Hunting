using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class TerrainDetector : MonoBehaviour
{
    private const float terrainWidth = 1000f;
    private const float terrainHeight = 1000f;

    private Dictionary<(int, int), TerrainInfo> terrainInfoDict;
    [SerializeField] private MaterialType[] terrainMaterials;

    private static TerrainDetector instance;

    public struct TerrainInfo
    {
        public Terrain terrain;
        public int numTextures;
        public float[,,] splatmapData;
    }

    private void Awake ()
    {
        // create singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Only 1 TerrainDetector should be in a scene. Deleting additional instances");
            Destroy(this);
        }
        terrainInfoDict = new Dictionary<(int, int), TerrainInfo>();
        Terrain[] terrains = FindObjectsOfType<Terrain>();
        for (int i = 0; i < terrains.Length; i++)
        {
            AddTerrain(terrains[i]);
        }
    }
    
    private void AddTerrain(Terrain terrain)
    {
        TerrainInfo info = new TerrainInfo();
        info.terrain = terrain;
        int alphamapWidth = terrain.terrainData.alphamapWidth;
        int alphamapHeight = terrain.terrainData.alphamapHeight;

        info.splatmapData = terrain.terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
        info.numTextures = info.splatmapData.Length / (alphamapWidth * alphamapHeight);
        int x, z;
        (x, z) = ConvertPositionToIndex(terrain.transform.position);
        terrainInfoDict[(x, z)] = info;
    }

    private static (int, int) ConvertPositionToIndex (Vector3 pos)
    {
        // TODO refactor this
        //int x = (int)(Mathf.Floor(pos.x / terrainWidth)) * (int)terrainWidth;
        //int z = (int)(Mathf.Floor(pos.z / terrainHeight)) * (int)terrainHeight;
        //return (x, z);
        return (-1000, -1000);
    }

    private static Vector3 ConvertToSplatMapCoordinate(Vector3 worldPos, Terrain terrain)
    {
        Vector3 splatPos = new Vector3();
        Vector3 terPos = terrain.transform.position;
        splatPos.x = ((worldPos.x - terPos.x) / terrain.terrainData.size.x) * terrain.terrainData.alphamapWidth;
        splatPos.z = ((worldPos.z - terPos.z) / terrain.terrainData.size.z) * terrain.terrainData.alphamapHeight;
        return splatPos;
    }

    /// <summary>
    /// Returns the index of the most prominent terrain material at a position on the terrain
    /// </summary>
    /// <param name="worldPos">the position on the terrain we are checking</param>
    /// <returns>integer corresponding to material index<br/>
    /// &lt; 0 : position is not on terrain<br/>
    /// 0 - 2 : grass<br/>
    /// 3 : gravel<br/>
    /// 4 : snow<br/>
    /// 5 - 8 : stone
    /// </returns>
    public static int GetActiveTerrainTextureIdx(Vector3 worldPos)
    {
        TerrainInfo info = instance.terrainInfoDict[ConvertPositionToIndex(worldPos)];
        Vector3 terrainCoord = ConvertToSplatMapCoordinate(worldPos, info.terrain);
        int activeTerrainIdx = 0;
        float largetstOpacity = 0f;

        for (int i = 0; i < info.numTextures; i++)
        {
            if (largetstOpacity < info.splatmapData[(int)terrainCoord.z, (int)terrainCoord.x, i])
            {    
                activeTerrainIdx = i;
                largetstOpacity = info.splatmapData[(int)terrainCoord.z, (int)terrainCoord.x, i];
            }
        }

        return activeTerrainIdx;
    }

    public static MaterialType GetMaterialFromTextureIdx(int idx)
    {
        if (idx > 0 && idx < instance.terrainMaterials.Length)
            return instance.terrainMaterials[idx];
        return MaterialType.Default;
    }

    /// <summary>
    /// samples the terrain height at a particular position
    /// </summary>
    /// <param name="worldPos">the postition in world space we are checking</param>
    /// <returns>the height of the terrain at that position</returns>
    public static float SampleHeight(Vector3 worldPos)
    {
        return instance.terrainInfoDict[ConvertPositionToIndex(worldPos)].terrain.SampleHeight(worldPos);
    }
}