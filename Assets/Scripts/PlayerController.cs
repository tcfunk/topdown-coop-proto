using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    public Interactable focus;
    public LayerMask movementMask;
    
    private PlayerMotor motor;
    private Camera cam;
    
    private PlayerControls playerControls;
    private PlayerControls.PlayerActions controls;

    private string inputType;
    private Vector2 inputDirection;
    private Vector2 mousePosition;

    private void Awake()
    {
        playerControls = new PlayerControls();
        controls = playerControls.Player;

        controls.Move.performed += ctx =>
        {
            inputType = ctx.control.device.name;

            if (inputType == "Mouse")
            {
                MouseMove(controls.Look.ReadValue<Vector2>());
            }
            else
            {
                inputDirection = ctx.ReadValue<Vector2>();
            }
        };
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDestroy()
    {
        controls.Disable();
    }

    void Start()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
    }

    void MouseMove(Vector2 mousePosition)
    {
        Ray ray = cam.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out var hit, 100, movementMask))
        {
            motor.MoveToPoint(hit.point);
        }
        RemoveFocus();
    }

    void GamepadMove(Vector2 direction)
    {
        var moveDir = new Vector3(
            (cam.transform.right * direction.x).x,
            0f,
            (cam.transform.forward * direction.y).z
        );

        motor.MoveInDirection(moveDir);
        RemoveFocus();
    }


    void Update()
    {

        if (inputDirection != Vector2.zero)
        {
            GamepadMove(inputDirection);
        }
    }

    void SetFocus(Interactable target)
    {
        if (focus != target)
        {
            focus?.OnDefocused();
            focus = target;
            motor.FollowTarget(focus);
        }
        
        target.OnFocused(transform);
    }

    void RemoveFocus()
    {
        focus?.OnDefocused();
        focus = null;
        motor.StopFollowingTarget();
    }
}
