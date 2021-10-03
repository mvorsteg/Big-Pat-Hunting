using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LevelLighting")]
public class LevelLighting : ScriptableObject
{

    public Material skybox;
    public Light sun;
    public float ambientLighting;
    
    public Color fogColor;
    public float fogDensity;

    private void Start()
    {
    }
}

