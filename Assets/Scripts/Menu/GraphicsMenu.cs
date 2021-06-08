using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicsMenu : MonoBehaviour
{

    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TextMeshProUGUI graphicsText;
    [SerializeField]
    private TMP_Dropdown resolutionDropdown;

    private List<string> dropdowns;

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        // adjust graphics quality and associated slider in settings menu
        int quality = PlayerPrefs.GetInt("Quality", 4);
        AdjustQuality(quality);
        slider.value = quality;
        
        // populate resolution dropdown with options
        dropdowns = new List<string>();
        for (int i = Screen.resolutions.Length - 1; i >= 0; i--)
        {
            string s = Screen.resolutions[i].width + "x" + Screen.resolutions[i].height + " (" + ReduceFraction(Screen.resolutions[i].width, Screen.resolutions[i].height).Item1 + ":" + ReduceFraction(Screen.resolutions[i].width, Screen.resolutions[i].height).Item2 + ")";
            //Debug.Log(s);
            if (!dropdowns.Contains(s) && IsValidResolution(Screen.resolutions[i].width, Screen.resolutions[i].height))
                dropdowns.Add(s);   
        }
        resolutionDropdown.AddOptions(dropdowns);
    }

    public void AdjustQuality(float val)
    {
        Debug.Log("setting quality " + (int)val);
        QualitySettings.SetQualityLevel((int)val, true);
        PlayerPrefs.SetInt("Quality", (int)val);
        switch ((int)val)
        {
            case 0 :
                graphicsText.text = "Graphics: Very Low";
                break;
            case 1 :
                graphicsText.text = "Graphics: Low";
                break;
            case 2 :
                graphicsText.text = "Graphics: Medium";
                break;
            case 3 :
                graphicsText.text = "Graphics: High";
                break;
            case 4 :
                graphicsText.text = "Graphics: Very High";
                break;
            case 5 :
                graphicsText.text = "Graphics: Ultra";
                break;
        }

    }

    public void SetResolution(int val)
    {
        // split string into resolution
        string s = dropdowns[val];
        string[] tok = s.Split('x');
        int w = Convert.ToInt32(tok[0]);
        int h = Convert.ToInt32(tok[1].Split(' ')[0]);
        Screen.SetResolution(w, h, true, 60);
    }

    /// <summary>
    /// returns true if the aspect ratio is either 16:9 or 4:3
    /// </summary>
    /// <param name="h">horizontal pixels</param>
    /// <param name="v">vertical pixels</param>
    /// <returns>true if 16:9 or 4:3, else false</returns>
    private bool IsValidResolution(int h, int v)
    {
        Tuple<int, int> aspect_ratio = ReduceFraction(h, v);
        if (aspect_ratio.Item1 == 16 && aspect_ratio.Item2 == 9)
            return true;
        if (aspect_ratio.Item1 == 4 && aspect_ratio.Item2 == 3)
            return true;
        return false;
    }

    /// <summary>
    /// reduces the fraction to its most simplified form
    /// </summary>
    /// <param name="n">numerator</param>
    /// <param name="d">denominator</param>
    /// <returns>tuple of type int-int, (numerator, denominator)</returns>
    private Tuple<int, int> ReduceFraction(int n, int d)
    {
        //find gcd
        int a = n;
        int b = d;
        while (b > 0)
        {
            int rem = a % b;
            a = b;
            b = rem;
        }
        return new Tuple<int, int>(n / a, d / a);
    }
}
