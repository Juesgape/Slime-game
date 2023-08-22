using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    private Animator animator;


    //movement force
    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float maxJumpForce = 20f;
    private float jumpForce = 0f;
    private bool isJumping = false;


    private Vector3 originalScale; // Left or right scale

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
        //We can move horizontally if the character is not falling
        if(player.velocity.y > -.1f)
        {
            //This gets the position of our character
            dirX = Input.GetAxisRaw("Horizontal");

            if (dirX < 0) // Left Movement
            {
                transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
            }
            else if (dirX > 0) // Right Movement
            {
                transform.localScale = originalScale;
            }
            //Moving our character based on its position
            player.velocity = new Vector2(dirX * moveSpeed, player.velocity.y);
        } else
        {
            //Make the velocity in the X axis 0 so the player falls vertically
            player.velocity = new Vector2(0f, player.velocity.y);
        }

        //Jumping mechanics
        if(Input.GetKeyDown("space") && !isJumping && IsGrounded())
        {
            isJumping = true;
            animator.SetBool("IsJumping", true);
        }

        if(isJumping)
        {
            //Adding up the jump force
            jumpForce += Time.deltaTime * 50f;
        }

        //verify when the space key is not on anymore
        if (Input.GetKeyUp("space") && isJumping && IsGrounded())
        {
            Jump();
        }

        //Animation controller
        updateAnimation();
    }

    private void Jump()
    {
        // Apply vertical force for the jump
        int jumpDirection = Mathf.RoundToInt(dirX);
        player.AddForce(Vector2.up * Mathf.Min(jumpForce, maxJumpForce), ForceMode2D.Impulse);

        // Reinitiate the values
        jumpForce = 0f;
        isJumping = false;
    }

    private bool IsGrounded()
    {
        //Create a box similar to the box collider
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down,  .1f, jumpableGround);
    }
  
    private void updateAnimation()
    {

        if (IsGrounded())
        {
            if (jumpForce != 0f)
            {
                // Preparing to jump
                SwitchToJumpAnimation();
            }
            else if (jumpForce == 0 && IsGrounded() == true)
            {
                // Just Landed
                SwitchToIdleAnimation();

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

    private void SwitchToJumpAnimation()
    {
        animator.runtimeAnimatorController = jumpController;
    }

    private void SwitchToIdleAnimation()
    {
        animator.runtimeAnimatorController = idleController;
    }

}
