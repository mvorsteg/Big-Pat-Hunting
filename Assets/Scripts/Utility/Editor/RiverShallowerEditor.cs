using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RiverShallower))]
public class RiverShallowerEditor : Editor
{
    private RiverShallower riverShallower;
    private Terrain terrain;
    private float riverDepth;
    private float maxDistance;

    private void OnEnable()
    {
        Debug.Log("OnEnable is called");
        riverShallower = (RiverShallower)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Our Custom Inspector");
        EditorGUILayout.Space();
        riverShallower.Terrain = (Terrain)EditorGUILayout.ObjectField("Terrain", riverShallower.Terrain, typeof(Terrain), true);
        EditorGUILayout.Space();
        riverShallower.maxDepth = EditorGUILayout.FloatField("River Depth", riverShallower.maxDepth);
        riverShallower.raycastDistance = EditorGUILayout.FloatField("Max Raycast Distance", riverShallower.raycastDistance);
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Undo"))
        {

        }
        if (GUILayout.Button("Modify Terrain"))
        {
            Undo.RecordObject(riverShallower.TerrainData, "terrainData");
            riverShallower.ModifyTerrain();
            EditorUtility.SetDirty(riverShallower.TerrainData);
        }

        GUILayout.EndHorizontal();
    }
}