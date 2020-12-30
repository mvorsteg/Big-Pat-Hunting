using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class NoiseGenerator : MonoBehaviour
{
    private List<INoiseListener> listeners;  // this may be changed to a dictionary later
    private SphereCollider col;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<SphereCollider>();
        listeners = new List<INoiseListener>();
    }

    /// <summary>
    /// Sets the radius of the trigger collider. Anything inside the trigger will hear the noise.
    /// </summary>
    /// <param name="range">The new radius</param>
    public void SetRadius(float range)
    {
        col.radius = range;
    }

    /// <summary>
    /// Creates a noise that all listeners in range will hear
    /// </summary>
    public void GenerateNoise()
    {
        foreach (INoiseListener s in listeners)
        {
            s.HearNoise(this);
        }
    }

    /// <summary>
    /// Returns the position of the noise generator
    /// </summary>
    /// <returns>The position</returns>
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    // add sensor to list so that it hears anything
    private void OnTriggerEnter(Collider other)
    {
        INoiseListener l = other.GetComponent<INoiseListener>();
        if (l != null)
        {
            listeners.Add(l);
        }
    }

    // remove sensor from list since it is too far now
    private void OnTriggerExit(Collider other)
    {
        INoiseListener l = other.GetComponent<INoiseListener>();
        if (l != null)
        {
            listeners.Remove(l);
        }
    }
}
