using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EntityType")]
public class EntityType : ScriptableObject
{
    public EntityType[] attacks;
    public EntityType[] runsFrom;
    public float baseWeight;
    public float baseValue = 100;
    // public string ;

    public bool RunsFrom(EntityType other)
    {
        for (int i = 0; i < runsFrom.Length; i++)
        {
            if (runsFrom[i] == other)
            {
                return true;
            }
        }
        return false;
    }

    public bool Attacks(EntityType other)
    {
        for (int i = 0; i < attacks.Length; i++)
        {
            if (attacks[i] == other)
            {
                return true;
            }
        }
        return false;
    }

}