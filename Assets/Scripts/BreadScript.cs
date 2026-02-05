using UnityEngine;
using System.Collections;

public class BreadScript : MonoBehaviour
{
    private Animator animator;
    private Collider2D coll;

    public float respawnDelay = 3f;     // how long bread stays inactive

    void Start()
    {
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    public void Collect()
    {
        // Trigger the “collected” animation
        animator.SetTrigger("Collected");

        // Disable collision so the player can't collect again
        coll.enabled = false;

        // Start the respawn coroutine
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        // Wait for a few seconds before re-enabling
        yield return new WaitForSeconds(respawnDelay);

        // Trigger respawn animation
        animator.SetTrigger("Respawn");

        // Re-enable collision
        coll.enabled = true;
    }
}
