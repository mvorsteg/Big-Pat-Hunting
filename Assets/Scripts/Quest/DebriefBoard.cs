using UnityEngine;
using TMPro;
using System;

public class DebriefBoard : MonoBehaviour
{
    [SerializeField]
    private GameObject textPrefab;
    [SerializeField]
    private Transform textParent;

    private void Start()
    {
        
    }

    public void AddRow(QuestManager.KillInfo info)
    {
        string weight;
        string distance;
        if (PlayerPrefs.GetInt("Units", 0) == 0)    // If using imperial units
        {
            weight = info.type.baseWeight * info.scale + " lb";
            distance = Utility.MetersToFeet(info.distance) + " ft";
        }
        else
        {
            weight = Utility.PoundsToKg(info.type.baseWeight * info.scale) + " kg";
            distance = info.distance + " m";
        }

        TextMeshProUGUI tmp = Instantiate(textPrefab, textParent).GetComponent<TextMeshProUGUI>();
        tmp.text = String.Format("{0}   {1}     {2}     {3} ", info.type.name, weight, distance, info.bodyArea);
    }
}