using UnityEngine.Networking;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 1f;
    public float jumpSpeed = 3f;
    public float airSpeed = 4.8f;
    public float maxAirSpeed = 20f;
    public float fallMultiplier = 2.5f;
    public float shortJumpMultiplier = 2f;
    public float swingLaunchMultiplier = 1.8f;
    public float dashSpeed = 7;
    public bool groundCheck;
    public bool isSwinging;
    private SpriteRenderer playerSprite;
    private Rigidbody2D rBody;
    public bool isJumping;
    private float jumpInput;
    private float horizontalInput;
    private Animator animator;
    public Vector2 ropeHook;
    public float swingForce = 4f;
    public float velocity;

    public float minCamDistance = 24;
    public float maxCamDistance = 30;

    public CinemachineVirtualCamera vcam;

    void Awake()
    {

    }
    
    public override void OnStartLocalPlayer()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        playerSprite = GetComponent<SpriteRenderer>();
        rBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        velocity = rBody.velocity.y;
        //Camera.main.GetComponent<CameraController>().setTarget(gameObject.transform);
    }

    void Update()
    {
        

        if (!isLocalPlayer)
        {
            return;
        }

        jumpInput = Input.GetAxis("Jump");
        horizontalInput = Input.GetAxis("Horizontal");
        
        
        animator.SetBool("IsSwinging", isSwinging);
        animator.SetBool("IsGrounded", groundCheck);
        animator.SetBool("IsJumping", isJumping);

        
        

    }

    void FixedUpdate()
    {

        if (!isLocalPlayer)
        {
            return;
        }

        var halfHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        groundCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - halfHeight - 0.04f), Vector2.down, 0.025f);
        cameraZoom();

        // Use this to track player jump arc for debug
        // Debug.DrawRay(transform.position, Vector2.down, Color.red, .75f);

        // Horizontal Input Exists
        if (horizontalInput < 0f || horizontalInput > 0f)
        {
            animator.SetBool("Idle", false);
            animator.SetTrigger("HorizontalInput");
            playerSprite.flipX = rBody.velocity.x < 0f;

            if (isSwinging)
            {
                applySwingForce();
            }
            else // Horizontal movement, but player is NOT swinging
            {
                animator.SetBool("IsSwinging", false);
                animator.SetBool("IsGrounded", true);

                var groundForce = speed * 2f;
                if (groundCheck)
                {

                    Debug.Log(new Vector2((horizontalInput * groundForce - rBody.velocity.x) * groundForce, 0));
                    rBody.AddForce(new Vector2((horizontalInput * groundForce - rBody.velocity.x) * groundForce, 0), ForceMode2D.Force);
                    rBody.velocity += Vector2.right * horizontalInput;
                }else
                {
                    Vector2 temp = rBody.velocity;
                   
                    rBody.AddForce(new Vector2((horizontalInput * groundForce - rBody.velocity.x) * groundForce, 0), ForceMode2D.Force);
                    //rBody.velocity += Vector2.right * horizontalInput;
                }

            }
        }
        else //No horizontal input
        {
            animator.SetBool("IsSwinging", false);
            animator.SetBool("Idle", true);
        }


        if (!isSwinging)
        {
            if (!groundCheck)
            {

                //Apply fall modifier to gravity once player reaches the peak of jump
                if (rBody.velocity.y < 0)
                {
                    rBody.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
                }

                return;
            }
            isJumping = jumpInput > 0f;
            if (isJumping)
            {
                rBody.velocity += Vector2.up * jumpSpeed;
            }
        }


    }

    void applySwingForce()
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
        // Apply swinging force perpindicular to rope contact point
        var force = perpendicularDirection * swingForce * 1.5f;
        rBody.AddForce(force, ForceMode2D.Force);
    }

    void cameraZoom()
    {
        // Move camera back when player moves faster, zoom back in when player slows down.
        var p = Mathf.Lerp(vcam.m_Lens.OrthographicSize, rBody.velocity.magnitude, Time.deltaTime);

        // Keep camera within acceptable distance from player
        if (p > minCamDistance && p < maxCamDistance)
        {
            vcam.m_Lens.OrthographicSize = p;
        }
        else if (p <= minCamDistance)
        {
            vcam.m_Lens.OrthographicSize = minCamDistance;
        }
        else if (p >= maxCamDistance)
        {
            vcam.m_Lens.OrthographicSize = maxCamDistance;
        }
    }

}
