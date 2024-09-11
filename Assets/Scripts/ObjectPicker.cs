using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    float throwForce = 600;
    public GameObject handle; // Assign in the inspector
    private GameObject pickedObject;
    private HingeJoint joint;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            DropObject();
            pickedObject.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce);
        }
        // Check if the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            PickObject();
        }
        // Check if the left mouse button is released
        else if (Input.GetMouseButtonUp(0))
        {
            DropObject();
        }
    }

    void PickObject()
    {
        // Create a ray from the camera through the mouse position
        Ray ray = new Ray(transform.position, transform.forward);

        // Check if the ray intersects with a collider
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Get the rigidbody of the hit object
            Rigidbody rb = hit.transform.GetComponent<Rigidbody>();

            // If the hit object has a rigidbody, create a fixed joint
            if (rb != null)
            {
                if(handle.GetComponent<HingeJoint>() == null) {
                    joint = handle.AddComponent<HingeJoint>();
                    // Increase the Spring and Damper values
                    JointSpring spring = joint.spring;
                    spring.targetPosition = 0;
                    spring.spring = 1000;
                    spring.damper = 10;

                    // Increase the Break Force and Break Torque values
                    joint.breakForce = 10000;
                    joint.breakTorque = 10000;
                } else {
                    joint = handle.GetComponent<HingeJoint>();
                }
                pickedObject = hit.transform.gameObject;
                joint.connectedBody = pickedObject.GetComponent<Rigidbody>();

                // Move the handle to the point of intersection
                handle.transform.localPosition = new Vector3(handle.transform.localPosition.x, handle.transform.localPosition.y, hit.distance);
            }
        }
    }

    void DropObject()
    {
        // If an object is picked, destroy the joint and reset the picked object
        if (pickedObject != null)
        {
            if(handle.GetComponent<HingeJoint>() != null) {
                handle.GetComponent<HingeJoint>().connectedBody = null;
                pickedObject = null;
            }
        }
    }

}
