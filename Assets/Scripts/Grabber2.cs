using UnityEngine;

public class Grabber2 : MonoBehaviour
{
    [SerializeField] private Transform grabPoint; // Point where the object will be attached
    [SerializeField] private float rotationSpeed = 100f; // Speed of rotation
    [SerializeField] private float moveSpeed = 1f; // Speed of grabPoint movement
    [SerializeField] float grabDistance = 5f;

    private GameObject grabbedObject;

    void Update()
    {
        HandleGrabRelease();
        HandleUseItem();
        HandleMouseWheel();
    }

    /// <summary>
    /// Handles grabbing and releasing objects with mouse button.
    /// </summary>
    private void HandleGrabRelease()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GrabObject();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ReleaseObject();
        }
    }

    /// <summary>
    /// Handles the use item action when the use button is pressed.
    /// </summary>
    private void HandleUseItem()
    {
        // Define the use item key, e.g., 'E'
        if (Input.GetKeyDown(KeyCode.E) && grabbedObject != null)
        {
            SpecialUse specialUse = grabbedObject.GetComponent<SpecialUse>();
            if (specialUse != null)
            {
                specialUse.UseItem();
            }
        }
    }

    /// <summary>
    /// Handles mouse wheel input for rotation or movement.
    /// </summary>
    private void HandleMouseWheel()
    {
        if (grabbedObject != null)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                // Check for rotation keys
                if (Input.GetKey(KeyCode.X))
                {
                    RotateGrabbedObject(Vector3.right, scroll);
                }
                else if (Input.GetKey(KeyCode.Y))
                {
                    RotateGrabbedObject(Vector3.up, scroll);
                }
                else if (Input.GetKey(KeyCode.Z))
                {
                    RotateGrabbedObject(Vector3.forward, scroll);
                }
                else
                {
                    // Move grabPoint along its local Z-axis
                    MoveGrabPoint(scroll);
                }
            }
        }
        else
        {
            // If no object is grabbed, allow moving grabPoint
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                MoveGrabPoint(scroll);
            }
        }
    }

    /// <summary>
    /// Rotates the grabbed object around the specified axis based on mouse wheel input.
    /// </summary>
    /// <param name="axis">The axis to rotate around.</param>
    /// <param name="scroll">The mouse wheel input.</param>
    private void RotateGrabbedObject(Vector3 axis, float scroll)
    {
        // Calculate rotation based on scroll and rotation speed
        float rotationAmount = scroll * rotationSpeed * Time.deltaTime;
        grabbedObject.transform.Rotate(axis, rotationAmount, Space.World);
    }

    /// <summary>
    /// Moves the grabPoint along its local Z-axis based on mouse wheel input.
    /// </summary>
    /// <param name="scroll">The mouse wheel input.</param>
    private void MoveGrabPoint(float scroll)
    {
        // Calculate movement based on scroll and move speed
        Vector3 movement = grabPoint.forward * scroll * moveSpeed;
        grabPoint.position += movement;
    }

    /// <summary>
    /// Attempts to grab an object in front of the grabPoint using a raycast.
    /// </summary>
    private void GrabObject()
    {
        RaycastHit hit;
        // Perform a raycast from the grabPoint forward
        if (Physics.Raycast(grabPoint.position, grabPoint.forward, out hit, grabDistance))
        {
            Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
            if (rb != null && !rb.isKinematic)
            {
                grabbedObject = hit.transform.gameObject;
                grabbedObject.transform.SetParent(grabPoint);
                rb.isKinematic = true;
            }
        }
    }

    /// <summary>
    /// Releases the currently grabbed object, restoring its physics properties.
    /// </summary>
    private void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                // Optionally, apply the grabPoint's velocity if it has a Rigidbody
                Rigidbody grabRb = grabPoint.GetComponent<Rigidbody>();
                if (grabRb != null)
                {
                    rb.velocity = grabRb.velocity;
                }
            }
            grabbedObject.transform.SetParent(null);
            grabbedObject = null;
        }
    }
}
