using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class NetworkPlayer : NetworkBehaviour
{
    private Vector2 inputDir;
    private NavMeshAgent agent;
    private Camera cam;

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        inputDir = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
        var moveDir = new Vector3(
            (cam.transform.right * inputDir.x).x,
            0f,
            (cam.transform.forward * inputDir.y).z
        );
        
        if (NavMesh.SamplePosition(transform.position + moveDir, out var navMeshHit, 100f, NavMesh.AllAreas))
        {
            agent.SetDestination(navMeshHit.position);
        }
    }
}
