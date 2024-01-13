using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput input;
    private Rigidbody2D rb;
    [SerializeField] private float movementSpeed = 1.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        //PlayerInputActions.inputActions = new PlayerInputActions();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        { 
            Debug.Log(context);
            Vector2 inputVector = context.ReadValue<Vector2>();
            rb.AddForce(inputVector * movementSpeed, ForceMode2D.Impulse);
        }
    }
}
