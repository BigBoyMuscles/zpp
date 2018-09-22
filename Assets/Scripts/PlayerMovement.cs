﻿using UnityEngine.Networking;
using UnityEngine;

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

    void Awake()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        rBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        velocity = rBody.velocity.y;
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    void Update()
    {
        velocity = rBody.velocity.y;

        if (!isLocalPlayer)
        {
            return;
        }

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

        if (!isLocalPlayer)
        {
            return;
        }

        Debug.DrawRay(transform.position, Vector2.down, Color.red, .75f);

        // If horizontal input exists
            if (horizontalInput < 0f || horizontalInput > 0f)
            {
                animator.SetBool("Idle", false);
                animator.SetTrigger("HorizontalInput");
                playerSprite.flipX = horizontalInput < 0f;

                // Accelerate the player through the swing arc
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

                    var force = perpendicularDirection * swingForce * 1.5f;
                    rBody.AddForce(force, ForceMode2D.Force);
                }
                else // Horizontal movement, but player is NOT swinging
                {
                    animator.SetBool("IsSwinging", false);
                    animator.SetBool("IsGrounded", true);

                    var groundForce = speed * 2f;
                    rBody.AddForce(new Vector2((horizontalInput * groundForce - rBody.velocity.x) * groundForce, 0));
                    rBody.velocity = new Vector2(rBody.velocity.x, rBody.velocity.y);
                    
                    
                    


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

                if (Input.GetKeyDown(KeyCode.E))
                {
                    int i = 0;
                    if(rBody.velocity.x > 0)
                    {
                        i = 1;
                    }else
                    {
                        i = -2;
                    }
                    rBody.AddForce(Vector2.right * i * dashSpeed * 10, ForceMode2D.Impulse);
                        
                }

                //If player is not grounded, do not allow jumps
                if(rBody.velocity.y < 0 )
                {
                  rBody.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
                }else if (rBody.velocity.y > 0 && !Input.GetButton("Jump"))
                {
                    //rBody.velocity = 
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

}
