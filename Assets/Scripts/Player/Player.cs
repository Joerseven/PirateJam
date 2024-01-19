using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SearchService;

public class Player : MonoBehaviour
{

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    
    [SerializeField] private float movementSpeed = 10.0f;
    [SerializeField] private float dodgeSpeed = 10.0f;
    
    private PlayerControls playerControls;
    private InputAction swipePosition; 
    private Vector2 inputValue;

    private bool swordSwinging;
    
    private Rigidbody2D rb;

    private Hurtbox hurtbox;
    private Vector2 swipeStart;

    private Vector2 swingDirection;
    
    //Temp for testing
    bool playerMovingUp;
    bool playerMovingRight;
    bool playerMovingDown;
    bool playerMovingLeft;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hurtbox = GetComponentInChildren<Hurtbox>(true);
        hurtbox.gameObject.SetActive(false);

        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        
        
    }

    private void Start()
    {
        hurtbox.OnHit.AddListener(SwordHit);
        
        var fireButton = playerControls.Player.Fire;
        swipePosition = playerControls.Player.Position;
        // This is working off the resolution rather than a normalised coordinate :/
        fireButton.performed += _ => { swipeStart = swipePosition.ReadValue<Vector2>(); };
        fireButton.canceled += _ => { FinishSwipe(); };

        playerControls.Player.Dodge.performed += Dodge;

    }
    
    private void Update()
    {
        inputValue = playerControls.Player.Move.ReadValue<Vector2>();

        
    }

    private void FinishSwipe()
    {
        Vector2 delta = swipePosition.ReadValue<Vector2>() - swipeStart;
        if (delta.magnitude < 100) return;
        //It's getting late don't @ me for this line of code you're welcome to not do it in a brainfuck way.
        if (delta.x * delta.x > delta.y * delta.y)
        {
            Attack(delta.x > 0 ? new Vector2(1, 0) : new Vector2(-1, 0));
        }
        else
        {
            Attack(delta.y > 0 ? new Vector2(0, 1): new Vector2(0, -1));
        }

    }

    private void Attack(Vector2 direction)
    {
        if (swordSwinging) return;
        swingDirection = direction;
        StartCoroutine(Swing());
    }

    IEnumerator Swing()
    {
        swordSwinging = true;
        hurtbox.gameObject.SetActive(true);
        
        //Moves the players hurtbox to up, down, left and right of the player's rigid body - Needs to stay relative to the direction the player is facing.
        
        yield return new WaitForSeconds(0.2f);
        hurtbox.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        swordSwinging = false;
    }

    private void SwordHit(Collider2D collidedWith)
    {
        if (collidedWith.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.ReceiveSlash(swingDirection);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(inputValue * (movementSpeed * Time.fixedDeltaTime), ForceMode2D.Impulse);
        if (inputValue != new Vector2(0, 0))
        {
            hurtbox.gameObject.transform.SetPositionAndRotation(rb.position + new Vector2(inputValue.x * .5f, inputValue.y * .5f), Quaternion.identity);
            
        }

    }

    private void Dodge(InputAction.CallbackContext context)
    {
        rb.AddForce(inputValue * (dodgeSpeed * Time.fixedDeltaTime), ForceMode2D.Impulse);
    }
}




    /* Save this function for later - This is the basis for moving the weapon hitbox relative to the mouse position. OT
     * 
    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
    }
     */
