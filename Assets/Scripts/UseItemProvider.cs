using UnityEngine;

[RequireComponent(typeof(SpecialUse))]
public class UseItemProvider : MonoBehaviour
{
    private SpecialUse specialUse;

    void Awake()
    {
        // Get the SpecialUse component attached to the same GameObject
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
    }

    /// <summary>
    /// This method defines the specific use-item behavior for the GameObject.
    /// Customize this method to perform different actions based on your needs.
    /// </summary>
    public void UseItem()
    {
        Debug.Log($"{gameObject.name} UseItem method executed by UseItemProvider.");
        // Add your custom use-item logic here.
    }

    void OnDestroy()
    {
        if (specialUse != null)
        {
            // Unsubscribe from the UseItemAction to prevent memory leaks
            specialUse.UseItemAction -= UseItem;
        }
    }
}
