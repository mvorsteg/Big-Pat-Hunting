using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{

    public UnityEvent myEvent;
    public string str;
    public TextMeshProUGUI interactionTextRef;
    //private bool isActive = false;

    private static Interaction loadedInteraction;
    private static TextMeshProUGUI interactionText;
 
    private void Awake()
    {
        if (interactionText == null)
        {
            interactionText = interactionTextRef;
        }    
    }

    public static void Go()
    {
        if (loadedInteraction != null)
        {
            loadedInteraction.myEvent.Invoke();
            Reset();
        }
    }

    public static void LoadInteraction(Interaction interaction)
    {
        interactionText.text = "[E] " + interaction.str;
        loadedInteraction = interaction;
    }

    public static void Reset()
    {
        if (interactionText != null)
        {
            interactionText.text = "";
        }
        //PlayerUI.interactionText.gameObject.SetActive(false);
        loadedInteraction = null;
    }

    public static bool IsLoaded()
    {
        return loadedInteraction != null;
    }
    
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.transform.tag == "Player" && Player.player.UI.IsUnpaused())
    //     {
    //         isActive = true;
    //         PlayerUI.interactionText.gameObject.SetActive(true);
    //         PlayerUI.interactionText.text = "[" + buttonType.ToString() + "] " + str;
    //         loadedInteraction = this;
    //     }
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.transform.tag == "Player")
    //     {
    //         isActive = false;
    //         if (loadedInteraction == this)
    //             Reset();
    //     }
    // }

    /*void OnDestroy()
    {
        PlayerUI.interactionText.text = "";
        PlayerUI.interactionText.gameObject.SetActive(false);
        loadedInteraction = null;
    }*/
    
}
