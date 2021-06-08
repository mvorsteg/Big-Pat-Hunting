using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    private PlayerControls controls;
    [SerializeField]
    private GameObject mainMenu;

    private void Awake()
    {
        controls = new PlayerControls();   

        controls.AnyKey.AnyKey.performed += ctx => AnyKeyPressed();
    }

    private void OnEnable()
    {
        controls.AnyKey.Enable();    
    }

    private void OnDisable()
    {
        controls.AnyKey.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void AnyKeyPressed()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
