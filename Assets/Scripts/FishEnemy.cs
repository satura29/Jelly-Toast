using UnityEngine;

public class Fish : MonoBehaviour
{
    public float verticalAmplitude = 5f;
    public float verticalFrequency = 2f;

    private Rigidbody2D rb;
    private Vector2 startPos;

    public Animator animator;
    private bool animationFlipped = false;
    private float timer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        startPos = rb.position;

        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float yOffset = Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude;
        rb.position = new Vector2(rb.position.x, startPos.y + yOffset);

        timer += Time.fixedDeltaTime;

        if (rb.linearVelocityY > 0)
            animator.SetTrigger("Fall");
        else if (rb.linearVelocityY < 0)
            animator.SetTrigger("Rise");
        //if (!animationFlipped && timer >= verticalFrequency/2)
        //{
        //    animator.SetTrigger("Fall");
        //    animationFlipped = true;
        //}

        //if (animationFlipped && timer >= verticalFrequency + (verticalFrequency/2))
        //{
        //    animator.SetTrigger("Rise");
        //    animationFlipped = false;
        //    timer = 0f;
        //}
    }
}
