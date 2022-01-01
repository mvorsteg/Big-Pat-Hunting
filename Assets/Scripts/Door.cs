using UnityEngine;
using System.Collections;

public class Door : Interaction 
{
    [System.Serializable]
    public struct Doorknob
    {
        public GameObject knob;
        public Transform lockedKnobPos;
        public Transform unlockedKnobPos;
        public Vector3 restKnobPos;
        public Quaternion restKnobRot;
        
    };

    [SerializeField]
    private AudioClip lockedSound, unlockedSound, openSound, closedSound;
    private AudioSource audioSource;

    private Quaternion openDoorRot;
    private Quaternion closedDoorRot;

    [SerializeField]
    private Doorknob[] doorknobs;

    public float lockedRotTime, unlcokedRotTime;

    public bool isLocked = false;
    public bool isOpen = false;
    public float swingTime = 1f;
    public float openDegrees = 90f;
    public float closedDegrees = 0f;

    private bool isMoving = false;

    private void Awake()
    {
        for (int i = 0; i < doorknobs.Length; i++)
        {
            doorknobs[i].restKnobPos = doorknobs[i].knob.transform.localPosition;
            doorknobs[i].restKnobRot = doorknobs[i].knob.transform.localRotation;
            audioSource = GetComponent<AudioSource>();
            openDoorRot = Quaternion.Euler(0f, isOpen ? closedDegrees : openDegrees, 0f);
            closedDoorRot = Quaternion.Euler(0f, isOpen ? openDegrees : closedDegrees, 0f);
        }
    }    

    public void SwingDoor()
    {
        StartCoroutine(SwingDoor(isOpen ? closedDoorRot : openDoorRot));
    }

    public void TryOpen()
    {
        if (!isMoving)
        {
            if (isLocked)
            {
                StartCoroutine(DoorknobLocked());
            }
            else
            {
                StartCoroutine(SwingDoor(isOpen ? closedDoorRot : openDoorRot));
            }
        }
    }

    private IEnumerator SwingDoor(Quaternion endRot)
    {
        isMoving = true;
        yield return StartCoroutine(DoorknobOpen());
        if (!isOpen)
        {
            audioSource.clip = openSound;
            audioSource.Play();
        }
        Quaternion startRot = transform.localRotation;   
        float elapsedTime = 0;
        while (elapsedTime < swingTime)
        {
            elapsedTime += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(startRot, endRot, elapsedTime / swingTime);
            yield return null;
        }
        isOpen = !isOpen;
        str = isOpen ? "Close Door" : "Open Door";
        if (!isOpen)
        {
            audioSource.clip = closedSound;
            audioSource.Play();
        }
        yield return StartCoroutine(DoorknobClose());
        isMoving = false;
    }

    private IEnumerator DoorknobLocked()
    {
        isMoving = true;
        float elapsedTime;
        audioSource.clip = lockedSound;
        audioSource.Play();
        for (int i = 0; i < 2; i++)
        {
            elapsedTime = 0;
            while (elapsedTime < lockedRotTime)
            {
                elapsedTime += Time.deltaTime;
                for (int j = 0; j < doorknobs.Length; j++)
                {
                    doorknobs[j].knob.transform.localPosition = Vector3.Lerp(doorknobs[j].restKnobPos, doorknobs[j].lockedKnobPos.localPosition, elapsedTime / lockedRotTime);
                    doorknobs[j].knob.transform.localRotation = Quaternion.Lerp(doorknobs[j].restKnobRot, doorknobs[j].lockedKnobPos.localRotation, elapsedTime / lockedRotTime);
                }
                yield return null;
            }
            elapsedTime = 0;
            while (elapsedTime < lockedRotTime)
            {
                elapsedTime += Time.deltaTime;
                for (int j = 0; j < doorknobs.Length; j++)
                {
                    doorknobs[j].knob.transform.localPosition = Vector3.Lerp(doorknobs[j].lockedKnobPos.localPosition, doorknobs[j].restKnobPos, elapsedTime / lockedRotTime);
                    doorknobs[j].knob.transform.localRotation = Quaternion.Lerp(doorknobs[j].lockedKnobPos.localRotation, doorknobs[j].restKnobRot, elapsedTime / lockedRotTime);
                
                }yield return null;
            }
        }
        isMoving = false;
    }

    private IEnumerator DoorknobOpen()
    {
        float elapsedTime = 0;
        while (elapsedTime < lockedRotTime)
        {
            elapsedTime += Time.deltaTime;
            for (int i = 0; i < doorknobs.Length; i++)
            {
                doorknobs[i].knob.transform.localPosition = Vector3.Lerp(doorknobs[i].restKnobPos, doorknobs[i].unlockedKnobPos.localPosition, elapsedTime / lockedRotTime);
                doorknobs[i].knob.transform.localRotation = Quaternion.Lerp(doorknobs[i].restKnobRot, doorknobs[i].unlockedKnobPos.localRotation, elapsedTime / lockedRotTime);
            }
            yield return null;
        }
    }

    private IEnumerator DoorknobClose()
    {
        float elapsedTime = 0;
        while (elapsedTime < lockedRotTime)
        {
            elapsedTime += Time.deltaTime;
            for (int i = 0; i < doorknobs.Length; i++)
            {
                doorknobs[i].knob.transform.localPosition = Vector3.Lerp(doorknobs[i].unlockedKnobPos.localPosition, doorknobs[i].restKnobPos, elapsedTime / lockedRotTime);
                doorknobs[i].knob.transform.localRotation = Quaternion.Lerp(doorknobs[i].unlockedKnobPos.localRotation, doorknobs[i].restKnobRot, elapsedTime / lockedRotTime);
            }
            yield return null;
        }
    }

}