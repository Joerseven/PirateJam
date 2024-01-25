using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SearchService;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public UnityEvent PlayerDeathEvent;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    
    [SerializeField] private float movementSpeed = 10.0f;
    [SerializeField] private float dodgeSpeed = 10.0f;
    [SerializeField] private bool invuln = false;
    
    private Rigidbody2D rb;
    private Knockback knockback;
    
    
    private PlayerControls playerControls;
    private Vector2 inputValue;

    private bool swordSwinging;
    private InputAction swipePosition; 
    private Vector2 swipeStart;
    private Vector2 swingDirection;
    public Vector2 Facing;
    private Hurtbox hurtbox;
    private Animator animator;

    private LevelManager levelManager;
    private Grid grid;
    
    
    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        hurtbox = GetComponentInChildren<Hurtbox>(true);
        hurtbox.gameObject.SetActive(false);
        animator = GetComponentInChildren<Animator>();
        levelManager = GetComponentInParent<LevelManager>();
        grid = GetComponentInParent<Grid>();
        
        playerControls = new PlayerControls();
        playerControls.Player.Enable();

        knockback = GetComponent<Knockback>();
        
    }

    private void Start()
    {
        hurtbox.OnHit.AddListener(SwordHit);
        
        var fireButton = playerControls.Player.Fire;
        swipePosition = playerControls.Player.Position;
        // This is working off the resolution rather than a normalised coordinate :/
        fireButton.performed += StartSwipe;
        fireButton.canceled += FinishSwipe;

        playerControls.Player.Dodge.performed += Dodge;
        PlayerDeathEvent.AddListener(PlayerDeath);
        
        transform.position = grid.GetCellCenterWorld(grid.WorldToCell(transform.position));
    }

    private void StartSwipe(InputAction.CallbackContext context)
    {
        
        swipeStart = swipePosition.ReadValue<Vector2>();
    }

    private void OnDisable()
    {
        var fireButton = playerControls.Player.Fire;
        fireButton.performed -= StartSwipe;
        fireButton.canceled -= FinishSwipe;
        playerControls.Player.Dodge.performed -= Dodge;
    }

    private void Update()
    {
        
        //TODO: Move into different function and keep first pressed direction down to avoid jitters.
        animator.SetBool("isWalkingRight", false);
        animator.SetBool("isWalkingLeft", false);
        animator.SetBool("isWalkingUp", false);
        animator.SetBool("isWalkingDown", false);
        
        inputValue = playerControls.Player.Move.ReadValue<Vector2>();

       
        
        if (inputValue.x >= 0.7)
        {
            animator.SetBool("isWalkingRight", true);
            Facing = Vector2.right;
            
        }
        else if (inputValue.x <= -0.7)
        {
            animator.SetBool("isWalkingLeft", true);
            Facing = Vector2.left;
            
        }
        else if (inputValue.y >= 0.7)
        {
            animator.SetBool("isWalkingUp", true);
            Facing = Vector2.up;
        }
        else if (inputValue.y <= -0.7)
        {
            animator.SetBool("isWalkingDown", true);
            Facing = Vector2.down;
        }
        
    }

    private void FinishSwipe(InputAction.CallbackContext context)
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

    public void HitPlayer()
    {
        if (invuln) return;
        PlayerDeathEvent.Invoke();
    }

    private void Dodge(InputAction.CallbackContext context)
    {
        var splurtInfo = levelManager.GetSplurtInfo(grid.WorldToCell(transform.position));
        
        if (splurtInfo?.SpurtAction == null)
        {
            rb.AddForce(inputValue * (dodgeSpeed * Time.fixedDeltaTime), ForceMode2D.Impulse);
            return;
        }

        splurtInfo.SpurtAction(this, splurtInfo.StartingCell, splurtInfo.EndingCell);

    }

    public void TakeDamage(Transform damageSource, float knockbackAmount)
    {
        if (invuln) return;
        knockback.KnockBack(damageSource, knockbackAmount);
    }

    private void PlayerDeath()
    {
        Debug.Log("The last bread slice has fallen!!!");
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
