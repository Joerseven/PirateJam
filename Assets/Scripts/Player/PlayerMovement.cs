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

    private Vector2 inputValue;

    [SerializeField] private float movementSpeed = 10.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }
    
    private void Update()
    {
        inputValue = playerControls.Player.Move.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(inputValue * (movementSpeed * Time.fixedDeltaTime), ForceMode2D.Impulse);
    }
}
