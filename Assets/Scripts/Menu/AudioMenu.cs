using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider soundSlider;
    [SerializeField]
    private Slider dialogueSlider;

    public void LoadSoundSettings()
    {
        // read volumes from playerprefs if they exist
        float val = PlayerPrefs.GetFloat("musicVol", 1);
        musicSlider.value = val;
        SetMusicVolume(val);

        val = PlayerPrefs.GetFloat("soundVol", 1);
        soundSlider.value = val;
        SetSoundVolume(val);

        val = PlayerPrefs.GetFloat("dialogueVol", 1);
        dialogueSlider.value = val;
        SetDialogueVolume(val);
    }

    public void SetMusicVolume(float val)
    {
        mixer.SetFloat("musicVol", Mathf.Log10(val) * 20);
        PlayerPrefs.SetFloat("musicVol", val);
    }

    public void SetSoundVolume(float val)
    {
        mixer.SetFloat("soundVol", Mathf.Log10(val) * 20);
        PlayerPrefs.SetFloat("SoundVol", val);
    }

    public void SetDialogueVolume(float val)
    {
        //mixer.SetFloat("radioVol", val);
        mixer.SetFloat("dialogueVol", Mathf.Log10(val) * 20);
        PlayerPrefs.SetFloat("dialogueVol", val);
    }
}