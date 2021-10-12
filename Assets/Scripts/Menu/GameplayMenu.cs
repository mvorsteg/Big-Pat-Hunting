using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown unitsDropdown;

    // Start is called before the first frame update
    void Start()
    {
        // adjust graphics quality and associated slider in settings menu
        int units = PlayerPrefs.GetInt("Units", 0);
        unitsDropdown.value = units;
    }

    public void SetUnits(int newUnit)
    {
        PlayerPrefs.SetInt("Units", newUnit);
    }
}
