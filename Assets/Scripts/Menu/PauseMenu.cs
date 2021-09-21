using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused = false;

    [SerializeField]
    private GameObject pauseMenuObj;

    public PostProcessVolume volume;
    private ColorGrading colorGrading;
    private DepthOfField depthOfField;

    [SerializeField]
    private AudioClip buttonHover;
    [SerializeField]
    private AudioClip buttonClick;
    private AudioSource audioSource;

    public float transitionTime = 0.5f;

    private void Awake()
    {
        volume.profile.TryGetSettings<ColorGrading>(out colorGrading);
        volume.profile.TryGetSettings<DepthOfField>(out depthOfField);
        audioSource = GetComponent<AudioSource>();
        audioSource.ignoreListenerPause = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        EnablePauseMenu(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnablePauseMenu(bool val)
    {
        pauseMenuObj.SetActive(val);
        Time.timeScale = val ? 0 : 1;
        Cursor.lockState = val ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = val;

        colorGrading.enabled.value = val;
        depthOfField.enabled.value = val;

        AudioListener.pause = val;
    }

    public void PlayButtonHoverSound()
    {
        audioSource.clip = buttonHover;
        audioSource.Play();
    }

    public void PlayButtonClickSound()
    {
        audioSource.clip = buttonClick;
        audioSource.Play();
    }
}
