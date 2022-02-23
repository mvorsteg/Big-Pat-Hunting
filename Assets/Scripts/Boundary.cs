using UnityEngine;

public class Boundary : MonoBehaviour
{

    [SerializeField]
    private GameObject warningText;

    private void Awake()
    {
        
    }

    private void Start()
    {
        warningText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        warningText.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        warningText.SetActive(true);   
    }
}