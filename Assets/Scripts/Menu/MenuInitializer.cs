using UnityEngine;
using UnityEngine.UI;

public class MenuInitializer : MonoBehaviour
{
    public AudioMenu audioMenu;
    private void Start()
    {
        audioMenu.LoadSoundSettings();
        // make sure we start on root menu
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        // remove debug panel image, if active
        transform.GetChild(0).GetComponent<Image>().enabled = false;    
    }    
}