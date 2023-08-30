using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coll_ramp : MonoBehaviour
{
    //Game object
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    void Start()
    {
        //Get the Player_movement public class
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ramp"))
        {
            Player_movement playerMovement = GetComponent<Player_movement>();
            if (playerMovement != null)
            {
                playerMovement.enableIsOnRamp();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Player_movement playerMovement = GetComponent<Player_movement>();
        playerMovement.disableIsOnRamp();
    }
}
