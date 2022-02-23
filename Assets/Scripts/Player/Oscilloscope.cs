using UnityEngine;
using UnityEngine.UI;

public class Oscilloscope : MonoBehaviour
{
    public float amplitude = 1f;
    public float frequency = 1f;
    public float movementSpeed = 1f;

    private const float tau = 2 * Mathf.PI;

    private float[] points;

    private LineRenderer lineRenderer;
    [SerializeField]
    private int numPoints = 100;
    [SerializeField]
    float xStart = 0, xFinish = tau;

    [SerializeField]
    private float minAmplitude = 5f, maxAmplitude = 65f;

    private void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer.positionCount = numPoints;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        DrawSinWave();
    }

    private void DrawSinWave()
    {   
        lineRenderer.positionCount = numPoints;
        for (int i = 0; i < numPoints; i++)
        {
            float progress = (float)i / (numPoints - 1);
            float x = Mathf.Lerp(xStart, xFinish, progress);
            float y = amplitude * Mathf.Sin((tau * frequency *x) + Time.timeSinceLevelLoad * movementSpeed);
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }

}