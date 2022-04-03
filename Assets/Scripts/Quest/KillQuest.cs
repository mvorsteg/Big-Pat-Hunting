using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/KillQuest")]
public class KillQuest : Quest
{
    [System.Serializable]
    public struct KillInfo
    {
        public EntityType type;
        public float score;
        public float scale;
        public float distance;
        public BodyArea bodyArea;

        public KillInfo(EntityType type, float score, float scale, float distance, BodyArea bodyArea)
        {
            this.type = type;
            this.score = score;
            this.scale = scale;
            this.distance = distance;
            this.bodyArea = bodyArea;
        }
    }

    public EntityType target;       // the type of entity that the player is required to kill
    public int requiredKills = 1;   // number of kills required to complete the quest

    private int totalKills = 0;   // number of kills currently counted toward the goal
    private int numAvaiableTargets = 0; // number of target entities currently available in scene

    public int TotalKills { get => totalKills; }
    public int NumAvaiableTargets
    { 
        get => numAvaiableTargets; 
        set
        {
            numAvaiableTargets = value; 
            if (numAvaiableTargets <= 0)
            {
                EndQuest();
            } 
        } 
    }

    // private void Awake()
    // {
    //        // CANNOT USE AWAKE ON SCRIPTABLE OBJECTS
    // }

    // private void Start()
    // {
    //     totalKills = 0;
    // }

    public override void Initialize()
    {
        totalKills = 0;
        numAvaiableTargets = 0;
    }

    /// <summary>
    /// registers an entity kill that counts toward
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>true if the quest is complete, otherwise false</returns>
    public bool RegisterKill(Entity entity)
    {
        totalKills++;
        NumAvaiableTargets--;
        QuestManager.UpdateQuest(this);
        if (totalKills >= requiredKills)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// generates a short description for the objectives menu
    /// </summary>
    /// <returns>the description string</returns>
    public override string GetShortDescription()
    {
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

    private void EndQuest()
    {
        if (totalKills >= requiredKills)
        {
            Messenger.SendMessage(MessageIDs.QuestComplete, this);
        }
        else
        {
            Messenger.SendMessage(MessageIDs.QuestFailed, this);
        }
    }
}