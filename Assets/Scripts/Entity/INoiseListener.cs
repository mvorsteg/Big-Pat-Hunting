public interface INoiseListener
{   
    /// <summary>
    /// Called when a noise generator plays a sound close to the sensor
    /// </summary>
    /// <param name="generator">The noise generator that created the noise</param>
    void HearNoise(NoiseGenerator generator);
}