using UnityEngine;
using UnityEngine.Events;
using System;

public class NoiseSensor : MonoBehaviour
{
    public float noiseThreshold = 5f;

    private INoiseListener noiseListener;

    private UnityAction<object> onNoiseGenerated;

    private void Awake()
    {
        noiseListener = GetComponent<INoiseListener>();

        onNoiseGenerated = new UnityAction<object>(OnNoiseGenerated);
    }

    private void OnEnable()
    {
        Messenger.Subscribe(MessageIDs.NoiseGenerated, onNoiseGenerated);
    }

    private void OnDisable()
    {
        Messenger.Unsubscribe(MessageIDs.NoiseGenerated, onNoiseGenerated);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    private void OnNoiseGenerated(object data)
    {
        NoiseInfo info = (NoiseInfo)data;
        float effectiveDecibels = Utility.CalculateVolumeAtDistance(info.decibels, Vector3.Distance(info.position, transform.position));
        if (effectiveDecibels >= noiseThreshold)
        {
            Debug.Log(String.Format("{0} heard {1} at {2} dB", transform.name, info.noiseType.ToString(), effectiveDecibels));
            noiseListener.HearNoise(info);
        }
        else
        {
            Debug.Log(transform.name + " did not hear noise: " + effectiveDecibels + " dB");
        }
    }

}