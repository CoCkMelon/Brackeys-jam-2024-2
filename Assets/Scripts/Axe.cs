using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script should be attached to the "Axe" GameObject.
/// It handles the UseItem action, which toggles attachments of the spring's bones to nearby attachable objects.
/// </summary>
[RequireComponent(typeof(SpecialUse))]
[RequireComponent(typeof(Rigidbody))]
public class Axe : MonoBehaviour
{
    [SerializeField] private SpecialUse specialUse;
    [SerializeField] private GameObject axeObjectCollider; //Used for the axe's collider object!

    private Rigidbody rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        axeObjectCollider.tag = "Untagged";
    }

    private void OnEnable()
    {
        if (specialUse != null)
        {
            specialUse.UseItemAction += UseItem;
        }

        else
        {
            Debug.LogError($"SpecialUse component is missing on {gameObject.name}.");
        }
    }

    private void OnDisable()
    {
        if (specialUse != null)
        {
            specialUse.UseItemAction -= UseItem;
        }
    }

    private void UseItem()
    {
        StartCoroutine(DisableObjectWithDelay());

        IEnumerator DisableObjectWithDelay()
        {
            axeObjectCollider.tag = "Axe";
            //Axe Collider is enabled and animation plays
            axeObjectCollider.SetActive(true);
            anim.SetTrigger("Whack");

            yield return new WaitForSeconds(1);

            //Axe Collider is disabled
            axeObjectCollider.SetActive(false);
            axeObjectCollider.tag = "Untagged";
        }
    }
}
