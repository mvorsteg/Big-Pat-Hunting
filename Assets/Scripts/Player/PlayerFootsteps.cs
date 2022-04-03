using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private float timeToFootstep;
    
    private Transform groundCheck;
    private float groundDistance;

    private LayerMask groundMask;
    public LayerMask terrainMask;
    public LayerMask waterMask;

    private AudioSource audioSource;

    public AudioClip[] grassFootsteps;
    public AudioClip[] gravelFootsteps;
    public AudioClip[] snowFootsteps;
    public AudioClip[] stoneFootsteps;
    public AudioClip[] woodFootsteps;
    public AudioClip[] metalFootsteps;
    public AudioClip[] waterFootsteps;

    public float walkingDecibelModifier = 1f;
    public float sprintingDecibelModifier = 1.5f;
    public float crouchingDecibelModifier = 0.5f;

    public float grassFootstepDecibels = 40f;
    public float gravelFootstepDecibels = 40f;
    public float snowFootstepDecibels = 40f;
    public float stoneFootstepDecibels = 40f;
    public float woodFootstepDecibels = 40f;
    public float metalFootstepDecibels = 40f;
    public float waterFootstepDecibels = 40f;

    public float footstepDelayWalking = 0.6f;
    public float footstepDelaySprinting = 0.4f;    

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            groundMask = playerMovement.groundMask;
            groundCheck = playerMovement.groundCheck;
            groundDistance = playerMovement.groundDistance;
        }
    }

    private void Update()
    {
        timeToFootstep -= Time.deltaTime;
        if (playerMovement.IsGrounded && playerMovement.Velocity.magnitude > 2f && timeToFootstep <= 0)
        {
            MaterialType materialUnderfoot = GetMaterialTypeUnderfoot();
            AudioClip sound = GetRandomFootstepClip(materialUnderfoot);
            audioSource.clip = sound;
            audioSource.volume = playerMovement.IsCrouching ? 0.5f : 1.0f;
            audioSource.Play();
            timeToFootstep = playerMovement.IsSprinting ? footstepDelaySprinting : footstepDelayWalking;

            // send footstep
            NoiseInfo info = new NoiseInfo(GetDecibelsFromMaterial(materialUnderfoot), transform.position, NoiseType.Footstep, this.gameObject);
            if (playerMovement.IsSprinting)
            {
                info.decibels *= sprintingDecibelModifier;
            }
            else if (playerMovement.IsCrouching)
            {
                info.decibels *= crouchingDecibelModifier;
            }
            Messenger.SendMessage(MessageIDs.NoiseGenerated, info);
        }
    }

    /// <summary>
    /// Finds the MaterialType of the terrain or object directly under the player
    /// </summary>
    /// <returns>MaterialType corresponding to the texture</returns>
    private MaterialType GetMaterialTypeUnderfoot()
    {
        int terrainTextureIdx = -1;
        // check for water
        Debug.DrawRay(transform.position, Vector3.down, Color.black, 10f);
        if (Physics.Raycast(transform.position, Vector3.down, 2f, waterMask))
            return MaterialType.Water;
        // check for everything else
        if (Physics.CheckSphere(groundCheck.position, groundDistance, terrainMask))
            terrainTextureIdx = TerrainDetector.GetActiveTerrainTextureIdx(transform.position);
        
        if (terrainTextureIdx < 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(groundCheck.position, -transform.up, out hit, 1f, groundMask))
            {
                if (hit.transform.CompareTag("Stone"))
                    return MaterialType.Stone;
                if (hit.transform.CompareTag("Wood"))
                    return MaterialType.Wood;
                if (hit.transform.CompareTag("Metal"))
                    return MaterialType.Metal;
                if (hit.transform.CompareTag("Cloth"))
                    return MaterialType.Cloth;
            }
            return MaterialType.Default;
        }
        return TerrainDetector.GetMaterialFromTextureIdx(terrainTextureIdx);
    }

    /// <summary>
    /// Probes the ground underfoot to determine which type footstep sound should play,
    /// then picks a random element from the associated array of sounds
    /// </summary>
    /// <returns>a random sound effect of the correct type</returns>
    private AudioClip GetRandomFootstepClip(MaterialType materialType)
    {
        if (materialType == MaterialType.Grass)
            return grassFootsteps[Random.Range(0, grassFootsteps.Length)];
        if (materialType == MaterialType.Gravel)
            return gravelFootsteps[Random.Range(0, gravelFootsteps.Length)];
        if (materialType == MaterialType.Snow)
            return snowFootsteps[Random.Range(0, snowFootsteps.Length)];
        if (materialType == MaterialType.Stone)
            return stoneFootsteps[Random.Range(0, stoneFootsteps.Length)];
        if (materialType == MaterialType.Wood)
            return woodFootsteps[Random.Range(0, woodFootsteps.Length)];
        if (materialType == MaterialType.Metal)
            return metalFootsteps[Random.Range(0, metalFootsteps.Length)];
        if (materialType == MaterialType.Water)
            return waterFootsteps[Random.Range(0, waterFootsteps.Length)];
        // default to grass
        return grassFootsteps[Random.Range(0, grassFootsteps.Length)];
    }

    /// <summary>
    /// Maps a MaterialType to the corresponding decibel value for footsteps
    /// </summary>
    /// <returns>the correct FootstepDecibel member</returns>
    private float GetDecibelsFromMaterial(MaterialType materialType)
    {
        if (materialType == MaterialType.Grass)
            return grassFootstepDecibels;
        if (materialType == MaterialType.Gravel)
            return grassFootstepDecibels;
        if (materialType == MaterialType.Snow)
            return snowFootstepDecibels;
        if (materialType == MaterialType.Stone)
            return stoneFootstepDecibels;
        if (materialType == MaterialType.Wood)
            return woodFootstepDecibels;
        if (materialType == MaterialType.Metal)
            return metalFootstepDecibels;
        if (materialType == MaterialType.Water)
            return waterFootstepDecibels;
        // default to grass
        return grassFootstepDecibels;
    }
}