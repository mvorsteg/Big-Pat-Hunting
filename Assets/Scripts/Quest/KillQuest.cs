using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/KillQuest")]
public class KillQuest : ScriptableObject, IQuest
{
    public EntityType target;       // the type of entity that the player is required to kill
    public int requiredKills = 1;   // number of kills required to complete the quest

    private float totalKills = 0;   // number of kills currently counted toward the goal

    public float TotalKills { get => totalKills; }

    // private void Start()
    // {
    //     totalKills = 0;
    // }

    /// <summary>
    /// registers an entity kill that counts toward
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>true if the quest is complete, otherwise false</returns>
    public bool RegisterKill(Entity entity)
    {
        totalKills++;
        QuestManager.UpdateQuest(this);
        if (totalKills >= requiredKills)
        {
            QuestManager.CompleteQuest(this);
            return true;
        }
        return false;
    }

    /// <summary>
    /// generates a short description for the objectives menu
    /// </summary>
    /// <returns>the description string</returns>
    public string GetShortDescription()
    {
        // return "Kill " + (requiredKills - totalKills) + " " + target.name + (requiredKills - totalKills == 1 ? "" : "s");
        return "Kill " + target.name + (requiredKills == 1 ? "" : "s") + " (" + totalKills + "/" + requiredKills + ")";
    }

    /// <summary>
    /// returns a dictionary with the target type and required amount
    /// </summary>
    /// <returns>the dictionary</returns>
    public Dictionary<EntityType, int> GetRequiredEntities()
    {
        Dictionary<EntityType, int> dict = new Dictionary<EntityType, int>();
        dict[target] = requiredKills;
        return dict;
    }

}