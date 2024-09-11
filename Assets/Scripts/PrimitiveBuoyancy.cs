using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimitiveBuoyancy : MonoBehaviour
{
    // Density of the surrounding fluid (e.g., water)
    public float fluidDensity = 1000f; // kg/m^3

    // Density of the object
    public float objectDensity = 500f; // kg/m^3

    // Viscosity of the fluid (e.g., water)
    public float fluidViscosity = 0.001f; // kg/m*s

    // The water level (Y position)
    public float waterLevel = 0f;
    [SerializeField] float force_multiplier; // NOTE: crutch

    // The volume of the object
    private float objectVolume;

    // The rigidbody of the object
    private Rigidbody rb;

    private void Start()
    {
        // Get the rigidbody of the object
        rb = GetComponent<Rigidbody>();

        // Calculate the volume of the object
        objectVolume = CalculateVolume();

        // Calculate the object density
        objectDensity = rb.mass / objectVolume;
    }

    private void FixedUpdate()
    {
        // Check if the object is partially or fully submerged
        float submergedVolume = CalculateSubmergedVolume();

        // Calculate the buoyancy force
        float buoyancyForce = CalculateBuoyancyForce(submergedVolume);

        // Calculate the drag force
        float dragForce = CalculateDragForce(submergedVolume);

        // Apply the buoyancy and drag forces to the object
        ApplyBuoyancyAndDragForces(buoyancyForce, dragForce);
    }

    // Calculate the volume of the object
    private float CalculateVolume()
    {
        // Get the bounds of the object
        Bounds bounds = GetComponent<Renderer>().bounds;

        // Calculate the volume of the object
        return bounds.size.x * bounds.size.y * bounds.size.z;
    }

    // Calculate the buoyancy force
    private float CalculateBuoyancyForce(float submergedVolume)
    {
        // Calculate the weight of the fluid displaced by the object
        float fluidWeight = fluidDensity * submergedVolume * Physics.gravity.magnitude;

        // Calculate the buoyancy force
        return fluidWeight;
    }

    // Calculate the drag force
    private float CalculateDragForce(float submergedVolume)
    {
        // Calculate the velocity of the object
        float velocity = rb.velocity.magnitude;

        // Calculate the drag force
        return fluidViscosity * submergedVolume * velocity * velocity; // Square the velocity
    }

    // Calculate the volume of the object that is submerged
    private float CalculateSubmergedVolume()
    {
        // Get the bounds of the object
        Bounds bounds = GetComponent<Renderer>().bounds;

        // Calculate the height of the object above and below the water level
        float aboveWaterHeight = Mathf.Max(0f, (transform.position.y + bounds.size.y / 2) - waterLevel);
        float belowWaterHeight = Mathf.Max(0f, waterLevel - (transform.position.y - bounds.size.y / 2));

        // Calculate the volume of the object that is submerged
        return bounds.size.x * bounds.size.y * belowWaterHeight; // Use bounds.size.y for width
    }

    // Apply the buoyancy and drag forces to the object
    private void ApplyBuoyancyAndDragForces(float buoyancyForce, float dragForce)
    {
        // Apply the buoyancy force to the object
        rb.AddForce(Vector3.up * buoyancyForce*force_multiplier, ForceMode.Acceleration);

        // Apply the drag force to the object
        rb.AddForce(-rb.velocity.normalized * dragForce, ForceMode.VelocityChange);
    }

    // Draw a line to represent the water level
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(-100f, waterLevel, 0f), new Vector3(100f, waterLevel, 0f));
    }
}
