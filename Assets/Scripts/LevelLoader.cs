using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelLoader : MonoBehaviour 
{
    public Slider bar;

    public void LoadLevel(int idx)
    {
        Debug.Log("starting");
        StartCoroutine(LoadAsync(idx));
    }

    private IEnumerator LoadAsync(int idx)
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(idx);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log(operation.progress);
            bar.value = progress;
            yield return null;
        }

    }

}