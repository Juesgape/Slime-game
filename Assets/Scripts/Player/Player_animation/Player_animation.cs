using Cinemachine.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_animation : MonoBehaviour
{
    //Game object
    private Rigidbody2D player;
    private BoxCollider2D coll;

    //Animations controllers
    public RuntimeAnimatorController idleController;
    public RuntimeAnimatorController jumpController;
    public RuntimeAnimatorController bounceController;
    private Animator animator;

    //Physic
    public PhysicsMaterial2D bounce;

    //getting the layer for the terrain 
    [SerializeField] private LayerMask jumpableGround;

    // Start is called before the first frame update
    void Start()
    {
        //Get the Player_movement public class
        animator = GetComponent<Animator>();
        player = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    public void PlayerAnimationController()
    {
        //Referencing the playerMovement class
        Player_movement playerMovement = GetComponent<Player_movement>();

        // Check for bounce animation
        if (playerMovement.GetIsBouncing() == true && player.sharedMaterial == bounce)
        {
            SwitchToBounceAnimation();

            return;
        }

        if (IsGrounded())
        {

            if (playerMovement.GetJumpForce() > 0f)
            {
                // Preparing to jump
                SwitchToJumpAnimation(); // Preparing to jump
            }

            if (playerMovement.GetJumpForce() == 0f)
            {
                // Just Landed
                SwitchToIdleAnimation(); // Just landed
            }

            //Finish execution
            return;

        }

        // In the air
        if (player.velocity.y > 0f)
        {
            // It's jumping
            SwitchToJumpAnimation();
            animator.SetBool("IsJumping", true);
        }

        if(player.velocity.y < 0f)
        {
            SwitchToJumpAnimation();
            animator.SetBool("IsJumping", false);
        }

    }

    //Check if is grounded
    private bool IsGrounded()
    {
        //Create a box similar to the box collider
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .5f, jumpableGround);
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
