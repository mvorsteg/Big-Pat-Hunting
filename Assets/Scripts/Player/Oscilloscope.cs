using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class Oscilloscope : MonoBehaviour
{
    public float amplitude = 1f;
    public float frequency = 1f;
    public float movementSpeed = 1f;

    [SerializeField]
    private float spikeTime = 0.5f;

    private const float tau = 2 * Mathf.PI;

    private float[] linePoints;
    private RectTransform[] bars;

    [SerializeField]
    private float minAmplitude = 5f, maxAmplitude = 65f;

    [SerializeField]
    private Transform barsParent;

    private UnityAction<object> onNoiseGenerated;

    private void Awake()
    {
        bars = barsParent.GetComponentsInChildren<RectTransform>();

        onNoiseGenerated = new UnityAction<object>(OnNoiseGenerated);
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        Messenger.Subscribe(MessageIDs.NoiseGenerated, onNoiseGenerated);
    }


    private void OnDisable()
    {
        Messenger.Unsubscribe(MessageIDs.NoiseGenerated, onNoiseGenerated);
    }

    private void Update()
    {
        DrawSinWave();
    }

    private void DrawSinWave()
    {   
        for (int i = 0; i < bars.Length; i++)
        {
            float progress = (float)i / (bars.Length - 1);
            float x = Mathf.Lerp(bars[i].position.x, bars[i].position.y, progress);
                float y = amplitude * (Mathf.Sin((tau * frequency * x) + Time.timeSinceLevelLoad * movementSpeed) + 1);
            bars[i].sizeDelta = new Vector2(bars[i].sizeDelta.x, y);
        }
    }

    private void OnNoiseGenerated(object data)
    {
        NoiseInfo info = (NoiseInfo)data;
        StartCoroutine(WaveSpike(Mathf.Clamp(info.decibels, minAmplitude, maxAmplitude)));
        
    }

    private IEnumerator WaveSpike(float peakAmplitude)
    {
        float elapsedTime = 0f;
        while (elapsedTime < spikeTime)
        {
            elapsedTime += Time.deltaTime;
            amplitude = Mathf.Lerp(peakAmplitude, minAmplitude, elapsedTime / spikeTime);
            yield return null;
        }
        
    }

}