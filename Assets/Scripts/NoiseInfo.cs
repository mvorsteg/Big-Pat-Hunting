using UnityEngine;

public enum NoiseType
{
    Footstep,
    Gunshot,
}
/// <summary>
/// A structure containing data about a noise
/// </summary>
public struct NoiseInfo
{
    public GameObject source;
    public NoiseType noiseType;
    public Vector3 position;
    public float decibels;

    public NoiseInfo(float decibels, Vector3 position, GameObject source)
    {
        this.decibels = decibels;
        this.position = position;
        this.noiseType = NoiseType.Footstep;
        this.source = source;
    }

}