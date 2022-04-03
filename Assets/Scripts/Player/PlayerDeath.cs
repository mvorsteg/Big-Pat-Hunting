using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour
{
    private PlayerControls controls;
    [SerializeField]
    private GameObject respawnText;

    private void Awake()
    {
        controls = new PlayerControls();   
        controls.AnyKey.AnyKey.performed += ctx => ReturnToLevelSelect();
    }

    private void OnEnable()
    {
        respawnText.SetActive(false);
        StartCoroutine(SetControls(true, 2f));
        //Debug.Break();
    }

    private void OnDisable()
    {
        respawnText.SetActive(false);
        controls.Disable();
    }

    private void ReturnToLevelSelect()
    {
        
    }

    private IEnumerator SetControls(bool active, float delay)
    {
        yield return new WaitForSeconds(delay);
        respawnText.SetActive(active);
        if (active)
            controls.Enable();
        else
            controls.Disable();
    }
}