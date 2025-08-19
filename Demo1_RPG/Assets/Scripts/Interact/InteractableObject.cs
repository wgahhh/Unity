using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    private NavMeshAgent playerAgent;
    private bool haveInteracted = false;
    public void OnClick(NavMeshAgent playerAgent)
    {
        haveInteracted = false;
        this.playerAgent = playerAgent; 
        playerAgent.stoppingDistance = 2;
        playerAgent.SetDestination(transform.position);
    }

    private void Update()
    {
        if (playerAgent != null && !haveInteracted && playerAgent.pathPending == false)
        {
            if (playerAgent.remainingDistance <= 2)
            {
                Interact();
                haveInteracted = true;
            }
        }
    }
    protected virtual void Interact()
    {
        print("Interacting with Interactable Object.");
    }
}
