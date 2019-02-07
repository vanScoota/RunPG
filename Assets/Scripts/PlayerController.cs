using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public bool jumpSkill = false;
    public bool doubleJumpSkill = false;
    public bool highJumpSkill = false;
    public bool duckSkill = false;
    public bool crouchSkill = false;

    private float speed = 5f;
    // best 11  and 14 with 4 gravity
    private float jumpHeight = 11f;
    private float highJumpHeight = 14f;

    public LayerMask groundLayers;
    public Animator animator;
    public GameObject crouchCollider;

    private GameObject checkpoint;
    //private CrouchColliderControler ccd;
    

    private bool isFacingRight = true;
    public bool isGrounded = false;
    private bool tryJump = false;
    public bool hasDoubleJumped = false;
    private bool tryCrouch = false;
    private bool tryDuck = false;
    //private bool trigger = false;
    //private bool implemented = false;

    new private Rigidbody2D rigidbody;
    new private BoxCollider2D collider;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Jump
        tryJump = Input.GetButtonDown("Jump") ? true : tryJump;

        // Duck
        if (Input.GetButtonDown("Crouch"))
        {
            tryDuck = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            tryDuck = false;
        }

        // Crouch
        if ((Input.GetButtonDown("Horizontal") && Input.GetButton("Crouch")) || (Input.GetButtonDown("Crouch") && Input.GetButton("Horizontal"))) 
        {
            tryCrouch = true;
        } 
        else if (Input.GetButtonUp("Crouch"))
        {
            tryCrouch = false;
        }

        // Game Over
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetBool("GameOver", true);

        }

        // Respawn
        if (Input.GetKeyDown(KeyCode.L))
        {
            animator.SetBool("GameOver", false);
        }
    }

    void FixedUpdate()
    {
        CheckForGround();
        Move();

        // Jumping
        if (!tryJump && (isGrounded && rigidbody.velocity.y <= 0))
        {
            animator.SetBool("IsJumping", false);
        }
    }

    /// <summary>
    /// Checks, if the player is touching the ground.
    /// The check result is saved in a class attribute.
    /// </summary>
    void CheckForGround()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        float horizontalOffset = collider.size.x / 2 - 0.01f;
        float verticalOffset = collider.size.y / 2;

        Vector2 topLeft = position + new Vector2(-horizontalOffset, -verticalOffset);
        Vector2 bottomRight = position + new Vector2(horizontalOffset, -verticalOffset);

        isGrounded = Physics2D.OverlapArea(topLeft, bottomRight, groundLayers);
    }

    /// <summary>
    /// Calculates and executes the player's next move.true
    /// </summary>
    void Move()
    {

        //GameObject crouchCollider = GameObject.Find("CrouchCollider 01");
        //ccd = crouchCollider.GetComponent<CrouchColliderControler>();

        // Use GetAxisRaw for more precise movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");

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
                }
                else if (doubleJumpSkill && !hasDoubleJumped)
                {
                    doJump = true;
                    hasDoubleJumped = true;
                }
            }
            tryJump = false;
        }

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
                //if(!implemented)
                //{
                //    headCollider = gameObject.AddComponent<BoxCollider2D>();
                //    headCollider.size = new Vector2(1, 0.5380561f);
                //    headCollider.offset = new Vector2(0, 0.3262531f);
                //    headCollider.isTrigger = true;
                //    implemented = true;
                //}

                const float ySize = 1f;
                const float xSize = 2f;
                collider.size = new Vector3(xSize, ySize);
                collider.offset = new Vector3(0, -0.5f);

                animator.SetBool("IsCrouching", true);
            }
        }

        if (!tryCrouch)
        {
            //Destroy(headCollider);
            //implemented = false;

            const float ySize = 2f;
            const float xSize = 1f;
            collider.size = new Vector3(xSize, ySize);
            collider.offset = new Vector3(0, 0);

            animator.SetBool("IsCrouching", false);
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

    /// <summary>
    /// Flips the player around horizontally.
    /// </summary>
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector2 flipped = transform.localScale;
        flipped.x *= -1;
        transform.localScale = flipped;
    }

    /// <summary>
    /// Revives the player at the last checkpoint.
    /// </summary>
    void Respawn()
    {
        Vector3 respawnPosition = this.checkpoint.transform.position;
        respawnPosition.y += 3;
        rigidbody.position = respawnPosition;

        if (!isFacingRight)
        {
            Flip();
        }
    }

    /// <summary>
    /// Checks for collisions with other game objects.
    /// If the other game object is a checkpoint, update the player's respawn position.
    /// If the other game object is a killzone, respawn the player.
    /// </summary>
    /// <param name="collider">Collider of the colliding object.</param>
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Checkpoint")
        {
            this.checkpoint = collider.gameObject;
        }
        else if (collider.gameObject.tag == "Killzone")
        {
            this.Respawn();
        }
    }
}
