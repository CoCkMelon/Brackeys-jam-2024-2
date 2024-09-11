using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float sprintSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float swimSpeed = 10f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public Transform target;

    Vector3 velocity;
    bool isGrounded;
    [HideInInspector]
    public bool isSwimming;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isSwimming)
        {
            if(gravity != 0)
            {
                gravity = 0;
            }
            if(Input.GetAxisRaw("Vertical") > 0)
            {
                transform.position += target.forward * swimSpeed * Time.deltaTime;
            }
            if(Input.GetAxisRaw("Vertical") < 0)
            {
                transform.position -= target.forward * swimSpeed * Time.deltaTime;
            }
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                transform.position += target.right * swimSpeed * Time.deltaTime;
            }
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                transform.position -= target.right * swimSpeed * Time.deltaTime;
            }
        }
        else
        {
            if(gravity == 0)
            {
                gravity = -9.81f;
            }
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;


            if (Input.GetKey(KeyCode.LeftShift))
            {
                controller.Move(move * (speed + sprintSpeed) * Time.deltaTime);
            }
            else
            {
                controller.Move(move * speed * Time.deltaTime);
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }
    }
}
