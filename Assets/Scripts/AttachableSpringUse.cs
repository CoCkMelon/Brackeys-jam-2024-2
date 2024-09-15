using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This script should be attached to the "Attachable Spring" GameObject.
/// It handles the UseItem action, which toggles attachments of the spring's bones to nearby attachable objects.
/// </summary>
[RequireComponent(typeof(SpecialUse))]
[RequireComponent(typeof(Rigidbody))]
public class AttachableSpringUse : MonoBehaviour
{
    [Header("Spring Configuration")]
    [Tooltip("Transform representing the first attachment point (bone).")]
    [SerializeField] private Transform bone1;

    [Tooltip("Transform representing the second attachment point (bone).")]
    [SerializeField] private Transform bone2;

    [Tooltip("Radius to detect attachable objects around each bone.")]
    [SerializeField] private float detectionRadius = 2f;

    [Tooltip("LayerMask specifying which layers are considered attachable.")]
    [SerializeField] private LayerMask attachableLayerMask;

    /// <summary>
    /// Dictionary to keep track of active joints for each bone.
    /// </summary>
    private Dictionary<Transform, FixedJoint> activeJoints = new Dictionary<Transform, FixedJoint>();

    /// <summary>
    /// Reference to the SpecialUse component.
    /// </summary>
    private SpecialUse specialUse;

    void Awake()
    {
        // Get the SpecialUse component
        specialUse = GetComponent<SpecialUse>();
        if (specialUse != null)
        {
            // Assign the UseItem method to the SpecialUse's UseItemAction delegate
            specialUse.UseItemAction += UseItem;
        }
        else
        {
            Debug.LogError($"SpecialUse component is missing on {gameObject.name}.");
        }

        // Ensure bones are assigned
        if (bone1 == null || bone2 == null)
        {
            Debug.LogError("Both bone1 and bone2 must be assigned in the Inspector.");
        }

        // Ensure the GameObject has a Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("AttachableSpringUse requires a Rigidbody component.");
        }
        else
        {
            // Optional: Configure Rigidbody settings
            // rb.isKinematic = false;
            // rb.useGravity = true;
        }
    }

    /// <summary>
    /// This method is called when the UseItem action is triggered.
    /// It toggles the attachments of the spring's bones to nearby attachable objects.
    /// </summary>
    public void UseItem()
    {
        ToggleAttachment(bone1, bone2);
    }

    /// <summary>
    /// Toggles the attachment state of the specified bones.
    /// If the bone is attached, it detaches it. Otherwise, it attempts to attach it.
    /// </summary>
    /// <param name="bone">The Transform representing the attachment bone.</param>
    private void ToggleAttachment(Transform bone1, Transform bone2)
    {
        if (bone1 == null)
        {
            Debug.LogWarning("Bone1 is not assigned.");
            return;
        }
        if (bone2 == null)
        {
            Debug.LogWarning("Bone2 is not assigned.");
            return;
        }

        // Check if the bone is already attached
        if (activeJoints.ContainsKey(bone1) || activeJoints.ContainsKey(bone2))
        {
            // Detach the bone
            DetachBone(bone1);
            DetachBone(bone2);
        }
        else
        {
            // Attempt to attach the bone
            AttachToBone(bone1);
            AttachToBone(bone2);
        }
    }

    /// <summary>
    /// Attempts to find and attach to the nearest attachable object around a specified bone.
    /// </summary>
    /// <param name="bone">The Transform representing the attachment bone.</param>
    private void AttachToBone(Transform bone)
    {
        // Use OverlapSphere to find colliders within the detection radius
        Collider[] colliders = Physics.OverlapSphere(bone.position, detectionRadius, attachableLayerMask);

        if (colliders.Length == 0)
        {
            Debug.Log($"No attachable objects found near {bone.name}.");
            return;
        }

        // Find the nearest collider with a Rigidbody
        Collider nearestCollider = null;
        float minDistance = Mathf.Infinity;
        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null && !rb.isKinematic)
            {
                float distance = Vector3.Distance(bone.position, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestCollider = col;
                }
            }
        }

        if (nearestCollider != null)
        {
            Rigidbody targetRigidbody = nearestCollider.GetComponent<Rigidbody>();
            if (targetRigidbody != null)
            {
                // Create a FixedJoint
                FixedJoint fixedJoint = bone.gameObject.AddComponent<FixedJoint>();
                fixedJoint.connectedBody = targetRigidbody;
                fixedJoint.autoConfigureConnectedAnchor = false;
                fixedJoint.anchor = transform.InverseTransformPoint(bone.position);
                fixedJoint.connectedAnchor = nearestCollider.transform.InverseTransformPoint(bone.position);

                // Optionally, configure joint properties
                fixedJoint.breakForce = Mathf.Infinity;
                fixedJoint.breakTorque = Mathf.Infinity;

                // Store the joint to manage the connection
                activeJoints.Add(bone, fixedJoint);

                Debug.Log($"Attached {gameObject.name} to {nearestCollider.gameObject.name} at {bone.name}.");
            }
            else
            {
                Debug.LogWarning($"Attached object {nearestCollider.gameObject.name} does not have a Rigidbody.");
            }
        }
        else
        {
            Debug.Log($"No suitable Rigidbody found near {bone.name}.");
        }
    }

    /// <summary>
    /// Detaches a specified bone by removing its FixedJoint.
    /// </summary>
    /// <param name="bone">The Transform representing the attachment bone.</param>
    private void DetachBone(Transform bone)
    {
        if (activeJoints.ContainsKey(bone))
        {
            FixedJoint joint = activeJoints[bone];
            if (joint != null)
            {
                Destroy(joint);
                Debug.Log($"Detached {gameObject.name} from {joint.connectedBody.gameObject.name} at {bone.name}.");
            }
            activeJoints.Remove(bone);
        }
        else
        {
            Debug.LogWarning($"No active joint found for {bone.name} to detach.");
        }
    }

    /// <summary>
    /// Cleans up all joints when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        // Unsubscribe the UseItem method
        if (specialUse != null)
        {
            specialUse.UseItemAction -= UseItem;
        }

        // Destroy all active joints
        foreach (var joint in activeJoints.Values)
        {
            if (joint != null)
            {
                Destroy(joint);
            }
        }
        activeJoints.Clear();
    }

    /// <summary>
    /// Visualize detection radii in the Editor.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (bone1 != null)
        {
            Gizmos.DrawWireSphere(bone1.position, detectionRadius);
        }
        if (bone2 != null)
        {
            Gizmos.DrawWireSphere(bone2.position, detectionRadius);
        }
    }
}
