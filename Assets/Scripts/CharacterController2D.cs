using System;
using UnityEngine;
using UnityEngine.InputSystem;

// CharacterController2D is based upon the 2DCharacterController from Sharp Coder Blog
// URL: https://www.sharpcoderblog.com/blog/2d-platformer-character-controller
// 
// Adapted by Tom Corbett on 10/20/25

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class CharacterController2DScript : MonoBehaviour
{
    // Move the player in 2D space
    [HeaderAttribute("Platforming Stats")]
    public float speed = 1f;
    public float jumpHeight = 2f;
    public float gravityScale = 1f;
    public AudioClip jumpClip;
    public AudioClip shootClip;

    // private values
    private Rigidbody2D rb;
    private InputAction moveAction, jumpAction;
    private float moveDirection = 0f;
    private AudioSource audioComponent;

    // Ground Detection
    [HeaderAttribute("Ground Detection")]
    public bool isGrounded = false;
    public float groundCheckRadius;
    public Vector2 groundCheckOffset;
    public LayerMask groundLayerMask;

    // Jump Stuff
    [HeaderAttribute("Jump Stuff")]
    public float maxJumps = 2f;
    public float jumpFatigue = 0.8f;
    public float currJumps = 0f;

    public GameObject breadProjectile;
    public Transform firePoint;

    [Header("CharacterSprites and Animation")]
    public Animator animator;
    public bool facingRight = true;
    public bool _isAlive = true;
    public bool jumpFlag = false;

    [Header("Jelly Machanics")]
    public bool hasJamPowerUp = false;

    [Header("Wall Stick Settings")]
    public LayerMask wallLayerMask;
    public float wallSlideSpeed = 1f;
    public float wallJumpForce = 5f;
    public float wallJumpVertical = 2f;
    public bool isTouchingWall = false;
    public bool canWallJump= false;

    [Header("Health System")]
    public int lives = 1; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get the components
        rb = GetComponent<Rigidbody2D>();
        audioComponent = GetComponent<AudioSource>();

        // did the animator get properly set? 
        if (animator == null)
        {
            // find a child animator
            animator = GetComponentInChildren<Animator>();
        }

        // configure the Rigidbody
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.gravityScale = gravityScale;

        // define our actions
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAlive)
        {
            // set the move direction
            moveDirection = moveAction.ReadValue<Vector2>().x;
        }
        else
        {
            moveDirection = 0;
        }

        // update the animator speed
        animator.SetFloat("Speed", Mathf.Abs(moveDirection));

        // get the jump action
        //if (jumpAction.WasPressedThisFrame() && _isAlive && (currJumps < maxJumps || isTouchingWall))
        //{
        //    currJumps++;

        //    jumpFlag = true;
        //    animator.SetBool("Grounded", false);
        //    animator.SetFloat("Jumps", currJumps);
        //    audioComponent.PlayOneShot(jumpClip);
        //    //Debug.Log(isGrounded + "" + currJumps);
        //    if (currJumps >= 2 && !isTouchingWall)
        //    {
        //        ShootBread();
        //    }
        //}

        if (jumpAction.WasPressedThisFrame() && _isAlive)
        {
            if (isTouchingWall && !isGrounded)
            {
                jumpFlag = true;
                audioComponent.PlayOneShot(jumpClip);
            }
            else if (currJumps < maxJumps)
            {
                currJumps++;
                jumpFlag = true;
                animator.SetBool("Grounded", false);
                animator.SetFloat("Jumps", currJumps);
                audioComponent.PlayOneShot(jumpClip);

                if (currJumps >= 2) ShootBread();
            }
        }



        if (jumpFlag == false)
        {
            animator.SetBool("Grounded", isGrounded);
        }

        // Set the facing direction
        if (moveDirection < -0.01f && facingRight)
        {
            // flip to the left
            facingRight = false;
            Vector3 currentScale = transform.localScale;
            currentScale.x *= -1f;
            transform.localScale = currentScale;
        }
        else if (moveDirection > 0.01 && !facingRight)
        {
            // flip to the right
            facingRight = true;
            Vector3 currentScale = transform.localScale;
            currentScale.x *= -1f;
            transform.localScale = currentScale;
        }
    }

    void FixedUpdate()
    {
        // reset the ground check
        isGrounded = false;

        // is the jump flag set?
        if (jumpFlag)
        {
            // jump force thingy
            rb.linearVelocityY = jumpHeight;
            jumpFlag = false;
        }
        else
        {
            // perform a ground check
            Vector3 groundCheck = groundCheckOffset;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + groundCheck, groundCheckRadius, groundLayerMask);

            // was there any collision within the mask layers?
            if (colliders.Length > 0)
            {
                isGrounded = true;
                currJumps = 0;
                animator.SetFloat("Jumps", 0);
            }
            else
            {
                isGrounded = false;
            }
        }

        if (hasJamPowerUp)
        {
            float checkDist = 0.5f;
            bool wallRight = Physics2D.OverlapCircle((Vector2)transform.position + Vector2.right * checkDist, 0.05f, wallLayerMask);
            bool wallLeft = Physics2D.OverlapCircle((Vector2)transform.position + Vector2.left * checkDist, 0.05f, wallLayerMask);
            bool wallTop = Physics2D.OverlapCircle((Vector2)transform.position + Vector2.up * checkDist, 0.05f, wallLayerMask);
            bool wallBottom = Physics2D.OverlapCircle((Vector2)transform.position + Vector2.down * checkDist, 0.05f, wallLayerMask);


            isTouchingWall = (wallRight || wallLeft || wallTop || wallBottom) && !isGrounded;

            if (isTouchingWall && !isGrounded)
            {
                rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -wallSlideSpeed, float.MaxValue);

                if (!jumpFlag && currJumps > 0)
                {
                    currJumps = 1;
                    animator.SetFloat("Jumps", 0);
                }
         
                if (jumpFlag)
                {
                    float jumpDir = wallRight ? -1f : 1f;
                    rb.linearVelocity = new Vector2(jumpDir * wallJumpForce, wallJumpVertical);


                    facingRight = (jumpDir > 0);
                    Vector3 currentScale = transform.localScale;
                    currentScale.x = facingRight ? Mathf.Abs(currentScale.x) : -Mathf.Abs(currentScale.x);
                    transform.localScale = currentScale;

                    // Set to 2 so they can't double jump after leaving the wall
                    currJumps = 2f;
                    animator.SetFloat("Jumps", 2f);
                    jumpFlag = false;
                }
            }

        }




        // apply the movement velocity
        rb.linearVelocityX = moveDirection * speed;
        animator.SetBool("Grounded", isGrounded || (isTouchingWall && currJumps == 0));
    }

    private void OnDrawGizmos()
    {
        // set ground status
        if (isGrounded) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Vector3 groundCheck = groundCheckOffset;
        // draw ground circle
        Gizmos.DrawWireSphere(transform.position + groundCheck, groundCheckRadius);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isAlive) { return; }

        if (collision.gameObject.tag == "Enemy")
        {
            // test the direction of the hit. 
            ContactPoint2D contact = collision.contacts[0];

            // Debug.Log("Normal: " + contact.normal);

            if (contact.normal.y > 0.75f)
            {
                BossScript boss = collision.gameObject.GetComponent<BossScript>();

                if (boss != null)
                {
                    rb.linearVelocityY = jumpHeight * 0.5f;
                    boss.TakeDamage();
                    GameManager.Gary.AddScore(5);
                    return;
                }

                // we killed the enemy
                EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
                rb.linearVelocityY = jumpHeight * 0.5f;

                if (enemy)
                {
                    // send it the death command
                    enemy.PlayerKilledEnemy();
                    GameManager.Gary.AddScore(5);

                }
                else { return; }

            }
            else
            {
                // turn off other game thing
                _isAlive = false;
                Die();
            }
        }

        //An invincible/unkillable sprite
        if (collision.gameObject.tag == "Hazard")
        {
            _isAlive = false;
            Die();
        }

        //A non jumpable sprite (still toastable)
        if (collision.gameObject.tag == "Threat")
        {
            _isAlive = false;
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isAlive) return;

        if (collision.CompareTag("Bread") && currJumps > 1)
        {
            // fully refresh double jumps
            currJumps = 1;
            animator.SetFloat("Jumps", 1);

            audioComponent.PlayOneShot(jumpClip);

            BreadScript bread = collision.GetComponent<BreadScript>();
            if (bread != null)
                bread.Collect();

            SoundManager.Steve.MakeParrySound();
        }
    }


    public void Die()
    {
        lives--;

        if (lives <= 0)
        {
            GameManager.Gary.LoseScore();
            GameManager.Gary.StopTimer();
            GameManager.Gary.OverText();

            // enemy has killed us
            Debug.Log("Player was injured");

            animator.SetTrigger("Death");
            moveDirection = 0f;
            rb.linearVelocity = Vector3.zero;

            rb.linearVelocityY = jumpHeight;

            foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
            {
                col.enabled = false;
            }

            this.enabled = false;
            SoundManager.Steve.MakeDeathSound();

            Invoke(nameof(BeginRestart), 3f);

        }
        else
        {
            float pushBack = facingRight ? -5f : 5f;
            rb.linearVelocity = new Vector2(pushBack, jumpHeight * 0.5f);
            _isAlive = true;
        }

    }

    private void BeginRestart()
    {
        LevelManager.Larry.ReloadLevel();
    }

    void ShootBread()
    {
        GameObject proj = Instantiate(breadProjectile, firePoint.position, Quaternion.identity);
        audioComponent.PlayOneShot(shootClip);
    }

    public void AddLife()
    {
        lives = 2;
    }
}