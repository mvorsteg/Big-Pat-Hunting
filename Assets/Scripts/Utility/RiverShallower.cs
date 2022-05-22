using System.Collections.Generic;
using UnityEngine;

public class RiverShallower : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData terrainData;

    public float raycastDistance = 5f;
    public float maxDepth = 1f;

    public Terrain Terrain { get => terrain; set => terrain = value; }
    public TerrainData TerrainData { get => Terrain.terrainData; }

    private void Awake()
    {
        
    }

    private void Start()
    {

    }

    public void ModifyTerrain()
    {
        int heightmapHeight = TerrainData.heightmapResolution;
        int heightmapWidth = TerrainData.heightmapResolution;

        float [,] heights = TerrainData.GetHeights(0, 0, heightmapHeight, heightmapWidth);

        Debug.DrawRay(new Vector3(40, 1, 40), Vector3.up * raycastDistance, Color.red, 1000);
        if (Physics.Raycast(new Vector3(40, 1, 40), Vector3.up, raycastDistance, LayerMask.GetMask("Water")))
        {
            Debug.Log("true");
        }
        else
        {
            Debug.Log("false");
        }

        // iterate all points in the terrain
        for (int i = 0; i < heightmapHeight; i++)
        {
            //Debug.Log(i * terrainData.heightmapScale.x + " " + heights[i, 0] * terrainData.heightmapScale.y);
            for (int j = 0; j < heightmapWidth; j++)
            {
                Vector3 pos = new Vector3(i * TerrainData.heightmapScale.x, heights[j, i] * TerrainData.heightmapScale.y , j * TerrainData.heightmapScale.z);
                // check if we are under a river
                RaycastHit hit;
                if (Physics.Raycast(pos, Vector3.up, out hit, raycastDistance, LayerMask.GetMask("Water")))
                {
                    Debug.Log("hit");
                    float waterHeight = hit.point.y;
                    if (waterHeight - heights[j, i] * TerrainData.heightmapScale.y > maxDepth)
                    {
                        heights[j, i] = (waterHeight - maxDepth) / TerrainData.heightmapScale.y;
                    }
                    
                }
            }
        }

        TerrainData.SetHeights(0, 0, heights);
    }

    public override bool Equals(object obj)
    {
        return obj is RiverShallower shallower &&
               base.Equals(obj) &&
               EqualityComparer<TerrainData>.Default.Equals(TerrainData, shallower.TerrainData);
    }

    public override int GetHashCode()
    {
        int hashCode = 1509846340;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<TerrainData>.Default.GetHashCode(TerrainData);
        return hashCode;
    }
}