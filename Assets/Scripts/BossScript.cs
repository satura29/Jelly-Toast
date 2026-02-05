using UnityEngine;

public class BossScript : MonoBehaviour
{
    [Header("Boss Stats")]
    //Stomp = 3, Bread = 1
    public int maxHP = 15;
    private int currentHP;
    public int maxSP = 3;
    private int currentSP;
    private int currPhase = 1;
    public string currAttack = "Taunt";
    private int currMoves;
    public GameObject powerJamPrefab;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float saveSpeed = 3f;
    public float dashSpeed = 7.5f;
    public float shellSpeed = 6f;
    public bool facingLeft = true;

    [Header("State Flags")]
    public bool isAlive = true;
    public bool inShellMode = false;
    public bool invincible = false;
    public bool armored = false;
    private bool spiky = false;

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;

    [Header("Attack Mode Settings")]
    //Inspired by The Koopalings and Bowser Jr Phase One in Wonder
    public float shellDuration = 2f;
    public float dizzyDuration = 1.5f;
    public float invincibleDuration = 3f;
    public float jumpForce = 12f;

    [Header("Sounds")]
    public AudioClip roarSound;
    public AudioClip hitSound;
    public AudioClip recoverSound;
    public AudioClip jumpSound;
    public AudioClip spinSound;
    public AudioClip dashSound;
    public AudioClip deathSound;

    private AudioSource thisAudio;
    private AudioSource loopAudio;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        thisAudio = GetComponent<AudioSource>();

        loopAudio = GetComponent<AudioSource>();
        loopAudio.clip = spinSound;
        loopAudio.loop = true;

        currentHP = maxHP;
        currentSP = maxSP;
        moveSpeed = 0;

        StartCoroutine(Taunt());
    }

    void Update()
    {
        if (!isAlive) return;

        // face sprite based on direction
        sprite.flipX = !facingLeft;
    }

    void FixedUpdate()
    {
        if (!isAlive) return;

        // movement depends on attack
        if (inShellMode)
        {
            rb.linearVelocityX = facingLeft ? -shellSpeed : shellSpeed;
        }
        else
        {
            rb.linearVelocityX = facingLeft ? -moveSpeed : moveSpeed;
        }
    }

    private System.Collections.IEnumerator Taunt()
    {
        yield return new WaitForSeconds(0.75f);
        thisAudio.PlayOneShot(roarSound);
        yield return new WaitForSeconds(1f);
        currAttack = "Move";
        moveSpeed = saveSpeed;
        Debug.Log(currAttack);
    }

    //Take DMG
    public void TakeDamage()
    {
        if (!isAlive) return;
        if (invincible) return;

        currentHP-= 3;
        CalculateDamage();
        Debug.Log("BOSS HIT! HP:" + currentHP);
        thisAudio.PlayOneShot(hitSound);
    }

    public void ToasterDamage()
    {
        if (!isAlive) return;
        if (armored)
        {
            currentSP -= 1;
            CalculateHelmet();
            StartCoroutine(Flash());
            Debug.Log("HELMET HIT! SP:" + currentSP + ", with current HP still HP:" + currentHP);
            thisAudio.PlayOneShot(hitSound);
        } else
        {
            currentHP -= 1;
            CalculateDamage();
            StartCoroutine(Flash());
            Debug.Log("BOSS HIT! HP:" + currentHP);
            thisAudio.PlayOneShot(hitSound);
        }
    }

    private System.Collections.IEnumerator Flash()
    {
        for (int i = 0; i < 4; i++)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.05f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void CalculateDamage()
    {
        if (currentHP <= 0)
        {
            Die();
            SoundManager.Steve.StopTheMusic();
            return;
        }
        else if (currentHP <=12 && currPhase == 1)
        {
            StartCoroutine(Hurt());
            currPhase += 1;
            currentHP = 12;
            thisAudio.PlayOneShot(recoverSound);
        }
        else if (currentHP <= 9 && currPhase == 2)
        {
            StartCoroutine(Hurt());
            currPhase += 1;
            currentHP = 9;
            thisAudio.PlayOneShot(recoverSound);
        }
        else if (currentHP <= 6 && currPhase == 3)
        {
            StartCoroutine(Hurt());
            currPhase += 1;
            currentHP = 6;
            thisAudio.PlayOneShot(recoverSound);
        }
        else if (currentHP <= 3 && currPhase == 4)
        {
            StartCoroutine(Hurt());
            currPhase += 1;
            currentHP = 3;
            thisAudio.PlayOneShot(recoverSound);

            StartCoroutine(PulseRed());
        }
    }

    //Now what attack?
    private System.Collections.IEnumerator Hurt()
    {
        currAttack = "Hurt";
        invincible = true;

        animator.SetTrigger("Hurt");
        moveSpeed = 0f;

        yield return new WaitForSeconds(0.65f);
        if (currPhase == 2)
        {
            invincible = true;
            animator.SetTrigger("Dash");
            currAttack = "Dash";
            moveSpeed = dashSpeed + (currPhase * 2) - 4;
            currMoves = currPhase - 1;
            Debug.Log(currAttack);
            thisAudio.PlayOneShot(dashSound);
        } else if (currPhase == 3)
        {
            SoundManager.Steve.BossIsAngry();
            animator.SetTrigger("Shell");
            currentSP = maxSP;
            armored = true;
            spiky = true;
            Debug.Log("Spin");

            yield return new WaitForSeconds(0.5f);
            animator.SetTrigger("ShellIdle");
            Debug.Log("Idle");

            yield return new WaitForSeconds(1f);
            StartCoroutine(ShellPhase());
        }
        else if (currPhase == 4)
        {
            animator.SetTrigger("Shell");
            currentSP = maxSP + 1;
            armored = true;
            spiky = true;

            //yield return new WaitForSeconds(0.2f);
            //animator.SetTrigger("Shell");

            yield return new WaitForSeconds(0.5f);
            animator.SetTrigger("ShellIdle");

            yield return new WaitForSeconds(1f);
            currMoves = 0;
            StartCoroutine(ShellPhase());
        }
        else if (currPhase == 5)
        {
            SoundManager.Steve.BossIsPissed();
            animator.SetTrigger("Shell");
            currentSP = maxSP + 2;
            armored = true;
            spiky = true;

            //yield return new WaitForSeconds(0.2f);
            //animator.SetTrigger("Shell");

            yield return new WaitForSeconds(0.5f);
            animator.SetTrigger("ShellIdle");

            yield return new WaitForSeconds(1f);
            currMoves = 2;
            StartCoroutine(ShellPhase());
        }
    }

    private void CalculateHelmet()
    {
        if (currentSP <= 0 && armored == true)
        {
            StartCoroutine(ShellBreak());
            return;
        }
    }

    //Dizzy from Dash Attack
    private System.Collections.IEnumerator ShellBreak()
    {
        invincible = false;
        armored = false;
        spiky = false;
        inShellMode = false;

        animator.SetTrigger("Break");
        loopAudio.Stop();
        moveSpeed = 0f;

        yield return new WaitForSeconds(dizzyDuration/currPhase);

        if (currAttack != "Hurt")
        {
            animator.SetTrigger("Dash");
            currAttack = "Dash";
            thisAudio.PlayOneShot(dashSound);
            spiky = true;
            moveSpeed = dashSpeed + (currPhase * 2) - 4;
            currMoves = currPhase - 1;
        }
    }

    //Dizzy from Dash Attack
    private System.Collections.IEnumerator Dizzy()
    {
        invincible = false;
        spiky = false;

        animator.SetTrigger("Idle");
        moveSpeed = 0f;

        yield return new WaitForSeconds(dizzyDuration);

        if (currAttack != "Hurt")
        {
            animator.SetTrigger("Dash");
            currAttack = "Dash";
            thisAudio.PlayOneShot(dashSound);
            invincible = true;
            spiky = true;
            moveSpeed = dashSpeed + (currPhase * 2) - 4;
            currMoves = currPhase - 1;
        }
    }

    private System.Collections.IEnumerator ShellPhase()
    {
        //StopAllCoroutines();
        if (currentSP > 0)
        {
            currAttack = "Shell";
            inShellMode = true;
            moveSpeed = shellSpeed + (currPhase * 20) - 40;
            animator.SetTrigger("Shell");
            Debug.Log("Shell");
            loopAudio.Play();

            yield return new WaitForSeconds(invincibleDuration);

            if (currentSP > 0)
            {
                inShellMode = false;

                if (currPhase >= 4 && currentSP > 0)
                {
                    if (currMoves > 0)
                    {
                        currMoves--;
                        Debug.Log("Move Jump");
                        thisAudio.PlayOneShot(jumpSound);

                        //float direction = facingLeft ? -1f : 1f;
                        //rb.linearVelocityX = direction * moveSpeed;

                        moveSpeed = 0;
                        moveSpeed = 2.5f;

                        rb.linearVelocityY = jumpForce;

                        animator.SetTrigger("Jump");
                        yield return new WaitForSeconds(0.8f);
                        animator.SetTrigger("Fall");
                        yield return new WaitForSeconds(0.7f);
                        animator.SetTrigger("Shell");
                        StartCoroutine(ShellPhase());
                    }
                    else
                    {
                        Debug.Log("Still Jump");
                        thisAudio.PlayOneShot(jumpSound);
                        moveSpeed = 0;
                        rb.linearVelocityX = 0f;
                        rb.linearVelocityY = jumpForce;

                        animator.SetTrigger("Jump");
                        yield return new WaitForSeconds(0.8f);
                        animator.SetTrigger("Fall");
                        yield return new WaitForSeconds(0.7f);

                        if (currentSP > 0)
                        {
                            yield return new WaitForSeconds(0.1f);
                            loopAudio.Stop();
                            StartCoroutine(Recover());
                        }
                    }
                } else if (currPhase < 4 && currentSP > 0)
                {
                    moveSpeed = 0;
                    loopAudio.Stop();
                    StartCoroutine(Recover());
                }
            }
        }
    }

    //Dizzy from Dash Attack
    private System.Collections.IEnumerator Recover()
    {
        animator.SetTrigger("ShellIdle");
        moveSpeed = 0f;

        yield return new WaitForSeconds(dizzyDuration);

        if (currentSP > 0)
        {
            animator.SetTrigger("Shell");
            if (currPhase <= 4)
            {
                currMoves = 0;
                StartCoroutine(ShellPhase());
            }
            else if (currPhase == 5)
            {
                currMoves = 2;
                StartCoroutine(ShellPhase());
            } 
        }
    }


    //Splat
    private void Die()
    {
        isAlive = false;
        animator.SetTrigger("Death");

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
            col.enabled = false;

        if (powerJamPrefab != null)
        {
            Instantiate(powerJamPrefab, transform.position, Quaternion.identity);
        }

        SoundManager.Steve.MakeEnemyHitSound();
        thisAudio.PlayOneShot(deathSound);

        Destroy(gameObject, 0.5f);
    }

    //Turn thing-a-ma-jig
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TurnAround"))
        {
            facingLeft = !facingLeft;
            if (currAttack == "Dash")
            {
                currMoves--;
                if (currMoves <= 0)
                {
                    StartCoroutine(Dizzy());
                }
                else
                {
                    thisAudio.PlayOneShot(dashSound);
                }
            }
        }
    }

    //Cool effect I thought would be neat.
    private System.Collections.IEnumerator PulseRed()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

        float speed = 6f;
        float intensity = 0.75f;

        while (true)
        {
            float t = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f;
            float redAmount = Mathf.Lerp(0f, intensity, t);

            foreach (var r in renderers)
                r.color = new Color(1f, 1f - redAmount, 1f - redAmount, 1f);

            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Player touches boss while boss is spiky
        if (spiky && collision.collider.CompareTag("Player"))
        {
            CharacterController2DScript player = collision.collider.GetComponent<CharacterController2DScript>();
            if (player != null)
            {
                player.Die();
            }
        }
    }

}
