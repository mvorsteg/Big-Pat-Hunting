using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : ScriptableObject
{

    public abstract void Initialize();

    /// <summary>
    /// generates a short description for the objectives menu
    /// </summary>
    /// <returns>the description string</returns>
    public abstract string GetShortDescription();

    //public abstract void UpdateQuest();

    //public abstract void CompleteQuest();

}