using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// This class controls the movement and animation of the player object.true
/// </summary>
public class PlayerController : MonoBehaviour
{
    public bool jumpSkill = false;
    public bool doubleJumpSkill = false;
    public bool highJumpSkill = false;
    public bool crouchSkill = false;

    private float speed = 5f;
    // best 11  and 14 with 4 gravity
    private float jumpHeight = 11f;
    private float highJumpHeight = 14f;

    public LayerMask groundLayers;
    public Animator animator;

    private GameObject lastCheckpoint;
    
    private bool isFacingRight = true;
    private bool isGrounded = false;
    private bool hasHeadSpace = true;
    private bool tryJump = false;
    private bool hasJumped = false;
    private bool hasDoubleJumped = false;
    private bool tryCrouch = false;
    private bool tryDuck = false;

    new private Rigidbody2D rigidbody;
    new private BoxCollider2D collider;

    /// <summary>
    /// Initially get properties from object.
    /// </summary>
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// Get user input every real frame.
    /// </summary>
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadSceneAsync("Menu");
        }

        // Jump
        tryJump = Input.GetButtonDown("Jump") ? true : tryJump;

        // Duck
        tryDuck = Input.GetButton("Crouch") ? true : false;

        // Crouch
        tryCrouch = tryDuck && Input.GetButton("Horizontal") ? true : false;
    }

    /// <summary>
    /// Check surroundings and handle user input every calculated frame.
    /// </summary>
    void FixedUpdate()
    {
        CheckForGround();
        CheckHeadSpace();

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
    /// Resets the markers for made (double) jump, if the player is indeed grounded.
    /// </summary>
    void CheckForGround()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        float horizontalOffset = collider.size.x / 2 - 0.01f;
        float verticalOffset = collider.size.y / 2;

        Vector2 topLeft = position + new Vector2(-horizontalOffset, -verticalOffset);
        Vector2 bottomRight = position + new Vector2(horizontalOffset, -verticalOffset);

        isGrounded = Physics2D.OverlapArea(topLeft, bottomRight, groundLayers);

        if (isGrounded)
        {
            hasJumped = hasDoubleJumped = false;
        }
    }

    /// <summary>
    /// Checks, if the player has enough room about his head to potentially get up.
    /// The check result is saved in a class attribute.
    /// </summary>
    void CheckHeadSpace()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        float horizontalOffset = 0.49f;
        float verticalOffset = 1.4f;

        Vector2 topLeft = position + new Vector2(-horizontalOffset, +verticalOffset);
        Vector2 bottomRight = position + new Vector2(horizontalOffset, 0);

        hasHeadSpace = !Physics2D.OverlapArea(topLeft, bottomRight, groundLayers);
    }

    /// <summary>
    /// Calculates and executes the player's next move.
    /// </summary>
    void Move()
    {
        // Use GetAxisRaw for more precise movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float horizontalMove = horizontalInput * speed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (crouchSkill && !hasJumped)
        {
            // Duck
            if (tryDuck)
            {
                animator.SetBool("IsDucking", true);
            }
            else
            {
                if (hasHeadSpace)
                {
                    animator.SetBool("IsDucking", false);
                }
            }

            // Crouch
            if (tryCrouch)
            {
                const float ySize = 1f;
                const float xSize = 2f;
                collider.size = new Vector3(xSize, ySize);
                collider.offset = new Vector3(0, -0.5f);

                animator.SetBool("IsCrouching", true);
            }
            else
            {
                if (hasHeadSpace)
                {
                    const float ySize = 2f;
                    const float xSize = 1f;
                    collider.size = new Vector3(xSize, ySize);
                    collider.offset = new Vector3(0, 0);

                    animator.SetBool("IsCrouching", false);
                }
            }
        }

        // Jump
        bool doJump = false;
        if (tryJump)
        {
            if (jumpSkill && !animator.GetBool("IsDucking"))
            {
                if (isGrounded)
                {
                    doJump = true;
                    hasJumped = true;
                    animator.SetBool("IsJumping", true);
                }
                else if (doubleJumpSkill && hasJumped && !hasDoubleJumped)
                {
                    doJump = true;
                    hasDoubleJumped = true;
                    animator.SetBool("IsJumping", true);
                }
            }
            tryJump = false;
        }

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
        Vector3 respawnPosition = this.lastCheckpoint.transform.position;
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
            this.lastCheckpoint = collider.gameObject;
        }
        else if (collider.gameObject.tag == "Killzone")
        {
            this.Respawn();
        }
    }
}
