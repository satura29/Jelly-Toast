using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool activated = false;
    public Animator animator;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !activated)
        {
            activated = true;
            GameManager.Gary.lastCheckpointPos = transform.position;

            SoundManager.Steve.MakeCheckpointSound();
            animator.SetTrigger("Activate");
        }
    }
}
