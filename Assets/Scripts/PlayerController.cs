using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public bool duckSkill = false;
    public bool crouchSkill = false;

    public float speed = 5f;
    // best 11  and 14 with 4 gravity
    public float jumpHeight = 11f;
    public float highJumpHeight = 14f;
    public LayerMask groundLayers;
    public Animator animator;
    public GameObject tilemap;

    private GameObject checkpoint;

    private bool isFacingRight = true;
    private bool isGrounded = false;
    private bool tryJump = false;
    private bool hasDoubleJumped = false;
    private bool tryCrouch = false;
    private bool tryDuck = false;

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

        // Ducking
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            tryDuck = true;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            tryDuck = false;
        }

        // Crouching
        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)) && animator.GetBool("IsDucking"))
        {
            tryCrouch = true;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            tryCrouch = false;
        }

        // GameOver
        if (Input.GetKeyDown(KeyCode.D))
        {
            animator.SetBool("GameOver", true);

        }

        // Resporn
        if (Input.GetKeyDown(KeyCode.L))
        {
            animator.SetBool("GameOver", false);
        }
    }

    void FixedUpdate()
    {
        CheckForGround();
        Move();
        //measureJump();

        // Jumping
        if (!tryJump && (isGrounded && rigidbody.velocity.y <= 0))
        {
            animator.SetBool("IsJumping", false);
        }
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

        speed = 5;

        float horizontalMove = horizontalInput * speed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        // Jump
        bool doJump = false;
        if (tryJump)
        {
            if (jumpSkill)
            {
                animator.SetBool("IsJumping", true);

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

        // Duck
        if (tryDuck)
        {
            if (duckSkill)
            {
                animator.SetBool("IsDucking", true);
            }
        }
        else if (!tryDuck)
        {
            animator.SetBool("IsDucking", false);
        }

        // Crouch
        if (tryCrouch)
        {
            if (crouchSkill)
            {
                const float ySize = 1f;
                const float xSize = 2f;
                collider.size = new Vector3(xSize, ySize);
                collider.offset = new Vector3(0, -0.5f);

                speed = 1f;

                animator.SetBool("IsCrouching", true);
            }
        }

        if (!tryCrouch)
        {
            //if (!collider.IsTouching(tilemap.GetComponent<Collider2D>())) {

                print("Hit");
                const float ySize = 2f;
                const float xSize = 1f;
                collider.size = new Vector3(xSize, ySize);
                collider.offset = new Vector3(0, 0);

                speed = 5f;

                animator.SetBool("IsCrouching", false);
            //}
        }

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

    void Respawn(Vector2 respawnPosition)
    {
        rigidbody.position = respawnPosition;
    
        if (!isFacingRight)
        {
            Flip();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Checkpoint")
        {
            this.checkpoint = collider.gameObject;
        }
        else if (collider.gameObject.tag == "Killzone")
        {
            this.Respawn(this.checkpoint.transform.position);
        }
    }
}
