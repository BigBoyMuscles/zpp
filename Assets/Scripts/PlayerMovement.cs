using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1f;
    public float jumpSpeed = 3f;
    public bool groundCheck;
    public bool isSwinging;
    private SpriteRenderer playerSprite;
    private Rigidbody2D rBody;
    private bool isJumping;
    private float jumpInput;
    private float horizontalInput;
    private Animator animator;
    public Vector2 ropeHook;
    public float swingForce = 4f;

    void Awake()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        rBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        jumpInput = Input.GetAxis("Jump");
        horizontalInput = Input.GetAxis("Horizontal");
        var halfHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        groundCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - halfHeight - 0.04f), Vector2.down, 0.025f);
        animator.SetBool("IsSwinging", isSwinging);
        animator.SetBool("IsGrounded", groundCheck);
        animator.SetBool("IsJumping", isJumping);
    }

    void FixedUpdate()
    {
        // If horizontal input exists
        if (horizontalInput < 0f || horizontalInput > 0f)
        {
            animator.SetBool("Idle", false);
            animator.SetTrigger("HorizontalInput");
            animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
            playerSprite.flipX = horizontalInput < 0f;
            // And the player IS swinging
            if (isSwinging)
            {
                
                // Get a normalized direction vector from the player to the hook point
                var playerToHookDirection = (ropeHook - (Vector2)transform.position).normalized;

                // Inverse the direction to get a perpendicular direction
                Vector2 perpendicularDirection;
                if (horizontalInput < 0)
                {
                    perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
                    var leftPerpPos = (Vector2)transform.position - perpendicularDirection * -2f;
                    Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
                }
                else
                {
                    perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
                    var rightPerpPos = (Vector2)transform.position + perpendicularDirection * 2f;
                    Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0f);
                }

                var force = perpendicularDirection * swingForce;
                rBody.AddForce(force, ForceMode2D.Force);
            }
            else // Horizontal movement, but player is NOT swinging
            {
                animator.SetBool("IsSwinging", false);
                if (groundCheck) // Player is moving AND grounded
                {
                    animator.SetBool("IsGrounded", true);
                    var groundForce = speed * 2f;
                    rBody.AddForce(new Vector2((horizontalInput * groundForce - rBody.velocity.x) * groundForce, 0));
                    rBody.velocity = new Vector2(rBody.velocity.x, rBody.velocity.y);
                } else // Player is moving and NOT grounded
                {
                    animator.SetBool("IsGrounded", false);
                    //Allow for tighter/looser air control if needed.
                    var groundForce = speed * 1.4f;
                    rBody.AddForce(new Vector2((horizontalInput * groundForce - rBody.velocity.x) * groundForce, 0));
                    rBody.velocity = new Vector2(rBody.velocity.x, rBody.velocity.y);
                }
            }
        }
        else //No horizontal input
        {
            animator.SetBool("IsSwinging", false);
            animator.SetBool("Idle", true);
            animator.SetFloat("Speed", 0f);
        }

        if (!isSwinging)
        {
            if (!groundCheck)
            {
                //If player is grounded, do not allow jumps
                animator.SetBool("IsAirborne", true);
                return; 
            }
            isJumping = jumpInput > 0f;
            if (isJumping)
            {
                
                animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
                rBody.velocity = new Vector2(rBody.velocity.x, jumpSpeed);
            }
        }
    }

}
