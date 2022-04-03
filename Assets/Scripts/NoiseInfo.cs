using UnityEngine;

public enum NoiseType
{
    Footstep,
    Gunshot,
    Misc,
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

    public NoiseInfo(float decibels, Vector3 position, NoiseType type, GameObject source)
    {
        this.decibels = decibels;
        this.position = position;
        this.noiseType = type;
        this.source = source;
    }

}