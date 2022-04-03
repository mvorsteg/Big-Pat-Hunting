using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour 
{

    public enum LevelIDs
    {
        MainMenu = 0,
        LevelSelect = 1,

        GR1,
        GR2,
        GR3,
        GR4,
        GR5,
    }

    public enum LevelState
    {
        Incomplete,
        Complete,
        Failed,
    }

    private static Dictionary<LevelIDs, LevelState> levelStates;
    private static LevelLoader levelLoader;

    public static Dictionary<LevelIDs, LevelState> LevelStates { get => levelStates; }
    public static LevelLoader instance
    {
        get
        {
            if (!levelLoader)
            {
                Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
            }

            return levelLoader;
        }
    }

    
    public GameObject canvas;
    public Slider bar;

    [SerializeField]
    public static int currRegionId = 0;

    private void Awake()
    {

        if (levelLoader == null)
        {
            levelLoader = this;
            DontDestroyOnLoad(this);
            levelStates = new Dictionary<LevelIDs, LevelState>();
            ResetLevelStates();
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    private void OnEnable()
    {
        canvas.SetActive(false);
    }

    public static void SelectLocation(int idx)
    {
        currRegionId = idx;
        LoadLevel(LevelIDs.LevelSelect);
    }

    public static void LoadLevel(LevelIDs levelID)
    {
        instance.StartCoroutine(instance.LoadAsync((int)levelID));
    }

    private void ResetLevelStates()
    {
        foreach (LevelIDs id in Enum.GetValues(typeof(LevelIDs)))
        {
            levelStates[id] = LevelState.Incomplete;
        }
    }

    private IEnumerator LoadAsync(int idx)
    {
        canvas.SetActive(true);
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(idx);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log(operation.progress);
            bar.value = progress;
            yield return null;
        }
        //canvas.SetActive(false);
    }

}