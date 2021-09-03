using UnityEngine;
using System.Collections;

public class Door : Interaction 
{
    private Quaternion openRot;
    private Quaternion closedRot;

    public bool isOpen = false;
    public float swingTime = 1f;
    public float openDegrees = 90f;
    public float closedDegrees = 0f;

    private void Start()
    {
        openRot = Quaternion.Euler(0f, isOpen ? closedDegrees : openDegrees, 0f);
        closedRot = Quaternion.Euler(0f, isOpen ? openDegrees : closedDegrees, 0f);
    }    

    public void SwingDoor()
    {
        StartCoroutine(SwingDoor(isOpen ? closedRot : openRot));
    }

    private IEnumerator SwingDoor(Quaternion endRot)
    {
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
    }
}