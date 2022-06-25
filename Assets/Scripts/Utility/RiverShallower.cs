using System.Collections.Generic;
using UnityEngine;

public class RiverShallower : MonoBehaviour
{
    private Terrain terrain;

    private Vector3 offset = new Vector3(-512, 0, -512);

    public float raycastDistance = 5f;
    public float maxDepth = 1f;

    public Transform testPosition;

    public Terrain Terrain { get => terrain; set => terrain = value; }
    public TerrainData TerrainData { get => Terrain.terrainData; }

    private void Awake()
    {
        
    }

    private void Start()
    {
        // terrain = Terrain.activeTerrain;
        // ModifyTerrain();
    }

    public void ModifyTerrain()
    {
        int heightmapHeight = TerrainData.heightmapResolution;
        int heightmapWidth = TerrainData.heightmapResolution;

        float [,] heights = TerrainData.GetHeights(0, 0, heightmapHeight, heightmapWidth);

        //Debug.DrawRay(testPosition.position, Vector3.up * raycastDistance, Color.red, 10);
        if (testPosition == null)
        {
            Debug.Log("Test position not defined");
        }
        else
        {
            
            Vector3 testPos = testPosition.position + Vector3.up * raycastDistance;
            Debug.Log("test y " + testPos.y);
            RaycastHit hit;
            Debug.DrawRay(testPos, Vector3.down * raycastDistance, Color.red, 10);
            if (Physics.Raycast(testPos, Vector3.down, out hit, raycastDistance, LayerMask.GetMask("Water")))
            {
                Debug.Log("true");
            }
            else
            {
                Debug.Log("false");
            }
        }

        // iterate all points in the terrain
        for (int i = 0; i < heightmapHeight; i++)
        {
            //Debug.Log(i * terrainData.heightmapScale.x + " " + heights[i, 0] * terrainData.heightmapScale.y);
            for (int j = 0; j < heightmapWidth; j++)
            {
                if (i == 1130 && j == 718)
                {
                    Debug.Log("here");
                }
                float x = offset.x + i * TerrainData.heightmapScale.x;
                float y = heights[j, i] * TerrainData.heightmapScale.y;
                float z = offset.z + j * TerrainData.heightmapScale.z;
                Vector3 pos = new Vector3(x, y, z);
                Vector3 testPos = pos + Vector3.up * raycastDistance;
                RaycastHit hit;
                if (Physics.Raycast(testPos, Vector3.down, out hit, raycastDistance, LayerMask.GetMask("Water")))
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