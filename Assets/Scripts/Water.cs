using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    PlayerMovement movement;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other.GetComponent<PlayerMovement>() != null)
        {
            movement = other.GetComponent<PlayerMovement>();
            movement.isSwimming = true;
        }
        if (other.CompareTag("EyeLevel"))
        {
            movement.gravity = 0;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerMovement>() != null)
        {
            PlayerMovement movement = other.GetComponent<PlayerMovement>();
            movement.isSwimming = false;
        }
        
    }
}
