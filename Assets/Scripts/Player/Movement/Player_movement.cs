using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Player_movement : MonoBehaviour

{
    //Game object
    private Rigidbody2D player;
    private BoxCollider2D coll;

    //getting the layer for the terrain 
    [SerializeField] private LayerMask jumpableGround;

    //Movement force
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float maxJumpForce = 20f;
    private float dirX = 0f;
    private float jumpForce = 0f;
    private bool canMove = true;
    private bool canJump = true;

    //Bouncing shit
    private bool isBouncing = false;

    // Physics materials
    public PhysicsMaterial2D bounce, normal, friction;

    //I don't have any idea of what this does
    private Vector3 originalScale; // Left or right scale

    // Ramp variables
    public float slideForce = 0.1f;
    private bool isOnRamp = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove) //Only can move the player if the variable "canMove" is set to true
        {
            mainPlayerMovements(); //Function that calls all the movements our player can make
        }
    }

    private void mainPlayerMovements()
    {
        //Basic player movements on the x axis
        playerDirectionOnXAxis();

        // Handle jumping
        jumpHandler();

        // Handle ramp movement
        rampMovementHandler();

        //Animation controller
        updateAnimation();
    }

    private void playerDirectionOnXAxis()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        // Flip the sprite when changing direction
        if (dirX < 0 && IsGrounded()) // Left Movement
        {
            transform.localScale = new Vector2(-originalScale.x, originalScale.y);
        }

        if (dirX > 0 && IsGrounded()) // Right Movement
        {
            transform.localScale = originalScale;
        }

        // Move the player when grounded
        if (jumpForce == 0.0f && IsGrounded())
        {
            player.velocity = new Vector2(dirX * moveSpeed, player.velocity.y);
        }
    }

    private void rampMovementHandler()
    {
        if (isOnRamp) // you can't move the player if it's on a ramp, right?
        {
            //disable movement for player
            disableMovement();

            // Apply force in the opposite direction of the ramp using Impulse
            player.AddForce(new Vector2(-0.5f, -0.5f) * slideForce, ForceMode2D.Impulse);

            return;
        }

        //enable movement again
        enableMovement();
    }


    private void jumpHandler()
    {
        if (IsGrounded())
        {
            player.sharedMaterial = normal; // Switch to normal material on the ground
            disableIsBouncing();

            //Allowing player to jump
            if (Input.GetKey("space") && canJump == true)
            {
                jumpForce += Time.deltaTime * 20f;
            }

            //Don't allow the gamer to press the space key infinitely
            if (jumpForce >= maxJumpForce)
            {
                float tempx = dirX * moveSpeed;
                float tempy = jumpForce;
                player.velocity = new Vector2(tempx, tempy);
                Invoke("ResetJump", 0.05f);
            }

            //If gamer press the space key, disable movement on the x axis
            if (Input.GetKeyDown("space"))
            {
                player.velocity = new Vector2(0.0f, player.velocity.y);
            }

            //Jump when gamer stops pressing the space key
            if (Input.GetKeyUp("space"))
            {
                player.velocity = new Vector2(dirX * moveSpeed, jumpForce);
                jumpForce = 0.0f;
                canJump = true;
            }

            return;
        }

        player.sharedMaterial = bounce; // Switch to bounce material in the air
    }


    //Reset Jump Variables
    private void ResetJump()
    {
        canJump = false;
        jumpForce = 0;
    }

    //Check if is grounded
    private bool IsGrounded()
    {
        //Create a box similar to the box collider
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .5f, jumpableGround);
    }

    // Update animations based on player state
    private void updateAnimation()
    {
        //Referencing the playerAnimation script
        Player_animation playerAnimation = GetComponent<Player_animation>();
        
        if (playerAnimation != null)
        {
            //Function that controlls our player animation
            playerAnimation.playerAnimationController();
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------
    //Various functions that will help us modify our variables in other scripts
    //------------------------------------------------------------------------------------------------------------------------------
    
    public void disableMovement()
    {
        canMove = false;
    }

    public void enableMovement()
    {
        canMove = true;
    }
    public void enableIsOnRamp()
    {
        isOnRamp = true;
    }
    public void disableIsOnRamp()
    {
        isOnRamp = false;
    }

    public void enableIsBouncing()
    {
        isBouncing = true;
    }

    public void disableIsBouncing()
    {
        isBouncing = false;
    }

    public bool getIsBouncing()
    {
        return isBouncing;
    }

    public float getJumpForce()
    {
        return jumpForce;
    }
}