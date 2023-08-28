using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Player_movement : MonoBehaviour

{
    //Game object
    private Rigidbody2D player;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;

    //getting the layer for the terrain 
    [SerializeField] private LayerMask jumpableGround;

    //Animations controllers
    public RuntimeAnimatorController idleController;
    public RuntimeAnimatorController jumpController;
    public RuntimeAnimatorController bounceController;      
    
    private bool isBouncing = false;
    private bool canBounce = true;
    private Animator animator;

        
    //Movement force
    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float maxJumpForce = 20f;
    private float jumpForce = 0f;
    private bool canJump = true;

    // Physics materials
    public PhysicsMaterial2D bounce, normal, friction;
    private Vector3 originalScale; // Left or right scale

    //This variable will set to true or false depending if the player is colliding with the slime_enemy
    private bool isCollidingWithSlimeEnemy = false;

    // Ramp variables
    public float slideForce = 0.1f;
    private bool isOnRamp = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //move normally as long as the player is not colliding with the slime_enemy
        if(isCollidingWithSlimeEnemy)
        {
            //Do nothing lol
        } 
        else
        {
            mainPlayerMovements();
        }

        // Check for bounce animation

        if (isBouncing && player.sharedMaterial == bounce)
        {
            SwitchToBounceAnimation();
            
        }
    }

    private void mainPlayerMovements()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        // Flip the sprite when changing direction
        if (dirX < 0 && IsGrounded()) // Left Movement
        {
            transform.localScale = new Vector2(-originalScale.x, originalScale.y);
        }
        else if (dirX > 0 && IsGrounded()) // Right Movement
        {
            transform.localScale = originalScale;
        }

        // Move the player when grounded

        if (jumpForce == 0.0f && IsGrounded())
        {
            player.velocity = new Vector2(dirX * moveSpeed, player.velocity.y);
        }

        // Handle jumping
        if (IsGrounded())
        {
            player.sharedMaterial = normal; // Switch to normal material on the ground
            isBouncing = false;

            if (Input.GetKey("space") && canJump == true)
            {
                jumpForce += Time.deltaTime * 20f;
            }

            if (jumpForce >= maxJumpForce)
            {
                float tempx = dirX * moveSpeed;
                float tempy = jumpForce;
                player.velocity = new Vector2(tempx, tempy);
                Invoke("ResetJump", 0.05f);
            }

            if (Input.GetKeyDown("space"))
            {
                player.velocity = new Vector2(0.0f, player.velocity.y);
            }

            if (Input.GetKeyUp("space"))
            {
                player.velocity = new Vector2(dirX * moveSpeed, jumpForce);
                jumpForce = 0.0f;
                canJump = true;
            }
        }
        else
        {

            player.sharedMaterial = bounce; // Switch to bounce material in the air
        }

        // Handle ramp movement
        if (isOnRamp)
        {
            dirX = 0f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                canJump = false;
            }

            // Apply force in the opposite direction of the ramp using Impulse
            player.AddForce(new Vector2(-0.5f, -0.5f) * slideForce, ForceMode2D.Impulse);

        }
        else
        {
            dirX = Input.GetAxisRaw("Horizontal");
        }
        //Animation controller
        updateAnimation();
    }

    //Reset Jump Variables
    void ResetJump()
    {
        canJump = false;
        jumpForce = 0;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ramp"))
        {
            isOnRamp = true;
        }

        if (collision.gameObject.CompareTag("slime_enemy"))
        {
            isCollidingWithSlimeEnemy = true;

            // Calculate the direction from the player to the slime_enemy
            Vector2 pushDirection = (transform.position - collision.transform.position).normalized;

            // Apply a force to the player's Rigidbody to push them off
            player.AddForce(pushDirection * 20f, ForceMode2D.Force);

        }

        if (collision.gameObject.CompareTag("Terrain") && player.sharedMaterial == bounce ) // Comparar con la etiqueta del suelo
        {
            // Verificar que no haya rebotado antes
            isBouncing = true;
            canBounce = false;  // Marcar que ha rebotado una vez
            
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ramp"))
        {
            isOnRamp = false;
        }

        if (collision.gameObject.CompareTag("slime_enemy"))
        {
            isCollidingWithSlimeEnemy = false;
        }

        if (collision.gameObject.CompareTag("Terrain") && player.sharedMaterial == bounce)
        {
            canBounce = true; // Habilitar el rebote cuando el personaje deje de tocar la pared
        }
    }

    private bool IsGrounded()
    {
        //Create a box similar to the box collider
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down,  .5f, jumpableGround);
    }

    // Update animations based on player state
    private void updateAnimation()
    {

        if (IsGrounded())
        {
            if (jumpForce != 0f)
            {
                // Preparing to jump
                SwitchToJumpAnimation(); // Preparing to jump
            }
            else if (jumpForce == 0 && IsGrounded() == true)
            {
                // Just Landed
                SwitchToIdleAnimation(); // Just landed

            }

        }
        else
        {
            // In the air
            if (player.velocity.y > 0f)
            {
                // It's jumping
                SwitchToJumpAnimation();
                animator.SetBool("IsJumping", true);

            }
            else
            {
                // It's falling down
                SwitchToJumpAnimation();
                animator.SetBool("IsJumping", false);

            }
        }
    }

    //Animation Switches
    private void SwitchToJumpAnimation()
    {
        animator.runtimeAnimatorController = jumpController;
    }

    private void SwitchToIdleAnimation()
    {
        animator.runtimeAnimatorController = idleController;
    }

    private void SwitchToBounceAnimation()
    {
        animator.runtimeAnimatorController = bounceController;
    }

}
