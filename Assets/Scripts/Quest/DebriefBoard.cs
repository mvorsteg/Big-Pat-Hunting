using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DebriefBoard : MonoBehaviour
{
    [SerializeField]
    private GameObject rowPrefab;
    [SerializeField]
    private Transform textParent;
    private RecordSerializer recordSerializer;

    private List<GameObject> entries;

    private void Awake()
    {
        entries = new List<GameObject>();
        recordSerializer = FindObjectOfType<RecordSerializer>();
    }

    private void Start()
    {
        //List<KillQuest.KillInfo> infoList = recordSerializer.ReadQuest();
    }

    public void Clear()
    {
        int count = entries.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(entries[i]);
        }
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

        entries.Add(row);
    }
}