using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RegionDefiniton")]
public class RegionDefinition : ScriptableObject
{

    public string regionName;
    [TextArea(5,20)]
    public string description;

    public LevelDefinition[] levels;
}
