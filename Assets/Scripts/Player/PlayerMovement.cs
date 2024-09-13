using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public PlayerStats playerStats;

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

    private Coroutine recharge;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isSwimming)
        {
            // Calculate movement direction
            Vector3 direction = target.forward * Input.GetAxisRaw("Vertical") + target.right * Input.GetAxisRaw("Horizontal");

            // Apply movement speed based on sprinting and stamina
            if (Input.GetKey(KeyCode.LeftShift) && playerStats.currentStamina > 0) {
                playerStats.LoseStamina(10);
            }
            float movementSpeed = (Input.GetKey(KeyCode.LeftShift) && playerStats.currentStamina > 0) ? sprintSpeed + swimSpeed : swimSpeed;
            if(playerStats.currentStamina<=0) {
                direction = (direction + Vector3.down).normalized;
            }

            // Move the character using the CharacterController
            controller.Move(direction * movementSpeed * Time.deltaTime);
            playerStats.LoseStamina(10);

            // Update stamina and breath
            if (playerStats.currentBreath > 0)
            {
                playerStats.RunOutOfBreath(5);
            }

            if (playerStats.currentBreath <= 0 && playerStats.currentHealth > 0)
            {
                playerStats.TakeDamage(10 * Time.deltaTime);
            }
        }
        else
        {
            // Update movement on land
            playerStats.FreshAir(20);

            if (playerStats.currentHealth < playerStats.maxHealth)
            {
                StartCoroutine(RecoverHealth());
            }

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            if (Input.GetKey(KeyCode.LeftShift) && playerStats.currentStamina > 0)
            {
                controller.Move(move * (speed + sprintSpeed) * Time.deltaTime);
                if (move.x != 0 && move.z != 0 && isGrounded)
                {
                    playerStats.LoseStamina(10);
                }
            }
            else
            {
                controller.Move(move * speed * Time.deltaTime);
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    StartCoroutine(RechargeStamina());
                }
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }
    }

    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);

        while (playerStats.currentStamina < playerStats.maxStamina)
        {
            playerStats.RestoreStamina(100);
            yield return new WaitForSeconds(.1f);
        }
    }

    private IEnumerator RecoverHealth()
    {
        yield return new WaitForSeconds(2f);

        while (playerStats.currentHealth < playerStats.maxHealth)
        {
            playerStats.RegainHealth(5 * Time.deltaTime);
            yield return new WaitForSeconds(.1f);
        }
    }
}
