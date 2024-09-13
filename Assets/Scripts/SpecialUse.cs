using UnityEngine;
using System;

public class SpecialUse : MonoBehaviour
{
    /// <summary>
    /// Delegate for the UseItem action.
    /// </summary>
    public Action UseItemAction;

    /// <summary>
    /// This method is called by the Grabber to execute the special use behavior.
    /// </summary>
    public void UseItem()
    {
        if (UseItemAction != null)
        {
            UseItemAction.Invoke();
        }
        else
        {
            Debug.LogWarning($"No UseItem action assigned to {gameObject.name}.");
        }
    }
}
