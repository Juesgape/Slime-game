using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour

{
    //Game object
    private Rigidbody2D player;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator animator;


    //movement force
    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField]  private float jumpForce = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //This gets the position of out character
        dirX = Input.GetAxisRaw("Horizontal");
        //Moving our character based on its position
        player.velocity = new Vector2(dirX * moveSpeed, player.velocity.y);
    }
}
