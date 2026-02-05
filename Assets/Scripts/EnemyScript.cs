using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float enemySpeed;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    public bool facingLeft = true;

    public bool _isAlive = true;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (facingLeft == sprite.flipX) { sprite.flipX = !facingLeft; } // if the values match, reverse the values
    }

    private void FixedUpdate()
    {
        // set the Rigidbody2D linear velocity x-value to speed * direction
        if (_isAlive)
        {
            rb.linearVelocityX = facingLeft ? -enemySpeed : enemySpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TurnAround")
        {
            facingLeft = !facingLeft;
        }
    }

    public void PlayerKilledEnemy()
    {
        if (_isAlive)
        {
            // handle the event of my death
            _isAlive = false;

            // run the death animation
            animator.SetTrigger("EnemyDeath");

            // change rb bodytype
            rb.bodyType = RigidbodyType2D.Kinematic;
            // change layer
            gameObject.layer = LayerMask.NameToLayer("NotForPlayer");
            SoundManager.Steve.MakeEnemyHitSound();

            // destroy the game object
            Destroy(gameObject, 3f);
        }
    }
}