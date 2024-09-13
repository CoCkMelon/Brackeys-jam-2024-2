using UnityEngine;

public class Grabber : MonoBehaviour
{
    [SerializeField] private Transform grabPoint; // Point where the object will be attached

    private GameObject grabbedObject;

    void Update()
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

    private void GrabObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(grabPoint.position, grabPoint.forward, out hit))
        {
            Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                grabbedObject = hit.transform.gameObject;
                grabbedObject.transform.SetParent(grabPoint);
                rb.isKinematic = true;
            }
        }
    }

    private void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.velocity = grabPoint.GetComponent<Rigidbody>().velocity;
            }
            grabbedObject.transform.SetParent(null);
            grabbedObject = null;
        }
    }
}
