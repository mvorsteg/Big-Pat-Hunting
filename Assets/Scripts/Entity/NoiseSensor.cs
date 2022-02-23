using UnityEngine;
using UnityEngine.Events;

public class NoiseSensor : MonoBehaviour
{
    public float noiseThreshold = 5f;

    private UnityAction<object> onFootstepSound;

    private void Awake()
    {
        onFootstepSound = new UnityAction<object>(OnFootstepSound);
    }

    private void OnEnable()
    {
        EventManager.StartListening("Footstep", onFootstepSound);
    }

    private void OnDisable()
    {
        EventManager.StopListening("Footstep", onFootstepSound);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    private void OnFootstepSound(object data)
    {
        NoiseInfo info = (NoiseInfo)data;
        float effectiveDecibels = Utility.CalculateVolumeAtDistance(info.decibels, Vector3.Distance(info.position, transform.position));
        if (effectiveDecibels >= noiseThreshold)
        {
            Debug.Log(transform.parent.name + " heard noise: " + effectiveDecibels + " dB");
        }
        else
        {
            Debug.Log(transform.parent.name + " did not hear noise: " + effectiveDecibels + " dB");
        }
    }

}