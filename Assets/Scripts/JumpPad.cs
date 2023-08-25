using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    private float bounce = 10.0f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = collider.GetComponent<Rigidbody2D>();
            playerRigidbody.AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
        }
    }
}
