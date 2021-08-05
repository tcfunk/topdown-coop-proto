using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public Transform interactionTransform;

    bool hasInteracted = false;
    bool isFocus = false;
    protected Transform player;

    private void Update()
    {
        if (isFocus && !hasInteracted)
        {
            var distance = Vector3.Distance(player.position, transform.position);
            if (distance <= radius)
            {
                Interact();
            }
        }
    }

    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
    }

    public void OnDefocused()
    {
        isFocus = false;
        player = null;
    }

    protected virtual void Interact()
    {
        hasInteracted = true;
    }

    private void OnDrawGizmosSelected()
    {
        var baseTransform = interactionTransform == null ? transform : interactionTransform;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(baseTransform.position, radius);
    }
}
