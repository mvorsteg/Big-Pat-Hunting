using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/LevelDefinition")]
public class LevelDefinition : ScriptableObject
{
    public string siteName;
    [TextArea(5,20)]
    public string siteDescription;
    public Sprite siteImage;
    public LevelLoader.LevelIDs levelID;
}