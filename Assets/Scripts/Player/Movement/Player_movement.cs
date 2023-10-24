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
    [SerializeField] private float maxJumpForce = 30f;
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

        if (PlayerPrefs.HasKey("initialPositionX") && PlayerPrefs.HasKey("initialPositionY"))
        {
            // Load the saved position from PlayerPrefs
            float initialPositionX = PlayerPrefs.GetFloat("initialPositionX");
            float initialPositionY = PlayerPrefs.GetFloat("initialPositionY");

            // Set the initial position of the character
            Vector2 initialPosition = new Vector2(initialPositionX, initialPositionY);
            player.transform.position = initialPosition;
        } else
        {
            // Set the default initial position
            Vector2 defaultInitialPosition = new Vector2(-1.95f, -10.23f);
            player.transform.position = defaultInitialPosition;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (canMove) //Only can move the player if the variable "canMove" is set to true
        {
            mainPlayerMovements(); //Function that calls all the movements our player can make
        }

        PlayerPrefs.SetFloat("initialPositionX", player.transform.position.x);
        PlayerPrefs.SetFloat("initialPositionY", player.transform.position.y);
    }

    private void mainPlayerMovements()
    {
        // Handle jumping
        JumpHandler();

        //Basic player movements on the x axis
        PlayerDirectionOnXAxis();

        // Handle ramp movement
        RampMovementHandler();

        //Animation controller
        UpdateAnimation();
    }

    private void PlayerDirectionOnXAxis()
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

    private void RampMovementHandler()
    {
        if (isOnRamp) // you can't move the player if it's on a ramp, right?
        {
            //disable movement for player
            DisableMovement();

            // Apply force in the opposite direction of the ramp using Impulse
            player.AddForce(new Vector2(-0.5f, -0.5f) * slideForce, ForceMode2D.Impulse);

            return;
        }

        //enable movement again
        EnableMovement();
    }


    private void JumpHandler()
    {

        if(IsGrounded() && canJump == false)
        {
            player.sharedMaterial = normal; // Switch to normal material on the ground
            player.velocity = new Vector2(0.0f, 0.0f);
            DisableIsBouncing();
        }

        if (IsGrounded())
        {
            //Allowing player to jump
            if (Input.GetKey("space") && canJump == true)
            {
                jumpForce += Time.deltaTime * 40f;
            }

            //Don't allow the gamer to press the space key infinitely
            if (jumpForce >= maxJumpForce)
            {
                float tempx = dirX * moveSpeed;
                float tempy = jumpForce;
                player.velocity = new Vector2(tempx, tempy);
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
                //Reset the jump force when we stop pressing the space key
                ResetJump();
            }

            //Whatever the case is, we want to eventually be able to jump
            canJump = true;
            return;
        }

        //Reset jump while the slime is not on the ground
        ResetJump(); 
        player.sharedMaterial = bounce; // Switch to bounce material in the air
    }


    //Reset Jump Variables
    private void ResetJump()
    {
        canJump = false;
        jumpForce = 0.0f;
    }

    //Check if is grounded
    private bool IsGrounded()
    {
        //Create a box similar to the box collider
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .5f, jumpableGround);
    }

    // Update animations based on player state
    private void UpdateAnimation()
    {
        //Referencing the playerAnimation script
        Player_animation playerAnimation = GetComponent<Player_animation>();
        
        if (playerAnimation != null)
        {
            //Function that controlls our player animation
            playerAnimation.PlayerAnimationController();
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------
    //Various functions that will help us modify our variables in other scripts
    //------------------------------------------------------------------------------------------------------------------------------
    
    public void DisableMovement()
    {
        canMove = false;
    }

    public void EnableMovement()
    {
        canMove = true;
    }
    public void EnableIsOnRamp()
    {
        isOnRamp = true;
    }
    public void DisableIsOnRamp()
    {
        isOnRamp = false;
    }

    public void EnableIsBouncing()
    {
        isBouncing = true;
    }

    public void DisableIsBouncing()
    {
        isBouncing = false;
    }

    public bool GetIsBouncing()
    {
        return isBouncing;
    }

    public float GetJumpForce()
    {
        return jumpForce;
    }
}