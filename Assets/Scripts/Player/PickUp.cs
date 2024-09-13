using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private const float throwForce = 600f;
    private const float maxDistance = 20f;
    private const float dropDistance = 2f;

    private Vector3 objectPos;
    private float distance;

    private GameObject item;
    private Transform tempParent;
    private bool isHolding = false;

    private void Start()
    {
        // Initialize tempParent as the transform of this GameObject
        tempParent = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (item != null)
        {
            distance = Vector3.Distance(item.transform.position, tempParent.position);
            if (distance >= dropDistance)
            {
                isHolding = false;
            }

            // Check if isholding
            if (isHolding)
            {
                item.GetComponent<Rigidbody>().velocity = Vector3.zero;
                item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                item.transform.SetParent(tempParent);

                if (Input.GetMouseButtonDown(1))
                {
                    item.GetComponent<Rigidbody>().AddForce(tempParent.forward * throwForce);
                    isHolding = false;
                }
            }
            else
            {
                objectPos = item.transform.position;
                item.transform.SetParent(null);
                item.GetComponent<Rigidbody>().useGravity = true;
                item.transform.position = objectPos;
            }
        }
    }

    void OnMouseDown()
    {
        PickObject();
        if (item != null && distance <= maxDistance)
        {
            isHolding = true;
            item.GetComponent<Rigidbody>().useGravity = false;
            item.GetComponent<Rigidbody>().detectCollisions = true;
        }
    }

    void OnMouseUp()
    {
        isHolding = false;
    }

    void PickObject()
    {
        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Check if the ray intersects with a collider
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Get the rigidbody of the hit object
            Rigidbody rb = hit.transform.GetComponent<Rigidbody>();

            // If the hit object has a rigidbody, create a fixed joint
            if (rb != null)
            {
                item = hit.transform.gameObject;
            }
        }
    }
}
