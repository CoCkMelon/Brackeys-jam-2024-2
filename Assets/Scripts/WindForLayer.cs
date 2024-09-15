using UnityEngine;

public class WindForLayer : MonoBehaviour
{
    public float baseWindForce = 10f; // Base strength of the wind
    public Vector3 boxSize;
    public LayerMask layerMask; // The layer(s) that will be affected by wind

    public float pulseMagnitude = 5f; // Magnitude of the pulse
    public float pulseDuration = 1f; // Duration of the pulse (in seconds)
    public float phaseShift = 0f; // Phase shift for the pulse (in radians)
    public float playerForceMul = 0.1f;

    private float pulseTimer = 0f; // Timer to keep track of pulse duration

    [SerializeField] float waterLevel = 0f;
    public float currentPulseForce;
    void FixedUpdate()
    {
        pulseTimer += Time.deltaTime; // Increment pulse timer

        // Calculate the current pulse force based on a rounded square wave with phase shift and pulse duration
        float pulseProgress = (pulseTimer % pulseDuration) / pulseDuration; // Normalize pulse timer within the pulse duration
        currentPulseForce = baseWindForce + (pulseMagnitude * WaveFunctions.SquareWave(pulseProgress * 2f * Mathf.PI + phaseShift));

        // Get all rigidbodies and character controllers in the scene that match the specified layer mask
        Collider[] colliders = Physics.OverlapBox(transform.position, boxSize, Quaternion.identity, layerMask);

        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            CharacterController cc = collider.GetComponent<CharacterController>();

            if (rb!= null)
            {
                // Calculate the wind direction vector and normalize it
                Vector3 windDirection = transform.forward.normalized;

                // Get the bounding box extents
                Vector3 extents = collider.bounds.extents;

                // Calculate the areas of the bounding box faces perpendicular to the wind direction
                float areaX = extents.y * extents.z;
                float areaY = extents.x * extents.z;
                float areaZ = extents.x * extents.y;

                // Find the largest area
                float largestArea = Mathf.Max(areaX, areaY, areaZ);

                // Apply wind force scaled by the largest area
                rb.AddForce(windDirection * currentPulseForce * largestArea, ForceMode.Force);
            }
            else if (cc!= null)
            {
                if(collider.transform.position.y > waterLevel+1) {
                    // Calculate the wind direction vector and normalize it
                    Vector3 windDirection = transform.forward.normalized;

                    // Apply wind force directly to the character controller's velocity
                    cc.Move(windDirection * currentPulseForce * Time.deltaTime * playerForceMul);
                } else {
                    // Calculate the wind direction vector and normalize it
                    Vector3 windDirection = transform.forward.normalized;

                    // Apply wind force directly to the character controller's velocity
                    cc.Move(windDirection * currentPulseForce * Time.deltaTime * 0.9f * playerForceMul);
                }
            }
        }
    }
}
