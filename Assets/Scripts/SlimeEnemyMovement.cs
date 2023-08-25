using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeEnemyMovement : MonoBehaviour
{
    [SerializeField] float sl_speed;
    [SerializeField] float rayDist;
    private bool movingRight;
    public Transform groundDetect;
    public LayerMask groundLayer;

    void Update()
    {
        //The slime movement is pure translations
        transform.Translate(Vector2.right * sl_speed * Time.deltaTime);
        //Raycast to detect when the slime is on the edge of a given plataform
        RaycastHit2D groundCheck = Physics2D.Raycast(groundDetect.position, Vector2.down, rayDist, groundLayer);

        if (groundCheck.collider == false)
        {
            if (movingRight)
            {
                Quaternion newRotation = Quaternion.Euler(0f, -180f, 0f);
                transform.rotation = newRotation;
                movingRight = false;
            }
            else
            {
                Quaternion newRotation = Quaternion.Euler(0f, 0f, 0f);
                transform.rotation = newRotation;
                movingRight = true;
            }
        }
    }
}
