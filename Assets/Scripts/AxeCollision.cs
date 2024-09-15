using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tree"))
        {
            Destroy(other.gameObject);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Tree"))
        {
            Destroy(other.gameObject);
        }
    }
}
