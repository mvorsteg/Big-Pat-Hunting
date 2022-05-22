using UnityEngine;
using UnityEngine.Events;

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
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            warningText.SetActive(false);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Animal"))
        {
            Animal animal = other.transform.GetComponent<Animal>();
            if (animal != null)
            {
                Messenger.SendMessage(MessageIDs.AnimalEnterBoundary, animal);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            warningText.SetActive(true);   
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Animal"))
        {
            Animal animal = other.transform.GetComponent<Animal>();
            if (animal != null)
            {
                animal.ExitBoundary();
                Messenger.SendMessage(MessageIDs.AnimalExitBoundary, animal);
            }
        }
    }
}