using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // PARAMETERS FOR DEBUGGING JUMPS
    //public float maxJump = 0;
    //public float minJump = 999;
    //public float thisJump = 0;
    //private float jumpStartHeight = 0;
    //public float djHeight = 0;
    //private bool doTheDJ = false;

    public bool jumpSkill = false;
    public bool doubleJumpSkill = false;
    public bool highJumpSkill = false;

    public float speed = 5f;
    // best 11  and 14 with 4 gravity
    public float jumpHeight = 11f;
    public float highJumpHeight = 14f;
    public LayerMask groundLayers;

    private bool isFacingRight = true;
    private bool isGrounded = false;
    private bool tryJump = false;
    private bool hasDoubleJumped = false;

    new private Rigidbody2D rigidbody;
    new private BoxCollider2D collider;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        tryJump = Input.GetButtonDown("Jump") ? true : tryJump;
    }

    void FixedUpdate()
    {
        CheckForGround();
        Move();
        //measureJump();
    }

    //void measureJump()
    //{
    //    float heightDelta = rigidbody.position.y - jumpStartHeight;
    //    if (heightDelta > thisJump)
    //    {
    //        thisJump = heightDelta;

    //        if (thisJump > maxJump)
    //        {
    //            maxJump = thisJump;
    //        }
    //    }
    //}

    //void autoDJ()
    //{
    //    float jH = rigidbody.position.y - jumpStartHeight;
    //    if (jH >= djHeight && !hasDoubleJumped && doubleJumpSkill)
    //    {
    //        print("DJ");
    //        rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpHeight);
    //        hasDoubleJumped = true;
    //    }
    //}

    void CheckForGround()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        float horizontalOffset = collider.size.x / 2 - 0.01f;
        float verticalOffset = collider.size.y / 2;

        Vector2 topLeft = position + new Vector2(-horizontalOffset, -verticalOffset);
        Vector2 bottomRight = position + new Vector2(horizontalOffset, -verticalOffset);

        isGrounded = Physics2D.OverlapArea(topLeft, bottomRight, groundLayers);

        //if (isGrounded && thisJump != 0 && thisJump < minJump)
        //{
        //    minJump = thisJump;
        //}
    }

    void Move()
    {
        // Use GetAxisRaw for more precise movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        float horizontalMove = horizontalInput * speed;

        bool doJump = false;
        if (tryJump)
        {
            if (jumpSkill)
            {
                if (isGrounded)
                {
                    doJump = true;
                    hasDoubleJumped = false;

                    //thisJump = 0;
                    //jumpStartHeight = rigidbody.position.y;
                }
                else if (doubleJumpSkill && !hasDoubleJumped)
                {
                    doJump = true;
                    hasDoubleJumped = true;
                }
            }
            tryJump = false;
        }

        //autoDJ();

        float verticalMove = doJump ? (highJumpSkill ? highJumpHeight : jumpHeight) : rigidbody.velocity.y;

        rigidbody.velocity = new Vector2(horizontalMove, verticalMove);

        // Flip the character around if it's facing the wrong direction
        if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector2 flipped = transform.localScale;
        flipped.x *= -1;
        transform.localScale = flipped;
    }
}
