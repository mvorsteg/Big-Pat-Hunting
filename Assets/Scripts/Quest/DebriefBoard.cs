using UnityEngine;
using TMPro;
using System;

public class DebriefBoard : MonoBehaviour
{
    [SerializeField]
    private GameObject rowPrefab;
    [SerializeField]
    private Transform textParent;

    private void Start()
    {
        
    }

    public void AddRow(KillQuest.KillInfo info)
    {
        string weight;
        string distance;
        if (PlayerPrefs.GetInt("Units", 0) == 0)    // If using imperial units
        {
            weight = string.Format("{0:0.##}", info.type.baseWeight * info.scale) + " lb";
            distance = string.Format("{0:0.##}", Utility.MetersToFeet(info.distance)) + " ft";
        }
        else
        {
            weight = string.Format("{0:0.##}", Utility.PoundsToKg(info.type.baseWeight * info.scale)) + " kg";
            distance = string.Format("{0:0.##}", info.distance) + " m";
        }

        GameObject row = Instantiate(rowPrefab, textParent);
        TextMeshProUGUI[] tmp = row.GetComponentsInChildren<TextMeshProUGUI>();
        tmp[0].text = info.type.name;
        tmp[1].text = weight;
        tmp[2].text = distance;
        tmp[3].text = info.bodyArea.ToString();
    }
}