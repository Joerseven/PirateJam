using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private PlayerControls playerControls;

    [SerializeField] private float movementSpeed = 10.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        playerControls.Player.Move.performed += Move_performed;
    }

    private void Move_performed(InputAction.CallbackContext context)
    {
        Vector2 inputValue = context.ReadValue<Vector2>();
        rb.AddForce(inputValue * movementSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 inputValue = playerControls.Player.Move.ReadValue<Vector2>();
        rb.AddForce(inputValue * movementSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
}
