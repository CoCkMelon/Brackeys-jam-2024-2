using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour
{
    [SerializeField] private AudioClip[] terrainCollisionSounds;
    [SerializeField] private AudioClip similarObjectCollisionSound;
    [SerializeField] private AudioClip rockCollisionSound;
    // [SerializeField] private Transform collisionPoint; // Optional: Specify the point to check for collisions


    [SerializeField] private int audioSourcePoolSize = 2; // Number of AudioSource GameObjects in the pool
    [SerializeField] private float minDistance = 1f; // Minimum distance for volume attenuation
    [SerializeField] private float maxDistance = 50f; // Maximum distance for volume attenuation
    [SerializeField] private float forceDivisor = 30f;
    [SerializeField] private float distanceDivisor = 3f;
    private AudioSource[] audioSourcePool;
    private int currentAudioSourceIndex = 0;
    private void Start()
    {
        // Initialize the pool of AudioSource GameObjects
        audioSourcePool = new AudioSource[audioSourcePoolSize];
        for (int i = 0; i < audioSourcePoolSize; i++)
        {
            GameObject audioSourceObject = new GameObject(name+" AudioSource_" + i);
            audioSourcePool[i] = audioSourceObject.AddComponent<AudioSource>();
            audioSourcePool[i].playOnAwake = false;
            audioSourcePool[i].spatialBlend = 1.0f;
            audioSourcePool[i].minDistance = minDistance;
            audioSourcePool[i].maxDistance = maxDistance;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Calculate the collision force magnitude
        float collisionForce = collision.impulse.magnitude;

        if (collision.gameObject.CompareTag("Ground"))
        {
            PlayTerrainSound(collision.gameObject.GetComponent<Terrain>(), collision.contacts[0].point, collisionForce);
        } else if (collision.gameObject.CompareTag("Rock"))
        {
            PlaySoundAtPoint(rockCollisionSound, collision.transform.position, collisionForce);
        } else if (collision.gameObject.CompareTag(gameObject.tag))
        {
            PlaySoundAtPoint(similarObjectCollisionSound, collision.transform.position, collisionForce);
        } else {
            PlaySoundAtPoint(similarObjectCollisionSound, collision.transform.position, collisionForce);
        }
    }

    // Method to play sound at a specific point with volume based on collision force
    private void PlaySoundAtPoint(AudioClip clip, Vector3 point, float force)
    {
        // Get the next available AudioSource from the pool
        AudioSource audioSource = audioSourcePool[currentAudioSourceIndex];
        currentAudioSourceIndex = (currentAudioSourceIndex + 1) % audioSourcePoolSize;

        // Move the AudioSource to the contact point
        audioSource.transform.position = point;

        // Set the clip and volume based on collision force
        audioSource.clip = clip;
        audioSource.volume = force / forceDivisor;
        audioSource.maxDistance = Mathf.Min(maxDistance, force / distanceDivisor);

        // Play the sound
        audioSource.Play();
    }

    // Method to play sound based on terrain texture at collision point
    private void PlayTerrainSound(Terrain terrain, Vector3 collisionPoint, float force)
    {
        // Get the terrain data
        TerrainData terrainData = terrain.terrainData;

        // Convert the collision point to terrain coordinates
        Vector3 terrainLocalPos = collisionPoint - terrain.transform.position;
        Vector3 normalizedPos = new Vector3(
            terrainLocalPos.x / terrainData.size.x,
            0,
            terrainLocalPos.z / terrainData.size.z
        );

        // Get the splat map index at the collision point
        int textureIndex = GetTextureIndexAtPosition(terrain, normalizedPos);

        // Play the corresponding sound
        if (textureIndex >= 0 && textureIndex < terrainCollisionSounds.Length)
        {
            PlaySoundAtPoint(terrainCollisionSounds[textureIndex], collisionPoint, force);
        }
    }

    // Method to get the splat map index at a specific position
    private int GetTextureIndexAtPosition(Terrain terrain, Vector3 normalizedPos)
    {
        // Get the splat map data
        TerrainData terrainData = terrain.terrainData;
        int mapX = Mathf.RoundToInt(normalizedPos.x * terrainData.alphamapWidth);
        int mapZ = Mathf.RoundToInt(normalizedPos.z * terrainData.alphamapHeight);
        float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        // Find the dominant texture at the position
        int dominantTextureIndex = 0;
        float maxOpacity = 0f;
        for (int i = 0; i < terrainData.alphamapLayers; i++)
        {
            if (splatmapData[0, 0, i] > maxOpacity)
            {
                maxOpacity = splatmapData[0, 0, i];
                dominantTextureIndex = i;
            }
        }

        return dominantTextureIndex;
    }
}
