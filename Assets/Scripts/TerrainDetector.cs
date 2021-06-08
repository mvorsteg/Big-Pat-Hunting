using UnityEngine;

[System.Serializable]
public class TerrainDetector
{
    private Terrain terrain;
    private TerrainData terrainData;
    private int alphamapWidth;
    private int alphamapHeight;
    private float[,,] splatmapData;
    private int numTextures;

    public Terrain Terrain { get => terrain; }

    public TerrainDetector(Terrain terrain)
    {
        if (terrain == null)
            return;
        this.terrain = terrain;
        terrainData = terrain.terrainData;
        alphamapWidth = terrainData.alphamapWidth;
        alphamapHeight = terrainData.alphamapHeight;

        splatmapData = terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
        numTextures = splatmapData.Length / (alphamapWidth * alphamapHeight);
    }

    private Vector3 ConvertToSplatMapCoordinate(Vector3 worldPos)
    {
        Vector3 splatPos = new Vector3();
        Terrain ter = terrain;
        Vector3 terPos = ter.transform.position;
        splatPos.x = ((worldPos.x - terPos.x) / ter.terrainData.size.x) * ter.terrainData.alphamapWidth;
        splatPos.z = ((worldPos.z - terPos.z) / ter.terrainData.size.z) * ter.terrainData.alphamapHeight;
        return splatPos;
    }

    public int GetActiveTerrainTextureIdx(Vector3 pos)
    {
        if (terrain == null)
            return -1;
        Vector3 terrainCord = ConvertToSplatMapCoordinate(pos);
        int activeTerrainIdx = 0;
        float largetstOpacity = 0f;

        for (int i = 0; i < numTextures; i++)
        {
            if (largetstOpacity < splatmapData[(int)terrainCord.z, (int)terrainCord.x, i])
            {    
                activeTerrainIdx = i;
                largetstOpacity = splatmapData[(int)terrainCord.z, (int)terrainCord.x, i];
            }
        }

        return activeTerrainIdx;
    }
}