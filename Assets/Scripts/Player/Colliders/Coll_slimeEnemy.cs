using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coll_slimeEnemy : MonoBehaviour
{
    //Game object
    private Rigidbody2D player;
    private BoxCollider2D coll;

    void Start()
    {
        //Get the Player_movement public class
        player = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("slime_enemy"))
        {
            Player_movement playerMovement = GetComponent<Player_movement>();

            if (playerMovement != null)
            {
                playerMovement.disableMovement();
            }

            // Calculate the direction from the player to the slime_enemy
            Vector2 pushDirection = (transform.position - collision.transform.position).normalized;

            // Apply a force to the player's Rigidbody to push them off
            player.AddForce(pushDirection * 20f, ForceMode2D.Force);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //When the player stops colliding tiwht the enemy allow it to move it
        Player_movement playerMovement = GetComponent<Player_movement>();
        playerMovement.enableMovement();
    }
}
